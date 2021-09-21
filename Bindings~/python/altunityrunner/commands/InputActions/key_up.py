from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.alt_unity_key_code import AltUnityKeyCode
from altunityrunner.altUnityExceptions import InvalidParameterTypeException


class KeyUp(BaseCommand):

    def __init__(self, connection, key_code):
        super().__init__(connection, "keyUp")

        if key_code not in AltUnityKeyCode and key_code not in AltUnityKeyCode.names():
            raise InvalidParameterTypeException()

        self.key_code = key_code

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyCode": str(self.key_code),
        })

        return parameters

    def execute(self):
        return self.send()
