import time

from loguru import logger

from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import WaitTimeOutException
from altunityrunner.commands.UnityCommands.get_current_scene import GetCurrentScene


class WaitForCurrentSceneToBe(CommandReturningAltElements):

    def __init__(self, socket, request_separator, request_end, scene_name, timeout, interval):
        super(WaitForCurrentSceneToBe, self).__init__(socket, request_separator, request_end)
        self.scene_name = scene_name
        self.timeout = timeout
        self.interval = interval

    def execute(self):
        t = 0
        current_scene = ""

        while t <= self.timeout:
            logger.debug("Waiting for scene to be {}...".format(self.scene_name))
            current_scene = GetCurrentScene(self.socket, self.request_separator, self.request_end).execute()

            if current_scene != self.scene_name:
                time.sleep(self.interval)
                t += self.interval
            else:
                break
        if t >= self.timeout:
            raise WaitTimeOutException("Scene {} not loaded after {} seconds".format(self.scene_name, self.timeout))

        return current_scene
