from altunityrunner.commands.base_command import validate_coordinates, BaseCommand


class MoveMouse(BaseCommand):

    def __init__(self, connection, coordinates, duration, wait):
        super().__init__(connection, "moveMouse")

        self.coordinates = validate_coordinates(coordinates)
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "coordinates": self.coordinates,
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
