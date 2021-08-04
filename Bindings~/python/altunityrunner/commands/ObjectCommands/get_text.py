from altunityrunner.commands.base_command import BaseCommand


class GetText(BaseCommand):

    def __init__(self, connection, alt_object):
        super().__init__(connection, "getText")

        self.alt_object = alt_object

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altUnityObject": self.alt_object.to_json(),
        })

        return parameters

    def execute(self):
        return self.send()
