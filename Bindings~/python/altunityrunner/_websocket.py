import time
import json
from collections import defaultdict, deque
from threading import Thread

from loguru import logger
import websocket

from .exceptions import ConnectionError, ConnectionTimeoutError


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
        self._store = Store()
        self._errors = deque()

        self._websocket = None
        self._is_open = False
        self.url = "ws://{}:{}/altws/".format(host, port)

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
                "Failed to connect to AltUnity host: {} port: {}.".format(self.host, self.port)
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
        self._store.push(response.get("commandName"), response)

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
        logger.info("Connecting to host: {} port: {}.".format(self.host, self.port))

        self._websocket = self._create_connection()
        self._wait_for_connection_to_open(timeout=self.timeout)

    def send(self, data):
        self._ensure_connection_is_open()
        self._current_command_name = data.get("commandName")

        message = json.dumps(data)
        logger.info("Message: {}".format(message))

        self._websocket.send(message)

    def recv(self):
        self._ensure_connection_is_open()

        if self._store.has(self._current_command_name):
            return self._store.pop(self._current_command_name)

        time.sleep(0.1)
        return self.recv()

    def close(self):
        logger.info("Closing connection to AltUnity on host: {} port: {}".format(self.host, self.port))

        if self._websocket is not None:
            self._websocket.close()
            self._websocket = None

        self._is_open = False
