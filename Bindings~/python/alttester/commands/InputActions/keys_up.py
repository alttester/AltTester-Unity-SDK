from alttester.commands.base_command import BaseCommand
from alttester.keycode import AltKeyCode
from alttester.exceptions import InvalidParameterTypeException


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
            if key_code not in AltKeyCode and key_code not in AltKeyCode.names():
                raise InvalidParameterTypeException(
                    parameter_name="key_codes",
                    expected_types=[AltKeyCode],
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
