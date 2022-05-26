import os
import time

import pytest

import altunityrunner.exceptions as exceptions
from altunityrunner.__version__ import VERSION
from altunityrunner import AltUnityDriver, By
from altunityrunner.logging import AltUnityLogLevel, AltUnityLogger
from altunityrunner.alt_unity_key_code import AltUnityKeyCode
from altunityrunner.player_pref_key_type import PlayerPrefKeyType
from altunityrunner.commands import GetServerVersion
from altunityrunner.commands.Notifications.notification_type import NotificationType
from tests.integration.notification_callbacks_for_testing import TestNotificationCallback


def get_altunitytester_port():
    port = os.environ.get("ALTUNITYDRIVER_PORT", 13000)
    return int(port)


def get_altunitytester_host():
    return os.environ.get("ALTUNITYDRIVER_HOST", "127.0.0.1")


@pytest.fixture(scope="session")
def altdriver():
    altdriver = AltUnityDriver(
        host=get_altunitytester_host(),
        port=get_altunitytester_port(),
        enable_logging=True,
        timeout=None
    )

    yield altdriver

    altdriver.stop()


class TestPythonBindings:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver: AltUnityDriver):
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

        alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_init = (alt_unity_object.x, alt_unity_object.y)

        self.altdriver.swipe(
            alt_unity_object.get_screen_position(),
            (alt_unity_object.x - 200, alt_unity_object.y - 200),
            duration=2
        )
        time.sleep(2)

        alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
        position_final = (alt_unity_object.x, alt_unity_object.y)

        assert position_init != position_final

    def test_resize_panel_with_multipoint_swipe(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

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
        altobject = self.altdriver.find_object(By.NAME, "Capsule")
        result = altobject.get_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", assembly="Assembly-CSharp")

        assert result, [1, 2, 3]

    def test_set_component_property(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.find_object(By.NAME, "Capsule").set_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", [2, 3, 4], assembly="Assembly-CSharp")
        altobject = self.altdriver.find_object(By.NAME, "Capsule")
        result = altobject.get_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", assembly="Assembly-CSharp")

        assert result == [2, 3, 4]

    def test_call_component_method(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        result = self.altdriver.find_object(By.NAME, "Capsule").call_component_method(
            "AltUnityExampleScriptCapsule", "Jump", ["setFromMethod"], assembly="Assembly-CSharp")
        assert result is None

        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=setFromMethod]", timeout=1)
        assert self.altdriver.find_object(By.NAME, "CapsuleInfo").get_text() == "setFromMethod"

    def test_call_component_method_assembly_not_found(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(exceptions.AssemblyNotFoundException) as execinfo:
            alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
            alt_unity_object.call_component_method(
                "RandomComponent", "TestMethodWithManyParameters",
                parameters=[1, "stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[],
                assembly="RandomAssembly"
            )

        assert str(execinfo.value) == "Assembly not found"

    def test_call_component_method_incorrect_number_of_parameters(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.MethodWithGivenParametersNotFoundException) as execinfo:
            alt_unity_object.call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                parameters=["stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[], assembly="Assembly-CSharp"
            )

        assert str(execinfo.value) == \
            "No method found with 3 parameters matching signature: TestMethodWithManyParameters(System.String[])"

    def test_call_component_method_invalid_method_argument_types(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.FailedToParseArgumentsException) as execinfo:
            alt_unity_object.call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                parameters=["stringnoint", "stringparams", 0.5, [1, 2, 3]],
                type_of_parameters=[], assembly="Assembly-CSharp"
            )

        assert str(execinfo.value) == "Could not parse parameter '\"stringnoint\"' to type System.Int32"

    def test_call_component_method_check_parameters(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        result = alt_unity_object.call_component_method(
            "AltUnityExampleScriptCapsule", "TestCallComponentMethod",
            parameters=[1, "stringparam", 0.5, [1, 2, 3]],
            type_of_parameters=[], assembly="Assembly-CSharp"
        )
        assert result == "1,stringparam,0.5,[1,2,3]"

        result = alt_unity_object.call_component_method(
            "AltUnityExampleScriptCapsule", "TestCallComponentMethod",
            parameters=(1, "stringparam", 0.5, [1, 2, 3]),
            type_of_parameters=[], assembly="Assembly-CSharp"
        )
        assert result == "1,stringparam,0.5,[1,2,3]"

    def test_pointer_enter_and_exit(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")

        alt_unity_object = self.altdriver.find_object(By.NAME, "Drop Image")
        color1 = alt_unity_object.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor", assembly="Assembly-CSharp")
        alt_unity_object.pointer_enter()
        color2 = alt_unity_object.get_component_property(
            "AltUnityExampleScriptDropMe", "highlightColor", assembly="Assembly-CSharp")

        assert color1["r"] != color2["r"] or \
            color1["g"] != color2["g"] or \
            color1["b"] != color2["b"] or \
            color1["a"] != color2["a"]

        alt_unity_object.pointer_exit()
        color3 = alt_unity_object.get_component_property(
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
        self.altdriver.load_scene("Scene 3 Drag And Drop")

        image1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image1.get_screen_position(), box1.get_screen_position(), 5, False)

        image2 = self.altdriver.find_object(By.NAME, "Drag Image2")
        box2 = self.altdriver.find_object(By.NAME, "Drop Box2")

        self.altdriver.swipe(image2.get_screen_position(), box2.get_screen_position(), 2, False)

        image3 = self.altdriver.find_object(By.NAME, "Drag Image3")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image3.get_screen_position(), box1.get_screen_position(), 3, False)

        time.sleep(6)

        image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

        image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

    def test_multiple_swipe_and_waits(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")

        image2 = self.altdriver.find_object(By.NAME, "Drag Image2")
        box2 = self.altdriver.find_object(By.NAME, "Drop Box2")

        self.altdriver.swipe(image2.get_screen_position(), box2.get_screen_position(), 2)

        image3 = self.altdriver.find_object(By.NAME, "Drag Image3")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image3.get_screen_position(), box1.get_screen_position(), 1)

        image1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

        self.altdriver.swipe(image1.get_screen_position(), box1.get_screen_position(), 3)

        image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

        image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite")
        image_source_drop_zone = self.altdriver.find_object(
            By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite")
        assert image_source["name"] != image_source_drop_zone["name"]

    def test_button_click_and_wait_with_swipe(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        button = self.altdriver.find_object(By.NAME, "UIButton")
        self.altdriver.hold_button(button.get_screen_position(), 1)

        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")
        text = capsule_info.get_text()
        assert text == "UIButton clicked to jump capsule!"

    def test_button_click_with_swipe(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        button = self.altdriver.find_object(By.NAME, "UIButton")
        self.altdriver.hold_button(button.get_screen_position(), 1, False)

        time.sleep(2)

        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")
        text = capsule_info.get_text()
        assert text, "UIButton clicked to jump capsule!"

    def test_multiple_swipe_and_waits_with_multipoint_swipe(self):
        self.altdriver.load_scene("Scene 3 Drag And Drop")
        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")

        multipointPositions = [alt_unity_object1.get_screen_position(), [alt_unity_object2.x, alt_unity_object2.y]]

        self.altdriver.multipoint_swipe(multipointPositions, 2)
        time.sleep(2)

        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")
        alt_unity_object3 = self.altdriver.find_object(By.NAME, "Drop Box2")

        positions = [
            [alt_unity_object1.x, alt_unity_object1.y],
            [alt_unity_object2.x, alt_unity_object2.y],
            [alt_unity_object3.x, alt_unity_object3.y]
        ]

        self.altdriver.multipoint_swipe(positions, 3)
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

        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object(By.NAME, "dlkasldkas", timeout=1)

        assert str(execinfo.value) == "Element dlkasldkas not found after 1 seconds"

    def test_wait_for_object_to_not_exist_fail(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object_to_not_be_present(By.NAME, "Capsule", timeout=1)

        assert str(execinfo.value) == "Element Capsule still found after 1 seconds"

    def test_wait_for_object_with_text_wrong_text(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=aaaaa]", timeout=1)

        assert str(execinfo.value) == "Element //CapsuleInfo[@text=aaaaa] not found after 1 seconds"

    def test_wait_for_current_scene_to_be_a_non_existing_scene(self):
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_current_scene_to_be("AltUnityDriverTestScenee", 1, 0.5)

        assert str(execinfo.value) == "Scene AltUnityDriverTestScenee not loaded after 1 seconds"

    def test_get_bool(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        text = alt_unity_object.get_component_property(
            "AltUnityExampleScriptCapsule", "TestBool", assembly="Assembly-CSharp")

        assert text is True

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
            "AltUnityExampleScriptCapsule", "Test", ["2"], type_of_parameters=["System.Int32"],
            assembly="Assembly-CSharp")
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
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldZ)
        self.altdriver.scroll(30, 1, False)
        self.altdriver.press_key(AltUnityKeyCode.K, 1, 2, False)
        time.sleep(2)
        self.altdriver.press_key(AltUnityKeyCode.O, 1, 1)
        cube = self.altdriver.find_object(By.NAME, "Player1")
        cubeFinalPosition = (cube.worldX, cube.worldY, cube.worldZ)

        assert cubeInitialPostion != cubeFinalPosition

    def test_camera_movement(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        cube = self.altdriver.find_object(By.NAME, "Player1")
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldY)

        self.altdriver.press_key(AltUnityKeyCode.W, 1, 2, False)
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

        self.altdriver.move_mouse(pressing_point_1.get_screen_position(), duration=1, wait=False)
        time.sleep(1.5)

        self.altdriver.press_key(AltUnityKeyCode.Mouse0, 1, 1, False)
        pressing_point_2 = self.altdriver.find_object(By.NAME, "PressingPoint2", By.NAME, "Player2")
        self.altdriver.move_mouse(pressing_point_2.get_screen_position(), duration=1)
        self.altdriver.press_key(AltUnityKeyCode.Mouse0, power=1, duration=1, wait=False)
        time.sleep(2)

        stars = self.altdriver.find_objects_which_contain(By.NAME, "Star")
        assert len(stars) == 3

    def test_find_object_by_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.TAG, "plane")
        assert alt_unity_object.name == "Plane"

    def test_find_object_by_layer(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.LAYER, "Water")
        assert alt_unity_object.name == "Capsule"

    def test_find_object_by_unity_component(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider")
        assert alt_unity_object.name == "Capsule"

    def test_find_object_by_component(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.COMPONENT, "AltUnityExampleScriptCapsule")
        assert alt_unity_object.name == "Capsule"

    def test_find_object_by_component_with_namespace(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(
            By.COMPONENT, "AltUnityTesterExamples.Scripts.AltUnityExampleScriptCapsule")
        assert alt_unity_object.name == "Capsule"

    def test_find_child(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.PATH, "//UIButton/*")
        assert alt_unity_object.name == "Text"

    def test_find_objects_by_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_objects = self.altdriver.find_objects(By.TAG, "plane")

        assert len(alt_unity_objects) == 2
        for alt_unity_object in alt_unity_objects:
            assert alt_unity_object.name == "Plane"

    def test_find_objects_by_layer(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        time.sleep(1)

        alt_unity_objects = self.altdriver.find_objects(By.LAYER, "Default")
        assert len(alt_unity_objects) == 12

    def test_find_objects_by_contains_name(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        time.sleep(1)

        alt_unity_objects = self.altdriver.find_objects_which_contain(By.NAME, "Capsule")

        assert len(alt_unity_objects) == 2
        for alt_unity_object in alt_unity_objects:
            assert "Capsule" in alt_unity_object.name

    def test_power_joystick(self):
        button_names = ["Horizontal", "Vertical"]
        keys_to_press = [AltUnityKeyCode.D, AltUnityKeyCode.W]
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        axisName = self.altdriver.find_object(By.NAME, "AxisName")
        axisValue = self.altdriver.find_object(By.NAME, "AxisValue")

        for index, key in enumerate(keys_to_press):
            self.altdriver.press_key(key, 0.5, 0.1)

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

        assert initialRotation["x"] != finalRotation["x"] or initialRotation["y"] != finalRotation["y"] or \
            initialRotation["z"] != finalRotation["z"] or initialRotation["w"] != finalRotation["w"]

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

        assert len(names) == 34
        for name in expected_names:
            assert name in names

    def test_find_object_which_contains(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_unity_object = self.altdriver.find_object_which_contains(By.NAME, "EventSy")
        assert alt_unity_object.name == "EventSystem"

    def test_find_with_find_object_which_contains_not_existing_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(exceptions.NotFoundException) as execinfo:
            self.altdriver.find_object_which_contains(By.NAME, "EventNonExisting")

        assert str(execinfo.value) == "Object //*[contains(@name,EventNonExisting)] not found"

    def test_screenshot(self):
        png_path = "testPython.png"
        self.altdriver.get_png_screenshot(png_path)
        assert os.path.exists(png_path)

    def test_wait_for_object(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.wait_for_object(By.NAME, "Capsule")
        assert alt_unity_object.name == "Capsule"

    def test_wait_for_object_to_not_be_present(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "Capsuule")

    def test_wait_for_object_which_contains(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_unity_object = self.altdriver.wait_for_object_which_contains(By.NAME, "Main")
        assert alt_unity_object.name == "Main Camera"

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
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
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
        color1 = p_panel.get_component_property("AltUnityExampleScriptPanel", "normalColor", assembly="Assembly-CSharp")
        p_panel.pointer_down()

        time.sleep(1)

        color2 = p_panel.get_component_property("AltUnityExampleScriptPanel",
                                                "highlightColor", assembly="Assembly-CSharp")
        assert color1 != color2

    def test_pointer_up_from_object(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        time.sleep(1)

        p_panel = self.altdriver.find_object(By.NAME, "Panel")
        color1 = p_panel.get_component_property("AltUnityExampleScriptPanel", "normalColor", assembly="Assembly-CSharp")
        p_panel.pointer_down()
        time.sleep(1)

        p_panel.pointer_up()
        color2 = p_panel.get_component_property("AltUnityExampleScriptPanel",
                                                "highlightColor", assembly="Assembly-CSharp")
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
        self.altdriver.scroll(4, 2, False)
        time.sleep(2)

        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]
        assert cubeInitialPostion != cubeFinalPosition

    def test_scroll_and_wait(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeInitialPostion = [player2.worldX, player2.worldY, player2.worldY]
        self.altdriver.scroll(4, 2)

        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]
        assert cubeInitialPostion != cubeFinalPosition

    def test_acceleration(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt([1, 1, 1], 1, False)

        time.sleep(1)

        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]

        assert initial_position != final_position

    def test_acceleration_and_wait(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt([1, 1, 1], 1)

        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        assert initial_position != final_position

    def test_find_object_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.altdriver.find_object(By.PATH, "//Camera")
        alt_unity_object = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        assert alt_unity_object.name == "Capsule"

        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_unity_object2 = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))

        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_wait_for_object_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.altdriver.find_object(By.PATH, "//Camera")
        alt_unity_object = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        assert alt_unity_object.name == "Capsule"

        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_unity_object2 = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))

        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_find_objects_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.altdriver.find_object(By.PATH, "//Camera")
        alt_unity_objects = self.altdriver.find_objects(By.NAME, "Plane", By.ID, str(camera.id))
        assert alt_unity_objects[0].name == "Plane"

        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_unity_objects2 = self.altdriver.find_objects(
            By.NAME, "Plane", By.ID, str(camera2.id))

        assert alt_unity_objects[0].x != alt_unity_objects2[0].x
        assert alt_unity_objects[0].y != alt_unity_objects2[0].y

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

        alt_unity_object = self.altdriver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.ID, str(camera.id),
            timeout=1
        )

        assert alt_unity_object is not None
        assert alt_unity_object.get_text() == text

    def test_wait_for_object_which_contains_with_camera_id(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        camera = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_unity_object = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.ID, str(camera.id)
        )
        assert alt_unity_object.name == "Canvas"

    def test_find_object_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_unity_object = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider",  By.TAG, "MainCamera")
        assert alt_unity_object.name == "Capsule"

        alt_unity_object2 = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged")
        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_wait_for_object_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_unity_object = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera")
        assert alt_unity_object.name == "Capsule"

        alt_unity_object2 = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider",  By.TAG, "Untagged")
        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_find_objects_with_tag(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_unity_object = self.altdriver.find_objects(By.NAME, "Plane", By.TAG, "MainCamera")
        assert alt_unity_object[0].name == "Plane"

        alt_unity_object2 = self.altdriver.find_objects(By.NAME, "Plane", By.TAG, "Untagged")
        assert alt_unity_object[0].x != alt_unity_object2[0].x
        assert alt_unity_object[0].y != alt_unity_object2[0].y

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

        alt_unity_object = self.altdriver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.TAG, "MainCamera",
            timeout=1
        )
        assert alt_unity_object is not None
        assert alt_unity_object.get_text() == text

    def test_wait_for_object_which_contains_with_tag(self):
        alt_unity_object = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.TAG, "MainCamera"
        )
        assert alt_unity_object.name == "Canvas"

    def test_find_object_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        button = self.altdriver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_unity_object = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider",
            camera_value="Camera"
        )
        assert alt_unity_object.name == "Capsule"

        alt_unity_object2 = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider",
            camera_by=By.NAME, camera_value="Main Camera"
        )

        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_wait_for_object_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        button = self.altdriver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_unity_object = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",
            camera_value="Camera"
        )
        assert alt_unity_object.name == "Capsule"

        alt_unity_object2 = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",
            camera_by=By.NAME, camera_value="Main Camera"
        )
        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_find_objects_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        button = self.altdriver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_unity_object = self.altdriver.find_objects(By.NAME, "Plane", By.NAME, "Camera")
        assert alt_unity_object[0].name == "Plane"

        alt_unity_object2 = self.altdriver.find_objects(By.NAME, "Plane", By.NAME, "Main Camera")
        assert alt_unity_object[0].x != alt_unity_object2[0].x
        assert alt_unity_object[0].y != alt_unity_object2[0].y

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

        alt_unity_object = self.altdriver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.NAME, "Main Camera",
            timeout=1
        )

        assert alt_unity_object is not None
        assert alt_unity_object.get_text() == text

    def test_wait_for_object_which_contains_by_camera(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        alt_unity_object = self.altdriver.wait_for_object_which_contains(By.NAME, "Canva", By.NAME, "Main Camera")
        assert alt_unity_object.name == "Canvas"

    def test_load_additive_scenes(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)

        initial_number_of_elements = self.altdriver.get_all_elements()
        self.altdriver.load_scene("Scene 2 Draggable Panel", load_single=False)
        final_number_of_elements = self.altdriver.get_all_elements()

        assert len(final_number_of_elements) > len(initial_number_of_elements)

        scenes = self.altdriver.get_all_loaded_scenes()
        assert len(scenes) == 2

    def test_load_scene_with_invalid_scene_name(self):
        with pytest.raises(exceptions.SceneNotFoundException):
            self.altdriver.load_scene("Scene 0")

    def test_get_component_property_complex_class(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        component_name = "AltUnityExampleScriptCapsule"
        property_name = "AltUnitySampleClass.testInt"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        property_value = alt_unity_object.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert property_value == 1

    def test_get_component_property_complex_class2(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)

        component_name = "AltUnityExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        property_value = alt_unity_object.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert property_value == "test2"

    def test_set_component_property_complex_class(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)

        component_name = "AltUnityExampleScriptCapsule"
        property_name = "AltUnitySampleClass.testInt"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        alt_unity_object.set_component_property(component_name, property_name, 2, assembly="Assembly-CSharp")
        property_value = alt_unity_object.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert property_value == 2

    def test_set_component_property_complex_class2(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)
        component_name = "AltUnityExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        alt_unity_object.set_component_property(component_name, property_name, "test3", assembly="Assembly-CSharp")
        propertyValue = alt_unity_object.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
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
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene", load_single=True)

        with pytest.raises(exceptions.CouldNotPerformOperationException):
            self.altdriver.unload_scene("Scene 1 AltUnityDriverTestScene")

    def test_set_server_logging(self):
        rule = self.altdriver.call_static_method(
            "Altom.AltUnityTester.Logging.ServerLogManager",
            "Instance.Configuration.FindRuleByName",
            ["AltUnityServerFileRule"],
            assembly="Assembly-CSharp"
        )

        # Default logging level in AltUnity Tester is Debug level
        assert len(rule["Levels"]) == 5

        self.altdriver.set_server_logging(AltUnityLogger.File, AltUnityLogLevel.Off)
        rule = self.altdriver.call_static_method(
            "Altom.AltUnityTester.Logging.ServerLogManager",
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
        with pytest.raises(exceptions.AltUnityInvalidPathException):
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
        self.altdriver.move_mouse(capsule_element.get_screen_position(), 1, True)
        time.sleep(1.5)

        self.altdriver.key_down(AltUnityKeyCode.Mouse0)
        self.altdriver.key_up(AltUnityKeyCode.Mouse0)
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_camera_not_found_exception(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        with pytest.raises(exceptions.CameraNotFoundException):
            self.altdriver.find_object(By.NAME, "Capsule", By.NAME, "Camera")

    def test_input_field_events(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        input_field = self.altdriver.find_object(By.NAME, "InputField").set_text("example", submit=True)

        assert input_field.get_text() == "example"
        assert input_field.get_component_property(
            "AltUnityInputFieldRaisedEvents", "onValueChangedInvoked", assembly="Assembly-CSharp")
        assert input_field.get_component_property(
            "AltUnityInputFieldRaisedEvents", "onSubmitInvoked", assembly="Assembly-CSharp")

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

    def test_load_scene_notification(self):
        test_notification_callbacks = TestNotificationCallback()
        self.altdriver.add_notification_listener(
            NotificationType.LOADSCENE, test_notification_callbacks.scene_loaded_callback)
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        assert test_notification_callbacks.last_scene_loaded == "Scene 1 AltUnityDriverTestScene"
        self.altdriver.remove_notification_listener(NotificationType.LOADSCENE)

    def test_unload_scene_notification(self):
        test_notification_callbacks = TestNotificationCallback()
        self.altdriver.add_notification_listener(
            NotificationType.UNLOADSCENE, test_notification_callbacks.scene_unloaded_callback)
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.load_scene("Scene 2 Draggable Panel", load_single=False)
        self.altdriver.unload_scene("Scene 2 Draggable Panel")
        assert test_notification_callbacks.last_scene_unloaded == "Scene 2 Draggable Panel"
        self.altdriver.remove_notification_listener(NotificationType.UNLOADSCENE)

    def test_log_notification(self):
        test_notification_callbacks = TestNotificationCallback()
        self.altdriver.add_notification_listener(
            NotificationType.LOG, test_notification_callbacks.log_callback)
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        assert "Scene Loaded" in test_notification_callbacks.log_message
        assert test_notification_callbacks.log_type == AltUnityLogLevel.Debug.value
        self.altdriver.remove_notification_listener(NotificationType.LOG)

    def test_application_paused_notification(self):
        test_notification_callbacks = TestNotificationCallback()
        self.altdriver.add_notification_listener(
            NotificationType.APPLICATION_PAUSED, test_notification_callbacks.application_paused_callback)
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.NAME, "AltUnityRunnerPrefab")
        alt_unity_object.call_component_method(
            "Altom.AltUnityTester.AltUnityRunner", "OnApplicationPause",
            parameters=[True],
            type_of_parameters=["System.Boolean"]
        )

        assert test_notification_callbacks.application_paused is True
        self.altdriver.remove_notification_listener(NotificationType.APPLICATION_PAUSED)

    def test_float_world_coordinates(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        plane = self.altdriver.find_object(By.NAME, "Plane")

        assert type(plane.worldX) == float
        assert type(plane.worldY) == float
        assert type(plane.worldZ) == float

    def test_set_command_response_timeout(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.set_command_response_timeout(1)

        with pytest.raises(exceptions.CommandResponseTimeoutException) as execinfo:
            alt_unity_object.call_component_method(
                "AltUnityExampleScriptCapsule", "JumpWithDelay",
                parameters=[], type_of_parameters=[], assembly="Assembly-CSharp"
            )

        self.altdriver.set_command_response_timeout(60)
        assert str(execinfo.value) == ""

    def test_keys_down(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        keys = [AltUnityKeyCode.K, AltUnityKeyCode.L]
        self.altdriver.keys_down(keys)
        self.altdriver.keys_up(keys)

        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        property_value = alt_unity_object.get_component_property(
            "AltUnityExampleScriptCapsule",
            "stringToSetFromTests",
            assembly="Assembly-CSharp"
        )
        assert property_value == "multiple keys pressed"

    def test_press_keys(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        keys = [AltUnityKeyCode.K, AltUnityKeyCode.L]
        self.altdriver.press_keys(keys)

        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        property_value = alt_unity_object.get_component_property(
            "AltUnityExampleScriptCapsule",
            "stringToSetFromTests",
            assembly="Assembly-CSharp"
        )
        assert property_value == "multiple keys pressed"

    def test_find_object_by_coordinates(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        counter_button = self.altdriver.find_object(By.NAME, "ButtonCounter")

        element = self.altdriver.find_object_at_coordinates([80 + counter_button.x, 15 + counter_button.y])
        assert element.name == "Text"

    def test_find_object_by_coordinates_no_element(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        element = self.altdriver.find_object_at_coordinates([-1, -1])
        assert element is None

    def test_scroll_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 10 Sample NIS.unity")
        player = self.altdriver.find_object(By.NAME, "Player")

        assert player.get_component_property(
            "AltUnityNIPDebugScript",
            "wasScrolled",
            assembly="Assembly-CSharp"
        ) is False

        self.altdriver.scroll(300, 1, True)

        assert player.get_component_property(
            "AltUnityNIPDebugScript",
            "wasScrolled",
            assembly="Assembly-CSharp"
        ) is True

    def test_tap_element_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.tap()
        component_name = "AltUnityExampleNewInputSystem"
        property_name = "jumpCounter"
        propertyValue = capsule.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert propertyValue == 1

    def test_tap_coordinates_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.tap(capsule.get_screen_position())
        action_info = self.altdriver.wait_for_object(
            By.PATH, "//ActionText[@text=Capsule was tapped!]", timeout=1)

        assert action_info.get_text() == "Capsule was tapped!"

    def test_scroll_element_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/scene 9 NIS.unity")
        scrollbar = self.altdriver.find_object(By.NAME, "Scrollbar Vertical")
        scrollbar_value = scrollbar.get_component_property(
            "UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI")
        self.altdriver.move_mouse(self.altdriver.find_object(
            By.NAME, "Scroll View").get_screen_position(), duration=1, wait=True)
        self.altdriver.scroll(-3000, 1, True)
        scrollbarFinal = self.altdriver.find_object(By.NAME, "Scrollbar Vertical")
        scrollbar_value_final = scrollbarFinal.get_component_property(
            "UnityEngine.UI.Scrollbar", "value", "UnityEngine.UI")
        assert scrollbar_value != scrollbar_value_final

    def test_click_element_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.click()
        component_name = "AltUnityExampleNewInputSystem"
        property_name = "jumpCounter"
        propertyValue = capsule.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert propertyValue == 1

    def test_click_coordinates_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.click(capsule.get_screen_position())
        action_info = self.altdriver.wait_for_object(
            By.PATH, "//ActionText[@text=Capsule was clicked!]", timeout=1)

        assert action_info.get_text() == "Capsule was clicked!"

    def test_tilt(self):
        self.altdriver.load_scene(
            "Assets/AltUnityTester/Examples/Scenes/Scene 7 New Input System Actions.unity")
        cube = self.altdriver.find_object(By.NAME, "Cube (1)")
        initialPosition = cube.get_world_position()
        self.altdriver.tilt([1000, 10, 10], 1)
        assert initialPosition != self.altdriver.find_object(By.NAME, "Cube (1)").get_world_position()
        isMoved = cube.get_component_property("AltUnityCubeNIS", "isMoved", "Assembly-CSharp")
        assert isMoved

    def test_key_down_and_key_up_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 10 Sample NIS.unity")
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_pos = player.get_component_property(
            "UnityEngine.Transform", "position")
        self.altdriver.key_down(AltUnityKeyCode.A)
        self.altdriver.key_up(AltUnityKeyCode.A)
        pos_left = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_left != initial_pos
        self.altdriver.key_down(AltUnityKeyCode.D)
        self.altdriver.key_up(AltUnityKeyCode.D)
        pos_right = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_right != pos_left
        self.altdriver.key_down(AltUnityKeyCode.W)
        self.altdriver.key_up(AltUnityKeyCode.W)
        pos_up = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_up != pos_right
        self.altdriver.key_down(AltUnityKeyCode.S)
        self.altdriver.key_up(AltUnityKeyCode.S)
        pos_down = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_down != pos_up

    def test_press_key_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 10 Sample NIS.unity")
        player = self.altdriver.find_object(By.NAME, "Player")
        initial_pos = player.get_component_property(
            "UnityEngine.Transform", "position")
        self.altdriver.press_key(AltUnityKeyCode.A)
        pos_left = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_left != initial_pos
        self.altdriver.press_key(AltUnityKeyCode.D)
        pos_right = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_right != pos_left
        self.altdriver.press_key(AltUnityKeyCode.W)
        pos_up = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_up != pos_right
        self.altdriver.press_key(AltUnityKeyCode.S)
        pos_down = player.get_component_property(
            "UnityEngine.Transform", "position")
        assert pos_down != pos_up

    def test_multipoint_swipe_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/Scene 7 Drag And Drop NIS.unity")
        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")

        multipointPositions = [alt_unity_object1.get_screen_position(), [alt_unity_object2.x, alt_unity_object2.y]]

        self.altdriver.multipoint_swipe(multipointPositions, 2)
        time.sleep(2)

        alt_unity_object1 = self.altdriver.find_object(By.NAME, "Drag Image1")
        alt_unity_object2 = self.altdriver.find_object(By.NAME, "Drop Box1")
        alt_unity_object3 = self.altdriver.find_object(By.NAME, "Drop Box2")

        positions = [
            [alt_unity_object1.x, alt_unity_object1.y],
            [alt_unity_object2.x, alt_unity_object2.y],
            [alt_unity_object3.x, alt_unity_object3.y]
        ]

        self.altdriver.multipoint_swipe(positions, 3)
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

    def test_swipe_NIS(self):
        self.altdriver.load_scene("Assets/AltUnityTester/Examples/Scenes/scene 9 NIS.unity")
        scrollbar = self.altdriver.find_object(By.NAME, "Handle")
        scrollbarPosition = scrollbar.get_screen_position()
        button = self.altdriver.find_object(By.PATH, "//Scroll View/Viewport/Content/Button (4)")
        self.altdriver.swipe(
            button.get_screen_position(),
            (button.x, button.y + 20),
            duration=2
            )
        scrollbarFinal = self.altdriver.find_object(By.NAME, "Handle")
        scrollbarPositionFinal = scrollbarFinal.get_screen_position()
        assert scrollbarPosition != scrollbarPositionFinal
