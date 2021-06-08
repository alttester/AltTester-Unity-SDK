from altunityrunner.commands.base_command import BaseCommand


class Tilt(BaseCommand):

    def __init__(self, socket, request_separator, request_end, x, y, z, duration_in_seconds):
        super(Tilt, self).__init__(socket, request_separator, request_end)
        self.x = x
        self.y = y
        self.z = z
        self.duration_in_seconds = duration_in_seconds

    def execute(self):
        acceleration = self.vector_to_json_string(self.x, self.y, self.z)
        return self.send_command("tilt", acceleration, str(self.duration_in_seconds))
