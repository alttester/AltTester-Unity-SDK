from altunityrunner.commands.base_command import BaseCommand


class PressKey(BaseCommand):

    def __init__(self, socket, request_separator, request_end, keyName, power, duration):
        super(PressKey, self).__init__(socket, request_separator, request_end)
        self.keyName = keyName
        self.power = power
        self.duration = duration

    def execute(self):
        return self.send_command("pressKeyboardKey", self.keyName, self.power, self.duration)
