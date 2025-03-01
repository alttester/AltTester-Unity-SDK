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

from alttester.altobject_unreal import AltObjectUnreal
from alttester.altdriver_base import AltDriverBase
from alttester.altby import AltBy


class AltDriverUnreal(AltDriverBase):
    """The driver object will help interacting with all the application objects, their properties and methods.

    When you instantiate an ``AltDriverUnreal`` object in your tests, you can use it to “drive” your application like
    one of your users would, by interacting with all the application objects, their properties and methods.
    An ``AltDriverUnreal`` instance will connect to the AltTester® Server.

    Args:
        host (:obj:`str`, optional): The host to connect to.
        port (:obj:`int`, optional): The port to connect to.
        app_name (:obj:`str`, optional): The application name. Defaults to ``__default__``.
        enable_logging (:obj:`bool`, optional): If set to ``True`` will turn on logging, by default logging is disabled.
        timeout (:obj:`int`, :obj:`float`, optional): The timeout duration for establishing a connection, in seconds.
            If set to ``None``, the connection attempt will wait indefinitely. The default value is ``60`` seconds.

    """

    def __init__(
        self,
        host="127.0.0.1",
        port=13000,
        app_name="__default__",
        enable_logging=False,
        timeout=60,
        platform="unknown",
        platform_version="unknown",
        device_instance_id="unknown",
        app_id="unknown",
    ):
        super().__init__(
            host,
            port,
            app_name,
            enable_logging,
            timeout,
            platform,
            platform_version,
            device_instance_id,
            app_id,
        )

    def find_object(self, altby: AltBy, enabled: bool = True) -> AltObjectUnreal:
        """Finds an object in the scene.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`AltObjectUnreal`: The object found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        alt_object = super().find_object(altby, enabled=enabled)
        return AltObjectUnreal(self, alt_object.to_json())

    def find_objects(self, altby: AltBy, enabled: bool = True) -> list:
        """Finds all objects in the scene.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`list`: The list of objects found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        alt_objects = super().find_objects(altby, enabled=enabled)
        return [AltObjectUnreal(self, obj.to_json()) for obj in alt_objects]

    def wait_for_object(
        self,
        altby: AltBy,
        timeout: int = 20,
        interval: int = 0.5,
        enabled: bool = True
    ) -> AltObjectUnreal:
        """Waits for an object to be found in the scene.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.
            timeout (:obj:`int`, optional): The timeout duration for the search, in seconds.

        Returns:
            :obj:`AltObjectUnreal`: The object found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        alt_object = super().wait_for_object(
            altby, timeout=timeout, interval=interval, enabled=enabled
        )
        return AltObjectUnreal(self, alt_object.to_json())

    def get_all_elements(self, enabled: bool = True) -> list:
        """Returns all the elements in the scene.

        Args:
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`list`: The list of objects found.

        """
        alt_objects = super().get_all_elements(enabled=enabled)
        return [AltObjectUnreal(self, obj.to_json()) for obj in alt_objects]

    def find_object_which_contains(self, altby: AltBy, enabled: bool = True) -> AltObjectUnreal:
        """Finds an object in the scene which contains the search criteria.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`AltObjectUnreal`: The object found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        alt_object = super().find_object_which_contains(altby, enabled=enabled)
        return AltObjectUnreal(self, alt_object.to_json())

    def find_objects_which_contain(self, altby: AltBy, enabled: bool = True) -> list:
        """Finds all objects in the scene which contain the search criteria.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`list`: The list of objects found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        alt_objects = super().find_objects_which_contain(altby, enabled=enabled)
        return [AltObjectUnreal(self, obj.to_json()) for obj in alt_objects]

    def wait_for_object_which_contains(
        self,
        altby: AltBy,
        timeout: int = 20,
        interval: int = 0.5,
        enabled: bool = True
    ) -> AltObjectUnreal:
        """Waits for an object to be found in the scene which contains the search criteria.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.
            timeout (:obj:`int`, optional): The timeout duration for the search, in seconds.

        Returns:
            :obj:`AltObjectUnreal`: The object found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        alt_object = super().wait_for_object_which_contains(
            altby, timeout=timeout, interval=interval, enabled=enabled
        )
        return AltObjectUnreal(self, alt_object.to_json())

    def wait_for_object_to_not_be_present(
        self,
        altby: AltBy,
        timeout: int = 20,
        interval: int = 0.5,
        enabled: bool = True,
    ) -> None:
        """Waits for an object not to be present in the scene.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            timeout (:obj:`int`, optional): The timeout duration for the search, in seconds.

        Raises:
            :obj:`Exception`: If the object is found.

        """
        return super().wait_for_object_to_not_be_present(
            altby, timeout=timeout, interval=interval, enabled=enabled
        )

    def key_down(self, key_code, power=1):
        """Simulates that a specific key was pressed without taking into consideration the duration of the press.

        Args:
            key_code (:obj:`AltKeyCode`): The key code of the key simulated to be pressed.
            power (:obj:`float`, optional): A value between [-1,1] used for joysticks to indicate how hard the button
                was pressed. Defaults to ``1``.

        """
        super().key_down(key_code, power)

    def keys_down(self, key_codes, power=1):
        """Simulates that multiple keys were pressed without taking into consideration the duration of the press.

        Args:
            key_codes (:obj:`list` of :obj:`AltKeyCode`): The key codes of the keys simulated to be pressed.
            power (:obj:`float`): A value between [-1,1] used for joysticks to indicate how hard the buttons were
                pressed. Defaults to ``1``.

        """
        super().keys_down(key_codes, power)

    def press_key(self, key_code, power=1, duration=0.1, wait=True):
        """Simulates key press action in your application.

        Args:
            key_code (:obj:`AltKeyCode`): The key code of the key simulated to be pressed.
            power (:obj:`int`, :obj:`float`, optional): A value between [-1,1] used for joysticks to indicate how hard
                the button was pressed. Defaults to ``1``.
            duration (:obj:`float`, optional): The time measured in seconds from the key press to the key release.
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.

        """
        super().press_key(key_code, power, duration, wait)

    def press_keys(self, key_codes, power=1, duration=0.1, wait=True):
        """Simulates multiple keypress action in your application.

        Args:
            key_codes (:obj:`list` of :obj:`AltKeyCode`): The key codes of the keys simulated to be pressed.
            power (:obj:`float`): A value between [-1,1] used for joysticks to indicate how hard the buttons were
                pressed. Defaults to ``1``.
            duration (:obj:`float`): The time measured in seconds from the key press to the key release.
            wait (:obj:`bool`): If set wait for command to finish. Defaults to ``True``.

        """
        super().press_keys(key_codes, power, duration, wait)

    def load_level(self, level_name):
        """Loads a level.

        Args:
            level_name (:obj:`str`): The name of the level to be loaded.

        """
        super().load_scene(level_name)

    def get_current_level(self):
        """Returns the name of the current level.

        Returns:
            str: The name of the current level.

        """
        return super().get_current_scene()

    def wait_for_current_level_to_be(self, level_name, timeout=10, interval=1):
        """Waits for the level to be loaded for a specified amount of time.

        Args:
            level_name (:obj:`str`): The name of the level to wait for.
            timeout (obj:`int`, :obj:`float`, optional): The time measured in seconds to wait for the specified level.
                Defaults to ``10``.
            interval (obj:`int`, :obj:`float`, optional): How often to check that the level was loaded in the given
                timeout. Defaults to ``1``.

        Returns:
            str: The name of the loaded level.

        """
        super().wait_for_current_scene_to_be(level_name, timeout, interval)

    def get_global_time_dilation(self):
        """Returns the value of the global time dilation.

        Returns:
            float: The value of the global time dilation.

        """
        return super().get_time_scale()

    def set_global_time_dilation(self, time_dilation):
        """Sets the value of the global time dilation.

        Args:
            time_dilation (:obj:`float`, :obj:`int`): The value of the global time dilation.

        """
        super().set_time_scale(time_dilation)

    def call_static_method(self, type_name, method_name, parameters=None, type_of_parameters=None):
        """Invoke a static method from your application.

        Args:
            type_name (:obj:`str`): The name of the script. If the script has a namespace the format should look like
                this: ``"namespace.typeName"``.
            method_name (:obj:`str`): The name of the public method that we want to call. If the method is inside a
                static property/field to be able to call that method, methodName need to be the following format
                ``"propertyName.MethodName"``.
            parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.
            type_of_parameters (:obj:`list`, :obj:`tuple`, optional): Defaults to ``None``.

        Return:
            str: The value returned by the method is serialized to a JSON string.

        """
        return super().call_static_method(type_name, method_name, "", parameters, type_of_parameters)
