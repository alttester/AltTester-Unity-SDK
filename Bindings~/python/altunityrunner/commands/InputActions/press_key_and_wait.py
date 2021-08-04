import time

from loguru import logger

from altunityrunner.commands.base_command import Command
from altunityrunner.commands.InputActions.press_key import PressKey
from altunityrunner.commands.InputActions.action_finished import ActionFinished


class PressKeyAndWait(Command):

    def __init__(self, connection, key_code, power, duration):
        self.connection = connection

        self.key_code = key_code
        self.power = power
        self.duration = duration

    def execute(self):
        data = PressKey.run(
            self.connection,
            self.key_code, self.power, self.duration
        )

        logger.debug("Wait for press key with keycode to finish")
        time.sleep(self.duration)

        action_in_progress = True
        while action_in_progress:
            action_finished = ActionFinished.run(self.connection)

            if action_finished == "Yes":
                break
            elif action_finished != "No":
                action_in_progress = False

        return data
