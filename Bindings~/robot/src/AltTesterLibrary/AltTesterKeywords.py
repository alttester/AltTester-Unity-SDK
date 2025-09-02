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

from alttester import AltDriver, AltObject, AltReversePortForwarding
from alttester import By, AltKeyCode, PlayerPrefKeyType, AltLogger, AltLogLevel
from alttester.commands.Notifications.notification_type import NotificationType
from loguru import logger
from robot.libraries.BuiltIn import BuiltIn


class AltTesterKeywords(object):

    DEFAULT_WAIT = 20

    def __init__(self):
        self._driver = None

    def initialize_altdriver(
        self,
        host="127.0.0.1",
        port=13000,
        app_name="__default__",
        enable_logging=False,
        timeout=60,
        platform="unknown",
        platform_version="unknown",
        device_instance_id="unknown",
        app_id="unknown"
    ):
        """Initialize AltDriver and return it.

        `host` : The host to connect to. The default value is "127.0.0.1".

        `port` : The port to connect to. The default value is 13000.

        `app_name` : The name of the Unity application. The default value is ``__default__``.

        `enable_logging` : If set to ``True`` will turn on logging, by default logging is disabled.

        `timeout` : The connect timeout in seconds. The default value is 60.

        `platform` : The platform of the device. The default value is ``unknown``.

        `platform_version` : The version of the platform. The default value is ``unknown``.

        `device_instance_id` : The id of the device. The default value is ``unknown``.

        `app_id` : The id of the application. The default value is ``unknown``.

        Example:

        | ${altDriver}= | Initialize AltDriver | 127.0.0.1 | 15001

        | ${altDriver}= | Initialize AltDriver | platform="Android"

        """
        self._driver = AltDriver(
            host=host,
            port=port,
            app_name=app_name,
            enable_logging=enable_logging,
            timeout=timeout,
            platform=platform,
            platform_version=platform_version,
            device_instance_id=device_instance_id,
            app_id=app_id
        )
        return self._driver

    def stop_altdriver(self):
        """Close the connection to AltTester.

        Example:

        | Stop AltDriver
        """
        self._driver.stop()

    def get_command_response_timeout(self):
        """Gets the current command response timeout for the AltTester® connection.

        Example:

        | ${timeout}= | Get Command Response Timeout
        """
        return self._driver.get_command_response_timeout()

    def set_command_response_timeout(self, timeout):
        """Sets the command response timeout for the AltTester® connection.

        timeout : The new command response timeout in seconds.

        Example:

        Set Command Response Timeout to 30 seconds.

        | Set Command Response Timeout | 30
        """
        self._driver.set_command_response_timeout(timeout)

    def enable_loguru_logger(self, logger_name):
        """Enable the specified Loguru logger.

        logger_name: The name of the logger.

        Example:

        | Enable Loguru Logger | alttester
        """
        logger.enable(logger_name)

    def disable_loguru_logger(self, logger_name):
        """Disable the specified Loguru logger.

        logger_name: The name of the logger.

        Example:

        | Disable Loguru Logger | alttester
        """
        logger.disable(logger_name)

    def reverse_port_forwarding_android(self, device_port=13000, local_port=13000, device_id=""):
        """This method calls adb reverse [-s {deviceId}] tcp:{remotePort} tcp:{localPort}.

        device_port : The port of the device to do reverse port forwarding to. The default value is ``13000``.

        local_port : The local port to do reverse port forwarding to. The default value is ``13000``.

        device_id : The id of the device.

        Example:

        | Reverse Port Forwarding Android | device_port=15500
        """
        AltReversePortForwarding.reverse_port_forwarding_android(
            device_port, local_port, device_id)

    def remove_reverse_port_forwarding_android(self, device_port=13000, device_id=""):
        """This method calls adb reverse --remove [-s {deviceId}] tcp:{devicePort} or adb reverse --remove-all if no port is provided.

        device_port : The device port to be removed. The default value is ``13000``.

        device_id : The id of the device.

        Example:

        | Remove Reverse Port Forwarding Android | device_port=15500
        """
        AltReversePortForwarding.remove_reverse_port_forwarding_android(
            device_port, device_id)

    def remove_all_reverse_port_forwardings_android(self):
        """This method calls adb reverse --remove-all.

        Example:

        | Remove All Reverse Port Forwardings Android
        """
        AltReversePortForwarding.remove_all_reverse_port_forwardings_android()

    def add_notification_listener(self, notification_type, callback, overwrite=True):
        """Adds a notification listener for the specified notification type.

        `notification_type` : The type of notification to listen for (e.g., LOADSCENE).
        `callback` : The callback function to be called when the notification is triggered.
        `overwrite` : If True, will overwrite any existing listener for the same notification type. Default is True.

        Example:
        | Add Notification Listener | LOADSCENE | ${callback} | overwrite=${True}
        """

        def keyword_runner(*args):
            # When the notification fires, this function is called.
            # It then uses Robot's BuiltIn library to execute the keyword
            # using the name we stored in the 'callback' variable.
            BuiltIn().run_keyword(callback, *args)

        self._driver.add_notification_listener(
            self.get_notification_type_enum(notification_type), keyword_runner, overwrite)

    def remove_notification_listener(self, notification_type):
        """Removes the notification listener for the specified notification type.

        `notification_type` : The type of notification to remove (e.g., LOADSCENE).

        Example:
        | Remove Notification Listener | LOADSCENE |
        """
        self._driver.remove_notification_listener(
            self.get_notification_type_enum(notification_type))

    def find_object(self, locator_strategy,
                    locator, camera_by="NAME", camera_value="", enabled=True):
        """Finds the first object in the scene that respects the given criteria.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT.

        `locator` : The actual locator value.

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT. The default value is ``NAME``

        `camera_value` : The actual camera value.The default value is ``""``

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects.The default value is ``True``

        Example:

        Find Object by PATH

        | ${logo}= | Find Object | PATH | //Canvas//Logo | enabled=${False}
        """
        return self._driver.find_object(self.get_by_enum(locator_strategy), locator,
                                        camera_by=self.get_by_enum(camera_by),
                                        camera_value=camera_value, enabled=enabled)

    def find_objects(self, locator_strategy,
                     locator, camera_by="NAME", camera_value="", enabled=True):
        """Finds all objects in the scene that respect the given criteria.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT.

        `locator` : The actual locator value.

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT.The default value is ``NAME``

        `camera_value` : The actual camera value. The default value is ``""``

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects.The default value is ``True``

        Example:

        Find Objects by PATH

        | ${logo}= | Find Objects | PATH | //Canvas//Logo | enabled=${False} 
        """
        return self._driver.find_objects(self.get_by_enum(locator_strategy), locator,
                                         camera_by=self.get_by_enum(camera_by),
                                         camera_value=camera_value, enabled=enabled)

    def find_object_which_contains(self, locator_strategy,
                                   locator, camera_by="NAME", camera_value="", enabled=True):
        """Finds the first object in the scene that respects the given criteria.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT.

        `locator` : The actual locator value.

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT.The default value is ``NAME``

        `camera_value` : The actual camera value.The default value is ``""``

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects.The default value is ``True``

        Example:

        Find Object Which Contains in Text

        | ${logo}= | Find Object Which Contains | TEXT | Logo | enabled=${False} 
        """
        return self._driver.find_object_which_contains(self.get_by_enum(locator_strategy), locator,
                                                       camera_by=self.get_by_enum(
                                                           camera_by),
                                                       camera_value=camera_value, enabled=enabled)

    def find_objects_which_contain(self, locator_strategy,
                                   locator, camera_by="NAME", camera_value="", enabled=True):
        """Finds all objects in the scene that respect the given criteria.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT.

        `locator` : The actual locator value.

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT. The default value is ``NAME``

        `camera_value` : The actual camera value.The default value is ``""``

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects.The default value is ``True``

        Example:

        Find Objects Which Contain in Text

        | ${logo}= | Find Objects Which Contain | TEXT | Logo | enabled=${False} 
        """
        return self._driver.find_objects_which_contain(self.get_by_enum(locator_strategy), locator,
                                                       camera_by=self.get_by_enum(
                                                           camera_by),
                                                       camera_value=camera_value, enabled=enabled)

    def find_object_at_coordinates(self, coordinates):
        """Retrieves the Unity object at given coordinates or  ``None`` otherwise.

        `coordinates` : The screen coordinates.

        Example:

        Find Object at coordinates [20, 20].

        | ${coordinates}= | Create List | ${20} | ${20}

        | ${object}= | Find Object At Coordinates | ${coordinates}

        """
        return self._driver.find_object_at_coordinates(coordinates)

    def find_object_from_object(self, alt_object: AltObject, locator_strategy,
                                locator, camera_by="NAME", camera_value="", enabled=True):
        """Finds the child of the object on which it is called that respects the given criteria.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT.

        `locator` : The actual locator value.

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT. The default value is ``NAME``

        `camera_value` : The actual camera value.The default value is ``""``

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects.The default value is ``True``

        Example:

        Find the child of the Canvas object by the name UIButton

        | ${object}= | Find Object | NAME | Canvas

        | ${child}= | Get Object From Object | ${object} | By.NAME | UIButton
        """
        return alt_object.find_object_from_object(self.get_by_enum(locator_strategy), locator,
                                                  camera_by=self.get_by_enum(
                                                      camera_by),
                                                  camera_value=camera_value, enabled=enabled)

    def get_all_elements(self, camera_by="NAME", camera_value="", enabled=True):
        """Returns information about every object loaded in the currently loaded scenes. This also means objects that
        are set as DontDestroyOnLoad.

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT. The default value is ``NAME``

        `camera_value` : The actual camera value. The default value is ``""``

        `enable` : If true will match only objects that are active in hierarchy. If false will match all objects.The default value is ``True``

        Example:

        Get All Elements

        | ${elements}= | Get All Elements | enabled=${False} 
        """
        return self._driver.get_all_elements(self.get_by_enum(camera_by), camera_value=camera_value, enabled=enabled)

    def wait_for_object(self, locator_strategy,
                        locator, camera_by="NAME", camera_value="", timeout=DEFAULT_WAIT, interval=0.5, enabled=True):
        """Wait for an object using a locator strategy and locator value, then return it.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT

        `locator` the actual locator value

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT. The default value is ``NAME``

        `camera_value` : The actual camera value. The default value is ``""``

        `timeout` : How long to wait for the object to appear. The default value is 20 seconds.

        `interval` : : The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5.

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects.The default value is ``True``

        Example:

        Wait For Object by PATH

        | ${logo}= | Wait for Object | PATH | //Canvas//Logo | timeout=5 

        """
        return self._driver.wait_for_object(self.get_by_enum(locator_strategy), locator,
                                            camera_by=self.get_by_enum(camera_by), camera_value=camera_value,
                                            timeout=timeout, interval=interval, enabled=enabled)

    def wait_for_object_which_contains(self, locator_strategy,
                                       locator, camera_by="NAME", camera_value="", timeout=DEFAULT_WAIT, interval=0.5, enabled=True):
        """Wait for an object using a locator strategy and locator value, then return it.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT

        `locator` : the actual locator value

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT. The default value is ``NAME``

        `camera_value` : The actual camera value. The default value is ``""``

        `timeout` : How long to wait for the object to appear. The default value is 20 seconds.

        `interval` : The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5.

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects. The default value is ``True``

        Example:

        Wait For Object Which Contains TEXT

        | ${logo}= | Wait for object Which Contains | TEXT | Logo | timeout=5 | enabled=${False}
        """
        return self._driver.wait_for_object_which_contains(self.get_by_enum(locator_strategy), locator,
                                                           camera_by=self.get_by_enum(camera_by), camera_value=camera_value,
                                                           timeout=timeout, interval=interval, enabled=enabled)

    def wait_for_object_to_not_be_present(self, locator_strategy,
                                          locator, camera_by="NAME", camera_value="", timeout=DEFAULT_WAIT, interval=0.5, enabled=True):
        """Waits until the object in the scene that respects the given criteria is no longer in the scene or until the timeout limit is reached.

        `locator_strategy` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT

        `locator` : the actual locator value

        `camera_by` one of the following: ID, NAME, PATH, LAYER,
        COMPONENT, TAG, TEXT. The default value is ``NAME``

        `camera_value` : The actual camera value. The default value is ``""``

        `timeout` : How long to wait for the object to appear. The default value is 20 seconds.

        `interval` : The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5.

        `enabled` : If true will match only objects that are active in hierarchy. If false will match all objects. The default value is ``True``

        Example:

        Wait For Object To Not Be Present with TEXT

        | ${logo}= | Wait For Object To Not Be Present | TEXT | Logo | timeout=5
        """
        self._driver.wait_for_object_to_not_be_present(self.get_by_enum(locator_strategy), locator,
                                                       camera_by=self.get_by_enum(camera_by), camera_value=camera_value,
                                                       timeout=timeout, interval=interval, enabled=enabled)

    def get_delay_after_command(self):
        """Gets the current delay after a command.

        Example:

        Get Delay After Command

        | ${delay}= | Get Delay After Command
        """
        return self._driver.get_delay_after_command()

    def set_delay_after_command(self, delay):
        """Sets the delay after a command.

        Example:

        Set Delay After Command to 10 seconds.

        | Set Delay After Command | 10
        """
        self._driver.set_delay_after_command(delay)

    def key_down(self,  key_code, power=1):
        """Simulates that a specific key was pressed without taking into consideration the duration of the press.

        key_code : The key code of the key simulated to be pressed.

        power : A value between [-1,1] used for joysticks to indicate how hard the button
            was pressed. Default value is ``1``.

        Example:

        Press Key A Down

        | Key Down | A
        """
        try:
            self._driver.key_down(getattr(AltKeyCode, key_code), power=power)
        except AttributeError:
            raise ValueError(
                "Invalid kay code for: {code}".format(code=key_code))

    def keys_down(self,  key_codes, power=1):
        """Simulates that multiple keys were pressed without taking into consideration the duration of the press.

        key_codes : The key codes of the keys simulated to be pressed.

        power : A value between [-1,1] used for joysticks to indicate how hard the button
            was pressed. Default value is ``1``.

        Example:

        Press Keys A and B Down

        | ${keys}=  | Create List | A | B

        | Keys Down | ${keys}
        """
        list = []
        for key in key_codes:
            try:
                list.append(getattr(AltKeyCode, key))
            except AttributeError:
                raise ValueError(
                    "Invalid kay code for: {code}".format(code=key))
        self._driver.keys_down(list, power=power)

    def key_up(self,  key_code):
        """Simulates a key up.

        key_code : The keyCode of the key simulated to be released.

        Example:

        Press Key A Up

        | Key Up | A
        """
        try:
            self._driver.key_up(getattr(AltKeyCode, key_code))
        except AttributeError:
            raise ValueError(
                "Invalid kay code for: {code}".format(code=key_code))

    def keys_up(self,  key_codes):
        """Simulates that multiple keys were released.

        key_codes : The key codes of the keys simulated to be released.

        Example:

        Press Key A and B Up

        | ${keys}= | Create List | A | B

        | Keys Up  | ${keys}
        """
        list = []
        for key in key_codes:
            try:
                list.append(getattr(AltKeyCode, key))
            except AttributeError:
                raise ValueError(
                    "Invalid kay code for: {code}".format(code=key))
        self._driver.keys_up(list)

    def hold_button(self, coordinates, duration=0.1, wait=True):
        """Simulates holding left click button down for a specified amount of time at given coordinates.

        coordinates : The coordinates where the button is held down.

        duration : The time measured in seconds to keep the button down. Default value is``0.1``.

        wait : If set wait for command to finish. Default value is ``True``.

        Example:

        Hold Button for 1 second

        | ${coordinates}= | Create List | 20 | 20

        | Hold Button | ${coordinates} | duration=1
        """
        self._driver.hold_button(coordinates, duration=duration, wait=wait)

    def move_mouse(self, coordinates, duration=0.1, wait=True):
        """Simulates mouse movement in your application.

        coordinates : The screen coordinates.

        duration : The time measured in seconds to move the mouse from the current position to the set location. Default value is``0.1``

        wait : If set wait for command to finish. Default value is ``True``.

        Example:

        Move Mouse for 1 second and don't wait to finish.

        | ${coordinates}= | Create List | 100 | 100

        | Move Mouse | ${coordinates} | duration=1 | wait=${False}
        """
        self._driver.move_mouse(coordinates, duration=duration, wait=wait)

    def press_key(self, key_code, power=1, duration=0.1, wait=True):
        """Simulates key press action in your application.

        key_code : The key code of the key simulated to be pressed.

        power : A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Default value is ``1``.

        duration : The time measured in seconds from the key press to the key release. Default value is ``0.1``

        wait : If set wait for command to finish. Default value is ``True``.

        Example:

        Press Key Mouse0 for 1 second.

        | Press Key | Mouse0 | duration=1
        """
        try:
            self._driver.press_key(getattr(AltKeyCode, key_code), power=power,
                                   duration=duration, wait=wait)
        except AttributeError:
            raise ValueError(
                "Invalid kay code for: {code}".format(code=key_code))

    def press_keys(self, key_codes, power=1, duration=0.1, wait=True):
        """Simulates multiple keypress action in your application.

        key_codes : The key codes of the keys simulated to be pressed.

        power : A value between [-1,1] used for joysticks to indicate how hard the button was pressed. Default value is ``1``.

        duration : The time measured in seconds from the key press to the key release. Default value is ``0.1``

        wait : If set wait for command to finish. Default value is ``True``.

        Example:

        Press Keys Mouse0 and Mouse1 for 1 second.

        | ${keys}= | Create List | Mouse0 | Mouse1

        | Press Keys | ${keys} | duration=1
        """
        list = []
        for key in key_codes:
            try:
                list.append(getattr(AltKeyCode, key))
            except AttributeError:
                raise ValueError(
                    "Invalid kay code for: {code}".format(code=key))
        self._driver.press_keys(list, power=power,
                                duration=duration, wait=wait)

    def scroll(self, speed_vertical=1, duration=0.1, wait=True, speed_horizontal=1):
        """Simulate scroll mouse action in your application.

        speed_vertical : Set how fast to scroll. Positive values will scroll up and negative values will scroll down. Default value is ``1``

        duration : The duration of the scroll in seconds. Default value is ``0.1``.

        wait : If set wait for command to finish. Default value is ``True``.

        speed_horizontal : Set how fast to scroll right or left. Default value is ``1``

        Example:

        Scroll down for 1 second

        | Scroll | speed_vertical=-1 | duration=1
        """
        self._driver.scroll(speed_vertical=speed_vertical, duration=duration,
                            wait=wait, speed_horizontal=speed_horizontal)

    def swipe(self, start, end, duration=0.1, wait=True):
        """Simulates a swipe action between two points.

        start: Coordinates of the screen where the swipe begins.

        end : Coordinates of the screen where the swipe ends.

        duration : The time measured in seconds to move the mouse from start to end location. Default value is ``0.1``.

        wait : If set wait for command to finish. Default value is ``True``.

        Example:

        Swipe for 1 second without waiting for command to finish.

        | ${start_coordinates}= | Create List | 10 | 10

        | ${end_coordinates}}= | Create List | 30 | 30

        | Swipe | ${start_coordinates} | ${end_coordinates} | duration=1 | wait=${False}
        """
        self._driver.swipe(start, end, duration=duration, wait=wait)

    def multipoint_swipe(self, positions, duration=0.1, wait=True):
        """Simulates a multipoint swipe action.

        positions : A list of positions on the screen where the swipe be made.

        duration : The time measured in seconds to swipe from the first position to the last position. Default value is ``0.1``.

        wait : If set wait for command to finish. Default value is ``True``.

        Example:

        Swipe for 1 second without waiting for command to finish.

        | ${position1}= | Create List | 10 | 10

        | ${position2}= | Create List | 20 | 20

        | ${position3}= | Create List | 30 | 30

        | ${positions}= | Create List | ${position1} | ${position2} | ${position3}

        | Multipoint Swipe | ${positions} | duration=1 | wait=${False}
        """
        self._driver.multipoint_swipe(positions, duration=duration, wait=wait)

    def begin_touch(self, coordinates):
        """Simulates the starting of a touch on the screen.

        coordinates : The screen coordinates.

        Example:

        Begin Touch at coodinates [10, 10]

        | ${coordinates}= | Create List | 10 | 10

        | Begin Touch | ${coordinates}
        """
        return self._driver.begin_touch(coordinates)

    def move_touch(self, finger_id, coordinates):
        """Simulates a touch movement on the screen. Move the touch created with ``begin_touch`` from the previous
        position to the position given as parameters.

        finger_id : The value returned by ``begin_touch``.

        coordinates : Screen coordinates where the touch will be moved.

        Example:

        Move Touch at coodinates [20, 20]

        | ${initial_coordinates}= | Create List | 10 | 10

        | ${finger_id}= | Begin Touch | ${initial_coordinates}

        | ${coordinates}= | Create List | 20 | 20

        | Move Touch | ${finger_id} | ${coordinates}
        """
        self._driver.move_touch(finger_id, coordinates)

    def end_touch(self, finger_id):
        """Simulates the ending of a touch on the screen. This command will destroy the touch making it no longer usable to
        other movements.

        finger_id : The value returned by ``begin_touch``.

        Example:

        Move Touch from [10, 10] to [20, 20]

        | ${initial_coordinates}= | Create List | 10 | 10

        | ${finger_id}= | Begin Touch | ${initial_coordinates}

        | ${coordinates}= | Create List | 20 | 20

        | Move Touch | ${finger_id} | ${coordinates}

        | End Touch  | ${finger_id}
        """
        self._driver.end_touch(finger_id)

    def click(self, coordinates, count=1, interval=0.1, wait=True):
        """Click at screen coordinates.

        coordinates : The screen coordinates.

        count : Number of clicks. Default value is ``1``.

        interval : The interval between clicks in seconds. Default value is ``0.1``.

        wait : If set to ``True`` Wait for command to finish. Default value is ``True``.

        Example:

        Click at coodinates [30, 30] for 3 times.

        | ${coordinates}= | Create List | 30 | 30

        | Click | ${coordinates} | count=3
        """
        self._driver.click(coordinates, count=count,
                           interval=interval, wait=wait)

    def tap(self, coordinates, count=1, interval=0.1, wait=True):
        """Tap at screen coordinates.

        coordinates : The screen coordinates.

        count : Number of taps. Default value is ``1``.

        interval : The interval between taps in seconds. Default value is ``0.1``.

        wait : If set to ``True`` Wait for command to finish. Default value is ``True``.

        Example:

        Tap at coodinates [30, 30] for 3 times.

        | ${coordinates}= | Create List | 30 | 30

        | Tap | ${coordinates} | count=3
        """
        self._driver.tap(coordinates, count=count,
                         interval=interval, wait=wait)

    def tilt(self, acceleration, duration=0.1, wait=True):
        """Simulates device rotation action in your application.

        acceleration : The linear acceleration of a device.

        duration : How long the rotation will take in seconds. Default value is ``0.1``.

        wait : If set wait for command to finish. Default value is ``True``.

        Example:

        Tilt with acceleration [1, 1, 1] for 1 second.

        | ${acceleration}= | Create List | 1 | 1 | 1

        | Tilt | ${acceleration} | duration=1
        """
        self._driver.tilt(acceleration, duration=duration, wait=wait)

    def reset_input(self):
        """Clear all active input actions simulated by AltTester.

        Example:

        | Reset Input |
        """
        self._driver.reset_input()

    def get_png_screenshot(self, path):
        """Creates a screenshot of the current scene in png format at the given path.

        path : The path where the image will be created.

        Example:

        | Get Png Screenshot | C:\\TestPNG
        """
        self._driver.get_png_screenshot(path)

    def get_player_pref_key(self, key_name, key_type):
        """Returns the value for a given key from PlayerPrefs.

        key_name : The name of the key to be retrieved.

        key_type : The type of the key. One of the following:  Int, String, Float.

        Example:

        | Get Player Pref Key | test | String
        """
        try:
            return self._driver.get_player_pref_key(key_name, getattr(PlayerPrefKeyType, key_type))
        except AttributeError:
            raise ValueError(
                "Invalid key type: {type}. Valid ones are: {options}.".format(
                    type=key_type, options=", ".join(value.name for value in PlayerPrefKeyType)))

    def set_player_pref_key(self, key_name, value, key_type):
        """Sets the value for a given key in PlayerPrefs.

        key_name : The name of the key to be set.

        value : The new value to be set.

        key_type : The type of the key.One of the following:  Int, String, Float.

        Example:

        | Set Player Pref Key | test | test | String
        """
        try:
            self._driver.set_player_pref_key(
                key_name, value, getattr(PlayerPrefKeyType, key_type))
        except AttributeError:
            raise ValueError("Invalid key type: {type}. Valid ones are: {options}.".format(
                type=key_type, options=", ".join(value.name for value in PlayerPrefKeyType)))

    def delete_player_pref_key(self, key_name):
        """Removes a key and its corresponding value from PlayerPrefs.

        key_name : The name of the key to be deleted.

        Example:

        | Delete Player Pref Key | test
        """
        self._driver.delete_player_pref_key(key_name)

    def delete_player_pref(self):
        """Removes all keys and values from PlayerPref.

        Example:

        | Delete Player Pref |
        """
        self._driver.delete_player_pref()

    def get_current_scene(self):
        """Returns the name of the current scene.

        Example:

        | ${scene}= | Get Current Scene |
        """
        return self._driver.get_current_scene()

    def load_scene(self, scene_name, load_single=True):
        """Loads a scene.

        scene_name : The name of the scene to be loaded.

        load_single : Sets the loading mode. If set to ``False`` the scene will be loaded additive, together with the currently loaded scenes. Default value is ``True``.

        Example:

        Load scene1

        | Load Scene | scene1
        """
        self._driver.load_scene(scene_name, load_single=load_single)

    def unload_scene(self, scene_name):
        """Unloads a scene.

        scene_name : The name of the scene to be unloaded.

        Example:

        Unload scene1

        | Unload Scene | scene1
        """
        self._driver.unload_scene(scene_name)

    def get_all_loaded_scenes(self):
        """Returns all the scenes that have been loaded.

        Example:

        | ${scenes}= | Get All Loaded Scenes
        """
        return self._driver.get_all_loaded_scenes()

    def wait_for_current_scene_to_be(self, scene_name, timeout=30, interval=1):
        """Waits for the scene to be loaded for a specified amount of time.

        scene_name : The name of the scene to wait for.

        timeout : The time measured in seconds to wait for the specified scene. Default value is ``30``.

        interval : How often to check that the scene was loaded in the given timeout. Default value is ``1``.

        Example:

        Wait for scene1 for 10 seconds.

        | Wait For Current Scene To Be | scene1 | timeout=10
        """
        self._driver.wait_for_current_scene_to_be(
            scene_name, timeout=timeout, interval=interval)

    def get_application_screensize(self):
        """Returns the value of the application screen size.

        Example:

        | ${app_scene_size}= | Get Application Screensize
        """
        return self._driver.get_application_screensize()

    def get_time_scale(self):
        """Returns the value of the time scale.

        Example:

        | ${time_scale}= | Get Time Scale
        """
        return self._driver.get_time_scale()

    def set_time_scale(self, time_scale):
        """Sets the value of the time scale.

        time_scale: The value of the time scale.

        Example:

        | Set Time Scale | 1
        """
        self._driver.set_time_scale(time_scale)

    def call_static_method(self, type_name, method_name, assembly, parameters=None, type_of_parameters=None):
        """Invoke a static method from your application.

        type_name : The name of the script. If the script has a namespace the format should look like
                this: ``"namespace.typeName"``.

        method_name : The name of the public method that we want to call. If the method is inside a
                static property/field to be able to call that method, methodName needs to be in the following format
                ``"propertyName.MethodName"``.

        assembly : The name of the assembly containing the script.

        parameters : Default value is ``None``.

        type_of_parameters : Default value is ``None``.

        Example:

        Call method GetInt

        | ${list_to_get}= | Create List | Test | ${2}

        | ${int_value}= | Call Static Method | UnityEngine.PlayerPrefs | GetInt | UnityEngine.CoreModule | parameters=${list_to_get}
        """
        return self._driver.call_static_method(type_name, method_name, assembly, parameters=parameters, type_of_parameters=type_of_parameters)

    def get_static_property(self, component_name, property_name, assembly, max_depth=2):
        """Returns the value of the static field or property given as parameter.

        component_name :The name of the component containing the field or property to be retrieved.

        property_name : The name of the field or property to be retrieved.

        assembly : The name of the assembly containing the component mentioned above.

        max_depth : The value determining how deep to go in the hierarchy of objects to find the field or property. Default value is ``2``.

        Example:

        Get Static Property for width resolution.

        | ${width_resolution}= | Get Static Property | UnityEngine.Screen | currentResolution.width | UnityEngine.CoreModule
        """
        return self._driver.get_static_property(component_name, property_name, assembly, max_depth=max_depth)

    def set_static_property(self, component_name, property_name, assembly, updated_value):
        """Set the value of the static field or property given as parameter.

        component_name : The name of the component containing the field or property to be retrieved.

        property_name : The name of the field or property to be retrieved.

        assembly : The name of the assembly containing the component mentioned above.

        updated_value : The value of the field or property to be updated.

        Example:

        Set Static Property for width resolution.

        | Set Static Property | UnityEngine.Screen | currentResolution.width | UnityEngine.CoreModule | 1920
        """
        self._driver.set_static_property(
            component_name, property_name, assembly, updated_value)

    def set_server_logging(self, logger, log_level):
        """Sets the level of logging on AltTester.

        logger : One of the following: File, Unity, Console. The type of logger.

        log_lever : One of the following: Trace, Debug, Info, Warn, Error, Fatal, Off. The logging level.

        Example:

        Set logging level Off for Files.

        | Set Server Logging | File | Off
        """
        self._driver.set_server_logging(self.get_logger(
            logger), self.get_log_level(log_level))

    def call_component_method(self, alt_object: AltObject, component_name, method_name, assembly, parameters=None, type_of_parameters=None):
        """Invokes a method from an existing component of the object.

        alt_object : The AltObject for which we want to call the method.
        component_name : The name of the script. If the script has a namespace the format should look
                like this: ``"namespace.typeName"``.

        method_name : The name of the public method that we want to call. If the method is inside a
                static property/field to be able to call that method, methodName needs to be in the following format
                ``"propertyName.MethodName"``.

        assembly : The name of the assembly containing the script.

        parameters : Default value is ``None``.

        type_of_parameters : Default value is ``None``.

        Example:

        Call method get_text for PlayButton

        | ${object}= | Find Object | PATH | //PlayButton

        | ${text}=   | Call Component Method | ${object} | UnityEngine.UI.Text | get_text | UnityEngine.UI
        """
        return alt_object.call_component_method(component_name, method_name, assembly, parameters=parameters, type_of_parameters=type_of_parameters)

    def update_object(self, alt_object: AltObject):
        """Update the altObject.
        alt_object : The AltObject for which we want to update.

        Example:

        Update Capsule object.

        | ${object}=    | Find Object | NAME | Capsule

        | Update Object | ${object}
        """
        alt_object.update_object()

    def get_screen_position(self, alt_object: AltObject):
        """Returns the screen position for alt_object.
        alt_object : The AltObject for which we want to get screen position.

        Example:

        Get Screen Position for the Capsule object.

        | ${object}=    | Find Object | NAME | Capsule

        | ${positions}= | Get Screen Position | ${object}
        """
        return alt_object.get_screen_position()

    def get_world_position(self, alt_object: AltObject):
        """Returns the world position for alt_object.
        alt_object : The AltObject for which we want to get world position.

        Example:

        Get World Position for the Capsule object.

        | ${object}=    | Find Object | NAME | Capsule

        | ${positions}= | Get World Position | ${object}
        """
        return alt_object.get_world_position()

    def get_parent(self, alt_object: AltObject):
        """Returns the parent object.
        alt_object : The AltObject for which we want to get the parent.

        Example:

        Get Parent for the Capsule object.

        | ${object}= | Find Object | NAME | Capsule

        | ${parent}= | Get Parent | ${object}
        """
        return alt_object.get_parent()

    def get_all_components(self, alt_object: AltObject):
        """Returns all components.
        alt_object : The AltObject for which we want to get components.

        Example:

        Get All Components for the Capsule object.

        | ${object}=     | Find Object | NAME | Capsule

        | ${components}= | Get All Components | ${object}
        """
        return alt_object.get_all_components()

    def wait_for_component_property(self, alt_object: AltObject, component_name, property_name,
                                    property_value, assembly, timeout=20, interval=0.5, get_property_as_string=False, max_depth=2):
        """Wait until a property has a specific value and returns the value of the given component property.

            alt_object : The AltObject for which we want to wait for property.

            component_name : The name of the component. If the component has a namespace the format should
                look like this: ``"namespace.componentName"``.

            property_name : The name of the property of which value you want. If the property is an array
                you can specify which element of the array to return by doing ``property[index]``, or if you want a
                property inside of another property you can get by doing ``property.subProperty``.

            property_value : The value of the component expected.

            assembly : The name of the assembly containing the component.

            timeout : The number of seconds that it will wait for property. Default value is 20 seconds.

            interval : The number of seconds after which it will try to find the object again. The interval should be smaller than timeout. Default value is 0.5.

            get_property_as_string: A boolean value that compares the property_value as a string with the property from the instrumented app.

            max_depth: An integer value that defines the maximum level from which to retrieve properties.

        Example:

        Wait for property TestBool from Capsule

        | ${object}= | Find Object | NAME | Capsule

        | ${result}= | Wait For Component Property | ${object} | AltExampleScriptCapsule | TestBool | ${True} | Assembly-CSharp
        """
        return alt_object.wait_for_component_property(component_name, property_name, property_value, assembly, timeout=timeout, interval=interval, get_property_as_string=get_property_as_string, max_depth=max_depth)

    def get_component_property(self, alt_object: AltObject, component_name, property_name, assembly, max_depth=2):
        """Returns the value of the given component property.

        alt_object : The AltObject for which we want to get for property.

        component_name : The name of the component. If the component has a namespace the format should
                look like this: ``"namespace.componentName"``.

        property_name : The name of the property of which value you want. If the property is an array
                you can specify which element of the array to return by doing ``property[index]``, or if you want a
                property inside of another property you can get by doing ``property.subProperty``.

        assembly : The name of the assembly containing the component.

        maxDepth : Set how deep to serialize the property. Default value is ``2``.

        Example:

        Get property arrayOfInts for Capsule.

        | ${object}=   | Find Object | NAME | Capsule

        | ${property}= | Get Component Property | ${object} | Capsule | arrayOfInts | ${True} | Assembly-CSharp

        """
        return alt_object.get_component_property(component_name, property_name, assembly, max_depth=max_depth)

    def set_component_property(self, alt_object: AltObject, component_name, property_name, assembly, value):
        """Sets a value for a given component property.

        alt_object : The AltObject for which we want to set for property.

        component_name : The name of the component. If the component has a namespace the format should
                look like this: ``"namespace.componentName"``.

        property_name : The name of the property of which value you want to set.

        assembly : The name of the assembly containing the component.

        value : The value to be set for the chosen component's property.

        Example:

        Set property stringToSetFromTests for Capsule.

        | ${object}=             | Find Object | NAME | Capsule

        | Set Component Property | ${object} | Capsule | stringToSetFromTests | Assembly-CSharp | 2
        """
        alt_object.set_component_property(
            component_name, property_name, assembly, value)

    def get_visual_element_property(self, alt_object: AltObject, property_name):
        """Returns the value of the given component property.

        alt_object : The AltObject for which we want to get for property.

        property_name : The name of the property of which value you want.

        Example:

        Get property stringToSetFromTests for Capsule.

        | ${object}=             | Find Object | NAME | Capsule

        | ${property}= | Get Visual Element Property | ${object} | stringToSetFromTests
        """
        return alt_object.get_visual_element_property(property_name)

    def wait_for_visual_element_property(self, alt_object: AltObject, property_name, property_value, timeout=20, interval=0.5, get_property_as_string=False):
        """Waits until a property of the current object has a specific value and returns the value of the given visual element property.

        alt_object : The AltObject for which we want to wait for property.

        property_name : The name of the property of which value you want. If the property is an array
            you can specify which element of the array to return by doing ``property[index]``, or if you want a
            property inside of another property you can get by doing ``property.subProperty``.

        property_value : The value of the component expected.

        timeout : The number of seconds that it will wait for property. Default value is 20 seconds.

        interval : The number of seconds after which it will try to find the object again. The interval should be smaller than timeout. Default value is 0.5.

        get_property_as_string: A boolean value that compares the property_value as a string with the property from the instrumented app.

        Example:

        Wait for property TestBool from Capsule

        | ${object}= | Find Object | NAME | Capsule

        | ${result}= | Wait For Component Property | ${object} | AltExampleScriptCapsule | TestBool | ${True} | Assembly-CSharp
        """
        return alt_object.wait_for_visual_element_property(property_name, property_value, timeout, interval, get_property_as_string)

    def get_text(self, alt_object: AltObject):
        """Returns text value from alt_object.

        alt_object : The AltObject for which we want to get text.

        Example:

        Get text for CapsuleInfo.

        | ${object}= | Find Object | NAME | CapsuleInfo

        | ${text}=   | Get Text | ${object}
        """
        return alt_object.get_text()

    def get_object_name(self, alt_object: AltObject):
        """Returns name for alt_object.

        alt_object : The AltObject for which we want to get the name.

        Example:

        Get Object Name for CapsuleInfo.

        | ${object}= | Find Object | PATH | //CapsuleInfo

        | ${name}=   | Get Object Name | ${object}
        """
        return alt_object.name

    def get_object_id(self, alt_object: AltObject):
        """Returns id for alt_object.

        alt_object : The AltObject for which we want to get id.

        Example:

        Get Object Id for CapsuleInfo.

        | ${object}= | Find Object | PATH | //CapsuleInfo

        | ${id}=     | Get Object Id | ${object}
        """
        return alt_object.id

    def get_object_x(self, alt_object: AltObject):
        """Returns x for alt_object.

        alt_object : The AltObject for which we want to get x.

        Example:

        Get Object X for CapsuleInfo.

        | ${object}=  | Find Object | PATH | //CapsuleInfo

        | ${x_param}= | Get Object X | ${object}
        """
        return alt_object.x

    def get_object_y(self, alt_object: AltObject):
        """Returns y for alt_object.

        alt_object : The AltObject for which we want to get y.

        Example:

        Get Object Y for CapsuleInfo.

        | ${object}=  | Find Object | PATH | //CapsuleInfo

        | ${y_param}= | Get Object Y | ${object}
        """
        return alt_object.y

    def get_object_z(self, alt_object: AltObject):
        """Returns z for alt_object.

        alt_object : The AltObject for which we want to get z.

        Example:

        Get Object Z for CapsuleInfo.

        | ${object}=  | Find Object | PATH | //CapsuleInfo

        | ${z_param}= | Get Object Z | ${object}
        """
        return alt_object.z

    def get_object_mobileY(self, alt_object: AltObject):
        """Returns mobileY for alt_object.

        alt_object : The AltObject for which we want to get mobileY.

        Example:

        Get Object MobileY for CapsuleInfo.

        | ${object}=  | Find Object | PATH | //CapsuleInfo

        | ${mobileY}= | Get Object MobileY | ${object}
        """
        return alt_object.mobileY

    def get_object_type(self, alt_object: AltObject):
        """Returns type for alt_object.

        alt_object : The AltObject for which we want to get type.

        Example:

        Get Object Type for CapsuleInfo.

        | ${object}= | Find Object | PATH | //CapsuleInfo

        | ${type}=   | Get Object Type | ${object}
        """
        return alt_object.type

    def get_object_enabled(self, alt_object: AltObject):
        """Returns enabled for alt_object.

        alt_object : The AltObject for which we want to get enabled.

        Example:

        Get Object Enabled for CapsuleInfo.

        | ${object}=  | Find Object | PATH | //CapsuleInfo

        | ${enabled}= | Get Object Enabled | ${object}
        """
        return alt_object.enabled

    def get_object_worldX(self, alt_object: AltObject):
        """Returns worldX for alt_object.

        alt_object : The AltObject for which we want to get worldX.

        Example:

        Get Object WorldX for CapsuleInfo.

        | ${object}= | Find Object | PATH | //CapsuleInfo

        | ${worldX}= | Get Object WorldX | ${object}
        """
        return alt_object.worldX

    def get_object_worldY(self, alt_object: AltObject):
        """Returns worldY for alt_object.

        alt_object : The AltObject for which we want to get worldY.

        Example:

        Get Object WorldY for CapsuleInfo.

        | ${object}= | Find Object | PATH | //CapsuleInfo

        | ${worldY}= | Get Object WorldY | ${object}
        """
        return alt_object.worldY

    def get_object_worldZ(self, alt_object: AltObject):
        """Returns worldZ for alt_object.

        alt_object : The AltObject for which we want to get worldZ.

        Example:

        Get Object WorldZ for CapsuleInfo.

        | ${object}= | Find Object | PATH | //CapsuleInfo

        | ${worldZ}= | Get Object WorldZ | ${object}
        """
        return alt_object.worldZ

    def get_object_idCamera(self, alt_object: AltObject):
        """Returns idCamera for alt_object.

        alt_object : The AltObject for which we want to get idCamera.

        Example:

        Get Object IdCamera for CapsuleInfo.

        | ${object}=   | Find Object | PATH | //CapsuleInfo

        | ${idCamera}= | Get Object IdCamera | ${object}
        """
        return alt_object.idCamera

    def get_object_transformParentId(self, alt_object: AltObject):
        """Returns transformParentId for alt_object.

        alt_object : The AltObject for which we want to get transformParentId.

        Example:

        Get Object TransformParentId for CapsuleInfo.

        | ${object}=            | Find Object | PATH | //CapsuleInfo

        | ${transformParentId}= | Get Object TransformParentId | ${object}
        """
        return alt_object.transformParentId

    def get_object_transformId(self, alt_object: AltObject):
        """Returns transformId for alt_object.

        alt_object : The AltObject for which we want to get transformId.

        Example:

        Get Object TransformId for CapsuleInfo.

        | ${object}=      | Find Object | PATH | //CapsuleInfo

        | ${transformId}= | Get Object TransformId | ${object}
        """
        return alt_object.transformId

    def set_text(self, alt_object: AltObject, text, submit=False):
        """Set text value for alt_object.

        alt_object : The AltObject for which we want to set text.

        text : The text to be set.

        submit : If set will trigger a submit event. Default value is False.

        Example:

        Set text InputFieldTest for CapsuleInfo.

        | ${object}= | Find Object | NAME | CapsuleInfo

        | Set Text   | ${object} | InputFieldTest
        """
        return alt_object.set_text(text, submit=submit)

    def tap_object(self, alt_object: AltObject, count=1, interval=0.1, wait=True):
        """Tap the alt_object.

        alt_object : The AltObject which we want to tap.

        count : Number of taps. Default value is ``1``.

        interval : Interval between taps in seconds. Default value is ``0.1``.

        wait : Wait for command to finish. Default value is ``True``.

        Example:

        Double tap on Capsule.

        | ${object}= | Find Object | NAME | Capsule

        | Tap Object | ${object} | count=2
        """
        alt_object.tap(count=count, interval=interval, wait=wait)

    def click_object(self, alt_object: AltObject, count=1, interval=0.1, wait=True):
        """Click the alt_object.

        alt_object : The AltObject which we want to click.

        count : Number of clicks. Default value is ``1``.

        interval : Interval between clicks in seconds. Default value is ``0.1``.

        wait : Wait for command to finish. Default value is ``True``.

        Example:

        Double-click on Capsule.

        | ${object}=   | Find Object | NAME | Capsule

        | Click Object | ${object} | count=2
        """
        alt_object.click(count=count, interval=interval, wait=wait)

    def pointer_down(self, alt_object: AltObject):
        """Simulates pointer down action on alt_object.

        alt_object : The AltObject which we want to pointer down.

        Example:

        Pointer Down on Panel.

        | ${object}=   | Find Object | NAME | Panel

        | Pointer Down | ${object} |
        """
        return alt_object.pointer_down()

    def pointer_up(self, alt_object: AltObject):
        """Simulates pointer up action on alt_object.

        alt_object : The AltObject which we want to pointer up.

        Example:

        Pointer Up on Panel.

        | ${object}= | Find Object | NAME | Panel

        | Pointer Up | ${object} |
        """
        return alt_object.pointer_up()

    def pointer_enter(self, alt_object: AltObject):
        """Simulates pointer enter action on alt_object.

        alt_object : The AltObject which we want to pointer enter.

        Example:

        Pointer Enter on Drop Image.

        | ${object}=    | Find Object | NAME | Drop Image

        | Pointer Enter | ${object} |
        """
        return alt_object.pointer_enter()

    def pointer_exit(self, alt_object: AltObject):
        """Simulates pointer exit action on alt_object.

        alt_object : The AltObject which we want to pointer exit.

        Example:

        Pointer Exit on Drop Image.

        | ${object}=   | Find Object | NAME | Drop Image

        | Pointer Exit | ${object} |
        """
        return alt_object.pointer_exit()

    def update_object(self, alt_object: AltObject):
        """Returns alt_object with new values.

        alt_object : The AltObject which we want to update.

        Example:

        Update Player1.

        | ${object}=     | Find Object | NAME | Player1

        | Key Down       | A

        | ${new_object}= | Update Object | ${object}
        """
        return alt_object.update_object()

    def get_parent(self, alt_object: AltObject):
        """Returns the parent alt_object.

        alt_object : The AltObject for which we want to find the parent.

        Example:

        Get CapsuleInfo parent.

        | ${object}= | Find Object | NAME | CapsuleInfo

        | ${parent}= | Get Parent  | ${object}
        """
        return alt_object.get_parent()

    def get_by_enum(self, locator):
        try:
            locator = getattr(By, str(locator).upper())
            return locator
        except AttributeError:
            raise ValueError(
                "Invalid locator strategy: {locator}. Valid ones are: {options}.".format(
                    locator=locator, options=", ".join(value.name for value in By)))

    def get_notification_type_enum(self, notification_type):
        try:
            notification_type = getattr(NotificationType, notification_type)
            return notification_type
        except AttributeError:
            raise ValueError(
                "Invalid notification type: {notification_type}. Valid ones are: {options}.".format(
                    notification_type=notification_type, options=", ".join(value.name for value in NotificationType)))

    def get_logger(self,  logger):
        try:
            logger = getattr(AltLogger, logger)
            return logger
        except AttributeError:
            raise ValueError("Invalid logger type: {logger}. Valid ones are: {options}.".format(
                logger=logger, options=", ".join(value.name for value in AltLogger)))

    def get_log_level(self,  log_level):
        try:
            log_level = getattr(AltLogLevel, log_level)
            return log_level
        except AttributeError:
            raise ValueError("Invalid log level type: {log_level}. Valid ones are: {options}.".format(
                log_level=log_level, options=", ".join(value.name for value in AltLogLevel)))

    def get_implicit_timeout(self):
        """Gets the current timeout for the AltTester® commands that use a timeout parameter.

        Example:

        | ${timeout}= | Get Implicit Timeout
        """
        return self._driver.get_implicit_timeout()

    def set_implicit_timeout(self, timeout):
        """Sets the timeout for the AltTester® commands that use a timeout parameter.

        timeout : The new timeout in seconds.

        Example:

        Set Implicit Timeout to 5 seconds.

        | Set Implicit Timeout | 5
        """
        self._driver.set_implicit_timeout(timeout)
