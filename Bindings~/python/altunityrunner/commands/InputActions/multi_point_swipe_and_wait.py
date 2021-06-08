import time

from loguru import logger

from altunityrunner.commands.InputActions.multi_point_swipe import MultipointSwipe
from altunityrunner.commands.base_command import BaseCommand


class MultipointSwipeAndWait(BaseCommand):

    def __init__(self, socket, request_separator, request_end, positions, duration_in_secs):
        super(MultipointSwipeAndWait, self).__init__(socket, request_separator, request_end)
        self.positions = positions
        self.duration_in_secs = duration_in_secs

    def execute(self):
        data = MultipointSwipe(
            self.socket, self.request_separator, self.request_end,
            self.positions, self.duration_in_secs
        ).execute()

        logger.debug("Wait for moving touch to finish")
        time.sleep(self.duration_in_secs)

        swipe_in_progress = True
        while swipe_in_progress:
            swipe_finished = self.send_command("actionFinished")
            if swipe_finished == "Yes":
                break
            elif swipe_finished != "No":
                swipe_in_progress = False

        return data
