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

import warnings
from alttester.altobject import AltObject
from alttester.altdriver import AltDriver
from alttester.altby import AltBy
from alttester.altdriver_base import AltDriverBase

warnings.filterwarnings("default", category=DeprecationWarning, module=__name__)


class AltDriverUnity(AltDriverBase):
    """The driver object will help interacting with all the application objects, their properties and methods.

    When you instantiate an ``AltDriverUnity`` object in your tests, you can use it to “drive” your application like
    one of your users would, by interacting with all the application objects, their properties and methods.
    An ``AltDriverUnity`` instance will connect to the AltTester® Server.

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

    def find_object(
        self, altby: AltBy, camera_altby: AltBy = AltBy.name(""), enabled: bool = True
    ) -> AltObject:
        # """Finds an object in the scene.

        # Args:
        #     altby (:obj:`AltBy`): The search criteria.
        #     camera_altby (:obj:`AltBy`, optional): The camera search criteria.
        #     enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        # Returns:
        #     :obj:`AltObject`: The object found.

        # Raises:
        #     :obj:`Exception`: If the object is not found.

        # """
        return self._AltDriverBase__find_object(altby, camera_altby, enabled)

    def find_objects(
        self, altby: AltBy, camera_altby: AltBy = AltBy.name(""), enabled: bool = True
    ) -> list:
        """Finds all objects in the scene.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            camera_altby (:obj:`AltBy`, optional): The camera search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`list`: The list of objects found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        return self._AltDriverBase__find_objects(altby, camera_altby, enabled)

    def find_object_which_contains(
        self, altby: AltBy, camera_altby: AltBy = AltBy.name(""), enabled: bool = True
    ) -> AltObject:
        """Finds an object in the scene which contains the search criteria.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            camera_altby (:obj:`AltBy`, optional): The camera search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`AltObject`: The object found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        return self._AltDriverBase__find_object_which_contains(altby, camera_altby, enabled)

    def find_objects_which_contain(
        self, altby: AltBy, camera_altby: AltBy = AltBy.name(""), enabled: bool = True
    ) -> list:
        """Finds all objects in the scene which contain the search criteria.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            camera_altby (:obj:`AltBy`, optional): The camera search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.

        Returns:
            :obj:`list`: The list of objects found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        return self._AltDriverBase__find_objects_which_contain(altby, camera_altby, enabled)

    def wait_for_object(
        self,
        altby: AltBy,
        camera_altby: AltBy = AltBy.name(""),
        timeout: int = 20,
        interval: int = 0.5,
        enabled: bool = True
    ) -> AltObject:
        """Waits for an object to be found in the scene.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            camera_altby (:obj:`AltBy`, optional): The camera search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.
            timeout (:obj:`int`, optional): The timeout duration for the search, in seconds.

        Returns:
            :obj:`AltObject`: The object found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        return self._AltDriverBase__wait_for_object(altby, camera_altby, timeout, interval, enabled)

    def wait_for_object_which_contains(
        self,
        altby: AltBy,
        camera_altby: AltBy = AltBy.name(""),
        timeout: int = 20,
        interval: int = 0.5,
        enabled: bool = True
    ) -> AltObject:
        """Waits for an object to be found in the scene which contains the search criteria.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            camera_altby (:obj:`AltBy`, optional): The camera search criteria.
            enabled (:obj:`bool`, optional): If set to ``True``, the search will only return enabled objects.
            timeout (:obj:`int`, optional): The timeout duration for the search, in seconds.

        Returns:
            :obj:`AltObject`: The object found.

        Raises:
            :obj:`Exception`: If the object is not found.

        """
        return self._AltDriverBase__wait_for_object_which_contains(altby, camera_altby, enabled, timeout, interval)

    def wait_for_object_to_not_be_present(
        self,
        altby: AltBy,
        camera_altby: AltBy = AltBy.name(""),
        timeout: int = 20,
        interval: int = 0.5,
        enabled: bool = True,
    ) -> None:
        """Waits for an object not to be present in the scene.

        Args:
            altby (:obj:`AltBy`): The search criteria.
            camera_altby (:obj:`AltBy`, optional): The camera search criteria.
            timeout (:obj:`int`, optional): The timeout duration for the search, in seconds.

        Raises:
            :obj:`Exception`: If the object is found.

        """
        return self._AltDriverBase__wait_for_object_to_not_be_present(altby, camera_altby, timeout, interval, enabled)

    def get_all_elements(self, camera_altby: AltBy = AltBy.name(""), enabled: bool = True) -> list:
        """Returns all the elements in the scene.

        Args:
            camera_altby (:obj:`AltBy`, optional): The camera search criteria.

        Returns:
            :obj:`list`: The list of objects found.

        """
        return self._AltDriverBase__get_all_elements(camera_altby, enabled)