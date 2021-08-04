from altunityrunner.commands.base_command import BaseCommand


class ScrollMouse(BaseCommand):

    def __init__(self, connection, speed, duration):
        super(ScrollMouse, self).__init__(connection, "scrollMouse")

        self.speed = speed
        self.duration = duration

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "speed": self.speed,
            "duration": self.duration,
        })

        return parameters

    def execute(self):
        return self.send()
