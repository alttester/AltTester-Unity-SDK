from altunityrunner.commands.base_command import BaseCommand


class ClickEvent(BaseCommand):

    def __init__(self, socket, request_separator, request_end, alt_object):
        super(ClickEvent, self).__init__(socket, request_separator, request_end)
        self.alt_object = alt_object

    def execute(self):
        return self.send_command("clickEvent", self.alt_object)
