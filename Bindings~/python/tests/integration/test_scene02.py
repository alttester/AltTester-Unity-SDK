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


class TestScene02:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene02)

    def test_get_all_elements(self):
        elements = self.alt_driver.get_all_elements(enabled=False)
        assert elements is not None

        expected_names = {
            "EventSystem", "Canvas", "Panel Drag Area", "Panel",
            "Header", "Text", "Drag Zone", "Resize Zone", "Close Button",
            "Debugging", "SF Scene Elements", "Main Camera", "Background",
            "Particle System"
        }

        input_marks = []
        names = []

        for element in elements:
            if element.name == "InputMark(Clone)":
                input_marks.append(element.transformId)
                continue  # skip InputMark and direct children
            if element.transformParentId in input_marks:
                continue  # skip InputMark and direct children

            names.append(element.name)

        for name in expected_names:
            assert name in names

    def test_get_all_enabled_elements(self):
        # time.sleep(1)

        elements = self.alt_driver.get_all_elements(enabled=True)
        assert elements is not None

        expected_names = {
            "EventSystem", "Canvas", "Panel Drag Area", "Panel",
            "Header", "Text", "Drag Zone", "Resize Zone", "Close Button",
            "Debugging", "SF Scene Elements", "Main Camera", "Background",
            "Particle System"
        }
        names = [element.name for element in elements]
        assert len(names) >= 22
        for name in expected_names:
            assert name in names

    def test_resize_panel(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Resize Zone")
        position_init = (alt_object.x, alt_object.y)

        self.alt_driver.swipe(
            alt_object.get_screen_position(),
            (alt_object.x - 200, alt_object.y - 200),
            duration=2
        )
        # time.sleep(2)

        alt_object = self.alt_driver.find_object(By.NAME, "Resize Zone")
        position_final = (alt_object.x, alt_object.y)

        assert position_init != position_final

    def test_resize_panel_with_multipoint_swipe(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Resize Zone")
        position_init = (alt_object.x, alt_object.y)

        positions = [
            alt_object.get_screen_position(),
            [alt_object.x - 200, alt_object.y - 200],
            [alt_object.x - 300, alt_object.y - 100],
            [alt_object.x - 50, alt_object.y - 100],
            [alt_object.x - 100, alt_object.y - 100]
        ]
        self.alt_driver.multipoint_swipe(positions, duration=4)

        alt_object = self.alt_driver.find_object(By.NAME, "Resize Zone")
        position_final = (alt_object.x, alt_object.y)

        assert position_init != position_final

    def test_pointer_down_from_object(self):
        panel = self.alt_driver.find_object(By.NAME, "Panel")
        color1 = panel.get_component_property(
            "AltExampleScriptPanel",
            "normalColor",
            "Assembly-CSharp"
        )
        panel.pointer_down()

        color2 = panel.get_component_property(
            "AltExampleScriptPanel",
            "highlightColor",
            "Assembly-CSharp"
        )

        assert color1 != color2

    def test_pointer_up_from_object(self):
        panel = self.alt_driver.find_object(By.NAME, "Panel")
        color1 = panel.get_component_property(
            "AltExampleScriptPanel",
            "normalColor",
            "Assembly-CSharp"
        )
        panel.pointer_down()

        panel.pointer_up()
        color2 = panel.get_component_property(
            "AltExampleScriptPanel",
            "highlightColor",
            "Assembly-CSharp"
        )

        assert color1 == color2

    def test_new_touch_commands(self):
        draggable_area = self.alt_driver.find_object(By.NAME, "Drag Zone")
        initial_position = draggable_area.get_screen_position()

        finger_id = self.alt_driver.begin_touch(
            draggable_area.get_screen_position())
        self.alt_driver.move_touch(
            finger_id, [draggable_area.x + 10, draggable_area.y + 10])
        self.alt_driver.end_touch(finger_id)

        draggable_area = self.alt_driver.find_object(By.NAME, "Drag Zone")
        assert initial_position != draggable_area
