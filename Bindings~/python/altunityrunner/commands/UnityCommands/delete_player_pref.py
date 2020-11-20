from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class DeletePlayerPref(BaseCommand):
    def __init__(self, socket, request_separator, request_end):
        super(DeletePlayerPref, self).__init__(
            socket, request_separator, request_end)

    def execute(self):
        logger.debug('Delete all Player Prefs')
        data = self.send_command('deletePlayerPref')
        return self.handle_errors(data)
