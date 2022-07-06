import pytest

from .utils import Scenes
from altunityrunner import By


class TestScene07A:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene07A)

    def test_tap_element_NIS(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.tap()

        component_name = "AltUnityExampleNewInputSystem"
        property_name = "jumpCounter"

        property_value = capsule.get_component_property(
            component_name, property_name,
            max_depth=1,
            assembly="Assembly-CSharp"
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

        component_name = "AltUnityExampleNewInputSystem"
        property_name = "jumpCounter"
        property_value = capsule.get_component_property(
            component_name, property_name,
            max_depth=1,
            assembly="Assembly-CSharp"
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
        self.altdriver.tilt([1000, 10, 10], duration=0.1)
        assert initial_position != self.altdriver.find_object(By.NAME, "Cube (1)").get_world_position()

        is_moved = cube.get_component_property("AltUnityCubeNIS", "isMoved", "Assembly-CSharp")
        assert is_moved


class TestScene07B:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene07B)

    def test_multipoint_swipe_NIS(self):
        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")

        multipoint_positions = [alt_unity_object1.get_screen_position(), [alt_unity_object2.x, alt_unity_object2.y]]

        self.altdriver.multipoint_swipe(multipoint_positions, duration=0.5)
        # time.sleep(2)

        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")
        alt_unity_object3 = self.altdriver.find_object(By.NAME, "Drop Box2")

        positions = [
            [alt_unity_object1.x, alt_unity_object1.y],
            [alt_unity_object2.x, alt_unity_object2.y],
            [alt_unity_object3.x, alt_unity_object3.y]
        ]

        self.altdriver.multipoint_swipe(positions, duration=0.5)
        image_source = self.altdriver.find_object(
            By.NAME, "Drag Image1").get_component_property("UnityEngine.UI.Image", "sprite")
        drop_zone_image_source = self.altdriver.find_object(
            By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != drop_zone_image_source["name"]

        image_source = self.altdriver.find_object(
            By.NAME, "Drag Image2").get_component_property("UnityEngine.UI.Image", "sprite")
        drop_zone_image_source = self.altdriver.find_object(
            By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != drop_zone_image_source["name"]
