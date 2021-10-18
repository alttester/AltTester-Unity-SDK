import os
import time

import pytest

from altunityrunner import *
from altunityrunner.__version__ import VERSION
from altunityrunner.commands import GetServerVersion


@pytest.fixture(scope="session")
def altdriver():
    altdriver = AltUnityDriver(port=13010, enable_logging=True, timeout=None)
    yield altdriver
    altdriver.stop()


class TestPythonBindings:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver

    def test_tap_ui_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.find_object(By.NAME, "UIButton").tap()
        capsule_info = self.altdriver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text=UIButton clicked to jump capsule!]", timeout=1)

        assert capsule_info.get_text() == "UIButton clicked to jump capsule!"

    def test_tap_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        capsule_element.tap()
        capsule_info = self.altdriver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

        assert capsule_info.get_text() == "Capsule was clicked to jump!"

    def test_load_and_wait_for_scene(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.wait_for_current_scene_to_be(
            "Scene 1 AltUnityDriverTestScene", 1)
        self.altdriver.load_scene("Scene 2 Draggable Panel")
        self.altdriver.wait_for_current_scene_to_be(
            "Scene 2 Draggable Panel", 1)

        assert self.altdriver.get_current_scene() == "Scene 2 Draggable Panel"

    def test_resize_panel(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        alt_element = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_init = (alt_element.x, alt_element.y)

        self.altdriver.swipe_and_wait(
            alt_element.x, alt_element.y,
            alt_element.x - 200, alt_element.y - 200,
            duration=2
        )
        time.sleep(2)

        alt_element = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_final = (alt_element.x, alt_element.y)

        assert position_init != position_final

    def test_resize_panel_with_multipoinit_swipe(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        alt_element = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_init = (alt_element.x, alt_element.y)

        positions = [
            alt_element.get_screen_position(),
            [alt_element.x - 200, alt_element.y - 200],
            [alt_element.x - 300, alt_element.y - 100],
            [alt_element.x - 50, alt_element.y - 100],
            [alt_element.x - 100, alt_element.y - 100]
        ]
        self.altdriver.multipoint_swipe_and_wait(positions, duration=4)

        time.sleep(4)

        alt_element = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_final = (alt_element.x, alt_element.y)

        assert position_init != position_final

    def test_find_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        plane = self.altdriver.find_object(By.NAME, "Plane")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")

        assert plane.name == "Plane"
        assert capsule.name == "Capsule"

    def test_find_object_by_text(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        text = self.altdriver.find_object(By.NAME, "CapsuleInfo").get_text()
        element = self.altdriver.find_object(By.TEXT, text)

        assert element.get_text() == text

    def test_wait_for_object_with_text(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        text_to_wait_for = self.altdriver.find_object(
            By.NAME, "CapsuleInfo").get_text()

        capsule_info = self.altdriver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text={}]".format(text_to_wait_for), timeout=1)

        assert capsule_info.name == "CapsuleInfo"
        assert capsule_info.get_text() == text_to_wait_for

    def test_find_objects(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        planes = self.altdriver.find_objects(By.NAME, "Plane")

        assert len(planes) == 2
        assert len(self.altdriver.find_objects(By.NAME, "something that does not exist")) == 0

    def test_find_object_which_contains_2(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        plane = self.altdriver.find_object_which_contains(By.NAME, "Pla")

        assert "Pla" in plane.name

    def test_find_object_by_name_and_parent(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule_element = self.altdriver.find_object(
            By.NAME, "Canvas/CapsuleInfo")

        assert capsule_element.name == "CapsuleInfo"

    def test_find_objects_by_component(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        altobjects = self.altdriver.find_objects(By.COMPONENT, "UnityEngine.MeshFilter")

        assert len(altobjects) == 5

    def test_get_component_property(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        result = self.altdriver.find_object(By.NAME,
                                            "Capsule").get_component_property("AltUnityExampleScriptCapsule", "arrayOfInts")

        assert result, "[1,2,3]"

    def test_set_component_property(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.find_object(By.NAME, "Capsule").set_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", "[2,3,4]")
        result = self.altdriver.find_object(By.NAME,
                                            "Capsule").get_component_property("AltUnityExampleScriptCapsule", "arrayOfInts")

        assert result == "[2,3,4]"

    def test_call_component_method(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        result = self.altdriver.find_object(By.NAME, "Capsule").call_component_method(
            "AltUnityExampleScriptCapsule", "Jump", ["setFromMethod"])
        assert result is None

        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=setFromMethod]", timeout=1)
        assert self.altdriver.find_object(By.NAME, "CapsuleInfo").get_text() == "setFromMethod"

    def test_call_component_method_assembly_not_found(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(AssemblyNotFoundException) as execinfo:
            altelement = self.altdriver.find_object(By.NAME, "Capsule")
            altelement.call_component_method(
                "RandomComponent", "TestMethodWithManyParameters",
                parameters=[1, "stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[],
                assembly="RandomAssembly"
            )

        assert str(execinfo.value) == "Assembly not found"

    def test_call_component_method_incorrect_number_of_parameters(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        altelement = self.altdriver.find_object(By.NAME, "Capsule")

        with pytest.raises(MethodWithGivenParametersNotFoundException) as execinfo:
            altelement.call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                parameters=["stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[]
            )

        assert str(
            execinfo.value) == "No method found with 3 parameters matching signature: TestMethodWithManyParameters(System.String[])"

    def test_call_component_method_invalid_method_argument_types(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        altelement = self.altdriver.find_object(By.NAME, "Capsule")

        with pytest.raises(FailedToParseArgumentsException) as execinfo:
            altelement.call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                parameters=["stringnoint", "stringparams", 0.5, [1, 2, 3]],
                type_of_parameters=[]
            )

        assert str(execinfo.value) == "Could not parse parameter '\"stringnoint\"' to type System.Int32"

    def test_call_component_method_check_parameters(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        altelement = self.altdriver.find_object(By.NAME, "Capsule")
        result = altelement.call_component_method(
            "AltUnityExampleScriptCapsule", "TestCallComponentMethod",
            parameters=[1, "stringparam", 0.5, [1, 2, 3]],
            type_of_parameters=[]
        )
        assert result == "1,stringparam,0.5,[1,2,3]"

        result = altelement.call_component_method(
            "AltUnityExampleScriptCapsule", "TestCallComponentMethod",
            parameters=(1, "stringparam", 0.5, [1, 2, 3]),
            type_of_parameters=[]
        )
        assert result == "1,stringparam,0.5,[1,2,3]"

    def test_pointer_enter_and_exit(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")

        alt_element = self.altdriver.find_object(By.NAME, "Drop Image")
        color1 = alt_element.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor")
        alt_element.pointer_enter()
        color2 = alt_element.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor")

        assert color1 != color2

        alt_element.pointer_exit()
        color3 = alt_element.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor")

        assert color3 != color2
        assert color3 == color1

    def test_multiple_swipes(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")

        image1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image1.x, image1.y, box1.x, box1.y, 5)

        image2 = self.altdriver.find_object(By.NAME, "Drag Image2")
        box2 = self.altdriver.find_object(By.NAME, "Drop Box2")

        self.altdriver.swipe(image2.x, image2.y, box2.x, box2.y, 2)

        image3 = self.altdriver.find_object(By.NAME, "Drag Image3")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image3.x, image3.y, box1.x, box1.y, 3)

        time.sleep(6)

        image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source != image_source_drop_zone

        image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source != image_source_drop_zone

    def test_multiple_swipe_and_waits(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")

        image2 = self.altdriver.find_object(By.NAME, "Drag Image2")
        box2 = self.altdriver.find_object(By.NAME, "Drop Box2")

        self.altdriver.swipe_and_wait(image2.x, image2.y, box2.x, box2.y, 2)

        image3 = self.altdriver.find_object(By.NAME, "Drag Image3")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe_and_wait(image3.x, image3.y, box1.x, box1.y, 1)

        image1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe_and_wait(image1.x, image1.y, box1.x, box1.y, 3)

        image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(By.NAME,"Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source != image_source_drop_zone

        image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source != image_source_drop_zone

    def test_button_click_and_wait_with_swipe(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        button = self.altdriver.find_object(By.NAME, "UIButton")
        self.altdriver.hold_button_and_wait(button.x, button.y, 1)
        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")

        time.sleep(1.4)

        text = capsule_info.get_text()
        assert text == "UIButton clicked to jump capsule!"

    def test_button_click_with_swipe(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        button = self.altdriver.find_object(By.NAME, "UIButton")
        self.altdriver.hold_button(button.x, button.y, 1)

        time.sleep(2)

        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")
        text = capsule_info.get_text()
        assert text, "UIButton clicked to jump capsule!"

    def test_multiple_swipe_and_waits_with_multipoint_swipe(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")
        altElement1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        altElement2 = self.altdriver.find_object(By.NAME, "Drop Box1")

        multipointPositions = [altElement1.get_screen_position(), [altElement2.x, altElement2.y]]

        self.altdriver.multipoint_swipe_and_wait(multipointPositions, 2)
        time.sleep(2)

        altElement1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        altElement2 = self.altdriver.find_object(By.NAME, "Drop Box1")
        altElement3 = self.altdriver.find_object(By.NAME, "Drop Box2")

        positions = [
            [altElement1.x, altElement1.y],
            [altElement2.x, altElement2.y],
            [altElement3.x, altElement3.y]
        ]

        self.altdriver.multipoint_swipe_and_wait(positions, 3)
        imageSource = self.altdriver.find_object(By.NAME, "Drag Image1").get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_object(By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert imageSource != imageSourceDropZone

        imageSource = self.altdriver.find_object(By.NAME, "Drag Image2").get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_object(By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert imageSource != imageSourceDropZone

    def test_set_player_pref_keys_int(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.delete_player_pref()
        self.altdriver.set_player_pref_key("test", 1, PlayerPrefKeyType.Int)
        value = self.altdriver.get_player_pref_key("test", PlayerPrefKeyType.Int)

        assert value == 1

    def test_set_player_pref_keys_float(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.delete_player_pref()
        self.altdriver.set_player_pref_key("test", 1.3, PlayerPrefKeyType.Float)
        value = self.altdriver.get_player_pref_key("test", PlayerPrefKeyType.Float)

        assert float(value) == 1.3

    def test_set_player_pref_keys_string(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.delete_player_pref()
        self.altdriver.set_player_pref_key("test", "string value", PlayerPrefKeyType.String)
        value = self.altdriver.get_player_pref_key("test", PlayerPrefKeyType.String)

        assert value == "string value"

    def test_wait_for_non_existing_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object(By.NAME, "dlkasldkas", timeout=1)

        assert str(execinfo.value) == "Element dlkasldkas not found after 1 seconds"

    def test_wait_for_object_to_not_exist_fail(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object_to_not_be_present(By.NAME, "Capsule", timeout=1)

        assert str(execinfo.value) == "Element Capsule still found after 1 seconds"

    def test_wait_for_object_with_text_wrong_text(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=aaaaa]", timeout=1)

        assert str(execinfo.value) == "Element //CapsuleInfo[@text=aaaaa] not found after 1 seconds"

    def test_wait_for_current_scene_to_be_a_non_existing_scene(self):
        with pytest.raises(WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_current_scene_to_be("AltUnityDriverTestScenee", 1, 0.5)

        assert str(execinfo.value) == "Scene AltUnityDriverTestScenee not loaded after 1 seconds"

    def test_get_bool(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.find_object(By.NAME, "Capsule")
        text = alt_element.get_component_property("AltUnityExampleScriptCapsule", "TestBool")

        assert text == "true"

    def test_call_static_method(self):
        self.altdriver.call_static_method(
            "UnityEngine.PlayerPrefs", "SetInt", ["Test", "1"], assembly="UnityEngine.CoreModule")
        a = int(self.altdriver.call_static_method(
            "UnityEngine.PlayerPrefs", "GetInt", ["Test", "2"], assembly="UnityEngine.CoreModule"))

        assert a == 1

    def test_call_method_with_multiple_definitions(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.call_component_method(
            "AltUnityExampleScriptCapsule", "Test", ["2"], type_of_parameters=["System.Int32"])
        capsuleInfo = self.altdriver.find_object(By.NAME, "CapsuleInfo")

        assert capsuleInfo.get_text() == "6"

    def test_tap_on_screen_where_there_are_no_objects(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        counter_button = self.altdriver.find_object(By.NAME, "ButtonCounter")
        self.altdriver.tap({"x": 1, "y": counter_button.y + 100})

    def test_set_and_get_time_scale(self):
        self.altdriver.set_time_scale(0.1)
        time.sleep(1)
        time_scale = self.altdriver.get_time_scale()
        assert time_scale == 0.1
        self.altdriver.set_time_scale(1)

    def test_movement_cube(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        cube = self.altdriver.find_object(By.NAME, "Player1")
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldY)
        self.altdriver.scroll_mouse(30, 1)
        self.altdriver.press_key_with_keycode(AltUnityKeyCode.K, 1, 2)
        time.sleep(2)
        cube = self.altdriver.find_object(By.NAME, "Player1")
        self.altdriver.press_key_with_keycode_and_wait(AltUnityKeyCode.O, 1, 1)
        cubeFinalPosition = (cube.worldX, cube.worldY, cube.worldY)

        assert cubeInitialPostion != cubeFinalPosition

    def test_camera_movement(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        cube = self.altdriver.find_object(By.NAME, "Player1")
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldY)

        self.altdriver.press_key_with_keycode(AltUnityKeyCode.W, 1, 2)
        time.sleep(2)
        cube = self.altdriver.find_object(By.NAME, "Player1")
        cubeFinalPosition = (cube.worldX, cube.worldY, cube.worldY)

        assert cubeInitialPostion != cubeFinalPosition

    def test_creating_stars(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        stars = self.altdriver.find_objects_which_contain(By.NAME, "Star", By.NAME, "Player2")
        assert len(stars) == 1

        self.altdriver.find_objects_which_contain(By.NAME, "Player", By.NAME, "Player2")
        pressing_point_1 = self.altdriver.find_object(By.NAME, "PressingPoint1", By.NAME, "Player2")

        self.altdriver.move_mouse(pressing_point_1.x, pressing_point_1.y, duration=1)
        time.sleep(1.5)

        self.altdriver.press_key_with_keycode(AltUnityKeyCode.Mouse0, 1, 1)
        pressing_point_2 = self.altdriver.find_object(By.NAME, "PressingPoint2", By.NAME, "Player2")
        self.altdriver.move_mouse_and_wait(pressing_point_1.x, pressing_point_2.y, duration=1)
        self.altdriver.press_key_with_keycode(AltUnityKeyCode.Mouse0, power=1, duration=1)
        time.sleep(2)

        stars = self.altdriver.find_objects_which_contain(By.NAME, "Star")
        assert len(stars) == 3

    def test_find_object_by_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.find_object(By.TAG, "plane")
        assert alt_element.name == "Plane"

    def test_find_object_by_layer(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.find_object(By.LAYER, "Water")
        assert alt_element.name == "Capsule"

    def test_find_object_by_unity_component(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider")
        assert alt_element.name == "Capsule"

    def test_find_object_by_component(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.find_object(By.COMPONENT, "AltUnityExampleScriptCapsule")
        assert alt_element.name == "Capsule"

    def test_find_object_by_component_with_namespace(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.find_object(By.COMPONENT, "AltUnityTesterExamples.Scripts.AltUnityExampleScriptCapsule")
        assert alt_element.name == "Capsule"

    def test_find_child(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.find_object(By.PATH, "//UIButton/*")
        assert alt_element.name == "Text"

    def test_find_objects_by_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_elements = self.altdriver.find_objects(By.TAG, "plane")

        assert len(alt_elements) == 2
        for alt_element in alt_elements:
            assert alt_element.name == "Plane"

    def test_find_objects_by_layer(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        time.sleep(1)

        alt_elements = self.altdriver.find_objects(By.LAYER, "Default")
        assert len(alt_elements) == 12

    def test_find_objects_by_contains_name(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        time.sleep(1)

        alt_elements = self.altdriver.find_objects_which_contain(By.NAME, "Capsule")

        assert len(alt_elements) == 2
        for alt_element in alt_elements:
            assert "Capsule" in alt_element.name

    def test_power_joystick(self):
        button_names = ["Horizontal", "Vertical"]
        keys_to_press = [AltUnityKeyCode.D, AltUnityKeyCode.W]
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        axisName = self.altdriver.find_object(By.NAME, "AxisName")
        axisValue = self.altdriver.find_object(By.NAME, "AxisValue")

        for index, key in enumerate(keys_to_press):
            self.altdriver.press_key_with_keycode_and_wait(key, 0.5, 0.1)

            assert axisValue.get_text() == "0.5"
            assert axisName.get_text() == button_names[index]

    def test_call_method_with_assembly(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initialRotation = capsule.get_component_property(
            "UnityEngine.Transform", "rotation")
        capsule.call_component_method(
            "UnityEngine.Transform", "Rotate",
            parameters=["10", "10", "10"],
            type_of_parameters=["System.Single", "System.Single", "System.Single"],
            assembly="UnityEngine.CoreModule"
        )
        capsuleAfterRotation = self.altdriver.find_object(By.NAME, "Capsule")
        finalRotation = capsuleAfterRotation.get_component_property(
            "UnityEngine.Transform", "rotation")

        assert initialRotation != finalRotation

    def test_get_all_enabled_elements(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        time.sleep(1)

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

    def test_get_all_elements(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")
        time.sleep(1)

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

        assert len(names) == 28
        for name in expected_names:
            assert name in names

    def test_find_object_which_contains(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_element = self.altdriver.find_object_which_contains(By.NAME, "EventSy")
        assert alt_element.name == "EventSystem"

    def test_find_with_find_object_which_contains_not_existing_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(NotFoundException) as execinfo:
            self.altdriver.find_object_which_contains(By.NAME, "EventNonExisting")

        assert str(execinfo.value) == "Object //*[contains(@name,EventNonExisting)] not found"

    def test_screenshot(self):
        png_path = "testPython.png"
        self.altdriver.get_png_screenshot(png_path)
        assert os.path.exists(png_path)

    def test_wait_for_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_element = self.altdriver.wait_for_object(By.NAME, "Capsule")
        assert alt_element.name == "Capsule"

    def test_wait_for_object_to_not_be_present(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "Capsuule")

    def test_wait_for_object_which_contains(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_element = self.altdriver.wait_for_object_which_contains(By.NAME, "Main")
        assert alt_element.name == "Main Camera"

    def test_get_chinese_letters(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        text = self.altdriver.find_object(By.NAME, "ChineseLetters").get_text()
        assert text == "哦伊娜哦"

    def test_non_english_text(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        text = self.altdriver.find_object(By.NAME, "NonEnglishText").get_text()
        assert text == "BJÖRN'S PASS"

    def test_find_objects_fail(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        plane = self.altdriver.wait_for_object(By.NAME, "Plane")
        capsule = self.altdriver.wait_for_object(By.NAME, "Capsule")

        assert plane.name == "Plane"
        assert capsule.name == "Capsule"

    def test_double_tap(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        counter_button = self.altdriver.find_object(By.NAME, "ButtonCounter")
        counter_button_text = self.altdriver.find_object(By.NAME, "ButtonCounter/Text")
        counter_button.tap(count=2)

        time.sleep(0.5)

        assert counter_button_text.get_text() == "2"

    def test_set_text_normal_text(self):
        text_object = self.altdriver.find_object(By.NAME, "NonEnglishText")
        original_text = text_object.get_text()
        after_text = text_object.set_text("ModifiedText").get_text()

        assert original_text != after_text
        assert after_text == "ModifiedText"

    def test_press_next_scene(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        initial_scene = self.altdriver.get_current_scene()
        self.altdriver.find_object(By.NAME, "NextScene").tap()

        time.sleep(1)

        current_scene = self.altdriver.get_current_scene()
        assert initial_scene != current_scene

    def test_find_parent_using_path(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        parent = self.altdriver.find_object(By.PATH, "//CapsuleInfo/..")
        assert parent.name == "Canvas"

    def test_pointer_down_from_object(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        time.sleep(1)

        p_panel = self.altdriver.find_object(By.NAME, "Panel")
        color1 = p_panel.get_component_property("AltUnityExampleScriptPanel", "normalColor")
        p_panel.pointer_down()

        time.sleep(1)

        color2 = p_panel.get_component_property("AltUnityExampleScriptPanel", "highlightColor")
        assert color1 != color2

    def test_pointer_up_from_object(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        time.sleep(1)

        p_panel = self.altdriver.find_object(By.NAME, "Panel")
        color1 = p_panel.get_component_property("AltUnityExampleScriptPanel", "normalColor")
        p_panel.pointer_down()

        time.sleep(1)

        p_panel.pointer_up()
        color2 = p_panel.get_component_property("AltUnityExampleScriptPanel", "highlightColor")
        assert color1 == color2

    def test_get_all_components(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        components = self.altdriver.find_object(By.NAME, "Canvas").get_all_components()
        assert len(components) == 5
        assert components[0]["componentName"] == "UnityEngine.RectTransform"
        assert components[0]["assemblyName"] == "UnityEngine.CoreModule"

    def test_scroll(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeInitialPostion = [player2.worldX, player2.worldY, player2.worldY]
        self.altdriver.scroll_mouse(4, 2)

        time.sleep(2)

        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]

        assert cubeInitialPostion != cubeFinalPosition

    def test_scroll_and_wait(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeInitialPostion = [player2.worldX, player2.worldY, player2.worldY]
        self.altdriver.scroll_mouse_and_wait(4, 2)
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]

        assert cubeInitialPostion != cubeFinalPosition

    def test_acceleration(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt(1, 1, 1, 1)

        time.sleep(1)

        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]

        assert initial_position != final_position

    def test_acceleration_and_wait(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt_and_wait(1, 1, 1, 1)
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]

        assert initial_position != final_position

    def test_find_object_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.altdriver.find_object(By.PATH, "//Camera")
        alt_element = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        assert alt_element.name == "Capsule"

        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_element2 = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))

        assert alt_element.x != alt_element2.x
        assert alt_element.y != alt_element2.y

    def test_wait_for_object_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.altdriver.find_object(By.PATH, "//Camera")
        alt_element = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        assert alt_element.name == "Capsule"

        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_element2 = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))

        assert alt_element.x != alt_element2.x
        assert alt_element.y != alt_element2.y

    def test_find_objects_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.altdriver.find_object(By.PATH, "//Camera")
        alt_elements = self.altdriver.find_objects(By.NAME, "Plane", By.ID, str(camera.id))
        assert alt_elements[0].name == "Plane"

        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_elements2 = self.altdriver.find_objects(
            By.NAME, "Plane", By.ID, str(camera2.id))

        assert alt_elements[0].x != alt_elements2[0].x
        assert alt_elements[0].y != alt_elements2[0].y

    def test_wait_for_object_not_be_present_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        camera = self.altdriver.find_object(By.PATH, "//Main Camera")
        self.altdriver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs",
            By.ID, str(camera.id)
        )

        elements = self.altdriver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_with_text_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        name = "CapsuleInfo"
        text = self.altdriver.find_object(By.NAME, name).get_text()
        camera = self.altdriver.find_object(By.PATH, "//Main Camera")

        alt_element = self.altdriver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.ID, str(camera.id),
            timeout=1
        )

        assert alt_element is not None
        assert alt_element.get_text() == text

    def test_wait_for_object_which_contains_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        camera = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_element = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.ID, str(camera.id)
        )
        assert alt_element.name == "Canvas"

    def test_find_object_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_element = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider",  By.TAG, "MainCamera")
        assert alt_element.name == "Capsule"

        alt_element2 = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged")
        assert alt_element.x != alt_element2.x
        assert alt_element.y != alt_element2.y

    def test_wait_for_object_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_element = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera")
        assert alt_element.name == "Capsule"

        alt_element2 = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider",  By.TAG, "Untagged")
        assert alt_element.x != alt_element2.x
        assert alt_element.y != alt_element2.y

    def test_find_objects_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_element = self.altdriver.find_objects(By.NAME, "Plane", By.TAG, "MainCamera")
        assert alt_element[0].name == "Plane"

        alt_element2 = self.altdriver.find_objects(By.NAME, "Plane", By.TAG, "Untagged")
        assert alt_element[0].x != alt_element2[0].x
        assert alt_element[0].y != alt_element2[0].y

    def test_wait_for_object_not_be_present_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "ObjectDestroyedIn5Secs", By.TAG, "MainCamera")

        elements = self.altdriver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_with_text_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        name = "CapsuleInfo"
        text = self.altdriver.find_object(By.NAME, name).get_text()

        alt_element = self.altdriver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.TAG, "MainCamera",
            timeout=1
        )
        assert alt_element is not None
        assert alt_element.get_text() == text

    def test_wait_for_object_which_contains_with_tag(self):
        alt_element = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.TAG, "MainCamera"
        )
        assert alt_element.name == "Canvas"

    def test_find_object_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        button = self.altdriver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_element = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider",
            camera_value="Camera"
        )
        assert alt_element.name == "Capsule"

        alt_element2 = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider",
            camera_by=By.NAME, camera_value="Main Camera"
        )

        assert alt_element.x != alt_element2.x
        assert alt_element.y != alt_element2.y

    def test_wait_for_object_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        button = self.altdriver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_element = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",
            camera_value="Camera"
        )
        assert alt_element.name == "Capsule"

        alt_element2 = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",
            camera_by=By.NAME, camera_value="Main Camera"
        )
        assert alt_element.x != alt_element2.x
        assert alt_element.y != alt_element2.y

    def test_find_objects_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        button = self.altdriver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_element = self.altdriver.find_objects(By.NAME, "Plane", By.NAME, "Camera")
        assert alt_element[0].name == "Plane"

        alt_element2 = self.altdriver.find_objects(By.NAME, "Plane", By.NAME, "Main Camera")
        assert alt_element[0].x != alt_element2[0].x
        assert alt_element[0].y != alt_element2[0].y

    def test_wait_for_object_not_be_present_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs",
            By.NAME, "Main Camera"
        )

        elements = self.altdriver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_by_camera_2(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        name = "CapsuleInfo"
        text = self.altdriver.find_object(By.NAME, name).get_text()

        alt_element = self.altdriver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.NAME, "Main Camera",
            timeout=1
        )

        assert alt_element is not None
        assert alt_element.get_text() == text

    def test_wait_for_object_which_contains_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_element = self.altdriver.wait_for_object_which_contains(By.NAME, "Canva", By.NAME, "Main Camera")
        assert alt_element.name == "Canvas"

    def test_load_additive_scenes(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)

        initial_number_of_elements = self.altdriver.get_all_elements()
        self.altdriver.load_scene("Scene 2 Draggable Panel", load_single=False)
        final_number_of_elements = self.altdriver.get_all_elements()

        assert len(final_number_of_elements) > len(initial_number_of_elements)

        scenes = self.altdriver.get_all_loaded_scenes()
        assert len(scenes) == 2

    def test_get_component_property_complex_class(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        component_name = "AltUnityExampleScriptCapsule"
        property_name = "AltUnitySampleClass.testInt"
        alt_element = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_element is not None

        property_value = alt_element.get_component_property(component_name, property_name, max_depth=1)
        assert property_value == "1"

    def test_get_component_property_complex_class2(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", True)

        component_name = "AltUnityExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_element = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_element is not None

        property_value = alt_element.get_component_property(component_name, property_name, max_depth=1)
        assert property_value == "test2"

    def test_set_component_property_complex_class(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", True)

        component_name = "AltUnityExampleScriptCapsule"
        property_name = "AltUnitySampleClass.testInt"
        alt_element = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_element is not None

        alt_element.set_component_property(component_name, property_name, "2")
        property_value = alt_element.get_component_property(component_name, property_name, max_depth=1)
        assert property_value == "2"

    def test_set_component_property_complex_class2(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", True)
        component_name = "AltUnityExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_element = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_element is not None

        alt_element.set_component_property(component_name, property_name, "test3")
        propertyValue = alt_element.get_component_property(component_name, property_name, max_depth=1)
        assert propertyValue == "test3"

    def test_get_version(self):
        serverVersion = GetServerVersion.run(self.altdriver._connection)
        assert VERSION.startswith(serverVersion)

    def test_get_parent(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        element = self.altdriver.find_object(By.NAME, "Canvas/CapsuleInfo")
        element_parent = element.get_parent()
        assert element_parent.name == "Canvas"

    def test_unload_scene(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)
        self.altdriver.load_scene("Scene 2 Draggable Panel", load_single=False)

        assert len(self.altdriver.get_all_loaded_scenes()) == 2

        self.altdriver.unload_scene("Scene 2 Draggable Panel")
        assert len(self.altdriver.get_all_loaded_scenes()) == 1
        assert self.altdriver.get_all_loaded_scenes()[0] == "Scene 1 AltUnityDriverTestScene"

    def test_unload_only_scene(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", True)

        with pytest.raises(CouldNotPerformOperationException):
            self.altdriver.unload_scene("Scene 1 AltUnityDriverTestScene")

    def test_set_server_logging(self):
        rule = self.altdriver.call_static_method(
            "Altom.Server.Logging.ServerLogManager",
            "Instance.Configuration.FindRuleByName",
            ["AltUnityServerFileRule"],
            assembly="Assembly-CSharp"
        )

        # Default logging level in AltUnity Server is Debug level
        assert len(rule["Levels"]) == 5

        self.altdriver.set_server_logging(AltUnityLogger.File, AltUnityLogLevel.Off)
        rule = self.altdriver.call_static_method(
            "Altom.Server.Logging.ServerLogManager",
            "Instance.Configuration.FindRuleByName",
            ["AltUnityServerFileRule"],
            assembly="Assembly-CSharp")

        assert len(rule["Levels"]) == 0

        # Reset logging level
        self.altdriver.set_server_logging(AltUnityLogger.File, AltUnityLogLevel.Debug)

    @pytest.mark.parametrize(
        "path", ["//[1]", "CapsuleInfo[@tag=UI]", "//CapsuleInfo[@tag=UI/Text", "//CapsuleInfo[0/Text"]
    )
    def test_invalid_paths(self, path):
        with pytest.raises(AltUnityInvalidPathException):
            self.altdriver.find_object(By.PATH, path)

    def test_tapcoordinates(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.tap(capsule_element.get_screen_position())
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_clickcoordinates(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.click(capsule_element.get_screen_position())
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_tapelement(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        capsule_element.tap(1)
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_clickelement(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        capsule_element.click()
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_new_touch_commands(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        draggable_area = self.altdriver.find_object(By.NAME, "Drag Zone")
        initial_position = draggable_area.get_screen_position()

        finger_id = self.altdriver.begin_touch(draggable_area.get_screen_position())
        self.altdriver.move_touch(finger_id, [draggable_area.x + 10, draggable_area.y + 10])
        self.altdriver.end_touch(finger_id)

        draggable_area = self.altdriver.find_object(By.NAME, "Drag Zone")
        assert initial_position != draggable_area

    def test_key_down_and_key_up(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        self.altdriver.key_down(AltUnityKeyCode.A)
        time.sleep(5)
        last_key_down = self.altdriver.find_object(By.NAME, "LastKeyDownValue")
        last_key_press = self.altdriver.find_object(By.NAME, "LastKeyPressedValue")

        assert last_key_down.get_text() == "A"
        assert last_key_press.get_text() == "A"

        self.altdriver.key_up(AltUnityKeyCode.A)
        time.sleep(5)
        last_key_up = self.altdriver.find_object(By.NAME, "LastKeyUpValue")

        assert last_key_up.get_text() == "A"

    def test_key_down_and_key_up_mouse0(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.move_mouse(capsule_element.x, capsule_element.y, 1)
        time.sleep(1.5)

        self.altdriver.key_down(AltUnityKeyCode.Mouse0)
        self.altdriver.key_up(AltUnityKeyCode.Mouse0)
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_camera_not_found_exception(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(CameraNotFoundException):
            self.altdriver.find_object(By.NAME, "Capsule", By.NAME, "Camera")

    def test_input_field_events(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        input_field = self.altdriver.find_object(By.NAME, "InputField").set_text("example", True)

        assert input_field.get_text() == "example"
        assert input_field.get_component_property("AltUnityInputFieldRaisedEvents", "onValueChangedInvoked")
        assert input_field.get_component_property("AltUnityInputFieldRaisedEvents", "onSubmitInvoked")

    def test_get_static_property(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.call_static_method(
            "UnityEngine.Screen", "SetResolution",
            parameters=["1920", "1080", "True"],
            type_of_parameters=["System.Int32", "System.Int32", "System.Boolean"],
            assembly="UnityEngine.CoreModule"
        )
        width = self.altdriver.get_static_property(
            "UnityEngine.Screen", "currentResolution.width",
            assembly="UnityEngine.CoreModule"
        )

        assert int(width) == 1920

    def test_get_static_property_instance_null(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        screen_width = self.altdriver.call_static_method(
            "UnityEngine.Screen", "get_width",
            assembly="UnityEngine.CoreModule"
        )
        width = self.altdriver.get_static_property(
            "UnityEngine.Screen", "width",
            assembly="UnityEngine.CoreModule"
        )

        assert int(width) == screen_width
