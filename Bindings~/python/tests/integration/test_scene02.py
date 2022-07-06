import pytest

from .utils import Scenes
from altunityrunner import By


class TestScene02:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene02)

    def test_get_all_elements(self):
        elements = self.altdriver.get_all_elements(enabled=False)
        assert elements is not None

        expected_names = {
            "EventSystem", "Canvas", "Panel Drag Area", "Panel",
            "Header", "Text", "Drag Zone", "Resize Zone", "Close Button",
            "Debugging", "SF Scene Elements", "Main Camera", "Background",
            "Particle System", "AltUnityDialog"
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

        assert len(names) == 34
        for name in expected_names:
            assert name in names

    def test_get_all_enabled_elements(self):
        # time.sleep(1)

        elements = self.altdriver.get_all_elements(enabled=True)
        assert elements is not None

        expected_names = {
            "EventSystem", "Canvas", "Panel Drag Area", "Panel",
            "Header", "Text", "Drag Zone", "Resize Zone", "Close Button",
            "Debugging", "SF Scene Elements", "Main Camera", "Background",
            "Particle System"
        }
        names = [element.name for element in elements]
        assert len(names) == 24
        for name in expected_names:
            assert name in names

    def test_resize_panel(self):
        alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_init = (alt_unity_object.x, alt_unity_object.y)

        self.altdriver.swipe(
            alt_unity_object.get_screen_position(),
            (alt_unity_object.x - 200, alt_unity_object.y - 200),
            duration=2
        )
        # time.sleep(2)

        alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_final = (alt_unity_object.x, alt_unity_object.y)

        assert position_init != position_final

    def test_resize_panel_with_multipoint_swipe(self):
        alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_init = (alt_unity_object.x, alt_unity_object.y)

        positions = [
            alt_unity_object.get_screen_position(),
            [alt_unity_object.x - 200, alt_unity_object.y - 200],
            [alt_unity_object.x - 300, alt_unity_object.y - 100],
            [alt_unity_object.x - 50, alt_unity_object.y - 100],
            [alt_unity_object.x - 100, alt_unity_object.y - 100]
        ]
        self.altdriver.multipoint_swipe(positions, duration=4)

        alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_final = (alt_unity_object.x, alt_unity_object.y)

        assert position_init != position_final

    def test_pointer_down_from_object(self):
        panel = self.altdriver.find_object(By.NAME, "Panel")
        color1 = panel.get_component_property(
            "AltUnityExampleScriptPanel",
            "normalColor",
            assembly="Assembly-CSharp"
        )
        panel.pointer_down()

        color2 = panel.get_component_property(
            "AltUnityExampleScriptPanel",
            "highlightColor",
            assembly="Assembly-CSharp"
        )

        assert color1 != color2

    def test_pointer_up_from_object(self):
        panel = self.altdriver.find_object(By.NAME, "Panel")
        color1 = panel.get_component_property(
            "AltUnityExampleScriptPanel",
            "normalColor",
            assembly="Assembly-CSharp"
        )
        panel.pointer_down()

        panel.pointer_up()
        color2 = panel.get_component_property(
            "AltUnityExampleScriptPanel",
            "highlightColor",
            assembly="Assembly-CSharp"
        )

        assert color1 == color2

    def test_new_touch_commands(self):
        draggable_area = self.altdriver.find_object(By.NAME, "Drag Zone")
        initial_position = draggable_area.get_screen_position()

        finger_id = self.altdriver.begin_touch(draggable_area.get_screen_position())
        self.altdriver.move_touch(finger_id, [draggable_area.x + 10, draggable_area.y + 10])
        self.altdriver.end_touch(finger_id)

        draggable_area = self.altdriver.find_object(By.NAME, "Drag Zone")
        assert initial_position != draggable_area
