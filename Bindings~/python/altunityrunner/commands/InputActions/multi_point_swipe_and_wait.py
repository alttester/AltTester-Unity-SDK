import time

from loguru import logger

from altunityrunner.commands.InputActions.multi_point_swipe import MultipointSwipe
from altunityrunner.commands.InputActions.action_finished import ActionFinished
from altunityrunner.commands.base_command import Command


class MultipointSwipeAndWait(Command):

    def __init__(self, connection, positions, duration_in_secs):
        self.connection = connection

        self.positions = positions
        self.duration_in_secs = duration_in_secs

    def execute(self):
        data = MultipointSwipe.run(
            self.connection,
            self.positions, self.duration_in_secs
        )

        logger.debug("Wait for moving touch to finish")
        time.sleep(self.duration_in_secs)

        swipe_in_progress = True
        while swipe_in_progress:
            swipe_finished = ActionFinished.run(self.connection)

            if swipe_finished == "Yes":
                break
            elif swipe_finished != "No":
                swipe_in_progress = False

        return data
