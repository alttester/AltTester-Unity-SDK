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

import pytest

from .utils import Scenes
from alttester import By


class TestScene09:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene09)

    def test_scroll_element_NIS(self):
        scrollbar_initial = self.alt_driver.find_object(
            By.NAME, "Scrollbar Vertical")
        scrollbar_initial_value = scrollbar_initial.get_component_property(
            "UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI")
        self.alt_driver.move_mouse(self.alt_driver.find_object(
            By.NAME, "Scroll View").get_screen_position(), duration=0.3, wait=True)
        self.alt_driver.scroll(-3000, duration=0.5, wait=True)

        scrollbar_final = self.alt_driver.find_object(
            By.NAME, "Scrollbar Vertical")
        scrollbar_final_value = scrollbar_final.get_component_property(
            "UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI")

        assert scrollbar_initial_value != scrollbar_final_value

    def test_swipe_NIS(self):
        scrollbar = self.alt_driver.find_object(By.NAME, "Handle")
        scrollbar_position = scrollbar.get_screen_position()
        button = self.alt_driver.find_object(
            By.PATH, "//Scroll View/Viewport/Content/Button (4)")
        self.alt_driver.swipe(
            button.get_screen_position(),
            (button.x, button.y + 20),
            duration=0.5
        )

        scrollbar_final = self.alt_driver.find_object(By.NAME, "Handle")
        scrollbar_final_position = scrollbar_final.get_screen_position()
        assert scrollbar_position != scrollbar_final_position
