from altunityrunner.commands.base_command import BaseCommand


class EndTouch(BaseCommand):

    def __init__(self, socket, request_separator, request_end, finger_id):
        super(EndTouch, self).__init__(socket, request_separator, request_end)
        self.finger_id = finger_id

    def execute(self):
        data = self.send_command("endTouch", self.finger_id)
        self.validate_response(data, "Ok")
