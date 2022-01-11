import time
import json
from collections import defaultdict, deque
from threading import Thread

from loguru import logger
import websocket
from altunityrunner.commands.Notifications.notification_type import NotificationType
from altunityrunner.commands.Notifications.load_scene_notification_result import LoadSceneNotificationResult
from altunityrunner.commands.Notifications.log_notification_result import LogNotificationResult
from altunityrunner.commands.Notifications.load_scene_mode import LoadSceneMode

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
        self.timeout = timeout

        self._current_command_name = None
        self._current_command_id = None
        self.command_timeout = 60
        self._store = Store()
        self._errors = deque()

        self._websocket = None
        self._is_open = False
        self.url = "ws://{}:{}/altws/".format(host, port)
        self.load_scene_callbacks = []
        self.log_callbacks = []
        self.application_paused_callbacks = []
        self.message_id_timeouts = []

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
        ws = websocket.WebSocketApp(
            self.url,
            on_open=self._on_open,
            on_message=self._on_message,
            on_error=self._on_error,
            on_close=self._on_close
        )
        Thread(target=ws.run_forever, daemon=True).start()

        return ws

    def _wait_for_connection_to_open(self, timeout=None, delay=0.1):
        elapsed_time = 0

        while (not self._is_open and not self._errors) and (timeout is None or elapsed_time < timeout):
            time.sleep(delay)
            elapsed_time += delay

        if self._errors:
            self.close()

            raise ConnectionError(self._errors.pop())

        if not self._is_open:
            self.close()

            raise ConnectionTimeoutError(
                "Failed to connect to AltUnity Tester host: {} port: {}.".format(self.host, self.port)
            )

    def _ensure_connection_is_open(self):
        if self._errors:
            self.close()
            raise ConnectionError(self._errors.pop())

        if self._websocket is None or not self._is_open:
            self.close()
            raise ConnectionError("Connection already closed.")

    def _on_message(self, ws, message):
        """A callback which is called when the connection is opened."""

        logger.debug("Message: {}", message)

        response = json.loads(message)
        if(response.get("isNotification")):
            self.handle_notification(response)
        else:
            self._store.push(response.get("commandName"), response)

    def handle_notification(self, message):
        if(message.get("commandName") == "loadSceneNotification"):
            data = json.loads(message.get("data"))
            load_scene_result = LoadSceneNotificationResult(
                data.get("sceneName"), LoadSceneMode(data.get("loadSceneMode")))
            for callback in self.load_scene_callbacks:
                callback(load_scene_result)
        elif(message.get("commandName") == "unloadSceneNotification"):
            scene_name = json.loads(message.get("data"))
            for callback in self.unload_scene_callbacks:
                callback(scene_name)
        elif(message.get("commandName") == "logNotification"):
            data = json.loads(message.get("data"))
            log_result = LogNotificationResult(
                data.get("message"), data.get("stack_trace"), data.get("level"),)
            for callback in self.log_callbacks:
                callback(log_result)
        elif(message.get("commandName") == "applicationPausedNotification"):
            data = json.loads(message.get("data"))
            application_paused_result = bool(data)
            for callback in self.application_paused_callbacks:
                callback(application_paused_result)

    def _on_error(self, ws, error):
        """A callback which is called when the connection gets an error."""

        logger.debug("Error: {}", error)
        self._errors.append(error)

    def _on_close(self, ws, close_status_code, close_msg):
        """A callback which is called when the connection is closed."""

        logger.debug("Connection to AltUnity closed with status code: {} and message: {}.",
                     close_status_code, close_msg)

        self._is_open = False
        self._websocket = None

    def _on_open(self, ws):
        """A callback which is called when the connection recives data."""

        logger.debug("Connection oppend successfully.")
        self._is_open = True

    def connect(self):
        logger.info("Connecting to host: {} port: {}.", self.host, self.port)

        self._websocket = self._create_connection()
        self._wait_for_connection_to_open(timeout=self.timeout)

    def send(self, data):
        self._ensure_connection_is_open()
        self._current_command_name = data.get("commandName")
        self._current_command_id = data.get("messageId")

        message = json.dumps(data)
        logger.info("Message: {}", message)

        self._websocket.send(message)

    def recv(self):
        self._ensure_connection_is_open()
        elapsed_time = 0
        delay = 0.1
        while True:
            while (elapsed_time <= self.command_timeout):
                if self._store.has(self._current_command_name):
                    if self._current_command_id in self.message_id_timeouts:
                        continue
                    return self._store.pop(self._current_command_name)

                elapsed_time += delay
                time.sleep(delay)
            if elapsed_time > self.command_timeout and self._is_open:
                self.message_id_timeouts.append(self._current_command_id)
                raise CommandResponseTimeoutException()

    def set_command_timeout(self, timeout):
        self.command_timeout = timeout

    def close(self):
        logger.info("Closing connection to AltUnity on host: {} port: {}", self.host, self.port)

        if self._websocket is not None:
            self._websocket.close()
            self._websocket = None

        self._is_open = False

    def add_notification_listener(self, notification_type, notification_callback, overwrite):
        if(notification_type == NotificationType.LOADSCENE):
            if(overwrite):
                self.load_scene_callbacks = [notification_callback]
            else:
                self.load_scene_callbacks += notification_callback
        elif(notification_type == NotificationType.UNLOADSCENE):
            if(overwrite):
                self.unload_scene_callbacks = [notification_callback]
            else:
                self.unload_scene_callbacks += notification_callback
        elif(notification_type == NotificationType.LOG):
            if(overwrite):
                self.log_callbacks = [notification_callback]
            else:
                self.log_callbacks += notification_callback
        elif(notification_type == NotificationType.APPLICATION_PAUSED):
            if(overwrite):
                self.application_paused_callbacks = [notification_callback]
            else:
                self.application_paused_callbacks += notification_callback

    def remove_notification_listener(self, notification_type):
        if(notification_type == NotificationType.LOADSCENE):
            self.load_scene_callbacks = []
        elif(notification_type == NotificationType.UNLOADSCENE):
            self.unload_scene_callbacks = []
        elif(notification_type == NotificationType.LOG):
            self.log_callbacks = []
        elif(notification_type == NotificationType.APPLICATION_PAUSED):
            self.application_paused_callbacks = []
