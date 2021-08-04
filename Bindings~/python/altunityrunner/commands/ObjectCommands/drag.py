from altunityrunner.commands.base_command import BaseCommand


class Drag(BaseCommand):

    def __init__(self, connection, x, y, alt_object):
        super().__init__(connection, "dragObject")

        self.alt_object = alt_object

        self.x = x
        self.y = y

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altUnityObject": self.alt_object.to_json(),
            "position": self.vector_to_json(self.x, self.y)
        })

        return parameters

    def execute(self):
        return self.send()
