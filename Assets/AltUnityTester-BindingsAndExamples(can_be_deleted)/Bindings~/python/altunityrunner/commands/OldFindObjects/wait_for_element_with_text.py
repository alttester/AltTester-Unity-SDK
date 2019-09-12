from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.commands.OldFindObjects.find_element import FindElement
from altunityrunner.commands.ObjectCommands.get_text import GetText
from altunityrunner.altUnityExceptions import WaitTimeOutException
import time
class WaitForElementWithText(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver,value,text,camera_name='', timeout=20, interval=0.5,enabled=True):
        super().__init__(socket,request_separator,request_end,appium_driver)
        self.value=value
        self.text=text
        self.camera_name=camera_name
        self.timeout=timeout
        self.interval=interval
        self.enabled=enabled
    
    def execute(self):
        t = 0
        alt_element = None
        object_text=""
        while (t <= self.timeout):
            try:
                alt_element = FindElement(self.socket,self.request_separator,self.request_end,self.appium_driver,self.value,self.camera_name,self.enabled).execute()
                alt_object = alt_element.toJSON()
                object_text=GetText(self.socket,self.request_separator,self.request_end,alt_object).execute()
                if object_text == self.text:
                    break
                raise Exception('Not the wanted text')
            except Exception:
                print('Waiting for element ' + self.value + ' to have text ' + self.text+ 'but had '+ object_text)
                time.sleep(self.interval)
                t += self.interval
        if t>=self.timeout:
            raise WaitTimeOutException('Element ' + self.value + ' should have text `' + self.text + '` but has `' + object_text + '` after ' + str(self.timeout) + ' seconds')
        return alt_element