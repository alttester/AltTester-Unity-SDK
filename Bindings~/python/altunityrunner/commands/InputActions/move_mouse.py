from altunityrunner.commands.base_command import BaseCommand


class MoveMouse(BaseCommand):

    def __init__(self, socket, request_separator, request_end, x, y, duration):
        super(MoveMouse, self).__init__(socket, request_separator, request_end)
        self.x = x
        self.y = y
        self.duration = duration

    def execute(self):
        location = self.vector_to_json_string(self.x, self.y)
        return self.send_command("moveMouse", location, self.duration)
