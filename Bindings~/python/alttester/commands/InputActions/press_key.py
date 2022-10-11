from alttester.commands.base_command import BaseCommand
from alttester.keycode import AltKeyCode
from alttester.exceptions import InvalidParameterTypeException


class PressKey(BaseCommand):

    def __init__(self, connection, key_code, power, duration, wait):
        super().__init__(connection, "pressKeyboardKey")

        if key_code not in AltKeyCode:
            raise InvalidParameterTypeException(
                parameter_name="key_code",
                expected_types=[AltKeyCode],
                received_type=type(key_code)
            )

        self.key_code = key_code
        self.power = power
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyCode": str(self.key_code),
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
