from altunityrunner.commands.base_command import BaseCommand


class MoveTouch(BaseCommand):

    def __init__(self, socket, request_separator, request_end, finger_id, coordinates):
        super(MoveTouch, self).__init__(socket, request_separator, request_end)
        self.finger_id = finger_id
        self.coordinates = coordinates

    def execute(self):
        if isinstance(self.coordinates, dict):
            x = self.coordinates["x"]
            y = self.coordinates["y"]
        else:
            x = self.coordinates[0]
            y = self.coordinates[1]

        position = self.vector_to_json_string(x, y)
        data = self.send_command("moveTouch", self.finger_id, position)
        self.validate_response(data, "Ok")
