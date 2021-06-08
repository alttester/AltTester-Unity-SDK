from loguru import logger

from altunityrunner.commands.base_command import BaseCommand


class SetTimeScale(BaseCommand):

    def __init__(self, socket, request_separator, request_end, time_scale):
        super(SetTimeScale, self).__init__(socket, request_separator, request_end)
        self.time_scale = time_scale

    def execute(self):
        logger.debug("Set time scale to: {}".format(self.time_scale))
        data = self.send_command("setTimeScale", str(self.time_scale))

        if (data == "Ok"):
            logger.debug("Time scale set to: {}".format(self.time_scale))
            return data

        return None
