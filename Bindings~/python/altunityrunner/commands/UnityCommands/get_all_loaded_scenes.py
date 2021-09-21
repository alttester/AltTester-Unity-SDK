from altunityrunner.commands.base_command import BaseCommand


class GetAllLoadedScenes(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "getAllLoadedScenes")

    def execute(self):
        return self.send()
