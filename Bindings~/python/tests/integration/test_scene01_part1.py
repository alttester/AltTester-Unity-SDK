"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

import pytest
import time

from .utils import Scenes
from alttester import By, PlayerPrefKeyType
import alttester.exceptions as exceptions


class TestScene01Part1:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene01)

    def test_tap_ui_object(self):
        expected_text = "UIButton clicked to jump capsule!"

        self.alt_driver.find_object(By.NAME, "UIButton").tap()
        capsule_info = self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text={}]".format(expected_text), timeout=1)

        assert capsule_info.get_text() == expected_text

    def test_tap_object(self):
        expected_text = "Capsule was clicked to jump!"

        self.alt_driver.find_object(By.NAME, "Capsule").tap()
        capsule_info = self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text={}]".format(expected_text), timeout=1)

        assert capsule_info.get_text() == expected_text

    def test_find_object_by_name(self):
        plane = self.alt_driver.find_object(By.NAME, "Plane")
        capsule = self.alt_driver.find_object(By.NAME, "Capsule")

        assert plane.name == "Plane"
        assert capsule.name == "Capsule"

    def test_find_object_by_name_and_parent(self):
        capsule_element = self.alt_driver.find_object(
            By.NAME, "Canvas/CapsuleInfo")

        assert capsule_element.name == "CapsuleInfo"

    def test_find_object_child(self):
        alt_object = self.alt_driver.find_object(By.PATH, "//UIButton/*")

        assert alt_object.name == "Text"

    def test_find_object_by_tag(self):
        alt_object = self.alt_driver.find_object(By.TAG, "plane")
        assert alt_object.name == "Plane"

    def test_find_object_by_layer(self):
        alt_object = self.alt_driver.find_object(By.LAYER, "Water")
        assert alt_object.name == "Capsule"

    def test_find_object_by_text(self):
        text = self.alt_driver.find_object(By.NAME, "CapsuleInfo").get_text()
        element = self.alt_driver.find_object(By.TEXT, text)

        assert element.get_text() == text

    @pytest.mark.parametrize("component, name", [
        ("CapsuleCollider", "Capsule"),
        ("AltExampleScriptCapsule", "Capsule"),
        ("AltTesterExamples.Scripts.AltExampleScriptCapsule", "Capsule")
    ])
    def test_find_object_by_component(self, component, name):
        alt_object = self.alt_driver.find_object(By.COMPONENT, component)
        assert alt_object.name == name

    @pytest.mark.parametrize("partial_name", ["Pla", "EventSy"])
    def test_find_object_which_contains(self, partial_name):
        plane = self.alt_driver.find_object_which_contains(
            By.NAME, partial_name)
        assert partial_name in plane.name

    @pytest.mark.parametrize("name, count", [
        ("Plane", 2),
        ("something that does not exist", 0)
    ])
    def test_find_objects_by_name(self, name, count):
        alt_objects = self.alt_driver.find_objects(By.NAME, name)
        assert len(alt_objects) == count

    def test_find_objects_by_tag(self):
        alt_objects = self.alt_driver.find_objects(By.TAG, "plane")

        assert len(alt_objects) == 2
        for alt_object in alt_objects:
            assert alt_object.name == "Plane"

    def test_find_objects_by_layer(self):
        alt_objects = self.alt_driver.find_objects(By.LAYER, "Default")
        assert len(alt_objects) >= 10

    def test_find_objects_by_component(self):
        alt_objects = self.alt_driver.find_objects(
            By.COMPONENT, "UnityEngine.MeshFilter")
        assert len(alt_objects) == 5

    def test_find_parent_using_path(self):
        parent = self.alt_driver.find_object(By.PATH, "//CapsuleInfo/..")
        assert parent.name == "Canvas"

    def test_find_objects_which_contain_by_name(self):
        alt_objects = self.alt_driver.find_objects_which_contain(
            By.NAME, "Capsule")

        assert len(alt_objects) == 2
        for alt_object in alt_objects:
            assert "Capsule" in alt_object.name

    def test_find_object_which_contains_with_not_existing_object(self):
        with pytest.raises(exceptions.NotFoundException) as execinfo:
            self.alt_driver.find_object_which_contains(
                By.NAME, "EventNonExisting")

        assert str(
            execinfo.value) == "Object //*[contains(@name,EventNonExisting)] not found"

    def test_get_all_components(self):
        components = self.alt_driver.find_object(
            By.NAME, "Canvas").get_all_components()
        assert len(components) == 5
        assert components[0]["componentName"] == "UnityEngine.RectTransform"
        assert components[0]["assemblyName"] == "UnityEngine.CoreModule"

    def test_wait_for_object(self):
        alt_object = self.alt_driver.wait_for_object(By.NAME, "Capsule")
        assert alt_object.name == "Capsule"

    def test_wait_for_object_with_non_existing_object(self):
        object_name = "Non Existing Object"

        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.alt_driver.wait_for_object(By.NAME, object_name, timeout=1)

        assert str(execinfo.value) == "Element {} not found after 1 seconds".format(
            object_name)

    def test_wait_for_object_by_name(self):
        plane = self.alt_driver.wait_for_object(By.NAME, "Plane")
        capsule = self.alt_driver.wait_for_object(By.NAME, "Capsule")

        assert plane.name == "Plane"
        assert capsule.name == "Capsule"

    def test_get_application_screen_size(self):
        screensize = self.alt_driver.get_application_screensize()
        # We cannot set resolution on iOS so we don't know the exact resolution
        # we just want to see that it returns a value and is different than 0
        assert screensize[0] != 0
        assert screensize[1] != 0

    def test_wait_for_object_with_text(self):
        text_to_wait_for = self.alt_driver.find_object(
            By.NAME, "CapsuleInfo").get_text()
        capsule_info = self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text={}]".format(text_to_wait_for), timeout=1)

        assert capsule_info.name == "CapsuleInfo"
        assert capsule_info.get_text() == text_to_wait_for

    def test_wait_for_object_with_wrong_text(self):
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.alt_driver.wait_for_object(
                By.PATH, "//CapsuleInfo[@text=aaaaa]", timeout=1)

        assert str(
            execinfo.value) == "Element //CapsuleInfo[@text=aaaaa] not found after 1 seconds"

    def test_wait_for_object_which_contains(self):
        alt_object = self.alt_driver.wait_for_object_which_contains(
            By.NAME, "Main")
        assert alt_object.name == "Main Camera"

    def test_wait_for_object_to_not_be_present(self):
        self.alt_driver.wait_for_object_to_not_be_present(By.NAME, "Capsuule")

    def test_wait_for_object_to_not_be_present_fail(self):
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.alt_driver.wait_for_object_to_not_be_present(
                By.NAME, "Capsule", timeout=1)

        assert str(
            execinfo.value) == "Element Capsule still found after 1 seconds"

    def test_get_text_with_non_english_text(self):
        text = self.alt_driver.find_object(
            By.NAME, "NonEnglishText").get_text()
        assert text == "BJÖRN'S PASS"

    def test_get_text_with_chinese_letters(self):
        text = self.alt_driver.find_object(
            By.NAME, "ChineseLetters").get_text()
        assert text == "哦伊娜哦"

    def test_set_text(self):
        text_object = self.alt_driver.find_object(By.NAME, "NonEnglishText")
        original_text = text_object.get_text()
        after_text = text_object.set_text("ModifiedText").get_text()

        assert original_text != after_text
        assert after_text == "ModifiedText"

    def test_double_tap(self):
        counter_button = self.alt_driver.find_object(By.NAME, "ButtonCounter")
        counter_button_text = self.alt_driver.find_object(
            By.NAME, "ButtonCounter/Text")
        counter_button.tap(count=2)

        # time.sleep(0.5)

        assert counter_button_text.get_text() == "2"

    def test_tap_on_screen_where_there_are_no_objects(self):
        counter_button = self.alt_driver.find_object(By.NAME, "ButtonCounter")
        self.alt_driver.tap({"x": 1, "y": counter_button.y + 100})

    def test_hold_button(self):
        button = self.alt_driver.find_object(By.NAME, "UIButton")
        self.alt_driver.hold_button(button.get_screen_position(), duration=1)

        capsule_info = self.alt_driver.find_object(By.NAME, "CapsuleInfo")
        text = capsule_info.get_text()
        assert text == "UIButton clicked to jump capsule!"

    def test_hold_button_without_wait(self):
        button = self.alt_driver.find_object(By.NAME, "UIButton")
        self.alt_driver.hold_button(
            button.get_screen_position(), duration=1, wait=False)

        time.sleep(2)

        capsule_info = self.alt_driver.find_object(By.NAME, "CapsuleInfo")
        text = capsule_info.get_text()
        assert text == "UIButton clicked to jump capsule!"

    def test_wait_for_component_property(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        result = alt_object.wait_for_component_property(
            "AltExampleScriptCapsule", "TestBool", True,
            "Assembly-CSharp")

        assert result is True

    @pytest.mark.iOSUnsupported
    @pytest.mark.WebGLUnsupported
    @pytest.mark.AndroidUnsupported
    def test_wait_for_component_property_get_property_as_string(self):
        Canvas = self.alt_driver.wait_for_object(By.PATH, "/Canvas")
        Canvas.wait_for_component_property("UnityEngine.RectTransform", "rect.x", "-960.0",
                                           "UnityEngine.CoreModule", 1, get_property_as_string=True)

        Canvas.wait_for_component_property("UnityEngine.RectTransform", "hasChanged", True,
                                           "UnityEngine.CoreModule", 1, get_property_as_string=True)

        Canvas.wait_for_component_property("UnityEngine.RectTransform", "constrainProportionsScale", False,
                                           "UnityEngine.CoreModule", 1, get_property_as_string=True)

        Canvas.wait_for_component_property("UnityEngine.RectTransform", "transform",
                                           "[[],[[]],[[]],[[]],[[]],[[],[],[]],[[[],[],[]]],[],[],[[]],[[]],[[]]]",
                                           "UnityEngine.CoreModule", 1, get_property_as_string=True)

        Canvas.wait_for_component_property("UnityEngine.RectTransform", "name", "Canvas",
                                           "UnityEngine.CoreModule", 1, get_property_as_string=True)

    def test_wait_for_component_property_component_not_found(self):
        componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunnerTest"
        propertyName = "InstrumentationSettings.AltServerPort"
        alt_object = self.alt_driver.find_object(By.NAME, "AltTesterPrefab")
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            alt_object.wait_for_component_property(
                componentName,
                propertyName,
                "Test",
                "AltTester.AltTesterUnitySDK",
                timeout=2,
            )
        assert str(
            execinfo.value
        ) == "After 2 seconds, exception was: Component not found for component: {} and property {}".format(
            componentName, propertyName
        )

    def test_wait_for_component_property_not_found(self):
        componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner"
        propertyName = "InstrumentationSettings.AltServerPortTest"
        alt_object = self.alt_driver.find_object(By.NAME, "AltTesterPrefab")
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            alt_object.wait_for_component_property(
                componentName,
                propertyName,
                "Test",
                "AltTester.AltTesterUnitySDK",
                timeout=2,
            )
        assert str(execinfo.value) == (
            "After 2 seconds, exception was: Property AltServerPortTest not found "
            "for component: {} and property {}".format(
                componentName, propertyName)
        )

    def test_wait_for_component_property_timeout(self):
        componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner"
        propertyName = "InstrumentationSettings.AltServerPort"
        alt_object = self.alt_driver.find_object(By.NAME, "AltTesterPrefab")
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            alt_object.wait_for_component_property(
                componentName, propertyName, "Test", "AltTester.AltTesterUnitySDK", 2
            )
        assert (
            str(execinfo.value)
            == "Property InstrumentationSettings.AltServerPort was 13005, not Test as expected, after 2 seconds"
        )

    @pytest.mark.iOSUnsupported
    @pytest.mark.WebGLUnsupported
    def test_wait_for_component_property_assembly_not_found(self):
        componentName = "AltExampleScriptCapsule"
        propertyName = "InstrumentationSettings.AltServerPort"
        alt_object = self.alt_driver.find_object(By.NAME, "AltTesterPrefab")
        with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            alt_object.wait_for_component_property(
                componentName, propertyName, "13000", "Assembly-CSharpTest", timeout=2
            )
        assert str(
            execinfo.value
        ) == "After 2 seconds, exception was: Assembly not found for component: {} and property {}".format(
            componentName, propertyName
        )

    def test_get_component_property(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        result = alt_object.get_component_property(
            "AltExampleScriptCapsule", "arrayOfInts", "Assembly-CSharp")

        assert result == [1, 2, 3]

    def test_get_component_property_with_bool(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        result = alt_object.get_component_property(
            "AltExampleScriptCapsule", "TestBool", "Assembly-CSharp")

        assert result is True

    def test_set_component_property(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        alt_object.set_component_property(
            "AltExampleScriptCapsule", "arrayOfInts", "Assembly-CSharp", [2, 3, 4])

        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        result = alt_object.get_component_property(
            "AltExampleScriptCapsule", "arrayOfInts", "Assembly-CSharp")

        assert result == [2, 3, 4]

    def test_call_component_method(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        result = alt_object.call_component_method(
            "AltExampleScriptCapsule", "Jump", "Assembly-CSharp", parameters=["setFromMethod"])
        assert result is None

        self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text=setFromMethod]", timeout=1)
        assert self.alt_driver.find_object(
            By.NAME, "CapsuleInfo").get_text() == "setFromMethod"

    def test_call_component_method_with_no_parameters(self):
        result = self.alt_driver.find_object(By.PATH, "/Canvas/Button/Text")
        text = result.call_component_method(
            "UnityEngine.UI.Text", "get_text", "UnityEngine.UI")
        assert text == "Change Camera Mode"

    def test_call_component_method_with_parameters(self):
        assembly = "UnityEngine.UI"
        expected_font_size = 16
        alt_object = self.alt_driver.find_object(
            By.PATH, "/Canvas/UnityUIInputField/Text")
        alt_object.call_component_method(
            "UnityEngine.UI.Text", "set_fontSize", assembly,
            parameters=["16"]
        )
        font_size = alt_object.call_component_method(
            "UnityEngine.UI.Text", "get_fontSize", assembly,
            parameters=[]
        )

        assert expected_font_size == font_size

    def test_call_component_method_with_assembly(self):
        capsule = self.alt_driver.find_object(By.NAME, "Capsule")
        initial_rotation = capsule.get_component_property(
            "UnityEngine.Transform", "rotation", "UnityEngine.CoreModule")
        capsule.call_component_method(
            "UnityEngine.Transform", "Rotate", "UnityEngine.CoreModule",
            parameters=["10", "10", "10"],
            type_of_parameters=["System.Single",
                                "System.Single", "System.Single"],
        )

        capsule_after_rotation = self.alt_driver.find_object(
            By.NAME, "Capsule")
        final_rotation = capsule_after_rotation.get_component_property(
            "UnityEngine.Transform", "rotation", "UnityEngine.CoreModule",)

        assert initial_rotation["x"] != final_rotation["x"] or initial_rotation["y"] != final_rotation["y"] or \
            initial_rotation["z"] != final_rotation["z"] or initial_rotation["w"] != final_rotation["w"]

    @pytest.mark.parametrize("parameters", [
        [1, "stringparam", 0.5, [1, 2, 3]],
        (1, "stringparam", 0.5, [1, 2, 3])
    ])
    def test_call_component_method_with_multiple_parameters(self, parameters):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        result = alt_object.call_component_method(
            "AltExampleScriptCapsule",
            "TestCallComponentMethod",
            "Assembly-CSharp",
            parameters=parameters
        )

        assert result == "1,stringparam,0.5,[1,2,3]"

    def test_call_component_method_with_multiple_definitions(self):
        capsule = self.alt_driver.find_object(By.NAME, "Capsule")
        capsule.call_component_method(
            "AltExampleScriptCapsule", "Test", "Assembly-CSharp",
            parameters=["2"],
            type_of_parameters=["System.Int32"]
        )
        capsule_info = self.alt_driver.find_object(By.NAME, "CapsuleInfo")

        assert capsule_info.get_text() == "6"

    def test_call_component_method_with_invalid_assembly(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.AssemblyNotFoundException) as execinfo:
            alt_object.call_component_method(
                "RandomComponent", "TestMethodWithManyParameters", "RandomAssembly",
                parameters=[1, "stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[],
            )

        assert str(execinfo.value) == "Assembly not found"

    def test_call_component_method_with_incorrect_number_of_parameters(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.MethodWithGivenParametersNotFoundException) as execinfo:
            alt_object.call_component_method(
                "AltExampleScriptCapsule", "TestMethodWithManyParameters", "Assembly-CSharp",
                parameters=["stringparam", 0.5, [1, 2, 3]],
                type_of_parameters=[]
            )

        assert str(execinfo.value) == \
            "No method found with 3 parameters matching signature: TestMethodWithManyParameters(System.String[])"

    def test_call_component_method_with_invalid_method_argument_types(self):
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")

        with pytest.raises(exceptions.FailedToParseArgumentsException) as execinfo:

            alt_object.call_component_method(
                "AltExampleScriptCapsule", "TestMethodWithManyParameters", "Assembly-CSharp",
                parameters=["stringnoint", "stringparams", 0.5, [1, 2, 3]],
                type_of_parameters=[]
            )

        assert str(
            execinfo.value) == "Could not parse parameter '\"stringnoint\"' to type System.Int32"

    def test_call_static_method(self):
        self.alt_driver.call_static_method(
            "UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", ["Test", "1"])
        value = self.alt_driver.call_static_method(
            "UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", ["Test", "2"])

        assert value == 1

    @pytest.mark.parametrize("key_value, key_type", [
        (1, PlayerPrefKeyType.Int),
        (1.3, PlayerPrefKeyType.Float),
        ("string value", PlayerPrefKeyType.String)
    ])
    def test_set_player_pref_keys(self, key_value, key_type):
        self.alt_driver.delete_player_pref()
        self.alt_driver.set_player_pref_key("test", key_value, key_type)
        actual_value = self.alt_driver.get_player_pref_key("test", key_type)

        assert actual_value == key_value

    def test_press_next_scene(self):
        initial_scene = self.alt_driver.get_current_scene()
        self.alt_driver.find_object(By.NAME, "NextScene").tap()

        current_scene = self.alt_driver.get_current_scene()
        assert initial_scene != current_scene

    def test_acceleration(self):
        capsule = self.alt_driver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.alt_driver.tilt([1, 1, 1], duration=0.1, wait=False)

        time.sleep(0.1)

        capsule = self.alt_driver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]

        assert initial_position != final_position

    def test_acceleration_and_wait(self):
        capsule = self.alt_driver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.alt_driver.tilt([1, 1, 1], duration=0.1)

        capsule = self.alt_driver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        assert initial_position != final_position
