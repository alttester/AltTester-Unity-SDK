from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.alt_unity_key_code import AltUnityKeyCode
from altunityrunner.exceptions import InvalidParameterTypeException


class KeyDown(BaseCommand):

    def __init__(self, connection, key_code, power):
        super().__init__(connection, "keyDown")

        if key_code not in AltUnityKeyCode and key_code not in AltUnityKeyCode.names():
            raise InvalidParameterTypeException(
                parameter_name="key_code",
                expected_types=[AltUnityKeyCode],
                received_type=type(key_code)
            )

        self.key_code = key_code
        self.power = power

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyCode": str(self.key_code),
            "power": self.power
        })

        return parameters

    def execute(self):
        return self.send()
