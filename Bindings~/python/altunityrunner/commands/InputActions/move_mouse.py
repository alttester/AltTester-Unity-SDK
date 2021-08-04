from altunityrunner.commands.base_command import BaseCommand


class MoveMouse(BaseCommand):

    def __init__(self, connection, x, y, duration):
        super().__init__(connection, "moveMouse")

        self.x = x
        self.y = y
        self.duration = duration

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "location": self.vector_to_json(self.x, self.y),
            "duration": self.duration
        })

        return parameters

    def execute(self):
        return self.send()
