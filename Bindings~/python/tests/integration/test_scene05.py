import time

import pytest

from .utils import Scenes
from altunityrunner import By, AltUnityKeyCode


class TestScene05:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene05)

    def test_movement_cube(self):
        cube = self.altdriver.find_object(By.NAME, "Player1")
        initial_position = (cube.worldX, cube.worldY, cube.worldZ)

        self.altdriver.scroll(speed_vertical=30, duration=0.1, wait=False)
        self.altdriver.press_key(AltUnityKeyCode.K, power=1, duration=0.1, wait=False)

        self.altdriver.press_key(AltUnityKeyCode.O, power=1, duration=0.1)
        cube = self.altdriver.find_object(By.NAME, "Player1")
        final_position = (cube.worldX, cube.worldY, cube.worldZ)

        assert initial_position != final_position

    def test_camera_movement(self):
        cube = self.altdriver.find_object(By.NAME, "Player1")
        initial_position = (cube.worldX, cube.worldY, cube.worldY)

        self.altdriver.press_key(AltUnityKeyCode.W, power=1, duration=0.1, wait=False)
        # time.sleep(2)

        cube = self.altdriver.find_object(By.NAME, "Player1")
        final_position = (cube.worldX, cube.worldY, cube.worldY)

        assert initial_position != final_position

    def test_creating_stars(self):
        stars = self.altdriver.find_objects_which_contain(By.NAME, "Star", By.NAME, "Player2")
        assert len(stars) == 1

        self.altdriver.find_objects_which_contain(By.NAME, "Player", By.NAME, "Player2")
        pressing_point_1 = self.altdriver.find_object(By.NAME, "PressingPoint1", By.NAME, "Player2")

        self.altdriver.move_mouse(pressing_point_1.get_screen_position(), duration=0.1, wait=False)
        time.sleep(0.1)

        self.altdriver.press_key(AltUnityKeyCode.Mouse0, power=1, duration=0.1, wait=False)
        pressing_point_2 = self.altdriver.find_object(By.NAME, "PressingPoint2", By.NAME, "Player2")
        self.altdriver.move_mouse(pressing_point_2.get_screen_position(), duration=0.1)
        self.altdriver.press_key(AltUnityKeyCode.Mouse0, power=1, duration=0.1, wait=False)
        time.sleep(0.1)

        stars = self.altdriver.find_objects_which_contain(By.NAME, "Star")
        assert len(stars) == 3

    def test_power_joystick(self):
        button_names = ["Horizontal", "Vertical"]
        keys_to_press = [AltUnityKeyCode.D, AltUnityKeyCode.W]

        axis_name = self.altdriver.find_object(By.NAME, "AxisName")
        axis_value = self.altdriver.find_object(By.NAME, "AxisValue")

        for button_name, key in zip(button_names, keys_to_press):
            self.altdriver.press_key(key, power=0.5, duration=0.1)

            assert axis_value.get_text() == "0.5"
            assert axis_name.get_text() == button_name

    def test_scroll(self):
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cube_initial_position = [player2.worldX, player2.worldY, player2.worldY]
        self.altdriver.scroll(4, duration=0.1, wait=False)
        time.sleep(0.2)

        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cube_final_position = [player2.worldX, player2.worldY, player2.worldY]
        assert cube_initial_position != cube_final_position

    def test_scroll_and_wait(self):
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cube_initial_position = [player2.worldX, player2.worldY, player2.worldY]
        self.altdriver.scroll(4, duration=0.1)

        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cube_final_position = [player2.worldX, player2.worldY, player2.worldY]
        assert cube_initial_position != cube_final_position

    def test_key_down_and_key_up(self):
        self.altdriver.key_down(AltUnityKeyCode.A)

        last_key_down = self.altdriver.find_object(By.NAME, "LastKeyDownValue")
        last_key_press = self.altdriver.find_object(By.NAME, "LastKeyPressedValue")

        assert last_key_down.get_text() == "97"
        assert last_key_press.get_text() == "97"

        self.altdriver.key_up(AltUnityKeyCode.A)
        last_key_up = self.altdriver.find_object(By.NAME, "LastKeyUpValue")

        assert last_key_up.get_text() == "97"
