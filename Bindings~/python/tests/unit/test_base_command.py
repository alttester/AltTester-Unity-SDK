from unittest.mock import MagicMock
from unittest import TestCase

from altunityrunner.commands.base_command import BaseCommand


class DoubleMessageCommand(BaseCommand):

    def __init__(self, socket, request_separator, request_end):
        super(DoubleMessageCommand, self).__init__(socket, request_separator, request_end)

    def execute(self):
        data = self.send_command("doublemessage")
        return data + "___" + self.recvall()


class BaseCommandTests(TestCase):
    def test_set_server_logging_invalid_response(self):
        socket = MagicMock()
        command = DoubleMessageCommand(
            socket, ';', '&')

        def send(message):

            self.assertTrue(message.decode(
                "utf-8").endswith(";doublemessage&"), message.decode("utf-8"))

        def recv(buffer_size):
            return ("altstart::"+command.messageId+"::response::part1::altLog::::altendaltstart::"+command.messageId+"::response::part2::altLog::::altend").encode('utf-8')
        socket.send.side_effect = send
        socket.recv.side_effect = recv
        response = command.execute()
        self.assertEqual(response, "part1___part2")
