from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.alt_unity_key_code import AltUnityKeyCode
from altunityrunner.exceptions import InvalidParameterTypeException


class KeysUp(BaseCommand):

    def __init__(self, connection, key_codes):
        super().__init__(connection, "keysUp")

        if not isinstance(key_codes, (list, tuple)):
            raise InvalidParameterTypeException(
                parameter_name="key_codes",
                expected_types=(list, tuple),
                received_type=type(key_codes)
            )

        for key_code in key_codes:
            if key_code not in AltUnityKeyCode and key_code not in AltUnityKeyCode.names():
                raise InvalidParameterTypeException(
                    parameter_name="key_codes",
                    expected_types=[AltUnityKeyCode],
                    received_type=type(key_code)
                )

        self.key_codes = key_codes

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyCodes": [str(key_code) for key_code in self.key_codes],
        })

        return parameters

    def execute(self):
        return self.send()
