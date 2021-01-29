from unittest.mock import MagicMock
from unittest import TestCase
from altunityrunner.commands.get_server_version import GetServerVersion

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