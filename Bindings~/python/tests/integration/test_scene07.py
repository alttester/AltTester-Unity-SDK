import pytest

from .utils import Scenes
from alttester import By


class TestScene07A:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.reset_input()
        self.altdriver.load_scene(Scenes.Scene07A)

    def test_tap_element_NIS(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.tap()

        component_name = "AltExampleNewInputSystem"
        property_name = "jumpCounter"

        property_value = capsule.get_component_property(
            component_name, property_name, "Assembly-CSharp",
            max_depth=1
        )

        assert property_value == 1

    def test_tap_coordinates_NIS(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.tap(capsule.get_screen_position())

        action_info = self.altdriver.wait_for_object(
            By.PATH, "//ActionText[@text=Capsule was tapped!]", timeout=1)

        assert action_info.get_text() == "Capsule was tapped!"

    def test_click_element_NIS(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.click()

        component_name = "AltExampleNewInputSystem"
        property_name = "jumpCounter"
        property_value = capsule.get_component_property(
            component_name, property_name, "Assembly-CSharp",
            max_depth=1
        )

        assert property_value == 1

    def test_click_coordinates_NIS(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.click(capsule.get_screen_position())
        action_info = self.altdriver.wait_for_object(
            By.PATH, "//ActionText[@text=Capsule was clicked!]",
            timeout=1
        )

        assert action_info.get_text() == "Capsule was clicked!"

    def test_tilt(self):
        cube = self.altdriver.find_object(By.NAME, "Cube (1)")
        initial_position = cube.get_world_position()
        self.altdriver.tilt([1000, 10, 10], duration=1)
        assert initial_position != self.altdriver.find_object(By.NAME, "Cube (1)").get_world_position()

        is_moved = cube.get_component_property("AltCubeNIS", "isMoved", "Assembly-CSharp")
        assert is_moved


class TestScene07B:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene07B)

    def get_sprite_name(self, source_image_name, image_source_drop_zone_name):
        image_source = self.altdriver.find_object(By.NAME, source_image_name).get_component_property(
            "UnityEngine.UI.Image", "sprite.name", assembly="UnityEngine.UI")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, image_source_drop_zone_name).get_component_property(
            "UnityEngine.UI.Image", "sprite.name", assembly="UnityEngine.UI")
        return image_source, image_source_drop_zone

    def drop_image_with_multipoint_swipe(self, object_names, duration, wait):
        positions = []
        for name in object_names:
            alt_object = self.altdriver.find_object(By.NAME, name)
            positions.append(alt_object.get_screen_position())

        self.altdriver.multipoint_swipe(positions, duration=duration, wait=wait)

    def test_multipoint_swipe_NIS(self):
        self.drop_image_with_multipoint_swipe(["Drag Image1", "Drop Box1"], 1, False)
        self.drop_image_with_multipoint_swipe(["Drag Image2", "Drop Box1", "Drop Box2"], 1, False)

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image1", "Drop Image")
        assert image_source == image_source_drop_zone

        image_source, image_source_drop_zone = self.get_sprite_name("Drag Image2", "Drop")
        assert image_source == image_source_drop_zone
