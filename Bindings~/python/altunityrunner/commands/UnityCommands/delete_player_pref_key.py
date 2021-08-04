from altunityrunner.commands.base_command import BaseCommand


class DeletePlayerPrefKey(BaseCommand):

    def __init__(self, connection, key_name):
        super().__init__(connection, "deleteKeyPlayerPref")

        self.key_name = key_name

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyName": self.key_name,
        })

        return parameters

    def execute(self):
        return self.send()
