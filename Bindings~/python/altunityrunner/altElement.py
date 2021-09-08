import json

import altunityrunner.commands as commands
from altunityrunner.by import By


class AltElement:
    """The element class represents an object present in the game and it allows you to interact with it.

    It is the return type of the methods in the “find_*” category from the AltUnityDriver class.
    """

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
        return "{}(altdriver, {!r})".format(self.__class__.__name__, self.to_json())

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
        data = commands.FindObject.run(
            self._connection,
            By.PATH, "//*[@id={}]/..".format(self.id), By.NAME, "", True
        )

        return AltElement(self._altdriver, data)

    def get_screen_position(self):
        return self.x, self.y

    def get_world_position(self):
        return self.worldX, self.worldY, self.worldZ

    def get_all_components(self):
        return commands.GetAllComponents.run(self._connection, self)

    def get_component_property(self, component_name, property_name, assembly="", max_depth=2):
        return commands.GetComponentProperty.run(
            self._connection,
            component_name, property_name, assembly, max_depth, self
        )

    def set_component_property(self, component_name, property_name, value, assembly=""):
        return commands.SetComponentProperty.run(
            self._connection,
            component_name, property_name, value, assembly, self
        )

    def call_component_method(self, component_name, method_name, parameters=None, type_of_parameters=None, assembly=""):
        """Invoke a method from an existing component of the object.

        Args:
            type_name (:obj:`str`): The name of the script. If the script has a namespace the format should look like
                this: ``"namespace.typeName"``.
            method_name (:obj:`str`): The name of the public method that we want to call. If the method is inside a
                static property/field to be able to call that method, methodName need to be the following format
                ``"propertyName.MethodName"``.
            parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.
            type_of_parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.
            assembly (:obj:`str`, optional): The name of the assembly containing the script. Defaults to ``""``.

        Return:
            str: The value returned by the method is serialized to a JSON string.
        """

        return commands.CallMethod.run(
            self._connection,
            component_name,
            method_name,
            alt_object=self,
            parameters=parameters,
            type_of_parameters=type_of_parameters,
            assembly=assembly
        )

    def get_text(self):
        return commands.GetText.run(self._connection, self)

    def set_text(self, text):
        data = commands.SetText.run(self._connection, text, self)
        return AltElement(self._altdriver, data)

    def pointer_up(self):
        data = commands.PointerUp.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def pointer_down(self):
        data = commands.PointerDown.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def pointer_enter(self):
        data = commands.PointerEnter.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def pointer_exit(self):
        data = commands.PointerExit.run(self._connection, self)
        return AltElement(self._altdriver, data)

    def tap(self, count=1, interval=0.1, wait=True):
        """Tap current object.

        Args:
            count (:obj:`int`, optional): Number of taps. Defaults to ``1``.
            interval (:obj:`int`, :obj:`float`, optional): Interval between taps in seconds. Defaults to ``0.1``.
            wait (:obj:`int`, optional): Wait for command to finish. Defaults to ``True``.

        Returns:
            AltElement: The tapped object.
        """

        data = commands.TapElement.run(
            self._connection,
            self, count, interval, wait
        )
        return AltElement(self._altdriver, data)

    def click(self, count=1, interval=0.1, wait=True):
        """Click current object.

        Parameters:
            count (:obj:`int`, optional): Number of clicks. Defaults to ``1``.
            interval (:obj:`int`, :obj:`float`, optional): Interval between clicks in seconds. Defaults to ``0.1``.
            wait (:obj:`int`, optional): Wait for command to finish. Defaults to ``True``.

        Returns:
            AltElement: The clicked object.
        """

        data = commands.ClickElement.run(
            self._connection,
            self, count, interval, wait
        )
        return AltElement(self._altdriver, data)
