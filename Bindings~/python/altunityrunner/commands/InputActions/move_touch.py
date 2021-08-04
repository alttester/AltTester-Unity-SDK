from altunityrunner.commands.base_command import BaseCommand


class MoveTouch(BaseCommand):

    def __init__(self, connection, finger_id, coordinates):
        super().__init__(connection, "moveTouch")

        if isinstance(coordinates, dict):
            self.coordinates = coordinates
        else:
            self.coordinates = {
                "x": coordinates[0],
                "y": coordinates[1]
            }

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
