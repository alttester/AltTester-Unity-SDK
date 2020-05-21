from altunityrunner.commands.base_command import BaseCommand
from loguru import logger
import time


class CloseConnection(BaseCommand):
    def __init__(self, socket, request_separator, request_end):
        super(CloseConnection, self).__init__(
            socket, request_separator, request_end)

    def execute(self):
        data = self.send_data(self.create_command('closeConnection'))
        logger.debug('Sent close connection command...')
        time.sleep(1)
        self.socket.close()
        logger.debug('Socket closed.')
