from altunityrunner.commands.base_command import validate_coordinates, BaseCommand


class MoveTouch(BaseCommand):

    def __init__(self, connection, finger_id, coordinates):
        super().__init__(connection, "moveTouch")

        self.coordinates = validate_coordinates(coordinates)
        self.finger_id = finger_id

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "fingerId": self.finger_id,
            "position": self.coordinates
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)
