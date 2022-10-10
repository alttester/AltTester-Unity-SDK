from alttester.commands.base_command import BaseCommand
from alttester.keycode import AltKeyCode
from alttester.exceptions import InvalidParameterTypeException


class PressKeys(BaseCommand):

    def __init__(self, connection, key_codes, power, duration, wait):
        super().__init__(connection, "pressKeyboardKeys")

        if not isinstance(key_codes, (list, tuple)):
            raise InvalidParameterTypeException(
                parameter_name="key_codes",
                expected_types=(list, tuple),
                received_type=type(key_codes)
            )

        for key_code in key_codes:
            if key_code not in AltKeyCode:
                raise InvalidParameterTypeException(
                    parameter_name="key_codes",
                    expected_types=[AltKeyCode],
                    received_type=type(key_code)
                )

        self.key_codes = key_codes
        self.power = power
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyCodes": [str(key_code) for key_code in self.key_codes],
            "power": self.power,
            "duration": self.duration,
            "wait": self.wait,
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        if self.wait:
            data = self.recv()
            self.validate_response("Finished", data)

        return data
