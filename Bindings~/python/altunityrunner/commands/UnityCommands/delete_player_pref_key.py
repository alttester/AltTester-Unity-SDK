from altunityrunner.commands.base_command import BaseCommand


class DeletePlayerPrefKey(BaseCommand):

    def __init__(self, socket, request_separator, request_end, key_name):
        super(DeletePlayerPrefKey, self).__init__(socket, request_separator, request_end)
        self.key_name = key_name

    def execute(self):
        return self.send_command("deleteKeyPlayerPref", self.key_name)
