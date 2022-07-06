import pytest

from .utils import Scenes
from altunityrunner import By, AltUnityKeyCode


class TestScene10:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene10)

    def test_scroll_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")

        assert not player.get_component_property(
            "AltUnityNIPDebugScript",
            "wasScrolled",
            assembly="Assembly-CSharp"
        )

        self.altdriver.scroll(300, duration=1, wait=True)

        assert player.get_component_property(
            "AltUnityNIPDebugScript",
            "wasScrolled",
            assembly="Assembly-CSharp"
        )

    def test_key_down_and_key_up_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property("UnityEngine.Transform", "position")

        self.altdriver.key_down(AltUnityKeyCode.A)
        self.altdriver.key_up(AltUnityKeyCode.A)
        lef_position = player.get_component_property("UnityEngine.Transform", "position")

        assert lef_position != initial_position

        self.altdriver.key_down(AltUnityKeyCode.D)
        self.altdriver.key_up(AltUnityKeyCode.D)
        right_position = player.get_component_property("UnityEngine.Transform", "position")

        assert right_position != lef_position

        self.altdriver.key_down(AltUnityKeyCode.W)
        self.altdriver.key_up(AltUnityKeyCode.W)
        up_position = player.get_component_property("UnityEngine.Transform", "position")

        assert up_position != right_position

        self.altdriver.key_down(AltUnityKeyCode.S)
        self.altdriver.key_up(AltUnityKeyCode.S)
        down_position = player.get_component_property("UnityEngine.Transform", "position")

        assert down_position != up_position

    def test_press_key_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property("UnityEngine.Transform", "position")

        self.altdriver.press_key(AltUnityKeyCode.A)
        left_position = player.get_component_property("UnityEngine.Transform", "position")

        assert left_position != initial_position

        self.altdriver.press_key(AltUnityKeyCode.D)
        right_position = player.get_component_property("UnityEngine.Transform", "position")

        assert right_position != left_position

        self.altdriver.press_key(AltUnityKeyCode.W)
        up_position = player.get_component_property("UnityEngine.Transform", "position")

        assert up_position != right_position

        self.altdriver.press_key(AltUnityKeyCode.S)
        down_position = player.get_component_property("UnityEngine.Transform", "position")

        assert down_position != up_position

    def test_press_keys_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property("UnityEngine.Transform", "position")

        keys = [AltUnityKeyCode.W, AltUnityKeyCode.Mouse0]
        self.altdriver.press_keys(keys)

        final_position = player.get_component_property("UnityEngine.Transform", "position")
        assert initial_position != final_position

        self.altdriver.wait_for_object(By.NAME, "SimpleProjectile(Clone)")
