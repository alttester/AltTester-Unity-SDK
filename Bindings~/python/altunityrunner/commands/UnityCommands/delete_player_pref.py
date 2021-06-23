from altunityrunner.commands.base_command import BaseCommand


class DeletePlayerPref(BaseCommand):

    def __init__(self, socket, request_separator, request_end):
        super(DeletePlayerPref, self).__init__(socket, request_separator, request_end)

    def execute(self):
        return self.send_command("deletePlayerPref")
