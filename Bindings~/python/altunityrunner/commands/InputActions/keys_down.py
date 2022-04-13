from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.alt_unity_key_code import AltUnityKeyCode
from altunityrunner.exceptions import InvalidParameterTypeException


class KeysDown(BaseCommand):

    def __init__(self, connection, key_codes, power):
        super().__init__(connection, "keysDown")

        if not isinstance(key_codes, (list, tuple)):
            raise InvalidParameterTypeException(
                parameter_name="key_codes",
                expected_types=(list, tuple),
                received_type=type(key_codes)
            )

        for key_code in key_codes:
            if key_code not in AltUnityKeyCode and key_code not in AltUnityKeyCode.names():
                raise InvalidParameterTypeException(
                    parameter_name="key_code",
                    expected_types=[AltUnityKeyCode],
                    received_type=type(key_code)
                )

        self.key_codes = key_codes
        self.power = power

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyCodes": [str(key_code) for key_code in self.key_codes],
            "power": self.power
        })

        return parameters

    def execute(self):
        return self.send()
