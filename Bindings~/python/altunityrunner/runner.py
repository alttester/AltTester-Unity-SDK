import socket
import time
import warnings
import sys

from deprecated import deprecated
from loguru import logger

import altunityrunner.commands as commands
from altunityrunner.__version__ import VERSION
from altunityrunner.by import By
from altunityrunner.altUnityExceptions import UnknownErrorException, AltUnityRecvallMessageFormatException


warnings.filterwarnings("default", category=DeprecationWarning, module=__name__)


class AltUnityDriver(object):

    def __init__(self, TCP_IP="127.0.0.1", TCP_PORT=13000, timeout=60, request_separator=";", request_end="&",
                 device_id=None, log_flag=False):
        self.TCP_IP = TCP_IP
        self.TCP_PORT = TCP_PORT

        self.request_separator = request_separator
        self.request_end = request_end
        self.log_flag = log_flag

        if device_id:
            raise DeprecationWarning("The 'device_id' argument is not longer supported.")

        if log_flag:
            logger.configure(
                handlers=[
                    dict(sink=sys.stdout),
                    dict(sink="./AltUnityTesterLog.txt", enqueue=False, serialize=True, mode="w"),
                ],
                levels=[dict(name="DEBUG")],
                activation=[("altunityrunner", True)],
            )
        else:
            logger.disable("altunityrunner")

        while timeout > 0:
            try:
                self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                self.socket.connect((TCP_IP, TCP_PORT))

                self._check_server_version()

                break
            except Exception as e:
                if self.socket is not None:
                    self.stop()

                logger.error(e)
                logger.warning(
                    "Trying to reach AltUnity Server at port {}, retrying (timing out in {} secs)...".format(
                        self.TCP_PORT,
                        timeout
                    )
                )

                timeout -= 1
                time.sleep(1)

        if timeout <= 0:
            raise Exception("Could not connect to AltUnityServer on: {}:{}".format(self.TCP_IP, self.TCP_PORT))

    @staticmethod
    def _split_version(version):
        parts = version.split(".")
        return (parts[0], parts[1]) if len(parts) > 1 else ("", "")

    def _check_server_version(self):
        serverVersion = ""
        try:
            serverVersion = commands.GetServerVersion(self.socket, self.request_separator, self.request_end).execute()
            logger.info("Connection established with AltUnity Server. Version: {}".format(serverVersion))
        except UnknownErrorException:
            serverVersion = "<=1.5.3"
        except AltUnityRecvallMessageFormatException:
            serverVersion = "<=1.5.7"

        majorServer, minorServer = self._split_version(serverVersion)
        majorDriver, minorDriver = self._split_version(VERSION)

        if majorServer != majorDriver or minorServer != minorDriver:
            message = "Version mismatch. AltUnity Driver version is {}. AltUnity Server version is {}.".format(
                VERSION,
                serverVersion
            )

            logger.warning(message)

    @staticmethod
    def is_camera_by_string(camera_by, camera_path):
        if isinstance(camera_by, str):
            return By.NAME, camera_by
        else:
            return camera_by, camera_path

    def stop(self):
        commands.CloseConnection(self.socket, self.request_separator, self.request_end).execute()
        self.socket.close()

    @deprecated(version="1.6.2", reason="Use call_static_method")
    def call_static_methods(self, type_name, method_name, parameters, type_of_parameters='', assembly=''):
        return commands.CallStaticMethod(
            self.socket, self.request_separator, self.request_end,
            type_name, method_name, parameters, type_of_parameters, assembly
        ).execute()

    def call_static_method(self, type_name, method_name, parameters, type_of_parameters='', assembly=''):
        return commands.CallStaticMethod(
            self.socket, self.request_separator, self.request_end,
            type_name, method_name, parameters, type_of_parameters, assembly
        ).execute()

    def get_all_elements(self, camera_by=By.NAME, camera_path="", enabled=True):
        return commands.GetAllElements(
            self.socket, self.request_separator, self.request_end,
            camera_by, camera_path, enabled
        ).execute()

    def find_object(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.FindObject(
            self.socket, self.request_separator, self.request_end,
            by, value, camera_by, camera_path, enabled
        ).execute()

    def find_object_which_contains(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.FindObjectWhichContains(
            self.socket, self.request_separator, self.request_end,
            by, value, camera_by, camera_path, enabled
        ).execute()

    def find_objects(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.FindObjects(
            self.socket, self.request_separator, self.request_end,
            by, value, camera_by, camera_path, enabled
        ).execute()

    def find_objects_which_contain(self, by, value, camera_by=By.NAME, camera_path="", enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.FindObjectsWhichContain(
            self.socket, self.request_separator, self.request_end,
            by, value, camera_by, camera_path, enabled
        ).execute()

    def get_current_scene(self):
        return commands.GetCurrentScene(self.socket, self.request_separator, self.request_end).execute()

    def swipe(self, x_start, y_start, x_end, y_end, duration_in_secs):
        return commands.Swipe(
            self.socket, self.request_separator, self.request_end,
            x_start, y_start, x_end, y_end, duration_in_secs
        ).execute()

    def swipe_and_wait(self, x_start, y_start, x_end, y_end, duration_in_secs):
        return commands.SwipeAndWait(
            self.socket, self.request_separator, self.request_end,
            x_start, y_start, x_end, y_end, duration_in_secs
        ).execute()

    def multipoint_swipe(self, positions, duration_in_secs):
        return commands.MultipointSwipe(
            self.socket, self.request_separator, self.request_end,
            positions, duration_in_secs
        ).execute()

    def multipoint_swipe_and_wait(self, positions, duration_in_secs):
        return commands.MultipointSwipeAndWait(
            self.socket, self.request_separator, self.request_end,
            positions, duration_in_secs
        ).execute()

    def tilt(self, x, y, z, duration=0):
        return commands.Tilt(
            self.socket, self.request_separator, self.request_end,
            x, y, z, duration
        ).execute()

    def tilt_and_wait(self, x, y, z, duration=0):
        return commands.TiltAndWait(
            self.socket, self.request_separator, self.request_end,
            x, y, z, duration
        ).execute()

    def hold_button(self, x_position, y_position, duration_in_secs):
        return commands.Swipe(
            self.socket, self.request_separator, self.request_end,
            x_position, y_position, x_position, y_position, duration_in_secs
        ).execute()

    def hold_button_and_wait(self, x_position, y_position, duration_in_secs):
        return commands.SwipeAndWait(
            self.socket, self.request_separator, self.request_end,
            x_position, y_position, x_position, y_position, duration_in_secs
        ).execute()

    @deprecated(version="1.6.5", reason="Use press_key_with_keycode(keyCode, power=1, duration=1) instead")
    def press_key(self, keyName, power=1, duration=1):
        return commands.PressKey(
            self.socket, self.request_separator, self.request_end,
            keyName, power, duration
        ).execute()

    def press_key_with_keycode(self, keyCode, power=1, duration=1):
        return commands.PressKeyWithKeyCode(
            self.socket, self.request_separator, self.request_end,
            keyCode, power, duration
        ).execute()

    @deprecated(version="1.6.5", reason="Use press_key_with_keycode_and_wait(keyCode, power=1, duration=1) instead")
    def press_key_and_wait(self, keyName, power=1, duration=1):
        return commands.PressKeyAndWait(
            self.socket, self.request_separator, self.request_end,
            keyName, power, duration
        ).execute()

    def press_key_with_keycode_and_wait(self, keyCode, power=1, duration=1):
        return commands.PressKeyWithKeyCodeAndWait(
            self.socket, self.request_separator, self.request_end,
            keyCode, power, duration
        ).execute()

    def key_down(self, keyCode, power=1):
        return commands.KeyDown(
            self.socket, self.request_separator, self.request_end,
            keyCode, power
        ).execute()

    def key_up(self, keyCode, power=1):
        return commands.KeyUp(
            self.socket, self.request_separator, self.request_end,
            keyCode
        ).execute()

    def move_mouse(self, x, y, duration):
        return commands.MoveMouse(
            self.socket, self.request_separator, self.request_end,
            x, y, duration
        ).execute()

    def move_mouse_and_wait(self, x, y, duration):
        return commands.MoveMouseAndWait(
            self.socket, self.request_separator, self.request_end,
            x, y, duration
        ).execute()

    def scroll_mouse(self, speed, duration):
        return commands.ScrollMouse(
            self.socket, self.request_separator, self.request_end,
            speed, duration
        ).execute()

    def scroll_mouse_and_wait(self, speed, duration):
        return commands.ScrollMouseAndWait(
            self.socket, self.request_separator, self.request_end,
            speed, duration
        ).execute()

    def set_player_pref_key(self, key_name, value, key_type):
        return commands.SetPlayerPrefKey(
            self.socket, self.request_separator, self.request_end,
            key_name, value, key_type
        ).execute()

    def get_player_pref_key(self, key_name, key_type):
        return commands.GetPlayerPrefKey(
            self.socket, self.request_separator, self.request_end,
            key_name, key_type
        ).execute()

    def delete_player_pref_key(self, key_name):
        return commands.DeletePlayerPrefKey(
            self.socket, self.request_separator, self.request_end,
            key_name
        ).execute()

    def delete_player_prefs(self):
        return commands.DeletePlayerPref(self.socket, self.request_separator, self.request_end).execute()

    def load_scene(self, scene_name, load_single=True):
        return commands.LoadScene(
            self.socket, self.request_separator, self.request_end,
            scene_name, load_single
        ).execute()

    def unload_scene(self, scene_name):
        return commands.UnloadScene(
            self.socket, self.request_separator, self.request_end,
            scene_name
        ).execute()

    def set_time_scale(self, time_scale):
        return commands.SetTimeScale(
            self.socket, self.request_separator, self.request_end,
            time_scale
        ).execute()

    def get_time_scale(self):
        return commands.GetTimeScale(self.socket, self.request_separator, self.request_end).execute()

    def wait_for_current_scene_to_be(self, scene_name, timeout=30, interval=1):
        return commands.WaitForCurrentSceneToBe(
            self.socket, self.request_separator, self.request_end,
            scene_name, timeout, interval
        ).execute()

    def wait_for_object(self, by, value, camera_by=By.NAME, camera_path="", timeout=20, interval=0.5, enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.WaitForObject(
            self.socket, self.request_separator, self.request_end,
            by, value, camera_by, camera_path, timeout, interval, enabled
        ).execute()

    def wait_for_object_which_contains(self, by, value, camera_by=By.NAME, camera_path="", timeout=20, interval=0.5,
                                       enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.WaitForObjectWhichContains(
            self.socket, self.request_separator, self.request_end,
            by, value, camera_by, camera_path, timeout, interval, enabled
        ).execute()

    def wait_for_object_to_not_be_present(self, by, value, camera_by=By.NAME, camera_path="", timeout=20, interval=0.5,
                                          enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.WaitForObjectToNotBePresent(
            self.socket, self.request_separator, self.request_end,
            by, value, camera_by, camera_path, timeout, interval, enabled
        ).execute()

    @deprecated(version="1.6.3", reason="Use altunityrunner.runner.AltUnityDriver.wait_for_object")
    def wait_for_object_with_text(self, by, value, text, camera_by=By.NAME, camera_path="", timeout=20, interval=0.5,
                                  enabled=True):
        camera_by, camera_path = self.is_camera_by_string(camera_by, camera_path)
        return commands.WaitForObjectWithText(
            self.socket, self.request_separator, self.request_end,
            by, value, text, camera_by, camera_path, timeout, interval, enabled
        ).execute()

    @deprecated(version="1.6.5", reason="Use tap")
    def tap_at_coordinates(self, x, y):
        return commands.TapAtCoordinates(
            self.socket, self.request_separator, self.request_end,
            x, y
        ).execute()

    @deprecated(version="1.6.5", reason="Use tap")
    def tap_custom(self, x, y, count, interval=0.1):
        return commands.TapCustom(
            self.socket, self.request_separator, self.request_end,
            x, y, count, interval
        ).execute()

    def tap(self, coordinates, count=1, interval=0.1, wait=True):
        '''Tap at screen coordinates

    Parameters:
        coordinates -- The screen coordinates
        count -- Number of taps (default 1)
        interval -- Interval between taps in seconds (default 0.1)
        wait -- Wait for command to finish
        '''
        return commands.TapCoordinates(
            self.socket, self.request_separator, self.request_end,
            coordinates, count, interval, wait
        ).execute()

    def click(self, coordinates, count=1, interval=0.1, wait=True):
        '''Click at screen coordinates

    Parameters:
        coordinates -- The screen coordinates
        count -- Number of taps (default 1)
        interval -- Interval between taps in seconds (default 0.1)
        wait -- Wait for command to finish
        '''
        return commands.ClickCoordinates(
            self.socket, self.request_separator, self.request_end,
            coordinates, count, interval, wait
        ).execute()

    def get_png_screenshot(self, path):
        commands.GetPNGScreenshot(
            self.socket, self.request_separator, self.request_end,
            path
        ).execute()

    def get_all_loaded_scenes(self):
        return commands.GetAllLoadedScenes(self.socket, self.request_separator, self.request_end).execute()

    def set_server_logging(self, logger, log_level):
        return commands.SetServerLogging(
            self.socket, self.request_separator, self.request_end,
            logger, log_level
        ).execute()

    def begin_touch(self, coordinates):
        return commands.BeginTouch(self.socket, self.request_separator, self.request_end, coordinates).execute()

    def move_touch(self, finger_id, coordinates):
        commands.MoveTouch(self.socket, self.request_separator, self.request_end, finger_id, coordinates).execute()

    def end_touch(self, finger_id):
        commands.EndTouch(self.socket, self.request_separator, self.request_end, finger_id).execute()
