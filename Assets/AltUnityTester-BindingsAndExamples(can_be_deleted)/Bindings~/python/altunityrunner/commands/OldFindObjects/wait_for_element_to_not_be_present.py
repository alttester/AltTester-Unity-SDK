from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.commands.OldFindObjects.find_element import FindElement
from altunityrunner.altUnityExceptions import WaitTimeOutException
import time
class WaitForElementToNotBePresent(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver,value,camera_name='', timeout=20, interval=0.5,enabled=True):
        super().__init__(socket,request_separator,request_end,appium_driver)
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
                alt_element=FindElement(self.socket,self.request_separator,self.request_end,self.appium_driver,self.value,self.camera_name,self.enabled).execute()
                time.sleep(self.interval)
                t += self.interval
            except Exception:
                break
        if t>=self.timeout:
            raise WaitTimeOutException('Element ' + self.value + ' still found after ' + str(self.timeout) + ' seconds')