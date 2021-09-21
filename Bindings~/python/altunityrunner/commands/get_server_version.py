from altunityrunner.commands.base_command import BaseCommand


class GetServerVersion(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "getServerVersion")

    def execute(self):
        return self.send()
