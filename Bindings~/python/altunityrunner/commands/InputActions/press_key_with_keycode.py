from altunityrunner.commands.base_command import BaseCommand


class PressKeyWithKeyCode(BaseCommand):

    def __init__(self, socket, request_separator, request_end, keyCode, power, duration):
        super(PressKeyWithKeyCode, self).__init__(socket, request_separator, request_end)
        self.power = power
        self.duration = duration
        self.keyCode = keyCode

    def execute(self):
        return self.send_command("pressKeyboardKey", self.keyCode.name, self.power, self.duration)
