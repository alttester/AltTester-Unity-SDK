from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class ClickAtCoordinates(BaseCommand):
    def __init__(self, socket, request_separator, request_end, x, y):
        super(ClickAtCoordinates, self).__init__(
            socket, request_separator, request_end)
        self.x = x
        self.y = y

    def execute(self):
        data = self.send_command(
            "clickScreenOnXY", self.x, self.y)
        logger.debug('Clicked at ' + str(self.x) + ', ' + str(self.y))
        return data
