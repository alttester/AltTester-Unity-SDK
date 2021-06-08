from altunityrunner.commands.base_command import BaseCommand


class PointerExit(BaseCommand):

    def __init__(self, socket, request_separator, request_end, alt_object):
        super(PointerExit, self).__init__(socket, request_separator, request_end)
        self.alt_object = alt_object

    def execute(self):
        return self.send_command("pointerExitObject", self.alt_object)
