from turtle import position
import pytest

from .utils import Scenes
from altunityrunner import By


class TestScene03:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene03)

    def waitForSwipeToFinish(self):
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "icon")
        
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
        
    def dropImage(self, dragLocationName, dropLocationName, duration, wait):
        dragLocation = self.altdriver.find_object(By.NAME, dragLocationName)
        dropLocation = self.altdriver.find_object(By.NAME, dropLocationName)

        self.altdriver.swipe(dragLocation.get_screen_position(), dropLocation.get_screen_position(), duration= duration, wait= wait)
        
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
        self.dropImage("Drag Image2", "Drop Box2", 1, False)
        self.dropImage("Drag Image3", "Drop Box1", 1, False)
        self.dropImage("Drag Image1", "Drop Box1", 1, False)
        self.waitForSwipeToFinish()
        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image1", "Drop Image")
        assert imageSource == imageSourceDropZone

        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image2", "Drop")
        assert imageSource == imageSourceDropZone

    def test_multiple_swipe_and_waits(self):
        self.dropImage("Drag Image2", "Drop Box2", 1, True)
        self.dropImage("Drag Image3", "Drop Box1", 1, True)
        self.dropImage("Drag Image1", "Drop Box1", 1, True)
        self.waitForSwipeToFinish()
        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image1", "Drop Image")
        assert imageSource == imageSourceDropZone

        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image2", "Drop")
        assert imageSource == imageSourceDropZone
    
    def test_multiple_swipe_with_multipoint_swipe(self):

        self.dropImageWithMultipointSwipe(["Drag Image1", "Drop Box1"], 1, False)
        self.dropImageWithMultipointSwipe(["Drag Image2", "Drop Box1", "Drop Box2"], 1, False)

        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image1", "Drop Image")
        assert imageSource == imageSourceDropZone
        
        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image2", "Drop")
        assert imageSource == imageSourceDropZone
   
    def test_multiple_swipe_and_waits_with_multipoint_swipe(self):

        self.dropImageWithMultipointSwipe(["Drag Image1", "Drop Box1"], 1, True)
        self.dropImageWithMultipointSwipe(["Drag Image2", "Drop Box1", "Drop Box2"], 1, True)

        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image1", "Drop Image")
        assert imageSource == imageSourceDropZone
        
        imageSource, imageSourceDropZone=self.getSpriteName("Drag Image2", "Drop")
        assert imageSource == imageSourceDropZone

    def test_begin_move_end_touch(self):
        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")

        id = self.altdriver.begin_touch(alt_unity_object1.get_screen_position())
        self.altdriver.move_touch(id, alt_unity_object2.get_screen_position())
        self.altdriver.end_touch(id)

        imageSource = alt_unity_object1.get_component_property("UnityEngine.UI.Image", "sprite.name")
        imageSourceDropZone = self.altdriver.find_object(
            By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite.name")

        assert imageSource == imageSourceDropZone