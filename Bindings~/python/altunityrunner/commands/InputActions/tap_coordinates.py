from altunityrunner.commands.base_command import validate_coordinates, BaseCommand


class TapCoordinates(BaseCommand):

    def __init__(self, connection, coordinates, count, interval, wait):
        super().__init__(connection, "tapCoordinates")

        self.coordinates = validate_coordinates(coordinates)
        self.count = count
        self.interval = interval
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "coordinates": self.coordinates,
            "count": self.count,
            "interval": self.interval,
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
