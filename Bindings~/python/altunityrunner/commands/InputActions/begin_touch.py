from altunityrunner.commands.base_command import BaseCommand


class BeginTouch(BaseCommand):

    def __init__(self, connection, coordinates):
        super().__init__(connection, "beginTouch")

        if isinstance(coordinates, dict):
            self.coordinates = coordinates
        else:
            self.coordinates = {
                "x": coordinates[0],
                "y": coordinates[1]
            }

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "coordinates": self.coordinates
        })

        return parameters

    def execute(self):
        return int(self.send())
