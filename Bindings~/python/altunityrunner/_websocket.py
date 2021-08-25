from collections import defaultdict
import time
import json

from loguru import logger
import websocket

from .altUnityExceptions import ConnectionError


class Store:

    def __init__(self):
        self._store = defaultdict(list)

    def has(self, key):
        if not key:
            return False

        return len(self._store[key]) > 0

    def push(self, key, value):
        self._store[key].append(value)

    def pop(self, key):
        try:
            self._store[key].pop(0)
        except IndexError:
            return None


class WebsocketConnection:
    """Handles the websocket connection with the AltUnityServer.

    Args:
        host (:obj:`str`): The host to connect to.
        port (:obj:`int`): The port to connect to.
        timeout (:obj:`int` or :obj:`float`): Socket timeout time.
        tries (:obj:`int`): The maximum number of attempts to connect.

    """

    def __init__(self, host="127.0.0.1", port=13000, timeout=60, tries=5):
        self.host = host
        self.port = port
        self.timeout = timeout

        self.url = "ws://{}:{}/altws/".format(host, port)
        self._websocket = self._connect(self.url, timeout=timeout, tries=tries)

        self._store = Store()
        self._current_command_name = None

    def __repr__(self):
        return "{}({!r}, {!r}, {!r})".format(self.__class__.__name__, self.host, self.port, self.timeout)

    def _connect(self, url, timeout=60, tries=5, delay=0.1):
        logger.info("Connecting to AltUnityServer on: {}".format(url))

        for x in range(tries):
            try:
                return self._create_connection(url, timeout=timeout)
            except Exception as ex:
                logger.exception("Unexpected error on connection try: {}.".format(x))
                logger.exception(ex)
                time.sleep(delay)
                delay = min(delay * 2, 5)

        raise ConnectionError("Could not connect to AltUnityServer on host: {} port: {}".format(self.host, self.port))

    def _create_connection(self, url, timeout=60):
        ws = websocket.WebSocket()
        ws.connect(url, timeout=timeout)

        return ws

    def send(self, data):
        command_name = data.get("commandName")
        if command_name:
            self._current_command_name = command_name

        message = json.dumps(data)
        logger.info("Message: {}".format(message))
        self._websocket.send(json.dumps(data))

    def recv(self):
        if self._store.has(self._current_command_name):
            return self._store.pop(self._current_command_name)

        response = self._websocket.recv()

        if response:
            logger.info("Response: {}".format(response))
            response = json.loads(response)
        else:
            return self.recv()

        command_name = response.get("commandName")
        if self._current_command_name and command_name != self._current_command_name:
            self._store.push(command_name, response)
            return self.recv()

        return response

    def close(self):
        logger.info("Closing connection to AltUnityServer on: {}".format(self.url))
        self._websocket.close()
