import json

from deprecated import deprecated

from altunityrunner.by import By
from altunityrunner.commands.ObjectCommands.get_text import GetText
from altunityrunner.commands.ObjectCommands.set_component_property import SetComponentProperty
from altunityrunner.commands.ObjectCommands.get_component_property import GetComponentProperty
from altunityrunner.commands.ObjectCommands.get_all_components import GetAllComponents
from altunityrunner.commands.ObjectCommands.set_text import SetText
from altunityrunner.commands.ObjectCommands.tap import Tap
from altunityrunner.commands.ObjectCommands.call_component_method import CallComponentMethodForObject
from altunityrunner.commands.ObjectCommands.click_event import ClickEvent
from altunityrunner.commands.ObjectCommands.get_all_fields import GetAllFields
from altunityrunner.commands.ObjectCommands.pointer_down import PointerDown
from altunityrunner.commands.ObjectCommands.pointer_enter import PointerEnter
from altunityrunner.commands.ObjectCommands.pointer_exit import PointerExit
from altunityrunner.commands.ObjectCommands.pointer_up import PointerUp
from altunityrunner.commands.ObjectCommands.tap_element import TapElement
from altunityrunner.commands.ObjectCommands.click_element import ClickElement


class AltElement(object):

    def __init__(self, alt_unity_driver, json_data):
        self.alt_unity_driver = alt_unity_driver

        data = json.loads(json_data)

        self.name = str(data.get("name", ""))
        self.id = str(data.get("id", 0))
        self.x = str(data.get("x", 0))
        self.y = str(data.get("y", 0))
        self.z = str(data.get("z", 0))
        self.mobileY = str(data.get("mobileY", 0))
        self.type = str(data.get("type", ""))
        self.enabled = str(data.get("enabled", True))
        self.worldX = str(data.get("worldX", 0))
        self.worldY = str(data.get("worldY", 0))
        self.worldZ = str(data.get("worldZ", 0))
        self.idCamera = str(data.get("idCamera", 0))
        self._parentId = str(data.get("parentId", 0))
        self.transformParentId = str(data.get("transformParentId", self._parentId))
        self.transformId = str(data.get("transformId", 0))

    def __repr__(self):
        return "altunityrunner.{}(driver, {!r})".format(type(self).__name__, self.toJSON())

    def __str__(self):
        return self.toJSON()

    @property
    @deprecated(version="1.6.2", reason="Use transformParentId instead.")
    def parentId(self):
        return self._parentId

    @parentId.setter
    @deprecated(version="1.6.2", reason="Use transformParentId instead.")
    def parentId(self, value):
        self._parentId = value

    def toJSON(self):
        return json.dumps({
            "name": self.name,
            "id": self.id,
            "x": self.x,
            "y": self.y,
            "z": self.z,
            "mobileY": self.mobileY,
            "type": self.type,
            "enabled": self.enabled,
            "worldX": self.worldX,
            "worldY": self.worldY,
            "worldZ": self.worldZ,
            "parentId": self.parentId,
            "transformParentId": self.transformParentId,
            "transformId": self.transformId,
            "idCamera": self.idCamera
        })

    def get_parent(self):
        from altunityrunner.commands.FindObjects.find_object import FindObject

        return FindObject(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            By.PATH, "//*[@id={}]/..".format(self.id), By.NAME, "", True
        ).execute()

    def get_screen_position(self):
        return self.x, self.y

    def get_world_position(self):
        return self.worldX, self.worldY, self.worldZ

    def get_all_components(self):
        return GetAllComponents(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            self
        ).execute()

    def get_component_property(self, component_name, property_name, assembly_name="", max_depth=2):
        alt_object = self.toJSON()
        return GetComponentProperty(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            component_name, property_name, assembly_name, max_depth, alt_object
        ).execute()

    def set_component_property(self, component_name, property_name, value, assembly_name=""):
        alt_object = self.toJSON()
        return SetComponentProperty(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            component_name, property_name, value, assembly_name, alt_object
        ).execute()

    def call_component_method(self, component_name, method_name, parameters, assembly_name="", type_of_parameters=""):
        alt_object = self.toJSON()
        return CallComponentMethodForObject(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            component_name, method_name, parameters, assembly_name, type_of_parameters, alt_object
        ).execute()

    def get_text(self):
        alt_object = self.toJSON()
        return GetText(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object
        ).execute()

    def get_all_fields(self, component):
        alt_object = self
        return GetAllFields(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object, component
        ).execute()

    def set_text(self, text):
        alt_object = self.toJSON()
        data = SetText(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            text, alt_object
        ).execute()
        return AltElement(self.alt_unity_driver, data)

    @deprecated(version="1.6.5", reason="Use click")
    def click_event(self):
        alt_object = self.toJSON()
        data = ClickEvent(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object
        ).execute()

        return AltElement(self.alt_unity_driver, data)

    def pointer_up(self):
        alt_object = self.toJSON()
        data = PointerUp(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object
        ).execute()
        return AltElement(self.alt_unity_driver, data)

    def pointer_down(self):
        alt_object = self.toJSON()
        data = PointerDown(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object
        ).execute()
        return AltElement(self.alt_unity_driver, data)

    def pointer_enter(self):
        alt_object = self.toJSON()
        data = PointerEnter(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object
        ).execute()
        return AltElement(self.alt_unity_driver, data)

    def pointer_exit(self):
        alt_object = self.toJSON()
        data = PointerExit(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object
        ).execute()
        return AltElement(self.alt_unity_driver, data)

    def tap(self, count=None, interval=0.1, wait=True):
        '''Tap current object

    Parameters:
        count -- Number of taps (default 1)
        interval -- Interval in seconds (default 0.1)
        wait -- Wait for command to finish

    Returns:
        The tapped object
        '''
        alt_object = self.toJSON()
        if not count:  # backwards compatibility
            data = Tap(
                self.alt_unity_driver.socket,
                self.alt_unity_driver.request_separator,
                self.alt_unity_driver.request_end, alt_object, 1
            ).execute()
            return AltElement(self.alt_unity_driver, data)
        else:
            data = TapElement(self.alt_unity_driver.socket,
                              self.alt_unity_driver.request_separator,
                              self.alt_unity_driver.request_end,
                              alt_object, count, interval, wait).execute()
            return AltElement(self.alt_unity_driver, data)

    @deprecated(version="1.6.5", reason="Use tap with parameter count=2")
    def double_tap(self):
        alt_object = self.toJSON()
        data = Tap(
            self.alt_unity_driver.socket, self.alt_unity_driver.request_separator, self.alt_unity_driver.request_end,
            alt_object, 2
        ).execute()
        return AltElement(self.alt_unity_driver, data)

    def click(self, count=1, interval=0.1, wait=True):
        '''Click current object

    Parameters:
        count -- Number of clicks (default 1)
        interval -- Interval between clicks in seconds (default 0.1)
        wait -- Wait for command to finish

    Returns:
        The clicked object
        '''
        alt_object = self.toJSON()
        data = ClickElement(self.alt_unity_driver.socket,
                            self.alt_unity_driver.request_separator,
                            self.alt_unity_driver.request_end,
                            alt_object, count, interval, wait).execute()
        return AltElement(self.alt_unity_driver, data)
