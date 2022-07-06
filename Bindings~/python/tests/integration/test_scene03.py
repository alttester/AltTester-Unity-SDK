import pytest

from .utils import Scenes
from altunityrunner import By


class TestScene03:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene03)

    def test_pointer_enter_and_exit(self):
        alt_object = self.altdriver.find_object(By.NAME, "Drop Image")
        color1 = alt_object.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor", assembly="Assembly-CSharp")
        alt_object.pointer_enter()
        color2 = alt_object.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor", assembly="Assembly-CSharp")

        assert color1["r"] != color2["r"] or \
            color1["g"] != color2["g"] or \
            color1["b"] != color2["b"] or \
            color1["a"] != color2["a"]

        alt_object.pointer_exit()
        color3 = alt_object.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor", assembly="Assembly-CSharp")

        assert color3["r"] != color2["r"] or \
            color3["g"] != color2["g"] or \
            color3["b"] != color2["b"] or \
            color3["a"] != color2["a"]

        assert color3["r"] == color1["r"] and \
            color3["g"] == color1["g"] and \
            color3["b"] == color1["b"] and \
            color3["a"] == color1["a"]

    def test_multiple_swipes(self):
        image1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image1.get_screen_position(), box1.get_screen_position(), duration=0.5, wait=False)

        image2 = self.altdriver.find_object(By.NAME, "Drag Image2")
        box2 = self.altdriver.find_object(By.NAME, "Drop Box2")

        self.altdriver.swipe(image2.get_screen_position(), box2.get_screen_position(), duration=0.5, wait=False)

        image3 = self.altdriver.find_object(By.NAME, "Drag Image3")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image3.get_screen_position(), box1.get_screen_position(), duration=0.5, wait=False)

        image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

        image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

    def test_multiple_swipe_and_waits(self):
        image2 = self.altdriver.find_object(By.NAME, "Drag Image2")
        box2 = self.altdriver.find_object(By.NAME, "Drop Box2")

        self.altdriver.swipe(image2.get_screen_position(), box2.get_screen_position(), duration=0.5)

        image3 = self.altdriver.find_object(By.NAME, "Drag Image3")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image3.get_screen_position(), box1.get_screen_position(), duration=0.5)

        image1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image1.get_screen_position(), box1.get_screen_position(), duration=0.5)

        image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

        image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

    def test_multiple_swipe_and_waits_with_multipoint_swipe(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")
        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")

        multipointPositions = [alt_unity_object1.get_screen_position(), [alt_unity_object2.x, alt_unity_object2.y]]

        self.altdriver.multipoint_swipe(multipointPositions, duration=0.5)

        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")
        alt_unity_object3 = self.altdriver.find_object(By.NAME, "Drop Box2")

        positions = [
            [alt_unity_object1.x, alt_unity_object1.y],
            [alt_unity_object2.x, alt_unity_object2.y],
            [alt_unity_object3.x, alt_unity_object3.y]
        ]

        self.altdriver.multipoint_swipe(positions, duration=0.5)
        imageSource = self.altdriver.find_object(
            By.NAME, "Drag Image1").get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_object(
            By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert imageSource["name"] != imageSourceDropZone["name"]

        imageSource = self.altdriver.find_object(
            By.NAME, "Drag Image2").get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_object(
            By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert imageSource["name"] != imageSourceDropZone["name"]
