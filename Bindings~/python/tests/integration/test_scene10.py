"""
    Copyright(C) 2023 Altom Consulting

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
from alttester import By, AltKeyCode


class TestScene10:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.altdriver.reset_input()
        self.altdriver.load_scene(Scenes.Scene10)

    def test_scroll_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")

        assert not player.get_component_property(
            "AltNIPDebugScript",
            "wasScrolled",
            "Assembly-CSharp"
        )

        self.altdriver.scroll(300, duration=1, wait=True)

        assert player.get_component_property(
            "AltNIPDebugScript",
            "wasScrolled",
            "Assembly-CSharp"
        )

    def test_key_down_and_key_up_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        self.altdriver.key_down(AltKeyCode.A)
        self.altdriver.key_up(AltKeyCode.A)
        lef_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert lef_position != initial_position

        self.altdriver.key_down(AltKeyCode.D)
        self.altdriver.key_up(AltKeyCode.D)
        right_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert right_position != lef_position

        self.altdriver.key_down(AltKeyCode.W)
        self.altdriver.key_up(AltKeyCode.W)
        up_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert up_position != right_position

        self.altdriver.key_down(AltKeyCode.S)
        self.altdriver.key_up(AltKeyCode.S)
        down_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert down_position != up_position

    def test_press_key_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        self.altdriver.press_key(AltKeyCode.A)
        left_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert left_position != initial_position

        self.altdriver.press_key(AltKeyCode.D)
        right_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert right_position != left_position

        self.altdriver.press_key(AltKeyCode.W)
        up_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert up_position != right_position

        self.altdriver.press_key(AltKeyCode.S)
        down_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        assert down_position != up_position

    def test_press_keys_NIS(self):
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")

        keys = [AltKeyCode.W, AltKeyCode.Mouse0]
        self.altdriver.press_keys(keys)

        final_position = player.get_component_property("UnityEngine.Transform", "position", "UnityEngine.CoreModule")
        assert initial_position != final_position

        self.altdriver.wait_for_object(By.NAME, "SimpleProjectile(Clone)")
