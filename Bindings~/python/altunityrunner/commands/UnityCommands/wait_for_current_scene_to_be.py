import time

from loguru import logger

from altunityrunner.commands.base_command import Command
from altunityrunner.exceptions import WaitTimeOutException
from altunityrunner.commands.UnityCommands.get_current_scene import GetCurrentScene


class WaitForCurrentSceneToBe(Command):

    def __init__(self, connection, scene_name, timeout, interval):
        self._connection = connection

        self.scene_name = scene_name
        self.timeout = timeout
        self.interval = interval

    def execute(self):
        t = 0
        current_scene = ""

        while t <= self.timeout:
            logger.debug("Waiting for scene to be {}...", self.scene_name)
            current_scene = GetCurrentScene(self._connection).execute()

            if current_scene == self.scene_name:
                return
            time.sleep(self.interval)
            t += self.interval
        if t >= self.timeout:
            raise WaitTimeOutException("Scene {} not loaded after {} seconds".format(self.scene_name, self.timeout))
