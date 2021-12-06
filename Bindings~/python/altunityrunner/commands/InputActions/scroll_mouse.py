from altunityrunner.commands.base_command import BaseCommand


class Scroll(BaseCommand):

    def __init__(self, connection, speed, duration, wait):
        super(Scroll, self).__init__(connection, "scroll")

        self.speed = speed
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "speed": self.speed,
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
