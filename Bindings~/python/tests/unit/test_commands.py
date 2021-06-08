from unittest.mock import MagicMock
from unittest import TestCase

from altunityrunner.commands import GetServerVersion, SetServerLogging
from altunityrunner.altUnityExceptions import InvalidParameterTypeException, AltUnityInvalidServerResponse
from altunityrunner.logging import AltUnityLogLevel, AltUnityLogger


class CommandsTests(TestCase):

    def test_GetServerVersion(self):
        socket = MagicMock()
        command = GetServerVersion(socket, ';', '&')

        def send(message):

            self.assertTrue(message.decode(
                "utf-8").endswith(";getServerVersion&"), message.decode("utf-8"))

        def recv(buffer_size):
            return ("altstart::"+command.messageId+"::response::1.6.1::altLog::::altend").encode('utf-8')
        socket.send.side_effect = send
        socket.recv.side_effect = recv
        version = command.execute()
        self.assertEqual("1.6.1", version)

    def test_set_server_logging_invalid_parameter(self):
        socket = MagicMock()
        try:
            command = SetServerLogging(socket, ';', '&', "a", "b")
            self.fail()
        except InvalidParameterTypeException:
            pass

    def test_set_server_logging_success(self):
        socket = MagicMock()
        command = SetServerLogging(
            socket, ';', '&', AltUnityLogger.Unity, AltUnityLogLevel.Debug)

        def send(message):

            self.assertTrue(message.decode(
                "utf-8").endswith(";setServerLogging;Unity;Debug&"), message.decode("utf-8"))

        def recv(buffer_size):
            return ("altstart::"+command.messageId+"::response::Ok::altLog::::altend").encode('utf-8')
        socket.send.side_effect = send
        socket.recv.side_effect = recv
        command.execute()

    def test_set_server_logging_invalid_response(self):
        socket = MagicMock()
        command = SetServerLogging(
            socket, ';', '&', AltUnityLogger.Console, AltUnityLogLevel.Trace)

        def send(message):

            self.assertTrue(message.decode(
                "utf-8").endswith(";setServerLogging;Console;Trace&"), message.decode("utf-8"))

        def recv(buffer_size):
            return ("altstart::"+command.messageId+"::response::notok::altLog::::altend").encode('utf-8')
        socket.send.side_effect = send
        socket.recv.side_effect = recv
        try:
            command.execute()
            self.fail()
        except AltUnityInvalidServerResponse:
            pass
