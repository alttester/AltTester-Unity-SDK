from unittest.mock import MagicMock
from unittest import TestCase
from altunityrunner.commands.get_server_version import GetServerVersion


class CommandsTests(TestCase):
    def test_GetServerVersion(self):
        socket = MagicMock()

        def send(message):
            self.assertEqual(b"getServerVersion;&", message)

        def recv(buffer_size):
            return b"altstart::1.6.0::altLog::::altend"
        socket.send.side_effect = send
        socket.recv.side_effect = recv
        version = GetServerVersion(socket, ';', '&').execute()
        self.assertEqual("1.6.0", version)
