import warnings
import sys

from loguru import logger

import altunityrunner.commands as commands
from altunityrunner.__version__ import VERSION
from altunityrunner._websocket import WebsocketConnection
from altunityrunner.altElement import AltElement
from altunityrunner.by import By


warnings.filterwarnings("default", category=DeprecationWarning, module=__name__)


class AltUnityDriver:
    """The driver object will help interacting with all the game objects, their properties and methods.

    When you instantiate an ``AltUnityDriver`` object in your tests, you can use it to “drive” your game like one of
    your users would, by interacting with all the game objects, their properties and methods.  An ``AltUnityDriver``
    instance will connect to the AltUnity Server that is running inside the game.

    Args:
        host (:obj:`str`): The server host to connect to.
        port (:obj:`int`): The server port to connect to.
        timeout (:obj:`int` or :obj:`float`): The server connection timeout time.
        tries (:obj:`int`): The maximum number of attempts to connect to the server.
        enable_logging (:obj:`bool`): If set to ``True`` will turn on logging, by default logging is disabled.

    """

    def __init__(self, host="127.0.0.1", port=13000, timeout=None, tries=5, enable_logging=False):
        self.host = host
        self.port = port
        self.enable_logging = enable_logging

        self._config_logging(self.enable_logging)
        self._connection = WebsocketConnection(host=host, port=port, timeout=timeout, tries=tries)

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
        logger.info("Connection established with AltUnity Server. Version: {}".format(server_version))

        major_server, minor_server = self._split_version(server_version)
        major_driver, minor_driver = self._split_version(VERSION)

        if major_server != major_driver or minor_server != minor_driver:
            message = "Version mismatch. AltUnity Driver version is {}. AltUnity Server version is {}.".format(
                VERSION,
                server_version
            )

            logger.warning(message)

    def _get_alt_element(self, data):
        if data is None:
            return None

        alt_element = AltElement(self, data)

        logger.debug("Element {} found at x:{} y:{} mobileY:{}".format(
            alt_element.name,
            alt_element.x,
            alt_element.y,
            alt_element.mobileY
        ))

        return alt_element

    def _get_alt_elements(self, data):
        if data is None:
            return None

        alt_elements = []

        for element in data:
            alt_element = AltElement(self, element)
            alt_elements.append(alt_element)

            logger.debug("Element {} found at x:{} y:{} mobileY:{}".format(
                alt_element.name,
                alt_element.x,
                alt_element.y,
                alt_element.mobileY
            ))

        return alt_elements

    @staticmethod
    def is_camera_by_string(camera_by, camera_path):
        if isinstance(camera_by, str):
            return By.NAME, camera_by
        else:
            return camera_by, camera_path

    def stop(self):
        self._connection.close()

    def call_static_method(self, type_name, method_name, parameters=None, type_of_parameters=None, assembly=""):
        parameters = parameters if parameters is not None else []
        type_of_parameters = type_of_parameters if type_of_parameters is not None else []
        return commands.CallStaticMethod.run(
            self._connection,
            type_name, method_name, parameters, type_of_parameters, assembly
        )

    def get_all_elements(self, camera_by=By.NAME, camera_path="", enabled=True):
        data = commands.GetAllElements.run(
            self._connection,
            camera_by, camera_path, enabled
        )

        return self._get_alt_elements(data)

    def find_object(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)

        data = commands.FindObject.run(
            self._connection,
            by, value, camera_by, camera_path, enabled
        )

        return self._get_alt_element(data)

    def find_object_which_contains(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)

        data = commands.FindObjectWhichContains.run(
            self._connection,
            by, value, camera_by, camera_path, enabled
        )

        return self._get_alt_element(data)

    def find_objects(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)

        data = commands.FindObjects.run(
            self._connection,
            by, value, camera_by, camera_path, enabled
        )

        return self._get_alt_elements(data)

    def find_objects_which_contain(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)

        data = commands.FindObjectsWhichContain.run(
            self._connection,
            by, value, camera_by, camera_path, enabled
        )

        return self._get_alt_elements(data)

    def get_current_scene(self):
        return commands.GetCurrentScene.run(self._connection)

    def swipe(self, x_start, y_start, x_end, y_end, duration_in_secs):
        return commands.Swipe.run(
            self._connection,
            x_start, y_start, x_end, y_end, duration_in_secs
        )

    def swipe_and_wait(self, x_start, y_start, x_end, y_end, duration_in_secs):
        return commands.SwipeAndWait.run(
            self._connection,
            x_start, y_start, x_end, y_end, duration_in_secs
        )

    def multipoint_swipe(self, positions, duration_in_secs):
        return commands.MultipointSwipe.run(
            self._connection,
            positions, duration_in_secs
        )

    def multipoint_swipe_and_wait(self, positions, duration_in_secs):
        return commands.MultipointSwipeAndWait.run(
            self._connection,
            positions, duration_in_secs
        )

    def tilt(self, x, y, z, duration=0):
        return commands.Tilt.run(
            self._connection,
            x, y, z, duration
        )

    def tilt_and_wait(self, x, y, z, duration=0):
        return commands.TiltAndWait.run(
            self._connection,
            x, y, z, duration
        )

    def hold_button(self, x_position, y_position, duration_in_secs):
        return commands.Swipe.run(
            self._connection,
            x_position, y_position, x_position, y_position, duration_in_secs
        )

    def hold_button_and_wait(self, x_position, y_position, duration_in_secs):
        return commands.SwipeAndWait.run(
            self._connection,
            x_position, y_position, x_position, y_position, duration_in_secs
        )

    def press_key_with_keycode(self, key_code, power=1, duration=1):
        return commands.PressKey.run(self._connection, key_code, power, duration)

    def press_key_with_keycode_and_wait(self, key_code, power=1, duration=1):
        return commands.PressKeyAndWait.run(
            self._connection,
            key_code, power, duration
        )

    def key_down(self, key_code, power=1):
        return commands.KeyDown.run(self._connection, key_code, power)

    def key_up(self, key_code):
        return commands.KeyUp.run(self._connection, key_code)

    def move_mouse(self, x, y, duration):
        return commands.MoveMouse.run(self._connection, x, y, duration)

    def move_mouse_and_wait(self, x, y, duration):
        return commands.MoveMouseAndWait.run(
            self._connection,
            x, y, duration
        )

    def scroll_mouse(self, speed, duration):
        return commands.ScrollMouse.run(
            self._connection,
            speed, duration
        )

    def scroll_mouse_and_wait(self, speed, duration):
        return commands.ScrollMouseAndWait.run(
            self._connection,
            speed, duration
        )

    def set_player_pref_key(self, key_name, value, key_type):
        return commands.SetPlayerPrefKey.run(
            self._connection,
            key_name, value, key_type
        )

    def get_player_pref_key(self, key_name, key_type):
        return commands.GetPlayerPrefKey.run(
            self._connection,
            key_name, key_type
        )

    def delete_player_pref_key(self, key_name):
        return commands.DeletePlayerPrefKey.run(self._connection, key_name)

    def delete_player_prefs(self):
        return commands.DeletePlayerPref.run(self._connection)

    def load_scene(self, scene_name, load_single=True):
        return commands.LoadScene.run(
            self._connection,
            scene_name, load_single
        )

    def unload_scene(self, scene_name):
        return commands.UnloadScene.run(self._connection, scene_name)

    def set_time_scale(self, time_scale):
        return commands.SetTimeScale.run(self._connection, time_scale)

    def get_time_scale(self):
        return commands.GetTimeScale.run(self._connection)

    def wait_for_current_scene_to_be(self, scene_name, timeout=30, interval=1):
        return commands.WaitForCurrentSceneToBe.run(
            self._connection,
            scene_name, timeout, interval
        )

    def wait_for_object(self, by, value, camera_by=By.NAME, camera_path="", timeout=20, interval=0.5, enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)

        data = commands.WaitForObject.run(
            self._connection,
            by, value, camera_by, camera_path, timeout, interval, enabled
        )

        return self._get_alt_element(data)

    def wait_for_object_which_contains(self, by, value, camera_by=By.NAME, camera_path="", timeout=20, interval=0.5,
                                       enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)

        data = commands.WaitForObjectWhichContains.run(
            self._connection,
            by, value, camera_by, camera_path, timeout, interval, enabled
        )

        return self._get_alt_element(data)

    def wait_for_object_to_not_be_present(self, by, value, camera_by=By.NAME, camera_path="", timeout=20, interval=0.5,
                                          enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)

        return commands.WaitForObjectToNotBePresent.run(
            self._connection,
            by, value, camera_by, camera_path, timeout, interval, enabled
        )

    def tap(self, coordinates, count=1, interval=0.1, wait=True):
        """Tap at screen coordinates.

        Args:
            coordinates: The screen coordinates.
            count: Number of taps (default 1).
            interval: Interval between taps in seconds (default 0.1).
            wait: Wait for command to finish.
        """

        return commands.TapCoordinates.run(self._connection, coordinates, count, interval, wait)

    def click(self, coordinates, count=1, interval=0.1, wait=True):
        """Click at screen coordinates.

        Args:
            coordinates: The screen coordinates.
            count: Number of taps (default 1).
            interval: Interval between taps in seconds (default 0.1).
            wait: Wait for command to finish.
        """

        return commands.ClickCoordinates.run(self._connection, coordinates, count, interval, wait)

    def get_png_screenshot(self, path):
        commands.GetPNGScreenshot.run(self._connection, path)

    def get_all_loaded_scenes(self):
        return commands.GetAllLoadedScenes.run(self._connection)

    def set_server_logging(self, logger, log_level):
        return commands.SetServerLogging.run(self._connection, logger, log_level)

    def begin_touch(self, coordinates):
        return commands.BeginTouch.run(self._connection, coordinates)

    def move_touch(self, finger_id, coordinates):
        commands.MoveTouch.run(self._connection, finger_id, coordinates)

    def end_touch(self, finger_id):
        commands.EndTouch.run(self._connection, finger_id)
