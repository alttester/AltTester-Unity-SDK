import json

from altunityrunner.commands.command_returning_alt_elements import BaseCommand


class GetAllLoadedScenes(BaseCommand):

    def __init__(self, socket, request_separator, request_end):
        super(GetAllLoadedScenes, self).__init__(socket, request_separator, request_end)

    def execute(self):
        data = self.send_command("getAllLoadedScenes")
        return json.loads(data)
