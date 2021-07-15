from altunityrunner.commands.base_command import BaseCommand


class BeginTouch(BaseCommand):

    def __init__(self, socket, request_separator, request_end, coordinates):
        super(BeginTouch, self).__init__(socket, request_separator, request_end)
        self.coordinates = coordinates

    def execute(self):
        if isinstance(self.coordinates, dict):
            x = self.coordinates["x"]
            y = self.coordinates["y"]
        else:
            x = self.coordinates[0]
            y = self.coordinates[1]

        position = self.vector_to_json_string(x, y)
        data = self.send_command("beginTouch", position)
        return int(data)
