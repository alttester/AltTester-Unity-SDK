from altunityrunner.commands.base_command import BaseCommand


class MultipointSwipe(BaseCommand):

    def __init__(self, socket, request_separator, request_end, positions, duration_in_secs):
        super(MultipointSwipe, self).__init__(socket, request_separator, request_end)
        self.positions = positions
        self.duration_in_secs = str(duration_in_secs)

    def execute(self):
        moving_position = self.positions_to_json_string(self.positions)
        return self.send_command("multipointSwipeChain", self.duration_in_secs, moving_position)
