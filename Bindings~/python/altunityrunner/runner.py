import warnings
import sys

from loguru import logger

import altunityrunner.commands as commands
from altunityrunner.commands.base_command import Command
from altunityrunner.__version__ import VERSION
from altunityrunner._websocket import WebsocketConnection
from altunityrunner.altUnityObject import AltUnityObject
from altunityrunner.by import By


warnings.filterwarnings("default", category=DeprecationWarning, module=__name__)


class AltUnityDriver:
    """The driver object will help interacting with all the game objects, their properties and methods.

    When you instantiate an ``AltUnityDriver`` object in your tests, you can use it to “drive” your game like one of
    your users would, by interacting with all the game objects, their properties and methods.  An ``AltUnityDriver``
    instance will connect to the AltUnity Proxy.

    Args:
        host (:obj:`str`): The proxy host to connect to.
        port (:obj:`int`): The proxy port to connect to.
        enable_logging (:obj:`bool`, optional): If set to ``True`` will turn on logging, by default logging is disabled.
        timeout (:obj:`int`, :obj:`float`, optional): The connect timeout time in seconds.

    """

    def __init__(self, host="127.0.0.1", port=13000, enable_logging=False, timeout=None):
        self.host = host
        self.port = port
        self.enable_logging = enable_logging

        self._config_logging(self.enable_logging)

        self._connection = WebsocketConnection(host=host, port=port, timeout=timeout)
        self._connection.connect()

    @staticmethod
    def _config_logging(enable_logging):
        if enable_logging:
            logger.configure(
                handlers=[
                    dict(sink=sys.stdout, diagnose=False),
                    dict(sink="./AltUnityTesterLog.txt", enqueue=False, serialize=True, mode="w", diagnose=False),
                ],
                levels=[dict(name="DEBUG")],
                activation=[("altunityrunner", True)],
            )
        else:
            logger.disable("altunityrunner")

    @staticmethod
    def _split_version(version):
        parts = version.split(".")
        return (parts[0], parts[1]) if len(parts) > 1 else ("", "")

    def _check_server_version(self):
        server_version = commands.GetServerVersion.run(self._connection)
        logger.info("Connection established with instrumented Unity app. AltUnity Tester Version: {}", server_version)

        major_server, minor_server = self._split_version(server_version)
        major_driver, minor_driver = self._split_version(VERSION)

        if major_server != major_driver or minor_server != minor_driver:
            message = "Version mismatch. AltUnity Driver version is {}. AltUnity Tester version is {}.".format(
                VERSION,
                server_version
            )

            logger.warning(message)

    def _get_alt_unity_object(self, data):
        if data is None:
            return None

        alt_unity_object = AltUnityObject(self, data)

        logger.debug(
            "Element {} found at x: {} y: {} mobileY: {}",
            alt_unity_object.name,
            alt_unity_object.x,
            alt_unity_object.y,
            alt_unity_object.mobileY
        )

        return alt_unity_object

    def _get_alt_unity_objects(self, data):
        if data is None:
            return None

        alt_unity_objects = []

        for element in data:
            alt_unity_object = AltUnityObject(self, element)
            alt_unity_objects.append(alt_unity_object)

            logger.debug(
                "Element {} found at x: {} y: {} mobileY: {}",
                alt_unity_object.name,
                alt_unity_object.x,
                alt_unity_object.y,
                alt_unity_object.mobileY
            )

        return alt_unity_objects

    def stop(self):
        """Close the connection to AltUnity."""

        self._connection.close()

    def get_command_response_timeout(self):
        """Gets the current command response timeout for the AltUnity connection.

        Return:
            int or float: The current command response time.

        """

        return self._connection.set_command_timeout()

    def set_command_response_timeout(self, timeout):
        """Sets the command response timeout for the AltUnity connection.

        Args:
            timeout (:obj:`int` or :obj:`float`): The new command response timeout in seconds.

        """

        self._connection.set_command_timeout(timeout)

    def get_delay_after_command(self):
        """Gets the current delay after a command."""

        return Command.get_delay_after_command()

    def set_delay_after_command(self, delay):
        """Set the delay after a command.

        Args:
            delay (:obj:`int` or :obj:`float`): The new delay a after a command.
        """

        Command.set_delay_after_command(delay)

    def set_server_logging(self, logger, log_level):
        """Sets the level of logging on AltUnity Tester.

        Args:
            logger (:obj:`AltUnityLogger`): The type of logger.
            log_lever (:obj:`AltUnityLogLevel`): The logging level.

        """

        commands.SetServerLogging.run(self._connection, logger, log_level)

    def call_static_method(self, type_name, method_name, parameters=None, type_of_parameters=None, assembly=""):
        """Invoke a static method from your game.

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
            type_name, method_name, parameters=parameters, type_of_parameters=type_of_parameters, assembly=assembly
        )

    def get_current_scene(self):
        """Returns the name of the current scene.

        Returns:
            str: The name of the current scene.

        """

        return commands.GetCurrentScene.run(self._connection)

    def load_scene(self, scene_name, load_single=True):
        """Loads a scene.

        Args:
            scene_name (:obj:`str`): The name of the scene to be loaded.
            load_single (:obj`bool`, optional): Sets the loading mode. If set to ``False`` the scene will be loaded
                additive, together with the current loaded scenes. Defaults to ``True``.

        """

        commands.LoadScene.run(
            self._connection,
            scene_name, load_single
        )

    def wait_for_current_scene_to_be(self, scene_name, timeout=30, interval=1):
        """Waits for the scene to be loaded for a specified amount of time.

        Args:
            scene_name (:obj:`str`): The name of the scene to wait for.
            timeout (obj:`int`, :obj:`float`, optional): The time measured in seconds to wait for the specified scene.
                Defaults to ``30``.
            interval (obj:`int`, :obj:`float`, optional): How often to check that the scene was loaded in the given
                timeout. Defaults to ``1``.

        Returns:
            str: The name of the loaded scene.

        """

        return commands.WaitForCurrentSceneToBe.run(
            self._connection,
            scene_name, timeout, interval
        )

    def unload_scene(self, scene_name):
        """Unloads a scene.

        Args:
            scene_name (:obj:`str`): The name of the scene to be unloaded.

        """

        commands.UnloadScene.run(self._connection, scene_name)

    def get_all_loaded_scenes(self):
        """Returns all the scenes that have been loaded.

        Returns:
            :obj:`list` of :obj:`str`: A list containing the names of all the loaded scenes.

        """

        return commands.GetAllLoadedScenes.run(self._connection)

    def get_time_scale(self):
        """Returns the value of the time scale.

        Returns:
            float: The value of the time scale.

        """

        return commands.GetTimeScale.run(self._connection)

    def set_time_scale(self, time_scale):
        """Sets the value of the time scale.

        Args:
            time_scale (:obj:`float`, :obj:`int`): The value of the time scale.

        """

        commands.SetTimeScale.run(self._connection, time_scale)

    def get_player_pref_key(self, key_name, key_type):
        """Returns the value for a given key from PlayerPrefs.

        Args:
            key_name (:obj:`str`): The name of the key to be retrived.
            key_type (:obj:`PlayerPrefKeyType`): The type of the key.

        """

        return commands.GetPlayerPrefKey.run(
            self._connection,
            key_name, key_type
        )

    def set_player_pref_key(self, key_name, value, key_type):
        """Sets the value for a given key in PlayerPrefs.

        Args:
            key_name (:obj:`str`): The name of the key to be set.
            value (:obj:`str`): The new value of be set.
            key_type (:obj:`PlayerPrefKeyType`): The type of the key.

        """

        commands.SetPlayerPrefKey.run(
            self._connection,
            key_name, value, key_type
        )

    def delete_player_pref_key(self, key_name):
        """Removes a key and its corresponding value from PlayerPrefs.

        Args:
            key_name (:obj:`str`): The name of the key to be deleted.

        """

        commands.DeletePlayerPrefKey.run(self._connection, key_name)

    def delete_player_pref(self):
        """Removes all keys and values from PlayerPref."""

        commands.DeletePlayerPref.run(self._connection)

    def find_object(self, by, value, camera_by=By.NAME, camera_value="", enabled=True):
        """Finds the first object in the scene that respects the given criteria.

        Args:
            by (:obj:`By`): Sets what criteria to use in order to find the object.
            value (:obj:`str`): The value to which an object will be compared to see if they respect the criteria or
                not.
            camera_by (:obj:`By`, optional): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            enabled (:obj:`bool`, optional): If ``True`` will match only objects that are active in hierarchy. If
                ``False`` will match all objects. Defaults to ``True``.

        Returns:
            AltUnityObject: The object.

        """

        data = commands.FindObject.run(
            self._connection,
            by, value, camera_by, camera_value, enabled
        )

        return self._get_alt_unity_object(data)

    def find_objects(self, by, value, camera_by=By.NAME, camera_value="", enabled=True):
        """Finds all objects in the scene that respects the given criteria.

        Args:
            by (:obj:`By`): Sets what criteria to use in order to find the objects.
            value (:obj:`str`): The value to which an object will be compared to see if they respect the criteria or
                not.
            camera_by (:obj:`By`, optional): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            enabled (:obj:`bool`, optional): If ``True`` will match only objects that are active in hierarchy. If
                ``False`` will match all objects. Defaults to ``True``.

        Returns:
            list of AltUnityObjects: The list of objects.

        """

        data = commands.FindObjects.run(
            self._connection,
            by, value, camera_by, camera_value, enabled
        )

        return self._get_alt_unity_objects(data)

    def find_object_which_contains(self, by, value, camera_by=By.NAME, camera_value="", enabled=True):
        """Finds the first object in the scene that respects the given criteria.

        Args:
            by (:obj:`By`): Sets what criteria to use in order to find the object.
            value (:obj:`str`): The value to which an object will be compared to see if they respect the criteria or
                not.
            camera_by (:obj:`By`, optional): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            enabled (:obj:`bool`, optional): If ``True`` will match only objects that are active in hierarchy. If
                ``False`` will match all objects. Defaults to ``True``.

        Returns:
            AltUnityObject: The object.

        """

        data = commands.FindObjectWhichContains.run(
            self._connection,
            by, value, camera_by, camera_value, enabled
        )

        return self._get_alt_unity_object(data)

    def find_objects_which_contain(self, by, value, camera_by=By.NAME, camera_value="", enabled=True):
        """Finds all objects in the scene that respects the given criteria.

        Args:
            by (:obj:`By`): Sets what criteria to use in order to find the objects.
            value (:obj:`str`): The value to which an object will be compared to see if they respect the criteria or
                not.
            camera_by (:obj:`By`): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            enabled (:obj:`bool`): If ``True`` will match only objects that are active in hierarchy. If ``False`` will
                match all objects. Defaults to ``True``.

        Returns:
            list of AltUnityObjects: The list of objects.

        """

        data = commands.FindObjectsWhichContain.run(
            self._connection,
            by, value, camera_by, camera_value, enabled
        )

        return self._get_alt_unity_objects(data)

    def wait_for_object(self, by, value, camera_by=By.NAME, camera_value="", timeout=20, interval=0.5, enabled=True):
        """Waits until it finds an object that respects the given criteria or until timeout limit is reached.

        Args:
            by (:obj:`By`): Sets what criteria to use in order to find the object.
            value (:obj:`str`): The value to which an object will be compared to see if they respect the criteria or
                not.
            camera_by (:obj:`By`, optional): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            timeout (:obj:`int`, :obj:`float`, optional): The number of seconds that it will wait for object.
            interval (:obj:`int`, :obj:`float`, optional): The number of seconds after which it will try to find the
                object again. The interval should be smaller than the timeout.
            enabled (:obj:`bool`, optional): If ``True`` will match only objects that are active in hierarchy. If
                ``False`` will match all objects. Defaults to ``True``.

        Returns:
            AltUnityObject: The object.

        """

        data = commands.WaitForObject.run(
            self._connection,
            by, value, camera_by, camera_value, timeout, interval, enabled
        )

        return self._get_alt_unity_object(data)

    def wait_for_object_which_contains(self, by, value, camera_by=By.NAME, camera_value="", timeout=20, interval=0.5,
                                       enabled=True):
        """Waits until it finds an object that respects the given criteria or time runs out and will throw an error.

        Args:
            by (:obj:`By`): Sets what criteria to use in order to find the object.
            value (:obj:`str`): The value to which an object will be compared to see if they respect the criteria or
                not.
            camera_by (:obj:`By`): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            timeout (:obj:`int`, :obj:`float`, optional): The number of seconds that it will wait for object.
            interval (:obj:`int`, :obj:`float`, optional): The number of seconds after which it will try to find the
                object again. The interval should be smaller than the timeout.
            enabled (:obj:`bool`, optional): If ``True`` will match only objects that are active in hierarchy. If
                ``False`` will match all objects. Defaults to ``True``.

        Returns:
            AltUnityObject: The object.

        """

        data = commands.WaitForObjectWhichContains.run(
            self._connection,
            by, value, camera_by, camera_value, timeout, interval, enabled
        )

        return self._get_alt_unity_object(data)

    def wait_for_object_to_not_be_present(self, by, value, camera_by=By.NAME, camera_value="", timeout=20, interval=0.5,
                                          enabled=True):
        """Waits until the object in the scene that respects the given criteria is no longer in the scene or until
        timeout limit is reached.

        Args:
            by (:obj:`By`): Sets what criteria to use in order to find the object.
            value (:obj:`str`): The value to which an object will be compared to see if they respect the criteria or
                not.
            camera_by (:obj:`By`): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            timeout (:obj:`int`, :obj:`float`, optional): The number of seconds that it will wait for object.
            interval (:obj:`int`, :obj:`float`, optional): The number of seconds after which it will try to find the
                object again. The interval should be smaller than the timeout.
            enabled (:obj:`bool`, optional): If ``True`` will match only objects that are active in hierarchy. If
                ``False`` will match all objects. Defaults to ``True``.

        """

        commands.WaitForObjectToNotBePresent.run(
            self._connection,
            by, value, camera_by, camera_value, timeout, interval, enabled
        )

    def get_all_elements(self, camera_by=By.NAME, camera_value="", enabled=True):
        """Returns information about every objects loaded in the currently loaded scenes. This also means objects that
        are set as DontDestroyOnLoad.

        Args:
            camera_by (:obj:`By`): Set what criteria to use in order to find the camera.
            camera_value (:obj:`str`, optional): The value to which all the cameras in the scene will be compared to
                see if they respect the criteria or not. If no camera is given it will search through all camera that
                are in the scene until some camera sees the object or return the screen coordinate of the object
                calculated to the last camera in the scene.
            enabled (:obj:`bool`, optional): If ``True`` will match only objects that are active in hierarchy. If
                ``False`` will match all objects. Defaults to ``True``.

        Returns:
            list of AltUnityObjects: The list of objects.

        """

        return self.find_objects(By.PATH, "//*", camera_by=camera_by, camera_value=camera_value, enabled=enabled)

    def move_mouse(self, coordinates, duration=0.1, wait=True):
        """Simulates mouse movement in your game.

        Args:
            coordinates (:obj:`dict`): The screen coordinates
            duration (:obj:`int`, optional): The time measured in seconds to move the mouse from current position to
                the set location. Defaults to ``0.1``
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.

        """

        commands.MoveMouse.run(self._connection, coordinates, duration, wait)

    def scroll(self, speed_vertical=1, duration=0.1, wait=True, speed_horizontal=1):
        """Simulate scroll mouse action in your game.

        Args:
            speed_vertical (:obj:`int`, :obj:`float`): Set how fast to scroll. Positive values will scroll up and
                negative values will scroll down. Defaults to ``1``
            duration (:obj:`int`, :obj:`float`, optional): The duration of the scroll in seconds. Defaults to ``0.1``.
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.
            speed_horizontal (:obj:`int`, :obj:`float`): Set how fast to scroll right or left. Defaults to ``1``

        """

        commands.Scroll.run(
            self._connection,
            speed_vertical, duration,
            wait, speed_horizontal
        )

    def click(self, coordinates, count=1, interval=0.1, wait=True):
        """Click at screen coordinates.

        Args:
            coordinates (:obj:`dict`): The screen coordinates.
            count (:obj:`int`, optional): Number of taps. Defaults to ``1``.
            interval (:obj:`int`, :obj:`float`, optional): The interval between taps in seconds. Defaults to ``0.1``.
            wait (:obj:`bool`, optional): If set to ``True`` Wait for command to finish.

        """

        commands.ClickCoordinates.run(self._connection, coordinates, count, interval, wait)

    def key_down(self, key_code, power=1):
        """Simulates that a specific key was pressed without taking into consideration the duration of the press.

        Args:
            key_code (:obj:`AltUnityKeyCode`): The key code of the key simulated to be pressed.
            power (:obj:`float`, optional): A value between [-1,1] used for joysticks to indicate how hard the button
                was pressed. Defaults to ``1``.

        """

        commands.KeyDown.run(self._connection, key_code, power)

    def keys_down(self, key_codes, power=1):
        """Simulates that multiple keys were pressed without taking into consideration the duration of the press.

        Args:
            key_codes (:obj:`list` of :obj:`AltUnityKeyCode`): The key codes of the keys simulated to be pressed.
            power (:obj:`float`): A value between [-1,1] used for joysticks to indicate how hard the button was
                pressed. Defaults to ``1``.

        """

        commands.KeysDown.run(self._connection, key_codes, power)

    def key_up(self, key_code):
        """Simulates that a specific key was released.

        Args:
            key_code (:obj:`AltUnityKeyCode`): The key code of the key simulated to be released.

        """

        commands.KeyUp.run(self._connection, key_code)

    def keys_up(self, key_codes):
        """Simulates that multiple keys were released.

        Args:
            key_codes (:obj:`list` of :obj:`AltUnityKeyCode`): The key codes of the keys simulated to be released.

        """

        commands.KeysUp.run(self._connection, key_codes)

    def press_key(self, key_code, power=1, duration=0.1, wait=True):
        """Simulates key press action in your game.

        Args:
            key_code (:obj:`AltUnityKeyCode`): The key code of the key simulated to be pressed.
            power (:obj:`int`, :obj:`float`, optional): A value between [-1,1] used for joysticks to indicate how hard
                the button was pressed. Defaults to ``1``.
            duration (:obj:`float`, optional): The time measured in seconds from the key press to the key release.
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.

        """

        commands.PressKey.run(self._connection, key_code, power, duration, wait)

    def press_keys(self, key_codes, power=1, duration=0.1, wait=True):
        """Simulates multiple keypress action in your game.

        Args:
            key_codes (:obj:`list` of :obj:`AltUnityKeyCode`): The key codes of the keys simulated to be pressed.
            power (:obj:`float`): A value between [-1,1] used for joysticks to indicate how hard the buttons were
                pressed. Defaults to ``1``.
            duration (:obj:`float`): The time measured in seconds from the key press to the key release.
            wait (:obj:`bool`): If set wait for command to finish. Defaults to ``True``.

        """

        commands.PressKeys.run(self._connection, key_codes, power, duration, wait)

    def begin_touch(self, coordinates):
        """Simulates starting of a touch on the screen.

        Args:
            coordinates (:obj:`dict`): The screen coordinates.

        Returns:
            str: A ``finger_id`` to use with ``move_touch`` and ``end_touch``.

        """

        return commands.BeginTouch.run(self._connection, coordinates)

    def move_touch(self, finger_id, coordinates):
        """Simulates a touch movement on the screen. Move the touch created with ``begin_touch`` from the previous
        position to the position given as parameters.

        Args:
            finger_id (:obj:`str`): The value returned by ``begin_touch``.
            coordinates (:obj:`dict`): Screen coordinates where the touch will be moved.

        """

        commands.MoveTouch.run(self._connection, finger_id, coordinates)

    def end_touch(self, finger_id):
        """Simulates ending of a touch on the screen. This command will destroy the touch making it no longer usable to
        other movements.

        Args:
            finger_id (:obj:`str`): The value returned by ``begin_touch``.

        """

        commands.EndTouch.run(self._connection, finger_id)

    def swipe(self, start, end, duration=0.1, wait=True):
        """Simulates a swipe action between two points.

        Args:
            start (:obj:`dict`): Coordinates of the screen where the swipe begins.
            end (:obj:`dict`): Coordinates of the screen where the swipe ends.
            duration (:obj:`int`, :obj:`float`, optional): The time measured in seconds to move the mouse from start to
                end location. Defaults to ``0.1``.
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.

        """

        commands.Swipe.run(
            self._connection,
            start, end, duration, wait
        )

    def multipoint_swipe(self, positions, duration=0.1, wait=True):
        """Simulates a multipoint swipe action.

        Args:
            positions (:obj:`List[dict]`): A list of positions on the screen where the swipe be made.
            duration (:obj:`float`): The time measured in seconds to swipe from first position to the last position.
                Defaults to ``0.1``.
            wait (:obj:`bool`): If set wait for command to finish. Defaults to ``True``.

        """

        commands.MultipointSwipe.run(self._connection, positions, duration, wait)

    def tap(self, coordinates, count=1, interval=0.1, wait=True):
        """Tap at screen coordinates.

        Args:
            coordinates (:obj:`dict`): The screen coordinates.
            count (:obj:`int`, optional): Number of taps. Defaults to ``1``.
            interval (:obj:`int`, :obj:`float`, optional): The interval between taps in seconds. Defaults to ``0.1``.
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.

        """

        commands.TapCoordinates.run(self._connection, coordinates, count, interval, wait)

    def tilt(self, acceleration, duration=0.1, wait=True):
        """Simulates device rotation action in your game.

        Args:
            acceleration (:obj:`dict`): The linear acceleration of a device.
            duration (:obj:`int`, :obj:`float`, optional): How long the rotation will take in seconds.
                Defaults to ``0.1``.
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.

        """

        commands.Tilt.run(self._connection, acceleration, duration, wait)

    def get_png_screenshot(self, path):
        """Creates a screenshot of the current scene in png format at the given path.

        Args:
            path (:obj:`str`): The path where the image will be created.

        """

        commands.GetPNGScreenshot.run(self._connection, path)

    def hold_button(self, coordinates, duration=0.1, wait=True):
        """Simulates holding left click button down for a specified amount of time at given coordinates.

        Args:
            coordinates (:obj:`dict`): The coordinates where the button is held down
            duration ((:obj:`int`, :obj:`float`, optional): The time measured in seconds to keep the button down.
                Defaults to ``0.1``.
            wait (:obj:`bool`, optional): If set wait for command to finish. Defaults to ``True``.

        """

        return commands.Swipe.run(
            self._connection,
            coordinates, coordinates, duration, wait
        )

    def get_static_property(self, component_name, property_name, assembly="", max_depth=2):
        """Returns the value of the static field or property given as parameter.

        Args:
            component_name (:obj:`str`): The name of the component containing the field or property
                to be retrieved.
            field_or_property_name (:obj:`str`): The name of the field or property to be retrieved.
            assembly (:obj:`str`, optional): The name of the assembly containing the component mentioned above.
            max_depth (:obj:`int`, optional): The value determining how deep to go in the hierarchy of objects
                to find the field or property.

        """

        return commands.GetStaticProperty.run(
            self._connection,
            component_name, property_name, assembly, max_depth
        )

    def find_object_at_coordinates(self, coordinates):
        """Retrieves the Unity object at given coordinates

        Uses EventSystem.RaycastAll to find object. If no object is found then it uses UnityEngine.Physics.Raycast
        and UnityEngine.Physics2D.Raycast and returns the one closer to the camera.

        Args:
            coordinates (:obj:`dict`): The screen coordinates.

        Returns:
            AltUnityObject: The UI object hit by event system Raycast, ``None`` otherwise.

        """

        data = commands.FindObjectAtCoordinates.run(self._connection, coordinates)
        return self._get_alt_unity_object(data)

    def set_notification(self, notification_type, notification_callback=None):
        """Sets what notifications will the tester send and what to do with those notifications.

        Args:
            notification_type (:obj:`int`): Flag that sets which notification to be turned on.
            notification_callback (:obj:`class`): Class which contains callbacks used by notifications.

        """

        self._connection.notification_callbacks = notification_callback
        commands.SetNotification.run(self._connection, notification_type)

    def add_notification_listener(self, notification_type, notification_callback, overwrite=True):
        """Activates a notification that the tester will send.

        Args:
            notification_type (:obj:`int`): Flag that indicates which notification to be turned on.
            notification_callback (:obj:`method`): callback used when a notification is received.
            overwrite (:obj:'bool', optional): Flag to set if the new callback will overwrite the other
                callbacks or just append.

        """

        commands.AddNotificationListener.run(self._connection, notification_type, notification_callback, overwrite)

    def remove_notification_listener(self, notification_type):
        """Clear list of callback for the notification type and turn off the notification in tester.

        Args:
            notification_type (:obj:`int`): Flag that indicates which notification to be turned off.

        """

        commands.RemoveNotificationListener.run(self._connection, notification_type)
