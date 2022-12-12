import pytest

from .utils import Scenes
from alttester import By


class TestScene03:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.reset_input()
        self.altdriver.load_scene(Scenes.Scene03)

    def wait_for_swipe_to_finish(self):
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "icon")

    def get_sprite_name(self, source_image_name, image_source_drop_zone_name):
        image_source = self.altdriver.find_object(
            By.NAME, source_image_name).get_component_property(
            "UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, image_source_drop_zone_name).get_component_property(
            "UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI")
        return image_source, image_source_drop_zone

    def drop_image_with_multipoint_swipe(self, object_names, duration, wait):
        positions = []
        for name in object_names:
            alt_object = self.altdriver.find_object(By.NAME, name)
            positions.append(alt_object.get_screen_position())

        self.altdriver.multipoint_swipe(positions, duration, wait)

    def drop_image(self, drag_location_name, drop_location_name, duration, wait):
        drag_location = self.altdriver.find_object(By.NAME, drag_location_name)
        drop_location = self.altdriver.find_object(By.NAME, drop_location_name)

        self.altdriver.swipe(drag_location.get_screen_position(), drop_location.get_screen_position(), duration, wait)

    def test_pointer_enter_and_exit(self):
        alt_object = self.altdriver.find_object(By.NAME, "Drop Image")
        color1 = alt_object.get_component_property(
            "AltExampleScriptDropMe",
            "highlightColor",
            "Assembly-CSharp"
        )
        alt_object.pointer_enter()
        color2 = alt_object.get_component_property(
            "AltExampleScriptDropMe",
            "highlightColor",
            "Assembly-CSharp"
        )

        assert color1["r"] != color2["r"] or \
            color1["g"] != color2["g"] or \
            color1["b"] != color2["b"] or \
            color1["a"] != color2["a"]

        alt_object.pointer_exit()
        color3 = alt_object.get_component_property(
            "AltExampleScriptDropMe",
            "highlightColor",
            "Assembly-CSharp"
        )

        assert color3["r"] != color2["r"] or \
            color3["g"] != color2["g"] or \
            color3["b"] != color2["b"] or \
            color3["a"] != color2["a"]

        assert color3["r"] == color1["r"] and \
            color3["g"] == color1["g"] and \
            color3["b"] == color1["b"] and \
            color3["a"] == color1["a"]

    def test_multiple_swipes(self):
        self.drop_image("Drag Image2", "Drop Box2", 1, False)
        self.drop_image("Drag Image2", "Drop Box1", 1, False)
        self.drop_image("Drag Image1", "Drop Box1", 2, False)
        self.wait_for_swipe_to_finish()
        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image1", "Drop Image")
        assert image_source == image_source_drop_zone

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image2", "Drop")
        assert image_source == image_source_drop_zone

    def test_multiple_swipe_and_waits(self):
        self.drop_image("Drag Image2", "Drop Box2", 1, True)
        self.drop_image("Drag Image2", "Drop Box1", 1, True)
        self.drop_image("Drag Image1", "Drop Box1", 1, True)
        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image1", "Drop Image")
        assert image_source == image_source_drop_zone

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image2", "Drop")
        assert image_source == image_source_drop_zone

    def test_multiple_swipe_with_multipoint_swipe(self):

        self.drop_image_with_multipoint_swipe(["Drag Image1", "Drop Box1"],  1, False)
        self.drop_image_with_multipoint_swipe(["Drag Image2", "Drop Box1", "Drop Box2"],  1, False)

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image1", "Drop Image")
        assert image_source == image_source_drop_zone

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image2", "Drop")
        assert image_source == image_source_drop_zone

    def test_multiple_swipe_and_waits_with_multipoint_swipe(self):

        self.drop_image_with_multipoint_swipe(["Drag Image1", "Drop Box1"],  1, True)
        self.drop_image_with_multipoint_swipe(["Drag Image2", "Drop Box1", "Drop Box2"],  1, True)

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image1", "Drop Image")
        assert image_source == image_source_drop_zone

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image2", "Drop")
        assert image_source == image_source_drop_zone

    def test_begin_move_end_touch(self):
        alt_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")

        id = self.altdriver.begin_touch(alt_object1.get_screen_position())
        self.altdriver.move_touch(id, alt_object2.get_screen_position())
        self.altdriver.end_touch(id)

        imageSource = alt_object1.get_component_property("UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI")
        imageSourceDropZone = self.altdriver.find_object(By.NAME, "Drop Image").get_component_property(
            "UnityEngine.UI.Image", "sprite.name", "UnityEngine.UI")

        assert imageSource == imageSourceDropZone
