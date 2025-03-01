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

from alttester.altobject import AltObject
from alttester.altby import AltBy
from alttester.by import By


class AltObjectUnreal(AltObject):
    """The AltObjectUnreal class represents an object present in the Unreal application and it allows you to interact with it.

    It is the return type of the methods in the “find_*” category from the AltDriverUnreal class.
    """

    def __init__(self, altdriver, data):
        super().__init__(altdriver, data)

    def __repr__(self):
        return f"AltObjectUnreal({repr(self._altdriver)}, {repr(self.to_json())})"

    def __str__(self):
        return super().__str__()

    def to_json(self) -> dict:
        return super().to_json()

    def get_component_property(self, component_name, property_name):
        """Returns the value of the given component property.

        Args:
            component_name (:obj:`str`): The name of the component.
            property_name (:obj:`str`): The name of the property of which value you want.

        Returns:
            str: The property value is serialized to a JSON string.

        """
        return super().get_component_property(component_name, property_name, "")

    def wait_for_component_property(
        self,
        component_name,
        property_name,
        property_value,
        timeout=20,
        interval=0.5,
        get_property_as_string=False,
    ):
        """Wait until a property has a specific value and returns the value of the given component property.

        Args:
            component_name (:obj:`str`): The name of the component.
            property_name (:obj:`str`): The name of the property of which value you want.
            property_value(:obj:`str`): The value of the component expected.
            timeout (:obj:`int`, optional): The number of seconds that it will wait for property.
            interval (:obj:`float`, optional): The number of seconds after which it will try to find the object again.
            get_property_as_string (:obj:`bool`, optional): A boolean value that makes the property_value
            to be compared as a string with the property from the instrumented app.

        Returns:
            str: The property value is serialized to a JSON string.

        """
        return super().wait_for_component_property(
            component_name,
            property_name,
            property_value,
            "",
            timeout,
            interval,
            get_property_as_string,
        )

    def set_component_property(self, component_name, property_name, value):
        """Sets a value for a given component property.

        Args:
            component_name (:obj:`str`): The name of the component.
            property_name (:obj:`str`): The name of the property of which value you want to set.
            value (:obj:`str`): The value to be set for the chosen component's property.

        Returns:
            str: The property value is serialized to a JSON string.

        """
        return super().set_component_property(component_name, property_name, value, "")

    def call_component_method(
        self, component_name, method_name, parameters=None, type_of_parameters=None
    ):
        """Invokes a method from an existing component of the object.

        Args:
            component_name (:obj:`str`): The name of the script.
            method_name (:obj:`str`): The name of the public method that we want to call.
            parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.
            type_of_parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.

        Return:
            str: The value returned by the method is serialized to a JSON string.

        """
        return super().call_component_method(
            component_name, method_name, "", parameters, type_of_parameters
        )

    def set_text(self, text) -> "AltObjectUnreal":
        """Sets text value for a Button, Text or InputField.

        Args:
            text (obj:`str`): The text to be set.

        Returns:
            AltObjectUnreal: The current AltObjectUnreal.

        """
        data = super().set_text(text, False)
        return AltObjectUnreal(self._altdriver, data.to_json())

    def find_object(self, altby: AltBy, enabled=True) -> "AltObjectUnreal":
        return AltObjectUnreal(
            super().find_object_from_object(altby.by, altby.value, By.NAME, "", enabled)
        )
