import time

from loguru import logger

from altunityrunner.commands.base_command import Command
from altunityrunner.commands.InputActions.action_finished import ActionFinished
from altunityrunner.commands.InputActions.scroll_mouse import ScrollMouse


class ScrollMouseAndWait(Command):

    def __init__(self, connection, speed, duration):
        self.connection = connection

        self.speed = speed
        self.duration = duration

    def execute(self):
        data = ScrollMouse.run(
            self.connection,
            self.speed, self.duration
        )

        logger.debug("Wait for scroll mouse to finish")
        time.sleep(self.duration)

        action_in_progress = True
        while action_in_progress:
            action_finished = ActionFinished.run(self.connection)

            if action_finished == "Yes":
                break
            elif action_finished != "No":
                action_in_progress = False

        return data
