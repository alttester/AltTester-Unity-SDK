# -*- coding: UTF-8
import os
import unittest
import sys
import time
from os import path
import json

from altunityrunner import *
from altunityrunner.logging import AltUnityLogLevel, AltUnityLogger
from altunityrunner.alt_unity_key_code import AltUnityKeyCode


def PATH(p): return os.path.abspath(
    os.path.join(os.path.dirname(__file__), p)
)


class PythonTests(unittest.TestCase):
    altdriver: AltUnityDriver = None
    platform = "android"  # set to `ios` or `android` to change platform

    @classmethod
    def setUpClass(cls):
        cls.altdriver = AltUnityDriver(log_flag=False, timeout=3)

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()

    def test_tap_ui_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_object(By.NAME, 'UIButton').tap()
        capsule_info = self.altdriver.wait_for_object_with_text(
            By.NAME, 'CapsuleInfo', 'UIButton clicked to jump capsule!', timeout=1)
        self.assertEqual('UIButton clicked to jump capsule!',
                         capsule_info.get_text())

    def test_tap_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule_element = self.altdriver.find_object(By.NAME, 'Capsule')
        capsule_element.tap()
        capsule_info = self.altdriver.wait_for_object_with_text(
            By.NAME, 'CapsuleInfo', 'Capsule was clicked to jump!', timeout=1)
        self.assertEqual('Capsule was clicked to jump!',
                         capsule_info.get_text())

    def test_tap_at_coordinates(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        ui_button = self.altdriver.find_object(By.NAME, 'UIButton')
        self.altdriver.tap_at_coordinates(ui_button.x, ui_button.y)
        capsule_info = self.altdriver.wait_for_object_with_text(
            By.NAME, 'CapsuleInfo', 'UIButton clicked to jump capsule!', '', timeout=1)
        self.assertEqual('UIButton clicked to jump capsule!',
                         capsule_info.get_text())

    def test_load_and_wait_for_scene(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.wait_for_current_scene_to_be(
            'Scene 1 AltUnityDriverTestScene', 1)
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        self.altdriver.wait_for_current_scene_to_be(
            'Scene 2 Draggable Panel', 1)
        self.assertEqual('Scene 2 Draggable Panel',
                         self.altdriver.get_current_scene())

    def test_resize_panel(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        altElement = self.altdriver.find_object(By.NAME, 'Resize Zone')
        positionInitX = altElement.x
        positionInitY = altElement.y
        self.altdriver.swipe_and_wait(altElement.x, altElement.y, int(
            altElement.x) - 200, int(altElement.y) - 200, 2)

        time.sleep(2)

        altElement = self.altdriver.find_object(By.NAME, 'Resize Zone')
        positionFinalX = altElement.x
        positionFinalY = altElement.y
        self.assertNotEqual(positionInitX, positionFinalX)
        self.assertNotEqual(positionInitY, positionFinalY)

    def test_resize_panel_with_multipoinit_swipe(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        altElement = self.altdriver.find_object(By.NAME, 'Resize Zone')
        positionInitX = altElement.x
        positionInitY = altElement.y
        positions = [
            altElement.get_screen_position(),
            [int(altElement.x) - 200, int(altElement.y) - 200],
            [int(altElement.x) - 300, int(altElement.y) - 100],
            [int(altElement.x) - 50, int(altElement.y) - 100],
            [int(altElement.x) - 100, int(altElement.y) - 100]
        ]
        self.altdriver.multipoint_swipe_and_wait(positions, 4)

        time.sleep(4)

        altElement = self.altdriver.find_object(By.NAME, 'Resize Zone')
        positionFinalX = altElement.x
        positionFinalY = altElement.y
        self.assertNotEqual(positionInitX, positionFinalX)
        self.assertNotEqual(positionInitY, positionFinalY)

    def test_find_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        plane = self.altdriver.find_object(By.NAME, 'Plane')
        capsule = self.altdriver.find_object(By.NAME, 'Capsule')
        self.assertEqual('Plane', plane.name)
        self.assertEqual('Capsule', capsule.name)

    def test_find_object_by_text(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        text = self.altdriver.find_object(By.NAME, 'CapsuleInfo').get_text()
        element = self.altdriver.find_object(By.TEXT, text)

        self.assertEqual(element.get_text(), text)

    def test_wait_for_object_with_text(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        text_to_wait_for = self.altdriver.find_object(By.NAME,
                                                      'CapsuleInfo').get_text()
        capsule_info = self.altdriver.wait_for_object_with_text(
            By.NAME, 'CapsuleInfo', text_to_wait_for, timeout=1)
        self.assertEqual('CapsuleInfo', capsule_info.name)
        self.assertEqual(text_to_wait_for, capsule_info.get_text())

    def test_find_objects(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        planes = self.altdriver.find_objects(By.NAME, "Plane")
        assert len(planes) == 2
        assert len(self.altdriver.find_objects(
            By.NAME, "something that does not exist")) == 0

    def test_find_object_which_contains_2(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        plane = self.altdriver.find_object_which_contains(By.NAME, 'Pla')
        self.assertTrue('Pla' in plane.name)

    # Fix in issue 184
    # def test_find_disabled_element_where_name_contains(self):
    #     self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
    #     self.altdriver.find_element_where_name_contains('Cube',enabled=False)

    def test_find_object_by_name_and_parent(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule_element = self.altdriver.find_object(
            By.NAME, 'Canvas/CapsuleInfo')
        assert capsule_element.name == 'CapsuleInfo'

    def test_find_objects_by_component(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.assertEqual(
            len(self.altdriver.find_objects(By.COMPONENT, "UnityEngine.MeshFilter")), 3)

    def test_get_component_property(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_object(By.NAME,
                                            "Capsule").get_component_property("AltUnityExampleScriptCapsule", "arrayOfInts")
        self.assertEqual(result, "[1,2,3]")

    def test_set_component_property(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_object(By.NAME, "Capsule").set_component_property(
            "AltUnityExampleScriptCapsule", "arrayOfInts", "[2,3,4]")
        result = self.altdriver.find_object(By.NAME,
                                            "Capsule").get_component_property("AltUnityExampleScriptCapsule", "arrayOfInts")
        self.assertEqual(result, "[2,3,4]")

    def test_call_component_method(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_object(By.NAME, "Capsule").call_component_method(
            "AltUnityExampleScriptCapsule", "Jump", "setFromMethod")
        self.assertEqual(result, "null")
        self.altdriver.wait_for_object_with_text(
            By.NAME, 'CapsuleInfo', 'setFromMethod')
        self.assertEqual('setFromMethod', self.altdriver.find_object(By.NAME,
                                                                     'CapsuleInfo').get_text())

    def test_call_component_method_invalid_parameter_type(self):
        try:
            self.altdriver.find_object(By.NAME, "Capsule").call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                "1?stringparam?0.5?[1,2,3]", "", "System.Stringgg")
            self.fail()
        except InvalidParameterTypeException as e:
            self.assertTrue(str(e).startswith(
                "error:invalidParameterType"), str(e))

    def test_call_component_method_assembly_not_found(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        try:
            self.altdriver.find_object(By.NAME, "Capsule").call_component_method(
                "RandomComponent", "TestMethodWithManyParameters",
                "1?stringparam?0.5?[1,2,3]", "RandomAssembly", "")
            self.fail()
        except AssemblyNotFoundException as e:
            self.assertTrue(str(e).startswith(
                "error:assemblyNotFound"), str(e))

    def test_call_component_method_incorrect_number_of_parameters(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        alt_element = self.altdriver.find_object(By.NAME, "Capsule")
        try:
            alt_element.call_component_method(
                "AltUnityExampleScriptCapsule", "TestMethodWithManyParameters",
                "stringparam?0.5?[1,2,3]", "", "")
            self.fail()
        except MethodWithGivenParametersNotFoundException as e:
            self.assertTrue(str(e).startswith(
                "error:methodWithGivenParametersNotFound"), str(e))

    def test_pointer_enter_and_exit(self):
        self.altdriver.load_scene('Scene 3 Drag And Drop')

        alt_element = self.altdriver.find_object(By.NAME, 'Drop Image')
        color1 = alt_element.get_component_property(
            'AltUnityExampleScriptDropMe', 'highlightColor')
        alt_element.pointer_enter()
        color2 = alt_element.get_component_property(
            'AltUnityExampleScriptDropMe', 'highlightColor')
        self.assertNotEqual(color1, color2)

        alt_element.pointer_exit()
        color3 = alt_element.get_component_property(
            'AltUnityExampleScriptDropMe', 'highlightColor')
        self.assertNotEqual(color3, color2)
        self.assertEqual(color3, color1)

    def test_multiple_swipes(self):
        self.altdriver.load_scene('Scene 3 Drag And Drop')

        image1 = self.altdriver.find_object(By.NAME, 'Drag Image1')
        box1 = self.altdriver.find_object(By.NAME, 'Drop Box1')

        self.altdriver.swipe(image1.x, image1.y, box1.x, box1.y, 5)

        image2 = self.altdriver.find_object(By.NAME, 'Drag Image2')
        box2 = self.altdriver.find_object(By.NAME, 'Drop Box2')

        self.altdriver.swipe(image2.x, image2.y, box2.x, box2.y, 2)

        image3 = self.altdriver.find_object(By.NAME, 'Drag Image3')
        box1 = self.altdriver.find_object(By.NAME, 'Drop Box1')

        self.altdriver.swipe(image3.x, image3.y, box1.x, box1.y, 3)

        time.sleep(6)

        image_source = image1.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_object(By.NAME,
                                                            'Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

        image_source = image2.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_object(By.NAME,
                                                            'Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

    def test_multiple_swipe_and_waits(self):
        self.altdriver.load_scene('Scene 3 Drag And Drop')

        image2 = self.altdriver.find_object(By.NAME, 'Drag Image2')
        box2 = self.altdriver.find_object(By.NAME, 'Drop Box2')

        self.altdriver.swipe_and_wait(image2.x, image2.y, box2.x, box2.y, 2)

        image3 = self.altdriver.find_object(By.NAME, 'Drag Image3')
        box1 = self.altdriver.find_object(By.NAME, 'Drop Box1')

        self.altdriver.swipe_and_wait(image3.x, image3.y, box1.x, box1.y, 1)

        image1 = self.altdriver.find_object(By.NAME, 'Drag Image1')
        box1 = self.altdriver.find_object(By.NAME, 'Drop Box1')

        self.altdriver.swipe_and_wait(image1.x, image1.y, box1.x, box1.y, 3)

        image_source = image1.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_object(By.NAME,
                                                            'Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

        image_source = image2.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_object(By.NAME,
                                                            'Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

    def test_button_click_and_wait_with_swipe(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        button = self.altdriver.find_object(By.NAME, 'UIButton')
        self.altdriver.hold_button_and_wait(button.x, button.y, 1)
        capsule_info = self.altdriver.find_object(By.NAME, 'CapsuleInfo')
        time.sleep(1.4)
        text = capsule_info.get_text()
        self.assertEqual(text, "UIButton clicked to jump capsule!")

    def test_button_click_with_swipe(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        button = self.altdriver.find_object(By.NAME, 'UIButton')
        self.altdriver.hold_button(button.x, button.y, 1)
        time.sleep(2)
        capsule_info = self.altdriver.find_object(By.NAME, 'CapsuleInfo')
        text = capsule_info.get_text()
        self.assertEqual(text, "UIButton clicked to jump capsule!")

    def test_multiple_swipe_and_waits_with_multipoint_swipe(self):
        self.altdriver.load_scene('Scene 3 Drag And Drop')
        altElement1 = self.altdriver.find_object(By.NAME, 'Drag Image1')
        altElement2 = self.altdriver.find_object(By.NAME, 'Drop Box1')

        multipointPositions = [altElement1.get_screen_position(), [
            altElement2.x, altElement2.y]]

        self.altdriver.multipoint_swipe_and_wait(multipointPositions, 2)
        time.sleep(2)

        altElement1 = self.altdriver.find_object(By.NAME, 'Drag Image1')
        altElement2 = self.altdriver.find_object(By.NAME, 'Drop Box1')
        altElement3 = self.altdriver.find_object(By.NAME, 'Drop Box2')

        positions = [
            [altElement1.x, altElement1.y],
            [altElement2.x, altElement2.y],
            [altElement3.x, altElement3.y]
        ]

        self.altdriver.multipoint_swipe_and_wait(positions, 3)
        imageSource = self.altdriver.find_object(By.NAME,
                                                 'Drag Image1').get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_object(By.NAME,
                                                         'Drop Image').get_component_property("UnityEngine.UI.Image", "sprite")
        self.assertNotEqual(imageSource, imageSourceDropZone)

        imageSource = self.altdriver.find_object(By.NAME,
                                                 'Drag Image2').get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_object(By.NAME,
                                                         'Drop').get_component_property("UnityEngine.UI.Image", "sprite")
        self.assertNotEqual(imageSource, imageSourceDropZone)

    def test_set_player_pref_keys_int(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.delete_player_prefs()
        self.altdriver.set_player_pref_key('test', 1, PlayerPrefKeyType.Int)
        value = self.altdriver.get_player_pref_key(
            'test', PlayerPrefKeyType.Int)
        self.assertEqual(int(value), 1)

    def test_set_player_pref_keys_float(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.delete_player_prefs()
        self.altdriver.set_player_pref_key(
            'test', 1.3, PlayerPrefKeyType.Float)
        value = self.altdriver.get_player_pref_key(
            'test', PlayerPrefKeyType.Float)
        self.assertEqual(float(value), 1.3)

    def test_set_player_pref_keys_string(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.delete_player_prefs()
        self.altdriver.set_player_pref_key(
            'test', 'string value', PlayerPrefKeyType.String)
        value = self.altdriver.get_player_pref_key(
            'test', PlayerPrefKeyType.String)
        self.assertEqual(value, 'string value')

    def test_wait_for_non_existing_object(self):
        try:
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            self.altdriver.wait_for_object(
                By.NAME, "dlkasldkas", timeout=1)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], "Element dlkasldkas not found after 1 seconds")

    def test_wait_for_object_to_not_exist_fail(self):
        try:
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            self.altdriver.wait_for_object_to_not_be_present(
                By.NAME, "Capsule", timeout=1)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Element Capsule still found after 1 seconds')

    def test_wait_for_object_with_text_wrong_text(self):
        try:
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            self.altdriver.wait_for_object_with_text(
                By.NAME, "CapsuleInfo", "aaaaa", '', timeout=1)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Element CapsuleInfo should have text `aaaaa` but has `Capsule Info` after 1 seconds')

    def test_wait_for_current_scene_to_be_a_non_existing_scene(self):
        try:
            self.altdriver.wait_for_current_scene_to_be(
                "AltUnityDriverTestScenee", 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Scene AltUnityDriverTestScenee not loaded after 1 seconds')

    def test_get_bool(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        alt_element = self.altdriver.find_object(By.NAME, 'Capsule')
        text = alt_element.get_component_property(
            'AltUnityExampleScriptCapsule', 'TestBool')
        self.assertEqual('true', text)

    def test_call_static_method(self):
        self.altdriver.call_static_method(
            "UnityEngine.PlayerPrefs", "SetInt", "Test?1", assembly="UnityEngine.CoreModule")
        a = int(self.altdriver.call_static_method(
            "UnityEngine.PlayerPrefs", "GetInt", "Test?2", assembly="UnityEngine.CoreModule"))
        self.assertEqual(1, a)

    def test_call_method_with_multiple_definitions(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        capsule.call_component_method(
            "AltUnityExampleScriptCapsule", "Test", "2", type_of_parameters="System.Int32")
        capsuleInfo = self.altdriver.find_object(By.NAME, "CapsuleInfo")
        self.assertEqual("6", capsuleInfo.get_text())

    def test_tap_on_screen_where_there_are_no_objects(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        counterButton = self.altdriver.find_object(By.NAME, "ButtonCounter")
        alt_element = self.altdriver.tap_at_coordinates(
            1, float(counterButton.y)+100)
        self.assertIsNone(alt_element)

    def test_set_and_get_time_scale(self):
        self.altdriver.set_time_scale(0.1)
        time.sleep(1)
        time_scale = self.altdriver.get_time_scale()
        self.assertEqual(0.1, time_scale)
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

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_camera_movement(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        cube = self.altdriver.find_object(By.NAME, "Player1")
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldY)

        self.altdriver.press_key_with_keycode(AltUnityKeyCode.W, 1, 2)
        time.sleep(2)
        cube = self.altdriver.find_object(By.NAME, "Player1")
        cubeFinalPosition = (cube.worldX, cube.worldY, cube.worldY)

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_creating_stars(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        stars = self.altdriver.find_objects_which_contain(
            By.NAME, "Star", By.NAME, "Player2")
        self.assertEqual(1, len(stars))
        self.altdriver.find_objects_which_contain(
            By.NAME, "Player", By.NAME, "Player2")
        pressing_point_1 = self.altdriver.find_object(
            By.NAME, "PressingPoint1", By.NAME, "Player2")

        self.altdriver.move_mouse(
            int(pressing_point_1.x), int(pressing_point_1.y), 1)
        time.sleep(1.5)

        self.altdriver.press_key_with_keycode(AltUnityKeyCode.Mouse0, 1, 1)
        pressing_point_2 = self.altdriver.find_object(
            By.NAME, "PressingPoint2", By.NAME, "Player2")
        self.altdriver.move_mouse_and_wait(
            int(pressing_point_1.x), int(pressing_point_2.y), 1)
        self.altdriver.press_key_with_keycode(AltUnityKeyCode.Mouse0, 1, 1)
        time.sleep(2)

        stars = self.altdriver.find_objects_which_contain(By.NAME, "Star")
        self.assertEqual(3, len(stars))

    def test_find_object_by_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object(By.TAG, "plane")
        self.assertTrue(altElement.name == "Plane")

    def test_find_object_by_layer(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object(By.LAYER, "Water")
        self.assertTrue(altElement.name == "Capsule")

    def test_find_object_by_unity_component(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider")
        self.assertTrue(altElement.name == "Capsule")

    def test_find_object_by_component(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object(
            By.COMPONENT, "AltUnityExampleScriptCapsule")
        self.assertTrue(altElement.name == "Capsule")

    def test_find_object_by_component_with_namespace(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object(
            By.COMPONENT, "AltUnityTesterExamples.Scripts.AltUnityExampleScriptCapsule")
        self.assertTrue(altElement.name == "Capsule")

    def test_find_child(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object(By.PATH, "//UIButton/*")
        self.assertTrue(altElement.name == "Text")

    def test_find_objects_by_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElements = self.altdriver.find_objects(By.TAG, "plane")
        self.assertEqual(2, len(altElements))
        for altElement in altElements:
            self.assertEqual("Plane", altElement.name)

    def test_find_objects_by_layer(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        time.sleep(1)
        altElements = self.altdriver.find_objects(By.LAYER, "Default")
        self.assertEqual(12, len(altElements))

    def test_find_objects_by_contains_name(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        time.sleep(1)
        altElements = self.altdriver.find_objects_which_contain(
            By.NAME, "Capsule")
        self.assertEqual(2, len(altElements), altElements)
        for altElement in altElements:
            self.assertTrue("Capsule" in altElement.name)

    def test_power_joystick(self):
        button_names = ['Horizontal', 'Vertical']
        keys_to_press = [AltUnityKeyCode.D, AltUnityKeyCode.W]
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        axisName = self.altdriver.find_object(By.NAME, "AxisName")
        axisValue = self.altdriver.find_object(By.NAME, "AxisValue")
        i = 0
        for key in keys_to_press:
            self.altdriver.press_key_with_keycode_and_wait(key, 0.5, 0.1)
            self.assertEqual('0.5', axisValue.get_text())
            self.assertEqual(button_names[i], axisName.get_text())
            i = i+1

    def test_call_method_with_assembly(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initialRotation = capsule.get_component_property(
            "UnityEngine.Transform", "rotation")
        capsule.call_component_method(
            "UnityEngine.Transform", "Rotate", "10?10?10",
            "UnityEngine.CoreModule", "System.Single?System.Single?System.Single")
        capsuleAfterRotation = self.altdriver.find_object(By.NAME, "Capsule")
        finalRotation = capsuleAfterRotation.get_component_property(
            "UnityEngine.Transform", "rotation")
        self.assertNotEqual(initialRotation, finalRotation)

    def test_get_all_enabled_elements(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        time.sleep(1)
        alt_elements = self.altdriver.get_all_elements(enabled=True)
        self.assertIsNotNone(alt_elements)

        list_of_elements = []
        for element in alt_elements:
            list_of_elements.append(element.name)

        self.assertEqual(24, len(list_of_elements), list_of_elements)
        self.assertTrue("EventSystem" in list_of_elements)
        self.assertTrue("Canvas" in list_of_elements)
        self.assertTrue("Panel Drag Area" in list_of_elements)
        self.assertTrue("Panel" in list_of_elements)
        self.assertTrue("Header" in list_of_elements)
        self.assertTrue("Text" in list_of_elements)
        self.assertTrue("Drag Zone" in list_of_elements)
        self.assertTrue("Resize Zone" in list_of_elements)
        self.assertTrue("Close Button" in list_of_elements)
        self.assertTrue("Debugging" in list_of_elements)
        self.assertTrue("SF Scene Elements" in list_of_elements)
        self.assertTrue("Main Camera" in list_of_elements)
        self.assertTrue("Background" in list_of_elements)
        self.assertTrue("Particle System" in list_of_elements)

    def test_get_all_elements(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        time.sleep(1)
        alt_elements = self.altdriver.get_all_elements(enabled=False)
        self.assertIsNotNone(alt_elements)

        list_of_elements = []
        for element in alt_elements:
            list_of_elements.append(element.name)

        self.assertEqual(29, len(list_of_elements))
        self.assertTrue("EventSystem" in list_of_elements)
        self.assertTrue("Canvas" in list_of_elements)
        self.assertTrue("Panel Drag Area" in list_of_elements)
        self.assertTrue("Panel" in list_of_elements)
        self.assertTrue("Header" in list_of_elements)
        self.assertTrue("Text" in list_of_elements)
        self.assertTrue("Drag Zone" in list_of_elements)
        self.assertTrue("Resize Zone" in list_of_elements)
        self.assertTrue("Close Button" in list_of_elements)
        self.assertTrue("Debugging" in list_of_elements)
        self.assertTrue("SF Scene Elements" in list_of_elements)
        self.assertTrue("Main Camera" in list_of_elements)
        self.assertTrue("Background" in list_of_elements)
        self.assertTrue("Particle System" in list_of_elements)
        self.assertTrue("PopUp" in list_of_elements)

    def test_find_object_which_contains(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object_which_contains(
            By.NAME, "Event")
        self.assertEqual("EventSystem", altElement.name)

    def test_find_with_find_object_which_contains_not_existing_object(self):
        try:
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            self.altdriver.find_object_which_contains(
                By.NAME, "EventNonExisting")
            self.assertEqual(False, True)
        except NotFoundException as e:
            self.assertEqual(e.args[0], "error:notFound")

    def test_screenshot(self):
        png_path = "testPython.png"
        self.altdriver.get_png_screenshot(png_path)
        self.assertTrue(path.exists(png_path))

    def test_wait_for_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.wait_for_object(By.NAME, "Capsule")
        self.assertEqual(altElement.name, "Capsule")

    def test_wait_for_object_to_not_be_present(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.wait_for_object_to_not_be_present(By.NAME, "Capsuule")

    def test_wait_for_object_which_contains(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Main")
        self.assertEqual(altElement.name, "Main Camera")

    def test_get_chinese_letters(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        text = self.altdriver.find_object(By.NAME, "ChineseLetters").get_text()
        self.assertEqual("哦伊娜哦", text)

    def test_non_english_text(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        text = self.altdriver.find_object(By.NAME, "NonEnglishText").get_text()
        self.assertEqual("BJÖRN'S PASS", text)

    def test_find_objects_fail(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        plane = self.altdriver.wait_for_object(By.NAME, 'Plane')
        capsule = self.altdriver.wait_for_object(By.NAME, 'Capsule')
        self.assertEqual('Plane', plane.name)
        self.assertEqual('Capsule', capsule.name)

    def test_double_tap(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        counterButton = self.altdriver.find_object(By.NAME, "ButtonCounter")
        counterButtonText = self.altdriver.find_object(
            By.NAME, "ButtonCounter/Text")
        counterButton.double_tap()
        time.sleep(0.5)
        self.assertEqual("2", counterButtonText.get_text())

    def test_custom_tap(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        counterButton = self.altdriver.find_object(By.NAME, "ButtonCounter")
        counterButtonText = self.altdriver.find_object(
            By.NAME, "ButtonCounter/Text")
        self.altdriver.tap_custom(counterButton.x, counterButton.y, 4)
        time.sleep(1)
        self.assertEqual("4", counterButtonText.get_text())

    # def test_drag_object(self):
    #     self.altdriver.load_scene('Scene 2 Draggable Panel')
    #     time.sleep(1)
    #     d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
    #     p_initial = (d_panel.x, d_panel.y, d_panel.z)
    #     d_panel.drag(200, 200)
    #     time.sleep(1)
    #     d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
    #     p_final = (d_panel.x, d_panel.y, d_panel.z)
    #     self.assertNotEqual(p_initial, p_final)

    # def test_drop_object(self):
    #     self.altdriver.load_scene('Scene 2 Draggable Panel')
    #     time.sleep(1)
    #     d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
    #     p_initial = (d_panel.x, d_panel.y, d_panel.z)

    #     d_panel.drag(100, 200)
    #     time.sleep(1)
    #     d_panel.drop(100, 200)
    #     time.sleep(1)
    #     d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
    #     p_final = (d_panel.x, d_panel.y, d_panel.z)
    #     self.assertNotEqual(p_initial, p_final)

    def test_set_text_normal_text(self):
        text_object = self.altdriver.find_object(By.NAME, "NonEnglishText")
        original_text = text_object.get_text()
        after_text = text_object.set_text("ModifiedText").get_text()
        self.assertNotEqual(original_text, after_text)

    def test_press_next_scene(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        initial_scene = self.altdriver.get_current_scene()
        self.altdriver.find_object(By.NAME, "NextScene").tap()
        current_scene = self.altdriver.get_current_scene()
        self.assertNotEqual(initial_scene, current_scene)

    def test_find_parent_using_path(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        parent = self.altdriver.find_object(By.PATH, "//CapsuleInfo/..")
        self.assertEqual("Canvas", parent.name)

    def test_pointer_down_from_object(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        time.sleep(1)
        p_panel = self.altdriver.find_object(By.NAME, 'Panel')
        color1 = p_panel.get_component_property(
            'AltUnityExampleScriptPanel', 'normalColor')
        p_panel.pointer_down()
        time.sleep(1)
        color2 = p_panel.get_component_property(
            'AltUnityExampleScriptPanel', 'highlightColor')
        self.assertNotEqual(color1, color2)

    def test_pointer_up_from_object(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        time.sleep(1)
        p_panel = self.altdriver.find_object(By.NAME, 'Panel')
        color1 = p_panel.get_component_property(
            'AltUnityExampleScriptPanel', 'normalColor')
        p_panel.pointer_down()
        time.sleep(1)
        p_panel.pointer_up()
        color2 = p_panel.get_component_property(
            'AltUnityExampleScriptPanel', 'highlightColor')
        self.assertEqual(color1, color2)

    def test_get_all_components(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        components = self.altdriver.find_object(
            By.NAME, "Canvas").get_all_components()
        self.assertEqual(5, len(components))
        self.assertEqual("UnityEngine.RectTransform",
                         components[0]["componentName"])
        self.assertEqual("UnityEngine.CoreModule",
                         components[0]["assemblyName"])

    def test_get_all_fields(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule = self.altdriver.find_object(
            By.NAME, "Capsule")
        components = capsule.get_all_components()
        capsule_component = None
        for component in components:
            if component["componentName"] == "AltUnityExampleScriptCapsule":
                capsule_component = component
        print(capsule_component["componentName"])
        fields = capsule.get_all_fields(capsule_component)
        self.assertEqual("intialValue", fields["stringToSetFromTests"])

    def test_scroll(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeInitialPostion = [player2.worldX, player2.worldY, player2.worldY]
        self.altdriver.scroll_mouse(4, 2)
        time.sleep(2)
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_scroll_and_wait(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeInitialPostion = [player2.worldX, player2.worldY, player2.worldY]
        self.altdriver.scroll_mouse_and_wait(4, 2)
        player2 = self.altdriver.find_object(By.NAME, "Player2")
        cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_click_event(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_object(By.NAME, 'UIButton').click_event()
        capsule_info = self.altdriver.wait_for_object_with_text(
            By.NAME, 'CapsuleInfo', 'UIButton clicked to jump capsule!', timeout=1)
        self.assertEqual('UIButton clicked to jump capsule!',
                         capsule_info.get_text())

    def test_acceleration(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt(1, 1, 1, 1)
        time.sleep(1)
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.assertNotEqual(initial_position, final_position)

    def test_acceleration_and_wait(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.altdriver.tilt_and_wait(1, 1, 1, 1)
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
        self.assertNotEqual(initial_position, final_position)

    def test_find_object_with_camera_id(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        camera = self.altdriver.find_object(By.PATH, "//Camera")
        altElement = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        self.assertTrue(altElement.name == "Capsule")
        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        altElement2 = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))
        self.assertNotEqual(altElement.x, altElement2.x)
        self.assertNotEqual(altElement.y, altElement2.y)

    def test_wait_for_object_with_camera_id(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        camera = self.altdriver.find_object(By.PATH, "//Camera")
        altElement = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera.id))
        self.assertTrue(altElement.name == "Capsule")
        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        altElement2 = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", By.ID, str(camera2.id))
        self.assertNotEqual(altElement.x, altElement2.x)
        self.assertNotEqual(altElement.y, altElement2.y)

    def test_find_objects_with_camera_id(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        camera = self.altdriver.find_object(By.PATH, "//Camera")
        altElement = self.altdriver.find_objects(
            By.NAME, "Plane", By.ID, str(camera.id))
        self.assertTrue(altElement[0].name == "Plane")
        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        altElement2 = self.altdriver.find_objects(
            By.NAME, "Plane", By.ID, str(camera2.id))
        self.assertNotEqual(altElement[0].x, altElement2[0].x)
        self.assertNotEqual(altElement[0].y, altElement2[0].y)

    def test_wait_for_object_not_be_present_with_camera_id(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        camera2 = self.altdriver.find_object(By.PATH, "//Main Camera")
        self.altdriver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs", By.ID, str(camera2.id))

        allObjectsInTheScene = self.altdriver.get_all_elements()
        objectSearched = None
        for obj in allObjectsInTheScene:
            if obj.name == "ObjectDestroyedIn5Secs":
                objectSearched = obj
                break
        self.assertTrue(objectSearched == None)

    def test_wait_for_object_with_text_with_camera_id(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        name = "CapsuleInfo"
        text = self.altdriver.find_object(By.NAME, name).get_text()
        camera = self.altdriver.find_object(By.PATH, "//Main Camera")
        altElement = self.altdriver.wait_for_object_with_text(
            By.NAME, name, text, By.ID, str(camera.id))
        self.assertIsNotNone(altElement)
        self.assertEqual(altElement.get_text(), text)

    def test_wait_for_object_which_contains_with_camera_id(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        camera = self.altdriver.find_object(By.PATH, "//Main Camera")
        altElement = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva", By.ID, str(camera.id))
        self.assertEqual("Canvas", altElement.name)

    def test_find_object_with_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        altElement = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider",  By.TAG, "MainCamera")
        self.assertTrue(altElement.name == "Capsule")
        altElement2 = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider", By.TAG, "Untagged")
        self.assertNotEqual(altElement.x, altElement2.x)
        self.assertNotEqual(altElement.y, altElement2.y)

    def test_wait_for_object_with_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        altElement = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", By.TAG, "MainCamera")
        self.assertTrue(altElement.name == "Capsule")
        altElement2 = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider",  By.TAG, "Untagged")
        self.assertNotEqual(altElement.x, altElement2.x)
        self.assertNotEqual(altElement.y, altElement2.y)

    def test_find_objects_with_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        altElement = self.altdriver.find_objects(
            By.NAME, "Plane", By.TAG, "MainCamera")
        self.assertTrue(altElement[0].name == "Plane")
        altElement2 = self.altdriver.find_objects(
            By.NAME, "Plane", By.TAG, "Untagged")
        self.assertNotEqual(altElement[0].x, altElement2[0].x)
        self.assertNotEqual(altElement[0].y, altElement2[0].y)

    def test_wait_for_object_not_be_present_with_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs", By.TAG, "MainCamera")

        allObjectsInTheScene = self.altdriver.get_all_elements()
        objectSearched = None
        for obj in allObjectsInTheScene:
            if obj.name == "ObjectDestroyedIn5Secs":
                objectSearched = obj
                break
        self.assertTrue(objectSearched == None)

    def test_wait_for_object_with_text_with_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        name = "CapsuleInfo"
        text = self.altdriver.find_object(By.NAME, name).get_text()
        print(text)
        altElement = self.altdriver.wait_for_object_with_text(
            By.NAME, name, text, By.TAG, "MainCamera")
        self.assertIsNotNone(altElement)
        self.assertEqual(altElement.get_text(), text)

    def test_wait_for_object_which_contains_with_tag(self):
        altElement = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva", By.TAG, "MainCamera")
        self.assertEqual("Canvas", altElement.name)

    def test_find_object_by_camera(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        altElement = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider", "Camera")
        self.assertTrue(altElement.name == "Capsule")
        altElement2 = self.altdriver.find_object(
            By.COMPONENT, "CapsuleCollider", By.NAME, "Main Camera")
        self.assertNotEqual(altElement.x, altElement2.x)
        self.assertNotEqual(altElement.y, altElement2.y)

    def test_wait_for_object_by_camera(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        altElement = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", "Camera")
        self.assertTrue(altElement.name == "Capsule")
        altElement2 = self.altdriver.wait_for_object(
            By.COMPONENT, "CapsuleCollider", By.NAME, "Main Camera")
        self.assertNotEqual(altElement.x, altElement2.x)
        self.assertNotEqual(altElement.y, altElement2.y)

    def test_find_objects_by_camera(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altButton = self.altdriver.find_object(By.PATH, "//Button")
        altButton.tap()
        altButton.tap()
        altElement = self.altdriver.find_objects(
            By.NAME, "Plane", By.NAME, "Camera")
        self.assertTrue(altElement[0].name == "Plane")
        altElement2 = self.altdriver.find_objects(
            By.NAME, "Plane", By.NAME, "Main Camera")
        self.assertNotEqual(altElement[0].x, altElement2[0].x)
        self.assertNotEqual(altElement[0].y, altElement2[0].y)

    def test_wait_for_object_not_be_present_by_camera(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.wait_for_object_to_not_be_present(
            By.NAME, "ObjectDestroyedIn5Secs", By.NAME, "Main Camera")

        allObjectsInTheScene = self.altdriver.get_all_elements()
        objectSearched = None
        for obj in allObjectsInTheScene:
            if obj.name == "ObjectDestroyedIn5Secs":
                objectSearched = obj
                break
        self.assertTrue(objectSearched == None)

    def test_wait_for_object_by_camera_2(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        name = "CapsuleInfo"
        text = self.altdriver.find_object(By.NAME, name).get_text()
        altElement = self.altdriver.wait_for_object_with_text(
            By.NAME, name, text, By.NAME, "Main Camera")
        self.assertIsNotNone(altElement)
        self.assertEqual(altElement.get_text(), text)

    def test_wait_for_object_which_contains_by_camera(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.wait_for_object_which_contains(
            By.NAME, "Canva", By.NAME, "Main Camera")
        self.assertEqual("Canvas", altElement.name)

    def test_load_additive_scenes(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        initial_number_of_elements = self.altdriver.get_all_elements()
        self.altdriver.load_scene('Scene 2 Draggable Panel', False)
        final_number_of_elements = self.altdriver.get_all_elements()
        self.assertGreater(len(final_number_of_elements),
                           len(initial_number_of_elements))
        scenes = self.altdriver.get_all_loaded_scenes()
        self.assertEqual(len(scenes), 2)

    def test_get_component_property_complex_class(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        componentName = "AltUnityExampleScriptCapsule"
        propertyName = "AltUnitySampleClass.testInt"
        altElement = self.altdriver.find_object(By.NAME, "Capsule")
        self.assertIsNotNone(altElement)
        propertyValue = altElement.get_component_property(
            componentName, propertyName, max_depth=1)
        self.assertEqual("1", propertyValue)

    def test_get_component_property_complex_class2(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        componentName = "AltUnityExampleScriptCapsule"
        propertyName = "listOfSampleClass[1].testString"
        altElement = self.altdriver.find_object(By.NAME, "Capsule")
        self.assertIsNotNone(altElement)
        propertyValue = altElement.get_component_property(
            componentName, propertyName, max_depth=1)
        self.assertEqual("test2", propertyValue)

    def test_set_component_property_complex_class(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        componentName = "AltUnityExampleScriptCapsule"
        propertyName = "AltUnitySampleClass.testInt"
        altElement = self.altdriver.find_object(By.NAME, "Capsule")
        self.assertIsNotNone(altElement)
        altElement.set_component_property(componentName, propertyName, "2")
        propertyValue = altElement.get_component_property(
            componentName, propertyName, max_depth=1)
        self.assertEqual("2", propertyValue)

    def test_set_component_property_complex_class2(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        componentName = "AltUnityExampleScriptCapsule"
        propertyName = "listOfSampleClass[1].testString"
        altElement = self.altdriver.find_object(By.NAME, "Capsule")
        self.assertIsNotNone(altElement)
        altElement.set_component_property(componentName, propertyName, "test3")
        propertyValue = altElement.get_component_property(
            componentName, propertyName, max_depth=1)
        self.assertEqual("test3", propertyValue)

    def test_get_version(self):
        serverVersion = GetServerVersion(
            self.altdriver.socket,
            self.altdriver.request_separator,
            self.altdriver.request_end).execute()
        self.assertTrue(VERSION.startswith(serverVersion))

    def test_altElement_parentId(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        element = self.altdriver.find_object(By.NAME, 'Capsule')
        self.assertEqual(element.parentId, element.transformParentId)

    def test_get_parent(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        element = self.altdriver.find_object(By.NAME, 'Canvas/CapsuleInfo')
        elementParent = element.get_parent()
        self.assertEqual('Canvas', elementParent.name)

    def test_unload_scene(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        self.altdriver.load_scene('Scene 2 Draggable Panel', False)
        self.assertEqual(2, len(self.altdriver.get_all_loaded_scenes()))
        self.altdriver.unload_scene('Scene 2 Draggable Panel')
        self.assertEqual(1, len(self.altdriver.get_all_loaded_scenes()))
        self.assertEqual("Scene 1 AltUnityDriverTestScene",
                         self.altdriver.get_all_loaded_scenes()[0])

    def test_unload_only_scene(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene', True)
        with self.assertRaises(CouldNotPerformOperationException):
            self.altdriver.unload_scene('Scene 1 AltUnityDriverTestScene')

    def test_set_server_logging(self):
        result = self.altdriver.call_static_method(
            "Altom.Server.Logging.ServerLogManager", "Instance.Configuration.FindRuleByName", "AltUnityServerFileRule", "", "Assembly-CSharp")
        rule = json.loads(result)

        # Default level in AltUnity Serveris Debug level
        self.assertEqual(5, len(rule["Levels"]), rule["Levels"])

        self.altdriver.set_server_logging(
            AltUnityLogger.File, AltUnityLogLevel.Off)
        result = self.altdriver.call_static_method("Altom.Server.Logging.ServerLogManager",
                                                   "Instance.Configuration.FindRuleByName", "AltUnityServerFileRule", "", "Assembly-CSharp")
        rule = json.loads(result)
        self.assertEqual(0, len(rule["Levels"]), rule["Levels"])
        self.altdriver.set_server_logging(
            AltUnityLogger.File, AltUnityLogLevel.Debug)  # reset logging level

    def test_invalid_paths(self):
        with self.assertRaises(AltUnityInvalidPathException):
            self.altdriver.find_object(By.PATH, "//[1]")
        with self.assertRaises(AltUnityInvalidPathException):
            self.altdriver.find_object(By.PATH, "CapsuleInfo[@tag=UI]")
        with self.assertRaises(AltUnityInvalidPathException):
            self.altdriver.find_object(By.PATH, "//CapsuleInfo[@tag=UI/Text")
        with self.assertRaises(AltUnityInvalidPathException):
            self.altdriver.find_object(By.PATH, "//CapsuleInfo[0/Text")


if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(PythonTests)
    run_result = unittest.TextTestRunner(verbosity=2).run(suite)
    sys.exit(not run_result.wasSuccessful())
