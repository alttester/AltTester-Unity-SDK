from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import NotFoundException


class TapAtCoordinates(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end, x, y):
        super(TapAtCoordinates, self).__init__(socket, request_separator, request_end)
        self.x = x
        self.y = y

    def execute(self):
        try:
            data = self.send_command("tapScreen", self.x, self.y)
            return self.get_alt_element(data)
        except NotFoundException:
            return None
