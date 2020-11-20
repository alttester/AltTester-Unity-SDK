from altunityrunner.commands.command_returning_alt_elements import BaseCommand
from loguru import logger
import json


class GetAllLoadedScenes(BaseCommand):
    def __init__(self, socket, request_separator, request_end):
        super(GetAllLoadedScenes, self).__init__(
            socket, request_separator, request_end)

    def execute(self):
        data = self.send_command('getAllLoadedScenes')
        if (data != '' and 'error:' not in data):
            return json.loads(data)
        return self.handle_errors(data)
