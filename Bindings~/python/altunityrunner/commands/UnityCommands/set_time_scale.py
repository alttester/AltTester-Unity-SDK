from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class SetTimeScale(BaseCommand):
    def __init__(self, socket, request_separator, request_end, time_scale):
        super(SetTimeScale, self).__init__(
            socket, request_separator, request_end)
        self.time_scale = time_scale

    def execute(self):
        logger.debug('Set time scale to: ' + str(self.time_scale))
        data = self.send_command(
            'setTimeScale', str(self.time_scale))
        if (data == 'Ok'):
            logger.debug('Time scale set to: ' + str(self.time_scale))
            return data
        return None
