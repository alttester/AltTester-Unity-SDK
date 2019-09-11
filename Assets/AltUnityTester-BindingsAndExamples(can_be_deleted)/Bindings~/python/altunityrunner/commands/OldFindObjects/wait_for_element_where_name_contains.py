from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.commands.OldFindObjects.find_element_where_name_contains import FindElementWhereNameContains
from altunityrunner.altUnityExceptions import WaitTimeOutException
import time
class WaitForElementWhereNameContains(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd,value,camera_name='', timeout=20, interval=0.5,enabled=True):
        super().__init__(socket,requestSeparator,requestEnd)
        self.value=value
        self.camera_name=camera_name
        self.timeout=timeout
        self.interval=interval
        self.enabled=enabled
    
    def execute(self):
        t = 0
        alt_element = None
        while (t <= self.timeout):
            try:
                alt_element = FindElementWhereNameContains(self.socket,self.requestSeparator,self.requestEnd,self.value,self.camera_name,self.enabled).execute()
                break
            except Exception:
                print('Waiting for element where name contains ' + self.value + '...')
                time.sleep(self.interval)
                t += self.interval
        if t>=self.timeout:
            raise WaitTimeOutException('Element where name contains ' + self.value + ' not found after ' + str(self.timeout) + ' seconds')
        return alt_element