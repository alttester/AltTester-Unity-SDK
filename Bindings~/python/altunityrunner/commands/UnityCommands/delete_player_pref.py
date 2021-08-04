from altunityrunner.commands.base_command import BaseCommand


class DeletePlayerPref(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "deletePlayerPref")

    def execute(self):
        return self.send()
