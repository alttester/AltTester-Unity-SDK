from altunityrunner.commands.base_command import BaseCommand


class SetText(BaseCommand):

    def __init__(self, socket, request_separator, request_end, text_value, alt_object):
        super(SetText, self).__init__(socket, request_separator, request_end)
        self.alt_object = alt_object
        self.text = text_value

    def execute(self):
        return self.send_command("setText", self.alt_object, self.text)
