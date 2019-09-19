from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import WaitTimeOutException
import time
class WaitForObjectWithText(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver, by,value,text,camera_name, timeout, interval,enabled):
        super().__init__(socket,request_separator,request_end,appium_driver)
        self.by=by
        self.value=value
        self.text=text
        self.camera_name=camera_name
        self.timeout=timeout
        self.interval=interval
        self.enabled=enabled
    
    def execute(self):
        t = 0
        alt_element = None
        while (t <= timeout):
            try:
                alt_element=self.find_object(value,camera_name,enabled)
                if alt_element.get_text() == text:
                    break
                raise Exception('Not the wanted text')
            except Exception:
                print('Waiting for element ' + value + ' to have text ' + text)
                time.sleep(interval)
                t += interval
        if t>=timeout:
            raise WaitTimeOutException('Element ' + value + ' should have text `' + text + '` but has `' + alt_element.get_text() + '` after ' + str(timeout) + ' seconds')
        return alt_element