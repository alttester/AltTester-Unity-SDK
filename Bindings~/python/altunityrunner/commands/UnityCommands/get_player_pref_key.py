from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.player_pref_key_type import PlayerPrefKeyType
from altunityrunner.exceptions import InvalidParameterTypeException


class GetPlayerPrefKey(BaseCommand):

    def __init__(self, connection, key_name, key_type):
        super().__init__(connection, "getKeyPlayerPref")

        if key_type not in PlayerPrefKeyType and key_type not in PlayerPrefKeyType.value():
            raise InvalidParameterTypeException(
                parameter_name="key_type",
                expected_types=[PlayerPrefKeyType],
                received_type=type(key_type)
            )

        self.key_name = key_name
        self.key_type = key_type

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyName": self.key_name,
            "keyType": str(self.key_type)
        })

        return parameters

    def execute(self):
        return self.send()
