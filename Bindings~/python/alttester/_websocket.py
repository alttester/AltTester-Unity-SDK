"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

import time
import json
from collections import defaultdict, deque
from urllib.parse import urlencode, urlunparse
from threading import Thread

from loguru import logger
import websocket

from . import exceptions
from .commands.Notifications.notification_type import NotificationType
from .commands.Notifications.load_scene_notification_result import LoadSceneNotificationResult
from .commands.Notifications.log_notification_result import LogNotificationResult
from .commands.Notifications.load_scene_mode import LoadSceneMode


class Store:
    """Stores the responses from AltTester® Server."""

    def __init__(self, dict=None):
        self._store = dict or defaultdict(deque)

    def __repr__(self):
        return "{}({!r})".format(self.__class__.__name__, self._store)

    def has(self, key):
        if not key:
            return False

        return len(self._store[key]) > 0

    def push(self, key, value):
        self._store[key].append(value)

    def pop(self, key):
        try:
            return self._store[key].popleft()
        except IndexError:
            return None


class NotificationHandler:
    """Handles the parsing of the notification messages from AltTester® Server."""

    def __init__(self):
        self._notification_callbacks = defaultdict(list)

    def __repr__(self):
        return "{}()".format(self.__class__.__name__)

    def handle_notification(self, message):
        result = None
        notification_type = None
        data = json.loads(message.get("data"))

        if message.get("commandName") == "loadSceneNotification":
            notification_type = NotificationType.LOADSCENE
            result = LoadSceneNotificationResult(
                data.get("sceneName"),
                LoadSceneMode(data.get("loadSceneMode"))
            )
        elif message.get("commandName") == "unloadSceneNotification":
            notification_type = NotificationType.UNLOADSCENE
            result = data
        elif message.get("commandName") == "logNotification":
            notification_type = NotificationType.LOG
            result = LogNotificationResult(
                data.get("message"),
                data.get("stack_trace"),
                data.get("level")
            )
        elif message.get("commandName") == "applicationPausedNotification":
            notification_type = NotificationType.APPLICATION_PAUSED
            result = bool(data)

        for callback in self._notification_callbacks[notification_type]:
            callback(result)

    def add_notification_listener(self, notification_type, callback, overwrite=False):
        if overwrite:
            self._notification_callbacks[notification_type] = [callback]

        self._notification_callbacks[notification_type].append(callback)

    def remove_notification_listener(self, notification_type):
        self._notification_callbacks[notification_type].clear()


class CommandHandler:
    """Handles the parsing of command messages from AltTester® Server."""

    def __init__(self):
        self._store = Store()

        self._current_command = None
        self._timeout_commands = []

    def __repr__(self):
        return "{}()".format(self.__class__.__name__)

    def set_current_command(self, message):
        self._current_command = (message.get(
            "messageId"), message.get("commandName"))

    def get_current_command(self):
        return self._current_command

    def timeout(self):
        """Mark the current command as timeout."""

        self._timeout_commands.append(self._current_command)

    def handle_command(self, message):
        command = (message.get("messageId"), message.get("commandName"))

        # Skip messages for commands that timeout
        if command in self._timeout_commands:
            return

        self._store.push(command, message)

    def has_response(self):
        return self._store.has(self._current_command)

    def get_response(self):
        return self._store.pop(self._current_command)


