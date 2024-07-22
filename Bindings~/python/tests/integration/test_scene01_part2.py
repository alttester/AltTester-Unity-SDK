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

from .utils import Scenes
from alttester import By, AltKeyCode
import alttester.exceptions as exceptions


class TestScene01Part2:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.alt_driver.reset_input()
        self.alt_driver.load_scene(Scenes.Scene01)

    def test_find_object_with_camera_id(self):
        alt_button = self.alt_driver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.alt_driver.find_object(By.PATH, "//Camera")
        alt_object = self.alt_driver.find_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        assert alt_object.name == "Capsule"

        camera2 = self.alt_driver.find_object(By.PATH, "//Main Camera")
        alt_object2 = self.alt_driver.find_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))

        assert alt_object.x != alt_object2.x
        assert alt_object.y != alt_object2.y

    def test_wait_for_object_with_camera_id(self):
        alt_button = self.alt_driver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.alt_driver.find_object(By.PATH, "//Camera")
        alt_object = self.alt_driver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        assert alt_object.name == "Capsule"

        camera2 = self.alt_driver.find_object(By.PATH, "//Main Camera")
        alt_object2 = self.alt_driver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))

        assert alt_object.x != alt_object2.x
        assert alt_object.y != alt_object2.y

    def test_find_objects_with_camera_id(self):
        alt_button = self.alt_driver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        camera = self.alt_driver.find_object(By.PATH, "//Camera")
        alt_objects = self.alt_driver.find_objects(
            By.NAME, "Plane", By.ID, str(camera.id))
        assert alt_objects[0].name == "Plane"

        camera2 = self.alt_driver.find_object(By.PATH, "//Main Camera")
        alt_objects2 = self.alt_driver.find_objects(
            By.NAME, "Plane", By.ID, str(camera2.id))

        assert alt_objects[0].x != alt_objects2[0].x
        assert alt_objects[0].y != alt_objects2[0].y

    def test_wait_for_object_not_be_present_with_camera_id(self):
        camera = self.alt_driver.find_object(By.PATH, "//Main Camera")
        self.alt_driver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs",
            By.ID, str(camera.id)
        )

        elements = self.alt_driver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_with_text_with_camera_id(self):
        name = "CapsuleInfo"
        text = self.alt_driver.find_object(By.NAME, name).get_text()
        camera = self.alt_driver.find_object(By.PATH, "//Main Camera")

        alt_object = self.alt_driver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.ID, str(camera.id),
            timeout=1
        )

        assert alt_object is not None
        assert alt_object.get_text() == text

    def test_wait_for_object_which_contains_with_camera_id(self):
        camera = self.alt_driver.find_object(By.PATH, "//Main Camera")
        alt_object = self.alt_driver.wait_for_object_which_contains(
            By.NAME, "Canva",
            By.ID, str(camera.id)
        )
        assert alt_object.name == "Canvas"

    def test_find_object_with_tag(self):
        alt_button = self.alt_driver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_object = self.alt_driver.find_object(
            By.COMPONENT, "CapsuleCollider",  By.TAG, "MainCamera")
        assert alt_object.name == "Capsule"

        alt_object2 = self.alt_driver.find_object(
            By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged")
        assert alt_object.x != alt_object2.x
        assert alt_object.y != alt_object2.y

    def test_wait_for_object_with_tag(self):
        alt_button = self.alt_driver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_object = self.alt_driver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera")
        assert alt_object.name == "Capsule"

        alt_object2 = self.alt_driver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",  By.TAG, "Untagged")
        assert alt_object.x != alt_object2.x
        assert alt_object.y != alt_object2.y

    def test_find_objects_with_tag(self):
        alt_button = self.alt_driver.find_object(By.PATH, "//Button")
        alt_button.tap()
        alt_button.tap()

        alt_object = self.alt_driver.find_objects(
            By.NAME, "Plane", By.TAG, "MainCamera")
        assert alt_object[0].name == "Plane"

        alt_object2 = self.alt_driver.find_objects(
            By.NAME, "Plane", By.TAG, "Untagged")
        assert alt_object[0].x != alt_object2[0].x
        assert alt_object[0].y != alt_object2[0].y

    def test_wait_for_object_not_be_present_with_tag(self):
        self.alt_driver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs", By.TAG, "MainCamera")

        elements = self.alt_driver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_with_text_with_tag(self):
        name = "CapsuleInfo"
        text = self.alt_driver.find_object(By.NAME, name).get_text()

        alt_object = self.alt_driver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.TAG, "MainCamera",
            timeout=1
        )
        assert alt_object is not None
        assert alt_object.get_text() == text

    def test_find_object_by_camera(self):
        button = self.alt_driver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_object = self.alt_driver.find_object(
            By.COMPONENT, "CapsuleCollider",
            camera_value="Camera"
        )

        assert alt_object.name == "Capsule"
        alt_object2 = self.alt_driver.find_object(
            By.COMPONENT, "CapsuleCollider",
            camera_by=By.NAME,
            camera_value="Main Camera"
        )

        assert alt_object.x != alt_object2.x
        assert alt_object.y != alt_object2.y

    def test_wait_for_object_by_camera(self):
        button = self.alt_driver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_object = self.alt_driver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",
            camera_value="Camera"
        )
        assert alt_object.name == "Capsule"

        alt_object2 = self.alt_driver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",
            camera_by=By.NAME, camera_value="Main Camera"
        )
        assert alt_object.x != alt_object2.x
        assert alt_object.y != alt_object2.y

    def test_find_objects_by_camera(self):
        button = self.alt_driver.find_object(By.PATH, "//Button")
        button.tap()
        button.tap()

        alt_object = self.alt_driver.find_objects(
            By.NAME, "Plane", By.NAME, "Camera")
        assert alt_object[0].name == "Plane"

        alt_object2 = self.alt_driver.find_objects(
            By.NAME, "Plane", By.NAME, "Main Camera")
        assert alt_object[0].x != alt_object2[0].x
        assert alt_object[0].y != alt_object2[0].y

    def test_wait_for_object_not_be_present_by_camera(self):
        self.alt_driver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs",
            By.NAME, "Main Camera"
        )

        elements = self.alt_driver.get_all_elements()
        names = [element.name for element in elements]
        assert "ObjectDestroyedIn5Secs" not in names

    def test_wait_for_object_by_camera_2(self):
        name = "CapsuleInfo"
        text = self.alt_driver.find_object(By.NAME, name).get_text()

        alt_object = self.alt_driver.wait_for_object(
            By.PATH, "//{}[@text={}]".format(name, text),
            By.NAME, "Main Camera",
            timeout=1
        )

        assert alt_object is not None
        assert alt_object.get_text() == text

    def test_wait_for_object_which_contains_by_camera(self):
        alt_object = self.alt_driver.wait_for_object_which_contains(
            By.NAME, "Canva", By.NAME, "Main Camera")
        assert alt_object.name == "Canvas"

    def test_get_component_property_complex_class(self):
        component_name = "AltExampleScriptCapsule"
        property_name = "AltSampleClass.testInt"

        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        assert alt_object is not None

        property_value = alt_object.get_component_property(
            component_name, property_name, "Assembly-CSharp",
            max_depth=1
        )
        assert property_value == 1

    def test_get_component_property_complex_class2(self):
        component_name = "AltExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        assert alt_object is not None

        property_value = alt_object.get_component_property(
            component_name, property_name, "Assembly-CSharp", max_depth=1)
        assert property_value == "test2"

    def test_set_component_property_complex_class(self):
        component_name = "AltExampleScriptCapsule"
        property_name = "AltSampleClass.testInt"

        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        assert alt_object is not None

        alt_object.set_component_property(
            component_name, property_name, "Assembly-CSharp", 2)
        property_value = alt_object.get_component_property(
            component_name, property_name, "Assembly-CSharp", max_depth=1)
        assert property_value == 2

    def test_set_component_property_complex_class2(self):
        component_name = "AltExampleScriptCapsule"
        property_name = "listOfSampleClass[1].testString"
        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        assert alt_object is not None

        alt_object.set_component_property(
            component_name, property_name, "Assembly-CSharp", "test3")
        propertyValue = alt_object.get_component_property(
            component_name, property_name, "Assembly-CSharp", max_depth=1)
        assert propertyValue == "test3"

    def test_get_parent(self):
        element = self.alt_driver.find_object(By.NAME, "Canvas/CapsuleInfo")
        element_parent = element.get_parent()
        assert element_parent.name == "Canvas"

    def test_tap_coordinates(self):
        capsule_element = self.alt_driver.find_object(By.NAME, "Capsule")
        self.alt_driver.tap(capsule_element.get_screen_position())
        self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_click_coordinates(self):
        capsule_element = self.alt_driver.find_object(By.NAME, "Capsule")
        self.alt_driver.click(capsule_element.get_screen_position())
        self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_tap_element(self):
        capsule_element = self.alt_driver.find_object(By.NAME, "Capsule")
        capsule_element.tap(1)
        self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_click_element(self):
        capsule_element = self.alt_driver.find_object(By.NAME, "Capsule")
        capsule_element.click()
        self.alt_driver.wait_for_object(
            By.PATH, "//CapsuleInfo[@text=Capsule was clicked to jump!]", timeout=1)

    def test_key_down_and_key_up_mouse0(self):
        button = self.alt_driver.find_object(By.NAME, "UIButton")
        self.alt_driver.move_mouse(
            button.get_screen_position(), duration=0.1, wait=True)

        self.alt_driver.key_down(AltKeyCode.Mouse0)
        self.alt_driver.key_up(AltKeyCode.Mouse0)
        text = self.alt_driver.find_object(
            By.NAME, "ChineseLetters").get_text()
        assert text != "????"

    def test_camera_not_found_exception(self):
        with pytest.raises(exceptions.CameraNotFoundException):
            self.alt_driver.find_object(By.NAME, "Capsule", By.NAME, "Camera")

    def test_input_field_events(self):
        input_field = self.alt_driver.find_object(
            By.NAME, "UnityUIInputField").set_text("example", submit=True)

        assert input_field.get_text() == "example"
        assert input_field.get_component_property(
            "AltInputFieldRaisedEvents", "onValueChangedInvoked", "Assembly-CSharp")
        assert input_field.get_component_property(
            "AltInputFieldRaisedEvents", "onSubmitInvoked", "Assembly-CSharp")

    @pytest.mark.WebGLUnsupported
    @pytest.mark.iOSUnsupported
    def test_get_static_property(self):

        self.alt_driver.call_static_method(
            "UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule",
            parameters=["1920", "1080", "True"],
            type_of_parameters=["System.Int32",
                                "System.Int32", "System.Boolean"]
        )
        width = self.alt_driver.get_static_property(
            "UnityEngine.Screen", "currentResolution.width",
            "UnityEngine.CoreModule"
        )

        assert int(width) == 1920

    def test_set_static_property(self):
        expectedValue = 5
        self.alt_driver.set_static_property(
            "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue)
        value = self.alt_driver.get_static_property(
            "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp")
        assert expectedValue == value

    def test_set_static_property2(self):
        newValue = 5
        expectedArray = [1, 5, 3]
        self.alt_driver.set_static_property(
            "AltExampleScriptCapsule", "staticArrayOfInts[1]", "Assembly-CSharp", newValue)
        value = self.alt_driver.get_static_property(
            "AltExampleScriptCapsule", "staticArrayOfInts", "Assembly-CSharp")
        assert expectedArray == value

    def test_get_static_property_instance_null(self):

        screen_width = self.alt_driver.call_static_method(
            "UnityEngine.Screen", "get_width",
            "UnityEngine.CoreModule"
        )
        width = self.alt_driver.get_static_property(
            "UnityEngine.Screen", "width",
            "UnityEngine.CoreModule"
        )

        assert int(width) == screen_width

    def test_float_world_coordinates(self):
        plane = self.alt_driver.find_object(By.NAME, "Plane")

        assert type(plane.worldX) == float
        assert type(plane.worldY) == float
        assert type(plane.worldZ) == float

    def test_set_command_response_timeout(self):
        self.alt_driver.set_command_response_timeout(1)
        with pytest.raises(exceptions.CommandResponseTimeoutException) as execinfo:
            self.alt_driver.tilt([1, 1, 1], duration=2, wait=True)
        self.alt_driver.set_command_response_timeout(60)
        assert str(execinfo.value) == ""

    def test_keys_down(self):
        keys = [AltKeyCode.K, AltKeyCode.L]
        self.alt_driver.keys_down(keys)
        self.alt_driver.keys_up(keys)

        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        property_value = alt_object.get_component_property(
            "AltExampleScriptCapsule",
            "stringToSetFromTests",
            "Assembly-CSharp"
        )
        assert property_value == "multiple keys pressed"

    def test_press_keys(self):
        keys = [AltKeyCode.K, AltKeyCode.L]
        self.alt_driver.press_keys(keys)

        alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
        property_value = alt_object.get_component_property(
            "AltExampleScriptCapsule",
            "stringToSetFromTests",
            "Assembly-CSharp"
        )
        assert property_value == "multiple keys pressed"

    def test_find_object_by_coordinates(self):
        counter_button = self.alt_driver.find_object(By.NAME, "ButtonCounter")

        element = self.alt_driver.find_object_at_coordinates(
            [80 + counter_button.x, 15 + counter_button.y])
        assert element.name == "ButtonCounter"

    def test_find_object_by_coordinates_no_element(self):
        element = self.alt_driver.find_object_at_coordinates([-1, -1])
        assert element is None

    def test_call_private_method(self):
        capsule_element = self.alt_driver.find_object(By.NAME, "Capsule")
        capsule_element.call_component_method("AltExampleScriptCapsule",
                                              "callJump", "Assembly-CSharp", parameters=[])
        capsule_info = self.alt_driver.find_object(By.NAME, "CapsuleInfo")
        assert capsule_info.get_text() == "Capsule jumps!"

    def test_reset_input(self):
        self.alt_driver.key_down(AltKeyCode.P, 1)
        assert self.alt_driver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
            "AltTester.AltTesterUnitySDK.InputModule.NewInputSystem",
            "Keyboard.pKey.isPressed", "AltTester.AltTesterUnitySDK.InputModule") is True
        self.alt_driver.reset_input()
        assert self.alt_driver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
            "AltTester.AltTesterUnitySDK.InputModule.NewInputSystem",
            "Keyboard.pKey.isPressed", "AltTester.AltTesterUnitySDK.InputModule") is False

        countKeyDown = self.alt_driver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
            "Input", "_keyCodesPressed.Count", "AltTester.AltTesterUnitySDK.InputModule")
        assert 0 == countKeyDown
