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

import warnings
import alttester.commands as commands
from alttester.commands.base_command import Command
from alttester.__version__ import VERSION
from alttester._websocket import (
    CommandHandler,
    NotificationHandler,
)
from alttester.altobject import AltObject
from alttester.by import By
from alttester.altby import AltBy
from alttester.altdriver_base import AltDriverBase


warnings.filterwarnings("default", category=DeprecationWarning, module=__name__)


class AltDriver(AltDriverBase):

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
        self.__command_handler = CommandHandler()
        self.__notification_handler = NotificationHandler()

    def find_object(self, by, value, camera_by=By.NAME, camera_value="", enabled=True):
        """
        Find an object in the scene using the specified locator strategy and value.

        :param by: The locator strategy to use.
        :param value: The value to search for.
        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :param enabled: The object enabled state.
        :return: The AltObject instance.
        """
        altby = AltBy(by, value)
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__find_object(altby, camera_altby, enabled)

    def find_objects(self, by, value, camera_by=By.NAME, camera_value="", enabled=True):
        """
        Find objects in the scene using the specified locator strategy and value.

        :param by: The locator strategy to use.
        :param value: The value to search for.
        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :param enabled: The object enabled state.
        :return: The list of AltObject instances.
        """
        altby = AltBy(by, value)
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__find_objects(altby, camera_altby, enabled)

    def find_object_which_contains(
        self, by, value, camera_by=By.NAME, camera_value="", enabled=True
    ):
        """
        Find an object in the scene which contains the specified value.

        :param by: The locator strategy to use.
        :param value: The value to search for.
        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :param enabled: The object enabled state.
        :return: The AltObject instance.
        """
        altby = AltBy(by, value)
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__find_object_which_contains(
            altby, camera_altby, enabled
        )

    def find_objects_which_contain(
        self, by, value, camera_by=By.NAME, camera_value="", enabled=True
    ):
        """
        Find objects in the scene which contain the specified value.

        :param by: The locator strategy to use.
        :param value: The value to search for.
        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :param enabled: The object enabled state.
        :return: The list of AltObject instances.
        """
        altby = AltBy(by, value)
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__find_objects_which_contain(
            altby, camera_altby, enabled
        )

    def wait_for_object(
        self,
        by,
        value,
        camera_by=By.NAME,
        camera_value="",
        timeout=20,
        interval=0.5,
        enabled=True,
    ):
        """
        Wait for an object in the scene using the specified locator strategy and value.

        :param by: The locator strategy to use.
        :param value: The value to search for.
        :param timeout: The timeout in seconds.
        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :param enabled: The object enabled state.
        :return: The AltObject instance.
        """
        altby = AltBy(by, value)
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__wait_for_object(
            altby, camera_altby, timeout, interval, enabled
        )

    def wait_for_object_which_contains(
        self,
        by,
        value,
        camera_by=By.NAME,
        camera_value="",
        timeout=20,
        interval=0.5,
        enabled=True,
    ):
        """
        Wait for an object in the scene which contains the specified value.

        :param by: The locator strategy to use.
        :param value: The value to search for.
        :param timeout: The timeout in seconds.
        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :param enabled: The object enabled state.
        :return: The AltObject instance.
        """
        altby = AltBy(by, value)
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__wait_for_object_which_contains(
            altby, camera_altby, timeout, interval, enabled
        )

    def wait_for_object_to_not_be_present(
        self,
        by,
        value,
        camera_by=By.NAME,
        camera_value="",
        timeout=20,
        interval=0.5,
        enabled=True,
    ):
        """
        Wait for an object to not be present in the scene using the specified locator strategy and value.

        :param by: The locator strategy to use.
        :param value: The value to search for.
        :param timeout: The timeout in seconds.
        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :param enabled: The object enabled state.
        :return: The AltObject instance.
        """
        altby = AltBy(by, value)
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__wait_for_object_to_not_be_present(
            altby, camera_altby, timeout, interval, enabled
        )

    def get_all_elements(self, camera_by=By.NAME, camera_value="", enabled=True):
        """
        Get all elements in the scene.

        :param camera_by: The locator strategy to use for the camera.
        :param camera_value: The value to search for the camera.
        :return: The list of AltObject instances.
        """
        camera_altby = AltBy(camera_by, camera_value)
        return self._AltDriverBase__get_all_elements(camera_altby, enabled)
