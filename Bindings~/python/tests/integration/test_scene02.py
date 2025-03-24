"""
    Copyright(C) 2025 Altom Consulting

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
        self.alt_driver.find_object(
            By.PATH, "/AltTesterPrefab//CloseButton", enabled=False).tap()
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene02)

    def test(self):
        controls__wrap_path = "//FlexboxDemo-container/background/demo__area/demo__area-bottom/controls__group-1/controls__wrap"
        unity_engine_ui_elements_visual_element_path = "//UnityEngine.UIElements.VisualElement"
        unity_engine_ui_elements_visual_element_path1 = "//UnityEngine.UIElements.VisualElement/UnityEngine.UIElements.ScrollView/UnityEngine.UIElements.VisualElement[2]/UnityEngine.UIElements.VisualElement"
        controls__direction_path = "//FlexboxDemo-container/background/demo__area/demo__area-bottom/controls__group-1/controls__direction"
        unity_engine_ui_elements_visual_element_path2 = "//UnityEngine.UIElements.VisualElement/UnityEngine.UIElements.VisualElement/UnityEngine.UIElements.ScrollView/UnityEngine.UIElements.VisualElement[2]"
        controls__align_path = "//FlexboxDemo-container/background/demo__area/demo__area-bottom/controls__group-2/controls__align"
        unity_engine_ui_elements_visual_element_path3 = "//UnityEngine.UIElements.VisualElement/UnityEngine.UIElements.VisualElement/UnityEngine.UIElements.ScrollView/UnityEngine.UIElements.VisualElement[1]/UnityEngine.UIElements.VisualElement"
        unity_engine_ui_elements_visual_element_path4 = "//UnityEngine.UIElements.VisualElement/UnityEngine.UIElements.VisualElement/UnityEngine.UIElements.ScrollView/UnityEngine.UIElements.VisualElement[3]/UnityEngine.UIElements.VisualElement"
        self.alt_driver.load_scene("FlexboxDemo", True)
        controls__wrap = self.alt_driver.wait_for_object(
            By.PATH, controls__wrap_path, timeout=3)
        unity_engine_ui_elements_visual_element = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path, timeout=3)
        self.alt_driver.swipe(controls__wrap.get_screen_position(
        ), unity_engine_ui_elements_visual_element.get_screen_position(), 0.09999847)
        unity_engine_ui_elements_visual_element1 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path1, timeout=3)
        self.alt_driver.hold_button(
            unity_engine_ui_elements_visual_element1.get_screen_position(), 0.8929825)
        controls__direction = self.alt_driver.wait_for_object(
            By.PATH, controls__direction_path, timeout=3)
        unity_engine_ui_elements_visual_element2 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path2, timeout=3)
        unity_engine_ui_elements_visual_element3 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path1, timeout=3)
        unity_engine_ui_elements_visual_element4 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path, timeout=3)
        controls__align = self.alt_driver.wait_for_object(
            By.PATH, controls__align_path, timeout=3)
        unity_engine_ui_elements_visual_element5 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path, timeout=3)
        self.alt_driver.swipe(controls__align.get_screen_position(
        ), unity_engine_ui_elements_visual_element5.get_screen_position(), 0.2331352)
        unity_engine_ui_elements_visual_element6 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path1, timeout=3)
        unity_engine_ui_elements_visual_element7 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path3, timeout=3)
        unity_engine_ui_elements_visual_element8 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path1, timeout=3)
        self.alt_driver.swipe(unity_engine_ui_elements_visual_element7.get_screen_position(
        ), unity_engine_ui_elements_visual_element8.get_screen_position(), 0.09999847)
        unity_engine_ui_elements_visual_element9 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path4, timeout=3)
        unity_engine_ui_elements_visual_element10 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path1, timeout=3)
        unity_engine_ui_elements_visual_element10.click()
        unity_engine_ui_elements_visual_element11 = self.alt_driver.wait_for_object(
            By.PATH, unity_engine_ui_elements_visual_element_path3, timeout=3)

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
