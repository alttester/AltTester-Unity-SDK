from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class GetTimeScale(BaseCommand):
    def __init__(self, socket, request_separator, request_end):
        super(GetTimeScale, self).__init__(
            socket, request_separator, request_end)

    def execute(self):
        logger.debug('Get time scale')
        data = self.send_command('getTimeScale')
        if (data != '' and 'error:' not in data):
            logger.debug('Got time scale: ' + data)
            return float(data)
        return None
