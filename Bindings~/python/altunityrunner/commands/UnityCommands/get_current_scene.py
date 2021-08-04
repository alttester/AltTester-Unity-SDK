from altunityrunner.commands.base_command import BaseCommand


class GetCurrentScene(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "getCurrentScene")

    def execute(self):
        data = self.send()
        return data.get("name", "")
