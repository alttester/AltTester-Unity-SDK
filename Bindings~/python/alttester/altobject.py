"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

import json

import alttester.commands as commands
from alttester.by import By


class AltObject:
    """The AltObject class represents an object present in the application and it allows you to interact with it.

    It is the return type of the methods in the “find_*” category from the AltDriver class.
    """

    def __init__(self, altdriver, data):
        self._altdriver = altdriver
        self._data = data

    def __repr__(self):
        return "{}(altdriver, {!r})".format(self.__class__.__name__, self.to_json())

    def __str__(self):
        return json.dumps(self.to_json())

    @property
    def _connection(self):
        return self._altdriver._connection

    @property
    def name(self):
        return self._data.get("name", "")

    @property
    def id(self):
        return self._data.get("id", 0)

    @property
    def x(self):
        return self._data.get("x", 0)

    @property
    def y(self):
        return self._data.get("y", 0)

    @property
    def z(self):
        return self._data.get("z", 0)

    @property
    def mobileY(self):
        return self._data.get("mobileY", 0)

    @property
    def type(self):
        return self._data.get("type", "")

    @property
    def enabled(self):
        return self._data.get("enabled", True)

    @property
    def worldX(self):
        return self._data.get("worldX", 0.0)

    @property
    def worldY(self):
        return self._data.get("worldY", 0.0)

    @property
    def worldZ(self):
        return self._data.get("worldZ", 0.0)

    @property
    def idCamera(self):
        return self._data.get("idCamera", 0)

    @property
    def transformParentId(self):
        return self._data.get("transformParentId", 0)

    @property
    def transformId(self):
        return self._data.get("transformId", 0)

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

    def update_object(self):
        altObject = commands.FindObject.run(
            self._connection,
            By.ID, self.id, By.NAME, "", enabled=True
        )
        return AltObject(self._altdriver, altObject)

    def get_screen_position(self):
        """Returns the screen position.

        Returns:
            tuple: A tuple containing ``x`` and ``y``.

        """

        return self.x, self.y

    def get_world_position(self):
        """Returns the world position.

        Returns:
            tuple: A tuple containing ``worldX``, ``worldY`` and ``worldZ``.

        """

        return self.worldX, self.worldY, self.worldZ

    def get_parent(self):
        """Returns the parent object.

        Returns:
            AltObject: The parent object.

        """

        data = commands.FindObject.run(
            self._connection,
            By.PATH, "//*[@id={}]/..".format(self.id), By.NAME, "", enabled=True
        )

        return AltObject(self._altdriver, data)

    def get_all_components(self):
        """Returns all components."""

        return commands.GetAllComponents.run(self._connection, self)

    def wait_for_component_property(self, component_name, property_name,
                                    property_value, assembly,  timeout=20, interval=0.5,
                                    get_property_as_string=False, max_depth=2):
        """Wait until a property has a specific value and returns the value of the given component property.

        Args:
            component_name (:obj:`str`): The name of the component. If the component has a namespace the format should
                look like this: ``"namespace.componentName"``.
            property_name (:obj:`str`): The name of the property of which value you want. If the property is an array
                you can specify which element of the array to return by doing ``property[index]``, or if you want a
                property inside of another property you can get by doing ``property.subProperty``.
            property_value(:obj:`str`): The value of the component expected
            assembly (:obj:`str`): The name of the assembly containing the component.
            timeout (:obj:`int`, optional): The number of seconds that it will wait for property.
            interval (:obj:`float`, optional): The number of seconds after which it will try to find the object again.
                The interval should be smaller than timeout.
            get_property_as_string (:obj:`bool`, optional): A boolean value that makes the property_value
            to be compared as a string with the property from the instrumented app.
            max_depth (:obj:`int`, optional): An integer value that defines the maximum level from which to retrieve
            properties.

        Returns:
            str: The property value is serialized to a JSON string.

        """
        return commands.WaitForComponentProperty.run(
            component_name, property_name, property_value,
            assembly, self, timeout, interval, get_property_as_string, max_depth
        )

    def get_component_property(self, component_name, property_name, assembly, max_depth=2):
        """Returns the value of the given component property.

        Args:
            component_name (:obj:`str`): The name of the component. If the component has a namespace the format should
                look like this: ``"namespace.componentName"``.
            property_name (:obj:`str`): The name of the property of which value you want. If the property is an array
                you can specify which element of the array to return by doing ``property[index]``, or if you want a
                property inside of another property you can get by doing ``property.subProperty``.
            assembly (:obj:`str`): The name of the assembly containing the component.
            maxDepth (:obj:`int`, optional): Set how deep to serialize the property. Defaults to ``2``.

        Returns:
            str: The property value is serialized to a JSON string.

        """

        return commands.GetComponentProperty.run(
            self._connection,
            component_name, property_name, assembly, max_depth, self
        )

    def set_component_property(self, component_name, property_name, assembly, value):
        """Sets a value for a given component property.

        Args:
            component_name (:obj:`str`): The name of the component. If the component has a namespace the format should
                look like this: ``"namespace.componentName"``.
            property_name (:obj:`str`): The name of the property of which value you want to set.
            assembly (:obj:`str`): The name of the assembly containing the component.
            value (:obj:`str`): The value to be set for the chosen component's property.

        Returns:
            str: The property value is serialized to a JSON string.

        """

        return commands.SetComponentProperty.run(
            self._connection,
            component_name, property_name, value, assembly, self
        )

    def call_component_method(self, component_name, method_name, assembly, parameters=None, type_of_parameters=None):
        """Invokes a method from an existing component of the object.

        Args:
            component_name (:obj:`str`): The name of the script. If the script has a namespace the format should look
                like this: ``"namespace.typeName"``.
            method_name (:obj:`str`): The name of the public method that we want to call. If the method is inside a
                static property/field to be able to call that method, methodName need to be the following format
                ``"propertyName.MethodName"``.
            assembly (:obj:`str`): The name of the assembly containing the script.
            parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.
            type_of_parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.

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
        """Returns text value from a Button, Text, InputField. This also works with TextMeshPro elements.

        Returns:
            str: The text value of the AltObject.

        """

        return commands.GetText.run(self._connection, self)

    def set_text(self, text, submit=False):
        """Sets text value for a Button, Text or InputField. This also works with TextMeshPro elements.

        Args:
            text (obj:`str`): The text to be set.
            submit (obj:`bool`): If set will trigger a submit event.

        Returns:
            AltObject: The current AltObject.

        """

        data = commands.SetText.run(self._connection, text, self, submit)
        return AltObject(self._altdriver, data)

    def pointer_up(self):
        """Simulates pointer up action on the object.

        Returns:
            AltObject: The current AltObject.

        """

        data = commands.PointerUp.run(self._connection, self)
        return AltObject(self._altdriver, data)

    def pointer_down(self):
        """Simulates pointer down action on the object.

        Returns:
            AltObject: The current AltObject.

        """

        data = commands.PointerDown.run(self._connection, self)
        return AltObject(self._altdriver, data)

    def pointer_enter(self):
        """Simulates pointer enter action on the object.

        Returns:
            AltObject: The current AltObject.

        """

        data = commands.PointerEnter.run(self._connection, self)
        return AltObject(self._altdriver, data)

    def pointer_exit(self):
        """Simulates pointer exit action on the object.

        Returns:
            AltObject: The current AltObject.

        """

        data = commands.PointerExit.run(self._connection, self)
        return AltObject(self._altdriver, data)

    def tap(self, count=1, interval=0.1, wait=True):
        """Taps the current object.

        Args:
            count (:obj:`int`, optional): Number of taps. Defaults to ``1``.
            interval (:obj:`int`, :obj:`float`, optional): Interval between taps in seconds. Defaults to ``0.1``.
            wait (:obj:`int`, optional): Wait for command to finish. Defaults to ``True``.

        Returns:
            AltObject: The tapped object.

        """

        data = commands.TapElement.run(
            self._connection,
            self, count, interval, wait
        )
        return AltObject(self._altdriver, data)

    def click(self, count=1, interval=0.1, wait=True):
        """Clicks the current object.

        Args:
            count (:obj:`int`, optional): Number of clicks. Defaults to ``1``.
            interval (:obj:`int`, :obj:`float`, optional): Interval between clicks in seconds. Defaults to ``0.1``.
            wait (:obj:`int`, optional): Wait for command to finish. Defaults to ``True``.

        Returns:
            AltObject: The clicked object.

        """

        data = commands.ClickElement.run(
            self._connection,
            self, count, interval, wait
        )
        return AltObject(self._altdriver, data)
