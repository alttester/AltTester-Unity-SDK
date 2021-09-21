from altunityrunner.commands.base_command import BaseCommand


class ClickCoordinates(BaseCommand):

    def __init__(self, connection, coordinates, count, interval, wait):
        super().__init__(connection, "clickCoordinates")

        if isinstance(coordinates, dict):
            self.coordinates = coordinates
        else:
            self.coordinates = {
                "x": coordinates[0],
                "y": coordinates[1]
            }

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
