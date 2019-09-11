import json
from altunityrunner.commands.ObjectCommands.get_text import GetText
from altunityrunner.commands.ObjectCommands.set_component_property import SetComponentProperty
from altunityrunner.commands.ObjectCommands.get_component_property import GetComponentProperty
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
        return GetComponentProperty(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,component_name,property_name,assembly_name,alt_object).execute()

    def set_component_property(self, component_name, property_name, value, assembly_name=''):
        alt_object = self.toJSON()
        return SetComponentProperty(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,component_name,property_name,value,assembly_name,alt_object).execute()        

    def call_component_method(self, component_name, method_name, parameters,assembly_name='',type_of_parameters=''):
        alt_object = self.toJSON()
        return CallComponentMethodForObject(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,component_name,method_name,parameters,assembly_name,type_of_parameters,alt_object).execute()        

    def get_text(self):
        alt_object = self.toJSON()
        return GetText(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,alt_object).execute()

    def click_Event(self):
        alt_object = self.toJSON()
        return ClickEvent(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,alt_object).execute()
        
    def mobile_tap(self, durationInSeconds=0.5):
        self.appium_driver.tap([[float(self.x), float(self.mobileY)]], durationInSeconds * 1000)
    
    def mobile_dragTo(self, end_x, end_y, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, end_x, end_y, durationIndSeconds* 1000)

    def mobile_dragToElement(self, other_element, durationIndSeconds=0.5):
        self.appium_driver.swipe(self.x, self.mobileY, other_element.x, other_element.mobileY, durationIndSeconds* 1000)
    
    def drag(self, x, y):
        alt_object = self.toJSON()
        return Drag(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,x,y,alt_object).execute()

    def drop(self, x, y):
        alt_object = self.toJSON()
        return Drop(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,x,y,alt_object).execute()
    
    def pointer_up(self):
        alt_object = self.toJSON()
        return PointerUp(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,alt_object).execute()

    def pointer_down(self):
        alt_object = self.toJSON()
        return PointerDown(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,alt_object).execute()

    def pointer_enter(self):
        alt_object = self.toJSON()
        return PointerEnter(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,alt_object).execute()
        
    def pointer_exit(self):
        alt_object = self.toJSON()
        return PointerExit(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,alt_object).execute()        
    
    def tap(self):
        alt_object=self.toJSON()
        return Tap(self.alt_unity_driver.socket,self.alt_unity_driver.requestSeparator,self.alt_unity_driver.requestEnd,alt_object).execute()
