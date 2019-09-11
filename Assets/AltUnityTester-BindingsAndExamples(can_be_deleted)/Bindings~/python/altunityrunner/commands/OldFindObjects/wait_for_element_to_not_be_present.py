from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.commands.OldFindObjects.find_element import FindElement
from altunityrunner.altUnityExceptions import WaitTimeOutException
import time
class WaitForElementToNotBePresent(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd,value,camera_name='', timeout=20, interval=0.5,enabled=True):
        super().__init__(socket,requestSeparator,requestEnd)
        self.value=value
        self.camera_name=camera_name
        self.timeout=timeout
        self.interval=interval
        self.enabled=enabled
    
    def execute(self):
        t = 0
        while (t <= self.timeout):
            try:
                print('Waiting for element ' + self.value + ' to not be present...')
                alt_element=FindElement(self.socket,self.requestSeparator,self.requestEnd,self.value,self.camera_name,self.enabled).execute()
                time.sleep(self.interval)
                t += self.interval
            except Exception:
                break
        if t>=self.timeout:
            raise WaitTimeOutException('Element ' + self.value + ' still found after ' + str(self.timeout) + ' seconds')