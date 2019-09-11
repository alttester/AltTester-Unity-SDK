from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import *
from altunityrunner.commands.UnityCommands.get_current_scene import *
import time
class WaitForCurrentSceneToBe(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd, scene_name, timeout, interval):
        super().__init__(socket,requestSeparator,requestEnd)
        self.scene_name=scene_name
        self.timeout=timeout
        self.interval=interval
    
    def execute(self):
        t = 0
        current_scene = ''
        while (t <= self.timeout):
            print('Waiting for scene to be ' + self.scene_name + '...')
            current_scene = GetCurrentScene(self.socket,self.requestSeparator,self.requestEnd).execute()
            if current_scene != self.scene_name:
                time.sleep(self.interval)
                t += self.interval
            else:
                break
        if t>=self.timeout:
            raise WaitTimeOutException('Scene ' + self.scene_name + ' not loaded after ' + str(self.timeout) + ' seconds')
        return current_scene