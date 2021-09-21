from altunityrunner.commands.base_command import BaseCommand


class Tilt(BaseCommand):

    def __init__(self, connection, x, y, z, duration):
        super().__init__(connection, "tilt")

        self.x = x
        self.y = y
        self.z = z
        self.duration = duration

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "acceleration": self.vector_to_json(self.x, self.y, self.z),
            "duration": self.duration,
        })

        return parameters

    def execute(self):
        return self.send()
