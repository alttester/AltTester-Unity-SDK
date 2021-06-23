from altunityrunner.commands.base_command import BaseCommand


class TapCoordinates(BaseCommand):

    def __init__(self, socket, request_separator, request_end, coordinates, count, interval, wait):
        super(TapCoordinates, self).__init__(socket, request_separator, request_end)
        self.coordinates = coordinates
        self.count = count
        self.interval = interval
        self.wait = wait

    def execute(self):
        if isinstance(self.coordinates, dict):
            x = self.coordinates["x"]
            y = self.coordinates["y"]
        else:
            x = self.coordinates[0]
            y = self.coordinates[1]

        position = self.vector_to_json_string(x, y)
        data = self.send_command("tapCoordinates", position, self.count, self.interval, self.wait)

        self.validate_response(data, "Ok")

        if self.wait:
            data = self.recvall()
            self.validate_response(data, "Finished")

        return data
