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

import time

import pytest

from .utils import Scenes
from alttester import By, AltKeyCode


class TestScene05:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene05)

    def test_movement_cube(self):
        cube = self.alt_driver.find_object(By.NAME, "Player1")
        initial_position = (cube.worldX, cube.worldY, cube.worldZ)

        self.alt_driver.scroll(speed_vertical=30, duration=0.1, wait=False)
        self.alt_driver.press_key(
            AltKeyCode.K, power=1, duration=0.1, wait=False)

        self.alt_driver.press_key(AltKeyCode.O, power=1, duration=0.1)
        cube = self.alt_driver.find_object(By.NAME, "Player1")
        final_position = (cube.worldX, cube.worldY, cube.worldZ)

        assert initial_position != final_position

    def test_camera_movement(self):
        cube = self.alt_driver.find_object(By.NAME, "Player1")
        initial_position = (cube.worldX, cube.worldY, cube.worldY)

        self.alt_driver.press_key(
            AltKeyCode.W, power=1, duration=0.1, wait=False)
        # time.sleep(2)

        cube = self.alt_driver.find_object(By.NAME, "Player1")
        final_position = (cube.worldX, cube.worldY, cube.worldY)

        assert initial_position != final_position

    def test_update_altObject(self):

        cube = self.alt_driver.find_object(By.NAME, "Player1")
        initial_position_z = cube.worldZ

        self.alt_driver.press_key(
            AltKeyCode.W, power=1, duration=0.1, wait=False)
        time.sleep(5)

        assert initial_position_z != cube.update_object().worldZ

    def test_creating_stars(self):
        stars = self.alt_driver.find_objects_which_contain(
            By.NAME, "Star", By.NAME, "Player2")
        assert len(stars) == 1

        self.alt_driver.find_objects_which_contain(
            By.NAME, "Player", By.NAME, "Player2")
        pressing_point_1 = self.alt_driver.find_object(
            By.NAME, "PressingPoint1", By.NAME, "Player2")

        self.alt_driver.move_mouse(
            pressing_point_1.get_screen_position(), duration=0.1, wait=False)
        time.sleep(0.1)

        self.alt_driver.press_key(
            AltKeyCode.Mouse0, power=1, duration=0.1, wait=False)
        pressing_point_2 = self.alt_driver.find_object(
            By.NAME, "PressingPoint2", By.NAME, "Player2")
        self.alt_driver.move_mouse(
            pressing_point_2.get_screen_position(), duration=0.1)
        self.alt_driver.press_key(
            AltKeyCode.Mouse0, power=1, duration=0.1, wait=False)
        time.sleep(0.1)

        stars = self.alt_driver.find_objects_which_contain(By.NAME, "Star")
        assert len(stars) == 3

    def test_power_joystick(self):
        button_names = ["Horizontal", "Vertical"]
        keys_to_press = [AltKeyCode.D, AltKeyCode.W]

        axis_name = self.alt_driver.find_object(By.NAME, "AxisName")
        axis_value = self.alt_driver.find_object(By.NAME, "AxisValue")

        for button_name, key in zip(button_names, keys_to_press):
            self.alt_driver.press_key(key, power=0.5, duration=1)

            assert axis_value.get_text() == "0.5"
            assert axis_name.get_text() == button_name

    def test_scroll(self):
        player2 = self.alt_driver.find_object(By.NAME, "Player2")
        cube_initial_position = [player2.worldX,
                                 player2.worldY, player2.worldY]
        self.alt_driver.scroll(4, duration=1, wait=False)
        time.sleep(1)

        player2 = self.alt_driver.find_object(By.NAME, "Player2")
        cube_final_position = [player2.worldX, player2.worldY, player2.worldY]
        assert cube_initial_position != cube_final_position

    def test_scroll_and_wait(self):
        player2 = self.alt_driver.find_object(By.NAME, "Player2")
        cube_initial_position = [player2.worldX,
                                 player2.worldY, player2.worldY]
        self.alt_driver.scroll(4, duration=0.3)

        player2 = self.alt_driver.find_object(By.NAME, "Player2")
        cube_final_position = [player2.worldX, player2.worldY, player2.worldY]
        assert cube_initial_position != cube_final_position

    def test_key_down_and_key_up(self):
        self.alt_driver.key_down(AltKeyCode.A)

        last_key_down = self.alt_driver.find_object(By.NAME, "LastKeyDownValue")
        last_key_press = self.alt_driver.find_object(
            By.NAME, "LastKeyPressedValue")

        assert last_key_down.get_text() == "97"
        assert last_key_press.get_text() == "97"

        self.alt_driver.key_up(AltKeyCode.A)
        last_key_up = self.alt_driver.find_object(By.NAME, "LastKeyUpValue")

        assert last_key_up.get_text() == "97"
