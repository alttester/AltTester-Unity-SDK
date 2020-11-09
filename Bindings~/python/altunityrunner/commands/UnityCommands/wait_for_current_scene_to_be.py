from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import *
from altunityrunner.commands.UnityCommands.get_current_scene import *
from loguru import logger
import time


class WaitForCurrentSceneToBe(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end, scene_name, timeout, interval):
        super(WaitForCurrentSceneToBe, self).__init__(
            socket, request_separator, request_end)
        self.scene_name = scene_name
        self.timeout = timeout
        self.interval = interval

    def execute(self):
        t = 0
        current_scene = ''
        while (t <= self.timeout):
            logger.debug('Waiting for scene to be ' + self.scene_name + '...')
            current_scene = GetCurrentScene(
                self.socket, self.request_separator, self.request_end).execute()
            if current_scene != self.scene_name:
                time.sleep(self.interval)
                t += self.interval
            else:
                break
        if t >= self.timeout:
            raise WaitTimeOutException(
                'Scene ' + self.scene_name + ' not loaded after ' + str(self.timeout) + ' seconds')
        return current_scene
