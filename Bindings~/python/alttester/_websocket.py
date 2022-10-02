import time
import json
from collections import defaultdict, deque
from threading import Thread

from loguru import logger
import websocket

from .commands.Notifications.notification_type import NotificationType
from .commands.Notifications.load_scene_notification_result import LoadSceneNotificationResult
from .commands.Notifications.log_notification_result import LogNotificationResult
from .commands.Notifications.load_scene_mode import LoadSceneMode
from .exceptions import ConnectionError, ConnectionTimeoutError, CommandResponseTimeoutException


class Store:
    """Stores the responses from AltUnity."""

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
    """Handles the parsing of the notification messages from AltUnity."""

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
    """Handles the parsing of command messages from AltUnity."""

    def __init__(self):
        self._store = Store()

        self._current_command = None
        self._timeout_commands = []

    def __repr__(self):
        return "{}()".format(self.__class__.__name__)

    def set_current_command(self, message):
        self._current_command = (message.get("messageId"), message.get("commandName"))

    def get_current_command(self):
        return self._current_command

    def timeout(self):
        """Mark the current command as timedout."""

        self._timeout_commands.append(self._current_command)

    def handle_command(self, message):
        command = (message.get("messageId"), message.get("commandName"))

        # Skip messages for commands that timedout
        if command in self._timeout_commands:
            return

        self._store.push(command, message)

    def has_response(self):
        return self._store.has(self._current_command)

    def get_response(self):
        return self._store.pop(self._current_command)


class WebsocketConnection:
    """Handles the websocket connection with AltUnity.

    Args:
        host (:obj:`str`): The host to connect to.
        port (:obj:`int`): The port to connect to.
        timeout (:obj:`int` or :obj:`float`): The connection timeout time.

    """

    def __init__(self, host="127.0.0.1", port=13000, timeout=None):
        self.host = host
        self.port = port
        self.url = "ws://{}:{}/altws/".format(host, port)

        self.timeout = timeout
        self.command_timeout = 60
        self.delay = 0.1

        self._errors = deque()

        self._thread = None
        self._websocket = None
        self._is_open = False

        self._command_handler = CommandHandler()
        self._notification_handler = NotificationHandler()

    def __repr__(self):
        return "{}({!r}, {!r}, {!r})".format(
            self.__class__.__name__,
            self.host,
            self.port,
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
        self._thread = Thread(target=self._websocket.run_forever, daemon=True).start()

    def _ensure_connection_is_open(self):
        if self._errors:
            error = self._errors.pop()
            self.close()
            raise ConnectionError(error)

        if self._websocket is None or not self._is_open:
            self.close()
            raise ConnectionError("Connection already closed.")

    def _on_message(self, ws, message):
        """A callback which is called when the connection is opened."""

        logger.debug("Received: {}", message)
        response = json.loads(message)

        if response.get("isNotification"):
            self._notification_handler.handle_notification(response)
        else:
            self._command_handler.handle_command(response)

    def _on_error(self, ws, error):
        """A callback which is called when the connection gets an error."""

        logger.debug("Error: {}", error)
        self._errors.append(error)

    def _on_close(self, ws, close_status_code, close_msg):
        """A callback which is called when the connection is closed."""

        logger.debug(
            "Connection to AltUnity closed with status code: {} and message: {}.",
            close_status_code,
            close_msg
        )

        self._is_open = False
        self._websocket = None

    def _on_open(self, ws):
        """A callback which is called when the connection recives data."""

        logger.debug("Connection oppend successfully.")
        self._is_open = True

    def set_command_timeout(self, timeout):
        self.command_timeout = timeout

    def get_command_timeout(self):
        return self.command_timeout

    def connect(self):
        logger.info("Connecting to host: {} port: {}.", self.host, self.port)

        elapsed_time = 0
        self._create_connection()

        while not self._is_open and (self.timeout is None or elapsed_time < self.timeout):
            time.sleep(self.delay)
            elapsed_time += self.delay

            if self._errors:
                self.close()
                self._create_connection()

        if self._errors and not self._is_open:
            error = self._errors.pop()
            self.close()

            raise ConnectionError(error)

        if not self._is_open:
            self.close()

            raise ConnectionTimeoutError(
                "Failed to connect to AltUnity Tester host: {} port: {}.".format(self.host, self.port)
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
            raise CommandResponseTimeoutException()

    def close(self):
        logger.info("Closing connection to AltUnity on host: {} port: {}", self.host, self.port)

        if self._websocket:
            self._websocket.close()
            self._websocket = None

        if self._thread:
            self._thread.join(0)
            self._thread = None

        self._errors = []
        self._is_open = False

    def add_notification_listener(self, notification_type, callback, overwrite=False):
        self._notification_handler.add_notification_listener(notification_type, callback, overwrite=overwrite)

    def remove_notification_listener(self, notification_type):
        self._notification_handler.remove_notification_listener(notification_type)
