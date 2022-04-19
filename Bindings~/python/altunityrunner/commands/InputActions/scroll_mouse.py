from altunityrunner.commands.base_command import BaseCommand


class Scroll(BaseCommand):

    def __init__(self, connection, speed_vertical, duration, wait, speed_horizontal):
        super(Scroll, self).__init__(connection, "scroll")

        self.speed_vertical = speed_vertical
        self.speed_horizontal = speed_horizontal
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "speed": self.speed_vertical,
            "speedHorizontal": self.speed_horizontal,
            "duration": self.duration,
            "wait": self.wait,
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        if self.wait:
            data = self.recv()
            self.validate_response("Finished", data)

        return data
