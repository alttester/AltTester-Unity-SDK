from loguru import logger

from altunityrunner.commands.base_command import BaseCommand


class ClickAtCoordinates(BaseCommand):

    def __init__(self, socket, request_separator, request_end, x, y):
        super(ClickAtCoordinates, self).__init__(socket, request_separator, request_end)
        self.x = x
        self.y = y

    def execute(self):
        data = self.send_command("clickScreenOnXY", self.x, self.y)
        logger.debug("Clicked at {}, {}".format(self.x, self.y))

        return data
