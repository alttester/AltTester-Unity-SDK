from alttester.commands.base_command import BaseCommand


class ResetInput(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "resetInput")

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        return data
