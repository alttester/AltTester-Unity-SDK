from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.player_pref_key_type import PlayerPrefKeyType


class GetPlayerPrefKey(BaseCommand):

    def __init__(self, socket, request_separator, request_end, key_name, key_type):
        super(GetPlayerPrefKey, self).__init__(socket, request_separator, request_end)
        self.key_name = key_name
        self.key_type = key_type

    def execute(self):
        data = ""

        if self.key_type == 1:
            data = self.send_command("getKeyPlayerPref", self.key_name, str(PlayerPrefKeyType.Int))
        if self.key_type == 2:
            data = self.send_command("getKeyPlayerPref", self.key_name, str(PlayerPrefKeyType.String))
        if self.key_type == 3:
            data = self.send_command("getKeyPlayerPref", self.key_name, str(PlayerPrefKeyType.Float))

        return data
