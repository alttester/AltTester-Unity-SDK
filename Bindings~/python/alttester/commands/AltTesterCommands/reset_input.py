from alttester.commands.base_command import BaseCommand
from alttester.logging import AltLogger, AltLogLevel
from alttester.exceptions import InvalidParameterTypeException


class ResetInput(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "resetInput")

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        return data
