from altunityrunner.commands.base_command import BaseCommand


class KeyDown(BaseCommand):

    def __init__(self, socket, request_separator, request_end, keyCode, power):
        super(KeyDown, self).__init__(socket, request_separator, request_end)
        self.keyCode = keyCode
        self.power = power

    def execute(self):
        return self.send_command("keyDown", self.keyCode.name, self.power)
