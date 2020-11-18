from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class DeletePlayerPrefKey(BaseCommand):
    def __init__(self, socket, request_separator, request_end, key_name):
        super(DeletePlayerPrefKey, self).__init__(
            socket, request_separator, request_end)
        self.key_name = key_name

    def execute(self):
        logger.debug('Delete Player Pref for key: ' + self.key_name)
        data = self.send_command(
            'deleteKeyPlayerPref', self.key_name)
        return self.handle_errors(data)
