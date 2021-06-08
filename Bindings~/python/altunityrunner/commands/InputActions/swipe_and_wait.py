import time

from loguru import logger

from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.commands.InputActions.swipe import Swipe


class SwipeAndWait(BaseCommand):

    def __init__(self, socket, request_separator, request_end, x_start, y_start, x_end, y_end, duration_in_secs):
        super(SwipeAndWait, self).__init__(socket, request_separator, request_end)
        self.x_start = x_start
        self.y_start = y_start
        self.x_end = x_end
        self.y_end = y_end
        self.duration_in_secs = duration_in_secs

    def execute(self):
        data = Swipe(
            self.socket, self.request_separator, self.request_end,
            self.x_start, self.y_start, self.x_end, self.y_end, self.duration_in_secs
        ).execute()

        logger.debug("Wait for swipe to finish")
        time.sleep(self.duration_in_secs)

        swipe_in_progress = True
        while swipe_in_progress:
            swipe_finished = self.send_command("actionFinished")
            if swipe_finished == "Yes":
                break
            elif swipe_finished != "No":
                swipe_in_progress = False

        return data
