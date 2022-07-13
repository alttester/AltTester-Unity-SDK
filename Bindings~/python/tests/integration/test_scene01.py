import time

import pytest

from .utils import Scenes
from altunityrunner import By, PlayerPrefKeyType, AltUnityKeyCode
import altunityrunner.exceptions as exceptions


class TestScene01:

    @pytest.fixture(autouse=True)
    def setup(self, altdriver):
        self.altdriver = altdriver
        self.altdriver.load_scene(Scenes.Scene01)

    def test_tap_ui_object(self):
        expected_text = "UIButton clicked to jump capsule!"

        self.altdriver.find_object(By.NAME, "UIButton").tap()
        capsule_info = self.altdriver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text={}]".format(expected_text), timeout=1)

        assert capsule_info.get_text() == expected_text

    def test_tap_object(self):
        expected_text = "Capsule was clicked to jump!"

        self.altdriver.find_object(By.NAME, "Capsule").tap()
        capsule_info = self.altdriver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text={}]".format(expected_text), timeout=1)

        assert capsule_info.get_text() == expected_text

    def test_find_object_by_name(self):
        plane = self.altdriver.find_object(By.NAME, "Plane")
        capsule = self.altdriver.find_object(By.NAME, "Capsule")

        assert plane.name == "Plane"
        assert capsule.name == "Capsule"

    def test_find_object_by_name_and_parent(self):
        capsule_element = self.altdriver.find_object(By.NAME, "Canvas/CapsuleInfo")

        assert capsule_element.name == "CapsuleInfo"

    def test_find_object_child(self):
        alt_object = self.altdriver.find_object(By.PATH, "//UIButton/*")

        assert alt_object.name == "Text"

    def test_find_object_by_tag(self):
        alt_object = self.altdriver.find_object(By.TAG, "plane")
        assert alt_object.name == "Plane"

    def test_find_object_by_layer(self):
        alt_object = self.altdriver.find_object(By.LAYER, "Water")
        assert alt_object.name == "Capsule"

    def test_find_object_by_text(self):
        text = self.altdriver.find_object(By.NAME, "CapsuleInfo").get_text()
        element = self.altdriver.find_object(By.TEXT, text)

        assert element.get_text() == text

    @pytest.mark.parametrize("component, name", [
        ("CapsuleCollider", "Capsule"),
        ("AltUnityExampleScriptCapsule", "Capsule"),
        ("AltUnityTesterExamples.Scripts.AltUnityExampleScriptCapsule", "Capsule")
    ])
    def test_find_object_by_component(self, component, name):
        alt_object = self.altdriver.find_object(By.COMPONENT, component)
        assert alt_object.name == name

    @pytest.mark.parametrize("partial_name", ["Pla", "EventSy"])
    def test_find_object_which_contains(self, partial_name):
        plane = self.altdriver.find_object_which_contains(By.NAME, partial_name)
        assert partial_name in plane.name

    @pytest.mark.parametrize("name, count", [
        ("Plane", 2),
        ("something that does not exist", 0)
    ])
    def test_find_objects_by_name(self, name, count):
        alt_objects = self.altdriver.find_objects(By.NAME, name)
        assert len(alt_objects) == count

    def test_find_objects_by_tag(self):
        alt_objects = self.altdriver.find_objects(By.TAG, "plane")

        assert len(alt_objects) == 2
        for alt_object in alt_objects:
            assert alt_object.name == "Plane"

    def test_find_objects_by_layer(self):
        alt_objects = self.altdriver.find_objects(By.LAYER, "Default")
        assert len(alt_objects) == 12

    def test_find_objects_by_component(self):
        alt_objects = self.altdriver.find_objects(By.COMPONENT, "UnityEngine.MeshFilter")
        assert len(alt_objects) == 5

    def test_find_parent_using_path(self):
        parent = self.altdriver.find_object(By.PATH, "//CapsuleInfo/..")
        assert parent.name == "Canvas"

    def test_find_objects_which_contain_by_name(self):
        alt_objects = self.altdriver.find_objects_which_contain(By.NAME, "Capsule")

        assert len(alt_objects) == 2
        for alt_object in alt_objects:
            assert "Capsule" in alt_object.name

    def test_find_object_which_contains_with_not_existing_object(self):
        with pytest.raises(exceptions.NotFoundException) as execinfo:
            self.altdriver.find_object_which_contains(By.NAME, "EventNonExisting")

        assert str(execinfo.value) == "Object //*[contains(@name,EventNonExisting)] not found"

    def test_get_all_components(self):
        components = self.altdriver.find_object(By.NAME, "Canvas").get_all_components()
        assert len(components) == 5
        assert components[0]["componentName"] == "UnityEngine.RectTransform"
        assert components[0]["assemblyName"] == "UnityEngine.CoreModule"

    def test_wait_for_object(self):
        alt_unity_object = self.altdriver.wait_for_object(By.NAME, "Capsule")
        assert alt_unity_object.name == "Capsule"

    def test_wait_for_object_with_non_existing_object(self):
        object_name = "Non Existing Object"

        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object(By.NAME, object_name, timeout=1)

        assert str(execinfo.value) == "Element {} not found after 1 seconds".format(object_name)

    def test_wait_for_object_by_name(self):
        plane = self.altdriver.wait_for_object(By.NAME, "Plane")
        capsule = self.altdriver.wait_for_object(By.NAME, "Capsule")

        assert plane.name == "Plane"
        assert capsule.name == "Capsule"

    def test_wait_for_object_with_text(self):
        text_to_wait_for = self.altdriver.find_object(By.NAME, "CapsuleInfo").get_text()
        capsule_info = self.altdriver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text={}]".format(text_to_wait_for), timeout=1)

        assert capsule_info.name == "CapsuleInfo"
        assert capsule_info.get_text() == text_to_wait_for

    def test_wait_for_object_with_wrong_text(self):
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=aaaaa]", timeout=1)

        assert str(execinfo.value) == "Element //CapsuleInfo[@text=aaaaa] not found after 1 seconds"

    def test_wait_for_object_which_contains(self):
        alt_unity_object = self.altdriver.wait_for_object_which_contains(By.NAME, "Main")
        assert alt_unity_object.name == "Main Camera"

    def test_wait_for_object_to_not_be_present(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "Capsuule")

    def test_wait_for_object_to_not_be_present_fail(self):
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.altdriver.wait_for_object_to_not_be_present(By.NAME, "Capsule", timeout=1)

        assert str(execinfo.value) == "Element Capsule still found after 1 seconds"

    def test_get_text_with_non_english_text(self):
        text = self.altdriver.find_object(By.NAME, "NonEnglishText").get_text()
        assert text == "BJÖRN'S PASS"

    def test_get_text_with_chinese_letters(self):
        text = self.altdriver.find_object(By.NAME, "ChineseLetters").get_text()
        assert text == "哦伊娜哦"

    def test_set_text(self):
        text_object = self.altdriver.find_object(By.NAME, "NonEnglishText")
        original_text = text_object.get_text()
        after_text = text_object.set_text("ModifiedText").get_text()

        assert original_text != after_text
        assert after_text == "ModifiedText"

    def test_double_tap(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")

        counter_button = self.altdriver.find_object(By.NAME, "ButtonCounter")
        counter_button_text = self.altdriver.find_object(By.NAME, "ButtonCounter/Text")
        counter_button.tap(count=2)

        # time.sleep(0.5)

        assert counter_button_text.get_text() == "2"

    def test_tap_on_screen_where_there_are_no_objects(self):
        counter_button = self.altdriver.find_object(By.NAME, "ButtonCounter")
        self.altdriver.tap({"x": 1, "y": counter_button.y + 100})

    def test_hold_button(self):
        button = self.altdriver.find_object(By.NAME, "UIButton")
        self.altdriver.hold_button(button.get_screen_position(), duration=1)

        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")
        text = capsule_info.get_text()
        assert text == "UIButton clicked to jump capsule!"

    def test_hold_button_without_wait(self):
        button = self.altdriver.find_object(By.NAME, "UIButton")
        self.altdriver.hold_button(button.get_screen_position(), duration=1, wait=False)

        # time.sleep(2)

        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")
        text = capsule_info.get_text()
        assert text, "UIButton clicked to jump capsule!"

    def test_get_component_property(self):
        alt_object = self.altdriver.find_object(By.NAME, "Capsule")
        result = alt_object.get_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", assembly="Assembly-CSharp")

        assert result, [1, 2, 3]

    def test_get_component_property_with_bool(self):
        alt_object = self.altdriver.find_object(By.NAME, "Capsule")
        result = alt_object.get_component_property(
            "AltUnityExampleScriptCapsule", "TestBool", assembly="Assembly-CSharp")

        assert result is True

    def test_set_component_property(self):
        alt_object = self.altdriver.find_object(By.NAME, "Capsule")
        alt_object.set_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", [2, 3, 4], assembly="Assembly-CSharp")

        alt_object = self.altdriver.find_object(By.NAME, "Capsule")
        result = alt_object.get_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", assembly="Assembly-CSharp")

        assert result == [2, 3, 4]

    def test_call_component_method(self):
        alt_object = self.altdriver.find_object(By.NAME, "Capsule")
        result = alt_object.call_component_method(
            "AltUnityExampleScriptCapsule", "Jump", parameters=["setFromMethod"], assembly="Assembly-CSharp")
        assert result is None

        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=setFromMethod]", timeout=1)
        assert self.altdriver.find_object(By.NAME, "CapsuleInfo").get_text() == "setFromMethod"

    def test_call_component_method_with_assembly(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_rotation = capsule.get_component_property(
            "UnityEngine.Transform", "rotation")
        capsule.call_component_method(
            "UnityEngine.Transform", "Rotate",
            parameters=["10", "10", "10"],
            type_of_parameters=["System.Single", "System.Single", "System.Single"],
            assembly="UnityEngine.CoreModule"
        )

        capsule_after_rotation = self.altdriver.find_object(By.NAME, "Capsule")
        final_rotation = capsule_after_rotation.get_component_property(
            "UnityEngine.Transform", "rotation")

        assert initial_rotation["x"] != final_rotation["x"] or initial_rotation["y"] != final_rotation["y"] or \
            initial_rotation["z"] != final_rotation["z"] or initial_rotation["w"] != final_rotation["w"]

    @pytest.mark.parametrize("parameters", [
        [1, "stringparam", 0.5, [1, 2, 3]],
        (1, "stringparam", 0.5, [1, 2, 3])
    ])
    def test_call_component_method_with_multiple_parameters(self, parameters):
        alt_object = self.altdriver.find_object(By.NAME, "Capsule")
        result = alt_object.call_component_method(
            "AltUnityExampleScriptCapsule",
            "TestCallComponentMethod",
            parameters=parameters,
            assembly="Assembly-CSharp"
        )

        assert result == "1,stringparam,0.5,[1,2,3]"

    def test_call_component_method_with_multiple_definitions(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.call_component_method(
            "AltUnityExampleScriptCapsule", "Test", ["2"], type_of_parameters=["System.Int32"],
            assembly="Assembly-CSharp")
        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")

        assert capsule_info.get_text() == "6"

    def test_call_component_method_with_invalid_assembly(self):
        alt_object = self.altdriver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.AssemblyNotFoundException) as execinfo:
            alt_object.call_component_method(
                "RandomComponent", "TestMethodWithManyParameters",
                parameters=[1, "stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[],
                assembly="RandomAssembly"
            )

        assert str(execinfo.value) == "Assembly not found"

    def test_call_component_method_with_incorrect_number_of_parameters(self):
        alt_object = self.altdriver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.MethodWithGivenParametersNotFoundException) as execinfo:
            alt_object.call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                parameters=["stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[], assembly="Assembly-CSharp"
            )

        assert str(execinfo.value) == \
            "No method found with 3 parameters matching signature: TestMethodWithManyParameters(System.String[])"

    def test_call_component_method_with_invalid_method_argument_types(self):
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.FailedToParseArgumentsException) as execinfo:
            alt_unity_object.call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                parameters=["stringnoint", "stringparams", 0.5, [1, 2, 3]],
                type_of_parameters=[], assembly="Assembly-CSharp"
            )

        assert str(execinfo.value) == "Could not parse parameter '\"stringnoint\"' to type System.Int32"

    def test_call_static_method(self):
        self.altdriver.call_static_method(
            "UnityEngine.PlayerPrefs", "SetInt", ["Test", "1"], assembly="UnityEngine.CoreModule")
        value = self.altdriver.call_static_method(
            "UnityEngine.PlayerPrefs", "GetInt", ["Test", "2"], assembly="UnityEngine.CoreModule")

        assert value == 1

    @pytest.mark.parametrize("key_value, key_type", [
        (1, PlayerPrefKeyType.Int),
        (1.3, PlayerPrefKeyType.Float),
        ("string value", PlayerPrefKeyType.String)
    ])
    def test_set_player_pref_keys(self, key_value, key_type):
        self.altdriver.delete_player_pref()
        self.altdriver.set_player_pref_key("test", key_value, key_type)
        actual_value = self.altdriver.get_player_pref_key("test", key_type)

        assert actual_value == key_value

    def test_press_next_scene(self):
        initial_scene = self.altdriver.get_current_scene()
        self.altdriver.find_object(By.NAME, "NextScene").tap()

        current_scene = self.altdriver.get_current_scene()
        assert initial_scene != current_scene

    def test_acceleration(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt([1, 1, 1], duration=0.1, wait=False)

        time.sleep(0.1)

        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]

        assert initial_position != final_position

    def test_acceleration_and_wait(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt([1, 1, 1], duration=0.1)

        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        assert initial_position != final_position

    def test_find_object_with_camera_id(self):
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
        camera = self.altdriver.find_object(By.PATH, "//Main Camera")
        self.altdriver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs",
            By.ID, str(camera.id)
        )

        elements = self.altdriver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_with_text_with_camera_id(self):
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
        camera = self.altdriver.find_object(By.PATH, "//Main Camera")
        alt_unity_object = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.ID, str(camera.id)
        )
        assert alt_unity_object.name == "Canvas"

    def test_find_object_with_tag(self):
        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_unity_object = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider",  By.TAG, "MainCamera")
        assert alt_unity_object.name == "Capsule"

        alt_unity_object2 = self.altdriver.find_object(By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged")
        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_wait_for_object_with_tag(self):
        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_unity_object = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera")
        assert alt_unity_object.name == "Capsule"

        alt_unity_object2 = self.altdriver.wait_for_object(By.COMPONENT, "CapsuleCollider",  By.TAG, "Untagged")
        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_find_objects_with_tag(self):
        alt_button = self.altdriver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_unity_object = self.altdriver.find_objects(By.NAME, "Plane", By.TAG, "MainCamera")
        assert alt_unity_object[0].name == "Plane"

        alt_unity_object2 = self.altdriver.find_objects(By.NAME, "Plane", By.TAG, "Untagged")
        assert alt_unity_object[0].x != alt_unity_object2[0].x
        assert alt_unity_object[0].y != alt_unity_object2[0].y

    def test_wait_for_object_not_be_present_with_tag(self):
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "ObjectDestroyedIn5Secs", By.TAG, "MainCamera")

        elements = self.altdriver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_with_text_with_tag(self):
        name = "CapsuleInfo"
        text = self.altdriver.find_object(By.NAME, name).get_text()

        alt_unity_object = self.altdriver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.TAG, "MainCamera",
            timeout=1
        )
        assert alt_unity_object is not None
        assert alt_unity_object.get_text() == text

    def test_find_object_by_camera(self):
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
            camera_by=By.NAME,
            camera_value="Main Camera"
        )

        assert alt_unity_object.x != alt_unity_object2.x
        assert alt_unity_object.y != alt_unity_object2.y

    def test_wait_for_object_by_camera(self):
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
        button = self.altdriver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_unity_object = self.altdriver.find_objects(By.NAME, "Plane", By.NAME, "Camera")
        assert alt_unity_object[0].name == "Plane"

        alt_unity_object2 = self.altdriver.find_objects(By.NAME, "Plane", By.NAME, "Main Camera")
        assert alt_unity_object[0].x != alt_unity_object2[0].x
        assert alt_unity_object[0].y != alt_unity_object2[0].y

    def test_wait_for_object_not_be_present_by_camera(self):
        self.altdriver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs",
            By.NAME, "Main Camera"
        )

        elements = self.altdriver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_by_camera_2(self):
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
        alt_unity_object = self.altdriver.wait_for_object_which_contains(By.NAME, "Canva", By.NAME, "Main Camera")
        assert alt_unity_object.name == "Canvas"

    def test_get_component_property_complex_class(self):
        component_name = "AltUnityExampleScriptCapsule"
        property_name = "AltUnitySampleClass.testInt"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        property_value = alt_unity_object.get_component_property(
            component_name, property_name,
            max_depth=1,
            assembly="Assembly-CSharp"
        )
        assert property_value == 1

    def test_get_component_property_complex_class2(self):
        component_name = "AltUnityExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        property_value = alt_unity_object.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert property_value == "test2"

    def test_set_component_property_complex_class(self):
        component_name = "AltUnityExampleScriptCapsule"
        property_name = "AltUnitySampleClass.testInt"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        alt_unity_object.set_component_property(component_name, property_name, 2, assembly="Assembly-CSharp")
        property_value = alt_unity_object.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert property_value == 2

    def test_set_component_property_complex_class2(self):
        component_name = "AltUnityExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
        assert alt_unity_object is not None

        alt_unity_object.set_component_property(component_name, property_name, "test3", assembly="Assembly-CSharp")
        propertyValue = alt_unity_object.get_component_property(
            component_name, property_name, max_depth=1, assembly="Assembly-CSharp")
        assert propertyValue == "test3"

    def test_get_parent(self):
        element = self.altdriver.find_object(By.NAME, "Canvas/CapsuleInfo")
        element_parent = element.get_parent()
        assert element_parent.name == "Canvas"

    def test_tap_coordinates(self):
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.tap(capsule_element.get_screen_position())
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_click_coordinates(self):
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.click(capsule_element.get_screen_position())
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_tap_element(self):
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        capsule_element.tap(1)
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_click_element(self):
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        capsule_element.click()
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_key_down_and_key_up_mouse0(self):
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        self.altdriver.move_mouse(capsule_element.get_screen_position(), duration=0.1, wait=True)

        self.altdriver.key_down(AltUnityKeyCode.Mouse0)
        self.altdriver.key_up(AltUnityKeyCode.Mouse0)
        self.altdriver.wait_for_object(By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_camera_not_found_exception(self):
        with pytest.raises(exceptions.CameraNotFoundException):
            self.altdriver.find_object(By.NAME, "Capsule", By.NAME, "Camera")

    def test_input_field_events(self):
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

    def test_float_world_coordinates(self):
        plane = self.altdriver.find_object(By.NAME, "Plane")

        assert type(plane.worldX) == float
        assert type(plane.worldY) == float
        assert type(plane.worldZ) == float

    def test_set_command_response_timeout(self):
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
        counter_button = self.altdriver.find_object(By.NAME, "ButtonCounter")

        element = self.altdriver.find_object_at_coordinates([80 + counter_button.x, 15 + counter_button.y])
        assert element.name == "Text"

    def test_find_object_by_coordinates_no_element(self):
        element = self.altdriver.find_object_at_coordinates([-1, -1])
        assert element is None

    def test_call_private_method(self):
        self.altdriver.load_scene("Scene 1 AltUnityDriverTestScene")
        capsule_element = self.altdriver.find_object(By.NAME, "Capsule")
        capsule_element.call_component_method("AltUnityExampleScriptCapsule",
                                              "callJump", [], assembly="Assembly-CSharp")
        capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")
        assert capsule_info.get_text() == "Capsule jumps!"
