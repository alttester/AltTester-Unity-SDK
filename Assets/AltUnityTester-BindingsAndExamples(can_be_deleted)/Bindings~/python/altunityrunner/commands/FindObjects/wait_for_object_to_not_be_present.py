from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import WaitTimeOutException
import time
class WaitForObjectToNotBePresent(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver, by,value,camera_name, timeout, interval,enabled):
        super().__init__(socket,request_separator,request_end,appium_driver)
        self.by=by
        self.value=value
        self.camera_name=camera_name
        self.timeout=timeout
        self.interval=interval
        self.enabled=enabled
    
    def execute(self):
        t = 0
        while (t <= timeout):
            try:
                print('Waiting for element ' + value + ' to not be present...')
                self.find_object(by,value,camera_name,enabled)
                time.sleep(interval)
                t += interval
            except Exception:
                break
        if t>=timeout:
            raise WaitTimeOutException('Element ' + value + ' still found after ' + str(timeout) + ' seconds')