"""
    Copyright(C) 2025 Altom Consulting

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
import alttester.exceptions as exceptions
from alttester.altby import AltBy
from alttester.by import By
from alttester.altobject_base import AltObjectBase


class AltObject(AltObjectBase):

    def __init__(self, altdriver, data):
        super().__init__(altdriver, data)

    def __repr__(self):
        return f"AltObject({repr(self._altdriver)}, {repr(self.to_json())})"

    def __str__(self):
        return json.dumps(self.to_json())

    def to_json(self) -> dict:
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
            "idCamera": self.idCamera,
        }

    def update_object(self) -> "AltObject":
        return super().update_object()

    def find_object(
        self, altby: AltBy, camera_altby: AltBy = AltBy.name(""), enabled=True
    ) -> "AltObject":
        return super().find_object_from_object(
            altby.by, altby.value, camera_altby.by, camera_altby.value, enabled
        )

    def get_visual_element_property(self, property_name: str) -> str:
        """Gets a value for a given visual element property.

        Args:
            property_name: The name of the property of which value you want to get.

        Returns:
            The property value is serialized to a JSON string.

        Raises:
            WrongAltObjectTypeException: The method is called on an object that is not a VisualElement.
        """
        if self.type != "UIToolkit":
            raise exceptions.WrongAltObjectTypeException(
                "This method is only available for VisualElement objects")
        return commands.GetVisualElementProperty.run(self._connection, property_name, self)

    def wait_for_visual_element_property(self, property_name,
                                         property_value, timeout=20, interval=0.5,
                                         get_property_as_string=False):
        """Waits until a property of the current object has a specific value.

        Args:
            property_name (str): The name of the property of which value you want to get.
            property_value (str): The value of the property expected.
            timeout (int, optional): The number of seconds that it will wait for property. Defaults to 20.
            interval (int, optional): Time in seconds before retrying. Defaults to 0.5.
            get_property_as_string (bool, optional): A boolean value that makes the property_value
                to be compared as a string with the property from the instrumented app. Defaults to False.

        Returns:
            The property value is serialized to a JSON string.

        Raises:
            WrongAltObjectTypeException: The method is called on an object that is not a VisualElement.
        """
        if self.type != "UIToolkit":
            raise exceptions.WrongAltObjectTypeException(
                "This method is only available for VisualElement objects")
        return commands.WaitForVisualElementProperty.run(
            property_name, property_value, self, timeout, interval, get_property_as_string)
