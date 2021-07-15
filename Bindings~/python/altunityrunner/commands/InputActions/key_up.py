from altunityrunner.commands.base_command import BaseCommand


class KeyUp(BaseCommand):

    def __init__(self, socket, request_separator, request_end, keyCode):
        super(KeyUp, self).__init__(socket, request_separator, request_end)
        self.keyCode = keyCode

    def execute(self):
        return self.send_command("keyUp", self.keyCode.name)
