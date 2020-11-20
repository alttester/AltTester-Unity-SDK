from altunityrunner.commands.base_command import BaseCommand


class Tap(BaseCommand):
    def __init__(self, socket, request_separator, request_end, alt_object, count):
        super(Tap, self).__init__(socket, request_separator, request_end)
        self.alt_object = alt_object
        self.count = count

    def execute(self):
        data = self.send_command('tapObject', self.alt_object, self.count)
        return self.handle_errors(data)
