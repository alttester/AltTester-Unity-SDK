import time

from loguru import logger

from altunityrunner.commands.base_command import Command
from altunityrunner.commands.InputActions.action_finished import ActionFinished
from altunityrunner.commands.InputActions.move_mouse import MoveMouse


class MoveMouseAndWait(Command):

    def __init__(self, connection, x, y, duration):
        self.connection = connection

        self.x = x
        self.y = y
        self.duration = duration

    def execute(self):
        data = MoveMouse.run(
            self.connection,
            self.x, self.y, self.duration
        )

        logger.debug("Wait for move mouse to finish")
        time.sleep(self.duration)

        action_in_progress = True
        while action_in_progress:
            action_finished = ActionFinished.run(self.connection)

            if action_finished == "Yes":
                break
            elif action_finished != "No":
                action_in_progress = False

        return data
