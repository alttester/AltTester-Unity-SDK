import time

from loguru import logger

from altunityrunner.commands.base_command import Command
from altunityrunner.commands.InputActions.tilt import Tilt
from altunityrunner.commands.InputActions.action_finished import ActionFinished


class TiltAndWait(Command):

    def __init__(self, connection, x, y, z, duration_in_secs):
        self.connection = connection

        self.x = x
        self.y = y
        self.z = z
        self.duration_in_secs = duration_in_secs

    def execute(self):
        data = Tilt.run(
            self.connection,
            self.x, self.y, self.z, self.duration_in_secs
        )

        logger.debug("Wait for tilt to finish")
        time.sleep(self.duration_in_secs)

        tilt_in_progress = True
        while tilt_in_progress:
            tilt_finished = ActionFinished.run(self.connection)

            if tilt_finished == "Yes":
                break
            elif tilt_finished != "No":
                tilt_in_progress = False
        return data
