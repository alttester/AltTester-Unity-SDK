from altunityrunner.commands.base_command import BaseCommand


class ScrollMouse(BaseCommand):

    def __init__(self, socket, request_separator, request_end, speed, duration):
        super(ScrollMouse, self).__init__(socket, request_separator, request_end)
        self.speed = speed
        self.duration = duration

    def execute(self):
        return self.send_command("scrollMouse", self.speed, self.duration)
