from alttester.commands.base_command import BaseCommand
from alttester.keycode import AltKeyCode
from alttester.exceptions import InvalidParameterTypeException


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
            if key_code not in AltKeyCode and key_code not in AltKeyCode.names():
                raise InvalidParameterTypeException(
                    parameter_name="key_code",
                    expected_types=[AltKeyCode],
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
