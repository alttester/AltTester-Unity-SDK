from altunityrunner.commands.base_command import BaseCommand
import time


class ScrollMouse(BaseCommand):
    def __init__(self, socket, request_separator, request_end, speed, duration):
        super(ScrollMouse, self).__init__(
            socket, request_separator, request_end)
        self.speed = speed
        self.duration = duration

    def execute(self):
        print('Scroll mouse with: ' + str(self.speed))
        data = self.send_command('scrollMouse', self.speed, self.duration)
        return self.handle_errors(data)
