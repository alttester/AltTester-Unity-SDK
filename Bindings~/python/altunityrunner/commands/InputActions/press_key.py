from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.alt_unity_key_code import AltUnityKeyCode
from altunityrunner.exceptions import InvalidParameterTypeException


class PressKey(BaseCommand):

    def __init__(self, connection, key_code, power, duration, wait):
        super().__init__(connection, "pressKeyboardKey")

        if key_code not in AltUnityKeyCode:
            raise InvalidParameterTypeException(
                parameter_name="key_code",
                expected_types=[AltUnityKeyCode],
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
