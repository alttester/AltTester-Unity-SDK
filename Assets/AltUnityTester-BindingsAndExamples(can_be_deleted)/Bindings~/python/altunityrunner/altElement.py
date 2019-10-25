import json
from altunityrunner.commands.ObjectCommands.get_text import GetText
from altunityrunner.commands.ObjectCommands.set_component_property import SetComponentProperty
from altunityrunner.commands.ObjectCommands.get_component_property import GetComponentProperty
from altunityrunner.commands.ObjectCommands.set_text import SetText
from altunityrunner.commands.ObjectCommands.tap import Tap
from altunityrunner.commands.ObjectCommands.call_component_method import CallComponentMethodForObject
from altunityrunner.commands.ObjectCommands.click_event import ClickEvent
from altunityrunner.commands.ObjectCommands.drag import Drag
from altunityrunner.commands.ObjectCommands.drop import Drop
from altunityrunner.commands.ObjectCommands.pointer_down import PointerDown
from altunityrunner.commands.ObjectCommands.pointer_enter import PointerEnter
from altunityrunner.commands.ObjectCommands.pointer_exit import PointerExit
from altunityrunner.commands.ObjectCommands.pointer_up import PointerUp

class AltElement(object):
    def __init__(self, alt_unity_driver, appium_driver, json_data):
        self.alt_unity_driver = alt_unity_driver
        self.appium_driver=None
        if (appium_driver != None):
            self.appium_driver = appium_driver
        data = json.loads(json_data)
        self.name = str(data['name'])
        self.id = str(data['id'])
        self.x = str(data['x'])
        self.y = str(data['y'])
        self.z=str(data['z'])
        self.mobileY = str(data['mobileY'])
        self.type = str(data['type'])
        self.enabled = str(data['enabled'])
        self.worldX = str(data['worldX'])
        self.worldY = str(data['worldY'])
        self.worldZ = str(data['worldZ'])
        self.idCamera=str(data['idCamera'])

    def toJSON(self):
        return '{"name":"' + self.name + '", \
                 "id":"' + self.id + '", \
                 "x":"' + self.x + '", \
                 "y":"' + self.y + '", \
                 "z":"'+self.z+'",\
                 "mobileY":"' + self.mobileY + '", \
                 "type":"' + self.type + '", \
                 "enabled":"' + self.enabled + '", \
                 "worldX":"' + self.worldX + '", \
                 "worldY":"' + self.worldY + '", \
                 "worldZ":"' + self.worldZ + '",\
                 "idCamera":"'+self.idCamera+'"}'
        
    def get_component_property(self, component_name, property_name, assembly_name=''):
        alt_object = self.toJSON()
        return GetComponentProperty(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,component_name,property_name,assembly_name,alt_object).execute()

    def set_component_property(self, component_name, property_name, value, assembly_name=''):
        alt_object = self.toJSON()
        return SetComponentProperty(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,component_name,property_name,value,assembly_name,alt_object).execute()        

    def call_component_method(self, component_name, method_name, parameters,assembly_name='',type_of_parameters=''):
        alt_object = self.toJSON()
        return CallComponentMethodForObject(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,component_name,method_name,parameters,assembly_name,type_of_parameters,alt_object).execute()        

    def get_text(self):
        alt_object = self.toJSON()
        return GetText(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,alt_object).execute()
    
    def set_text(self, text):
        alt_object = self.toJSON()
        data = SetText(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,text,alt_object).execute()
        return AltElement(self.alt_unity_driver, self.appium_driver, data)
    
    def click_Event(self):
        alt_object = self.toJSON()
        data= ClickEvent(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,alt_object).execute()
        return AltElement(self.alt_unity_driver,self.appium_driver,data)
        
    def mobile_tap(self, durationInSeconds=0.5):
        self.appium_driver.tap([[float(self.x), float(self.mobileY)]], durationInSeconds * 1000)
    
    def mobile_dragTo(self, end_x, end_y, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, end_x, end_y, durationIndSeconds* 1000)

    def mobile_dragToElement(self, other_element, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, other_element.x, other_element.mobileY, durationIndSeconds* 1000)
    
    def drag(self, x, y):
        alt_object = self.toJSON()
        data= Drag(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,x,y,alt_object).execute()
        return AltElement(self.alt_unity_driver,self.appium_driver,data)

    def drop(self, x, y):
        alt_object = self.toJSON()
        data= Drop(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,x,y,alt_object).execute()
        return AltElement(self.alt_unity_driver,self.appium_driver,data)
    
    def pointer_up(self):
        alt_object = self.toJSON()
        data= PointerUp(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,alt_object).execute()
        return AltElement(self.alt_unity_driver,self.appium_driver,data)

    def pointer_down(self):
        alt_object = self.toJSON()
        data= PointerDown(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,alt_object).execute()
        return AltElement(self.alt_unity_driver,self.appium_driver,data)

    def pointer_enter(self):
        alt_object = self.toJSON()
        data= PointerEnter(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,alt_object).execute()
        return AltElement(self.alt_unity_driver,self.appium_driver,data)
        
    def pointer_exit(self):
        alt_object = self.toJSON()
        data= PointerExit(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,alt_object).execute()        
        return AltElement(self.alt_unity_driver,self.appium_driver,data)
    
    def tap(self):
        alt_object=self.toJSON()
        data= Tap(self.alt_unity_driver.socket,self.alt_unity_driver.request_separator,self.alt_unity_driver.request_end,alt_object).execute()
        return AltElement(self.alt_unity_driver,self.appium_driver,data)
