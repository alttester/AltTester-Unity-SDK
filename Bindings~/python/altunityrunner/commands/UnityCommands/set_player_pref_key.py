from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.player_pref_key_type import PlayerPrefKeyType
from altunityrunner.exceptions import InvalidParameterTypeException


class SetPlayerPrefKey(BaseCommand):

    def __init__(self, connection, key_name, value, key_type):
        super().__init__(connection, "setKeyPlayerPref")

        if key_type not in PlayerPrefKeyType and key_type not in PlayerPrefKeyType.values():
            raise InvalidParameterTypeException(
                parameter_name="key_type",
                expected_types=[PlayerPrefKeyType],
                received_type=type(key_type)
            )

        self.key_name = key_name
        self.value = value
        self.key_type = key_type

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyName": self.key_name,
            "keyType": str(self.key_type)
        })

        if self.key_type in [PlayerPrefKeyType.Int, PlayerPrefKeyType.Int.value]:
            parameters["intValue"] = self.value
        elif self.key_type in [PlayerPrefKeyType.Float, PlayerPrefKeyType.Float.value]:
            parameters["floatValue"] = self.value
        else:
            parameters["stringValue"] = self.value

        return parameters

    def execute(self):
        return self.send()
