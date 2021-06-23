from altunityrunner.commands.base_command import BaseCommand


class ClickElement(BaseCommand):

    def __init__(self, socket, request_separator, request_end, alt_object, count, interval, wait):
        super(ClickElement, self).__init__(socket, request_separator, request_end)
        self.alt_object = alt_object
        self.count = count
        self.interval = interval
        self.wait = wait

    def execute(self):
        data = self.send_command("clickElement", self.alt_object, self.count, self.interval, self.wait)

        if self.wait:
            self.validate_response("Finished", self.recvall())

        return data
