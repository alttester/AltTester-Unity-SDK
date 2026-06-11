"""
    Copyright(C) 2026 Altom Consulting
"""

import pytest

from .utils import Scenes
from alttester import By


class TestScene09:

    @pytest.fixture(autouse=True)
    def setup(self, alt_driver):
        self.alt_driver = alt_driver
        self.alt_driver.find_object(
            By.PATH, "/AltTesterPrefab//CloseButton", enabled=False).tap()
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
