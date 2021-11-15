from loguru import logger

from altunityrunner.commands.base_command import BaseCommand


class GetTimeScale(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "getTimeScale")

    def execute(self):
        logger.debug("Get time scale")
        data = self.send()

        logger.debug("Got time scale: {}", data)
        return float(data)
