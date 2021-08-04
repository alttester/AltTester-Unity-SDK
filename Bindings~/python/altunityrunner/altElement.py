import json

from altunityrunner.by import By
from altunityrunner.commands.FindObjects.find_object import FindObject
from altunityrunner.commands.ObjectCommands.get_text import GetText
from altunityrunner.commands.ObjectCommands.set_component_property import SetComponentProperty
from altunityrunner.commands.ObjectCommands.get_component_property import GetComponentProperty
from altunityrunner.commands.ObjectCommands.get_all_components import GetAllComponents
from altunityrunner.commands.ObjectCommands.set_text import SetText
from altunityrunner.commands.ObjectCommands.call_component_method import CallComponentMethodForObject
from altunityrunner.commands.ObjectCommands.pointer_down import PointerDown
from altunityrunner.commands.ObjectCommands.pointer_enter import PointerEnter
from altunityrunner.commands.ObjectCommands.pointer_exit import PointerExit
from altunityrunner.commands.ObjectCommands.pointer_up import PointerUp
from altunityrunner.commands.ObjectCommands.tap_element import TapElement
from altunityrunner.commands.ObjectCommands.click_element import ClickElement


class AltElement:

    def __init__(self, altdriver, data):
        self._altdriver = altdriver

        self.name = data.get("name", "")
        self.id = data.get("id", 0)
        self.x = data.get("x", 0)
        self.y = data.get("y", 0)
        self.z = data.get("z", 0)
        self.mobileY = data.get("mobileY", 0)
        self.type = data.get("type", "")
        self.enabled = data.get("enabled", True)
        self.worldX = data.get("worldX", 0)
        self.worldY = data.get("worldY", 0)
        self.worldZ = data.get("worldZ", 0)
        self.idCamera = data.get("idCamera", 0)
        self.transformParentId = data.get("transformParentId", "")
        self.transformId = data.get("transformId", 0)

    def __repr__(self):
        return "{}(driver, {!r})".format(self.__class__.__name__, self.to_json())

    def __str__(self):
        return json.dumps(self.to_json())

    @property
    def _connection(self):
        return self._altdriver._connection

    def to_json(self):
        return {
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
            "transformParentId": self.transformParentId,
            "transformId": self.transformId,
            "idCamera": self.idCamera
        }

    def get_parent(self):
        data = FindObject.run(
            self._connection,
            By.PATH, "//*[@id={}]/..".format(self.id), By.NAME, "", True
        )

        return AltElement(self._altdriver, data)

    def get_screen_position(self):
        return self.x, self.y

    def get_world_position(self):
        return self.worldX, self.worldY, self.worldZ

    def get_all_components(self):
        return GetAllComponents.run(self._connection, self)

    def get_component_property(self, component_name, property_name, assembly_name="", max_depth=2):
        return GetComponentProperty.run(
            self._connection,
            component_name, property_name, assembly_name, max_depth, self
        )

    def set_component_property(self, component_name, property_name, value, assembly_name=""):
        return SetComponentProperty.run(
            self._connection,
            component_name, property_name, value, assembly_name, self
        )

    def call_component_method(self, component_name, method_name, parameters, assembly_name="", type_of_parameters=""):
        return CallComponentMethodForObject.run(
            self._connection,
            component_name, method_name, parameters, assembly_name, type_of_parameters, self
        )

    def get_text(self):
        return GetText.run(self._connection, self)

    def set_text(self, text):
        data = SetText.run(self._connection, text, self)
        return AltElement(self._altdriver, data)

    def pointer_up(self):
        data = PointerUp.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def pointer_down(self):
        data = PointerDown.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def pointer_enter(self):
        data = PointerEnter.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def pointer_exit(self):
        data = PointerExit.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def tap(self, count=1, interval=0.1, wait=True):
        """Tap current object.

        Args:
            count: Number of taps.
            interval: Interval in seconds.
            wait: Wait for command to finish.

        Returns:
            The tapped object.
        """

        data = TapElement.run(self._connection, self, count, interval, wait)
        return AltElement(self._altdriver, data)

    def click(self, count=1, interval=0.1, wait=True):
        """Click current object.

        Parameters:
            count: Number of clicks (default 1)
            interval: Interval between clicks in seconds (default 0.1)
            wait: Wait for command to finish

        Returns:
            The clicked object.
        """

        data = ClickElement.run(
            self._connection,
            self, count, interval, wait
        )
        return AltElement(self._altdriver, data)
