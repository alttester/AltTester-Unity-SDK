from altunityrunner.commands.base_command import validate_coordinates, BaseCommand


class BeginTouch(BaseCommand):

    def __init__(self, connection, coordinates):
        super().__init__(connection, "beginTouch")

        self.coordinates = validate_coordinates(coordinates)

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "coordinates": self.coordinates
        })

        return parameters

    def execute(self):
        return int(self.send())
