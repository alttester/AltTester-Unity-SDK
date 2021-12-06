from altunityrunner.commands.base_command import validate_coordinates_3, BaseCommand


class Tilt(BaseCommand):

    def __init__(self, connection, acceleration, duration, wait):
        super().__init__(connection, "tilt")

        self.acceleration = validate_coordinates_3(acceleration)
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "acceleration": self.acceleration,
            "duration": self.duration,
            "wait": self.wait
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        if self.wait:
            data = self.recv()
            self.validate_response("Finished", data)

        return data
