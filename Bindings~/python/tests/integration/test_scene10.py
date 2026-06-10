"""
    Copyright(C) 2026 Altom Consulting
"""

import pytest

from .utils import Scenes
from alttester import By, AltKeyCode


class TestScene10:

    @pytest.fixture(autouse=True)
    def setup(self, alt_driver):
        self.alt_driver = alt_driver
        self.alt_driver.find_object(
            By.PATH, "/AltTesterPrefab//CloseButton", enabled=False).tap()
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene10)

    @pytest.mark.skip(reason="This test needs more investigation")
    def test_scroll_NIS(self):
        player = self.alt_driver.find_object(By.NAME, "Player")

        assert not player.get_component_property(
            "AltNIPDebugScript",
            "wasScrolled",
            "Assembly-CSharp"
        )

        self.alt_driver.scroll(300, duration=1, wait=True)

        assert player.get_component_property(
            "AltNIPDebugScript",
            "wasScrolled",
            "Assembly-CSharp"
        )

    def test_key_down_and_key_up_NIS(self):
        player = self.alt_driver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        self.alt_driver.key_down(AltKeyCode.A)
        self.alt_driver.key_up(AltKeyCode.A)
        lef_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert lef_position != initial_position

        self.alt_driver.key_down(AltKeyCode.D)
        self.alt_driver.key_up(AltKeyCode.D)
        right_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert right_position != lef_position

        self.alt_driver.key_down(AltKeyCode.W)
        self.alt_driver.key_up(AltKeyCode.W)
        up_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert up_position != right_position

        self.alt_driver.key_down(AltKeyCode.S)
        self.alt_driver.key_up(AltKeyCode.S)
        down_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert down_position != up_position

    def test_press_key_NIS(self):
        player = self.alt_driver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        self.alt_driver.press_key(AltKeyCode.A)
        left_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert left_position != initial_position

        self.alt_driver.press_key(AltKeyCode.D)
        right_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert right_position != left_position

        self.alt_driver.press_key(AltKeyCode.W)
        up_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert up_position != right_position

        self.alt_driver.press_key(AltKeyCode.S)
        down_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert down_position != up_position

    def test_press_keys_NIS(self):
        player = self.alt_driver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        keys = [AltKeyCode.W, AltKeyCode.Mouse0]
        self.alt_driver.press_keys(keys)

        final_position = player.get_component_property(
            "UnityEngine.Transform", "position", "UnityEngine.CoreModule")
        assert initial_position != final_position

        self.alt_driver.wait_for_object(By.NAME, "SimpleProjectile(Clone)")