class WebsocketConnection:
    """Handles the websocket connection with AltTester® Server.

    Args:
        host (:obj:`str`, optional): The host to connect to. Defaults to ``127.0.0.1``.
        port (:obj:`int`, optional): The port to connect to. Defaults to ``13000``.
        path (:obj:`int`, optional): The path section of the url. Defaults to ``/``.
        params (:obj:`dict`, optional): The params/query component of the url. Default to ``None``.
        timeout (:obj:`int` or :obj:`float`, optional): The connection timeout time.

    """

    def __init__(self, host="127.0.0.1", port=13000, path="/", params=None, timeout=None, command_handler=None,
                 notification_handler=None):
        self.host = host
        self.port = port
        self.path = path
        self.params = params or {}

        self.url = urlunparse(["ws", "{}:{}".format(
            self.host, self.port), self.path, "", urlencode(self.params), ""])

        self.timeout = timeout
        self.command_timeout = 60
        self.delay = 0.1

        self._errors = deque()
        self._close_message = None

        self._thread = None
        self._websocket = None
        self._is_open = False

        self._command_handler = command_handler
        self._notification_handler = notification_handler
        self._driver_registered_called = False

    def __repr__(self):
        return "{}({!r}, {!r}, {!r}, {!r}, {!r})".format(
            self.__class__.__name__,
            self.host,
            self.port,
            self.path,
            self.params,
            self.timeout,
        )

    def _create_connection(self):
        # TODO: Enable and disable the trace based on an environment variable or config option
        # Uncomment the following line if you are debugging the websocket connection
        # websocket.enableTrace(True)
        self._websocket = websocket.WebSocketApp(
            self.url,
            on_open=self._on_open,
            on_message=self._on_message,
            on_error=self._on_error,
            on_close=self._on_close
        )
        self._thread = Thread(
            target=self._websocket.run_forever, daemon=True).start()

    def _check_close_message(self, close_message):
        if close_message:
            reason = close_message[1]

            if close_message[0] == 4001:
                raise exceptions.NoAppConnected(reason)
            if close_message[0] == 4002:
                raise exceptions.AppDisconnectedError(reason)
            if close_message[0] == 4005:
                raise exceptions.MultipleDriverError(reason)
            if close_message[0] == 4007:
                raise exceptions.MultipleDriversTryingToConnectException(
                    reason)
            if close_message[0] == 4009:
                raise exceptions.MaxNoOfConnectionsDriversExceeded(
                    reason)

            raise exceptions.ConnectionError(
                "Connection closed by AltTester(R) Server with reason: {}.".format(reason))

    def _check_errors(self):
        if self._errors:
            error = self._errors.pop()
            self.close()
            raise exceptions.ConnectionError(error)

    def _ensure_connection_is_open(self):
        self._check_close_message(self._close_message)
        self._check_errors()

        if self._websocket is None or not self._is_open:
            self.close()
            raise exceptions.ConnectionError(
                "Connection closed. An unexpected error ocurred.")

    def _on_message(self, ws, message):
        """A callback which is called when the connection receives data."""

        logger.debug("Received: {}", message)
        response = json.loads(message)

        if response.get("isNotification"):
            if "driverRegistered" in message:
                self._driver_registered_called = True
            else:
                self._notification_handler.handle_notification(response)
        else:
            self._command_handler.handle_command(response)

    def _on_error(self, ws, error):
        """A callback which is called when the connection gets an error."""

        logger.error("Error: {}", error)
        self._errors.append(error)

    def _on_close(self, ws, close_status_code, close_msg):
        """A callback which is called when the connection is closed."""

        logger.debug(
            "Connection to AltTester(R) Server closed with status code: {} and message: '{}'.",
            close_status_code,
            close_msg
        )

        self._close_message = (close_status_code, close_msg)
        self._driver_registered_called = False
        self._is_open = False
        self._websocket = None
        if close_status_code == 4002:
            self.connect()

    def _on_open(self, ws):
        """A callback which is called when the connection is opened."""

        logger.debug("Connection opened successfully.")
        self._is_open = True

    def set_command_timeout(self, timeout):
        self.command_timeout = timeout

    def get_command_timeout(self):
        return self.command_timeout

    def connect(self):
        logger.info("Connecting to URL: '{}'.", self.url)
        last_close_message = None
        elapsed_time = 0
        while self.timeout is None or elapsed_time < self.timeout:
            self._create_connection()
            wait_for_notification = 0
            try:
                while wait_for_notification < 5:
                    if self._driver_registered_called:
                        logger.debug("Connected to: '{0}'.".format(self.url))
                        return
                    time.sleep(self.delay)
                    wait_for_notification += self.delay
                    self._check_close_message(self._close_message)
            except Exception:
                last_close_message = self._close_message
                self.close()

            elapsed_time += wait_for_notification

            if self._is_open:  # Added this to be also backward compatible but it will be slower
                return

        self._check_close_message(last_close_message)
        self._check_errors()
        raise exceptions.ConnectionTimeoutError(
            "Failed to connect to AltTester(R) Server host: {} port: {}.".format(
                self.host, self.port)
        )

    def send(self, data):
        self._ensure_connection_is_open()

        message = json.dumps(data)
        logger.debug("Sent: {}", message)

        self._command_handler.set_current_command(data)
        self._websocket.send(message)

    def recv(self):
        self._ensure_connection_is_open()
        elapsed_time = 0
        delay = 0.1

        while elapsed_time <= self.command_timeout:
            if self._command_handler.has_response():
                return self._command_handler.get_response()

            elapsed_time += delay
            time.sleep(delay)

        if elapsed_time > self.command_timeout:
            self._command_handler.timeout()
            raise exceptions.CommandResponseTimeoutException()

    def close(self):
        logger.info(
            "Closing connection to AltTester(R) Server on host: {} port: {}", self.host, self.port)

        if self._websocket:
            self._websocket.close()
            self._websocket = None

        if self._thread:
            self._thread.join(0)
            self._thread = None

        self._errors = []
        self._close_message = None
        self._is_open = False
        self._driver_registered_called = False
