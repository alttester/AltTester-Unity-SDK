from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import WaitTimeOutException
import time
class WaitForObjectWhichContains(CommandReturningAltElements):
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
        alt_element = None
        while (t <= timeout):
            try:
                alt_element = self.find_object_which_contains(by,value,camera_name,enabled)
                break
            except Exception:
                print('Waiting for element where name contains ' + value + '...')
                time.sleep(interval)
                t += interval
        if t>=timeout:
            raise WaitTimeOutException('Element where name contains ' + value + ' not found after ' + str(timeout) + ' seconds')
        return alt_element