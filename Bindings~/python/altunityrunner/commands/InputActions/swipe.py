from altunityrunner.commands.base_command import BaseCommand


class Swipe(BaseCommand):

    def __init__(self, connection, x_start, y_start, x_end, y_end, duration):
        super().__init__(connection, "multipointSwipe")

        self.x_start = x_start
        self.y_start = y_start
        self.x_end = x_end
        self.y_end = y_end
        self.duration = duration

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "start": self.vector_to_json(self.x_start, self.y_start),
            "end": self.vector_to_json(self.x_end, self.y_end),
            "duration": self.duration,
        })

        return parameters

    def execute(self):
        return self.send()
