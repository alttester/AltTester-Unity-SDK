import time

from loguru import logger

from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.commands.InputActions.press_key import PressKey


class PressKeyAndWait(BaseCommand):

    def __init__(self, socket, request_separator, request_end, keyName, power, duration):
        super(PressKeyAndWait, self).__init__(socket, request_separator, request_end)
        self.keyName = keyName
        self.power = power
        self.duration = duration

    def execute(self):
        data = PressKey(
            self.socket, self.request_separator, self.request_end,
            self.keyName, self.power, self.duration
        ).execute()

        logger.debug("Wait for press key to finish")
        time.sleep(self.duration)

        action_in_progress = True
        while action_in_progress:
            action_finished = self.send_command("actionFinished")
            if action_finished == "Yes":
                break
            elif action_finished != "No":
                action_in_progress = False

        return data
