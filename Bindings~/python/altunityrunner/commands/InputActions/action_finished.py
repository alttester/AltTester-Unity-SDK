from altunityrunner.commands.base_command import BaseCommand


class ActionFinished(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "actionFinished")

    def execute(self):
        return self.send()
