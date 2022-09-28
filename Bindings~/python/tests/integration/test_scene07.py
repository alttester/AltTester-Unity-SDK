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

    def getSpriteName(self, imageSource, imageSourceDropZone, sourceImageName, imageSourceDropZoneName):
        imageSource = self.altdriver.find_object(By.NAME, sourceImageName).get_component_property(
            "UnityEngine.UI.Image", "sprite.name", assembly="UnityEngine.UI")
        imageSourceDropZone = self.altdriver.find_object(By.NAME, imageSourceDropZoneName).get_component_property(
            "UnityEngine.UI.Image", "sprite.name", assembly="UnityEngine.UI")
        return imageSource, imageSourceDropZone

    def dropImageWithMultipointSwipe(self, objectNames, duration, wait):
        positions=[]
        for i in objectNames.size():
            obj = self.altdriver.find_object(By.NAME, objectNames[i])
            positions[i] =obj .getScreenPosition()
        
        self.altdriver.multipoint_swipe(positions, duration=duration, wait=wait)

    def test_multipoint_swipe_NIS(self):
        self.dropImageWithMultipointSwipe(["Drag Image1", "Drop Box1"], 1, False)
        self.dropImageWithMultipointSwipe(["Drag Image2", "Drop Box1", "Drop Box2"], 1, False)

        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image1", "Drop Image")
        assert imageSource == imageSourceDropZone
        
        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image2", "Drop")
        assert imageSource == imageSourceDropZone
    