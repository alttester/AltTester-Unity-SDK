from altunityrunner.commands.base_command import BaseCommand


class GetAllComponents(BaseCommand):

    def __init__(self, connection, alt_object):
        super(GetAllComponents, self).__init__(connection, "getAllComponents")

        self.alt_object = alt_object

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altUnityObjectId": self.alt_object.id
        })

        return parameters

    def execute(self):
        return self.send()
