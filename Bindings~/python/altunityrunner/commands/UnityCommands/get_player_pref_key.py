from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.player_pref_key_type import PlayerPrefKeyType
from loguru import logger


class GetPlayerPrefKey(BaseCommand):
    def __init__(self, socket, request_separator, request_end, key_name, key_type):
        super(GetPlayerPrefKey, self).__init__(
            socket, request_separator, request_end)
        self.key_name = key_name
        self.key_type = key_type

    def execute(self):
        data = ''
        if self.key_type == 1:
            logger.debug('Get Int Player Pref for key: ' + self.key_name)
            data = self.send_command(
                'getKeyPlayerPref', self.key_name, str(PlayerPrefKeyType.Int))
        if self.key_type == 2:
            logger.debug('Get String Player Pref for key: ' + self.key_name)
            data = self.send_command(
                'getKeyPlayerPref', self.key_name, str(PlayerPrefKeyType.String))
        if self.key_type == 3:
            logger.debug('Get Float Player Pref for key: ' + self.key_name)
            data = self.send_command(
                'getKeyPlayerPref', self.key_name, str(PlayerPrefKeyType.Float))
        return self.handle_errors(data)
