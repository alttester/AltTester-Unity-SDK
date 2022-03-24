from altunityrunner.commands.base_command import validate_coordinates, BaseCommand


class FindObjectAtCoordinates(BaseCommand):

    def __init__(self, connection, coordinates):
        super().__init__(connection, "findObjectAtCoordinates")

        self.coordinates = validate_coordinates(coordinates)

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "coordinates": self.coordinates,
        })

        return parameters

    def execute(self):
        return self.send()
