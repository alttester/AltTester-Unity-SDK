import time

from loguru import logger

from altunityrunner.commands.base_command import Command
from altunityrunner.commands.InputActions.swipe import Swipe
from altunityrunner.commands.InputActions.action_finished import ActionFinished


class SwipeAndWait(Command):

    def __init__(self, connection, x_start, y_start, x_end, y_end, duration_in_secs):
        self.connection = connection

        self.x_start = x_start
        self.y_start = y_start
        self.x_end = x_end
        self.y_end = y_end
        self.duration_in_secs = duration_in_secs

    def execute(self):
        data = Swipe.run(
            self.connection,
            self.x_start, self.y_start, self.x_end, self.y_end, self.duration_in_secs
        )

        logger.debug("Wait for swipe to finish")
        time.sleep(self.duration_in_secs)

        swipe_in_progress = True
        while swipe_in_progress:
            swipe_finished = ActionFinished.run(self.connection)

            if swipe_finished == "Yes":
                break
            elif swipe_finished != "No":
                swipe_in_progress = False

        return data
