import time

from loguru import logger

from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements


class TapCustom(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end, x, y, count, interval):
        super(TapCustom, self).__init__(socket, request_separator, request_end)
        self.x = x
        self.y = y
        self.count = count
        self.interval = interval

    def execute(self):
        position = self.vector_to_json_string(self.x, self.y)
        data = self.send_command("tapCustom", position, self.count, self.interval)

        logger.debug("Wait for custom tap to be finished")
        time.sleep(self.interval * self.count)

        action_in_progress = True
        while action_in_progress:
            action_finished = self.send_command("actionFinished")
            if action_finished == "Yes":
                break
            elif action_finished != "No":
                action_in_progress = False

        return data
