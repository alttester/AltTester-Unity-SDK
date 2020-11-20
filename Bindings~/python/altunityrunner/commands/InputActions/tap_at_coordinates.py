from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements


class TapAtCoordinates(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end, x, y):
        super(TapAtCoordinates, self).__init__(
            socket, request_separator, request_end)
        self.x = x
        self.y = y

    def execute(self):
        data = self.send_command('tapScreen', self.x, self.y)
        if 'error:notFound' in data:
            return None
        return self.get_alt_element(data)
