# -*- coding: UTF-8
import os
import unittest
import sys
import json
import time
from os import path
from altunityrunner import *


def PATH(p): return os.path.abspath(
    os.path.join(os.path.dirname(__file__), p)
)


class PythonTests(unittest.TestCase):
    altdriver = None
    platform = "android"  # set to `ios` or `android` to change platform

    @classmethod
    def setUpClass(cls):
        cls.altdriver = AltrunUnityDriver(None, 'android', log_flag=False)

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()

    def test_tap_ui_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element('UIButton').tap()
        capsule_info = self.altdriver.wait_for_element_with_text(
            'CapsuleInfo', 'UIButton clicked to jump capsule!', '', 1)
        self.assertEqual('UIButton clicked to jump capsule!',
                         capsule_info.get_text())

    def test_tap_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule_element = self.altdriver.find_element('Capsule')
        capsule_element.tap()
        capsule_info = self.altdriver.wait_for_element_with_text(
            'CapsuleInfo', 'Capsule was clicked to jump!', '', 1)
        self.assertEqual('Capsule was clicked to jump!',
                         capsule_info.get_text())

    def test_tap_at_coordinates(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule_element = self.altdriver.find_element('Capsule')
        self.altdriver.tap_at_coordinates(capsule_element.x, capsule_element.y)
        capsule_info = self.altdriver.wait_for_element_with_text(
            'CapsuleInfo', 'Capsule was clicked to jump!', '', 1)
        self.assertEqual('Capsule was clicked to jump!',
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
        altElement = self.altdriver.find_element('Resize Zone')
        positionInitX = altElement.x 
        positionInitY = altElement.y
        self.altdriver.swipe_and_wait(altElement.x, altElement.y, int(altElement.x) - 200, int(altElement.y) - 200, 2)

        time.sleep(2)

        altElement = self.altdriver.find_element('Resize Zone')
        positionFinalX = altElement.x 
        positionFinalY = altElement.y 
        self.assertNotEqual(positionInitX, positionFinalX)
        self.assertNotEqual(positionInitY, positionFinalY)

    def test_resize_panel_with_multipoinit_swipe(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        altElement = self.altdriver.find_element('Resize Zone')
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

        altElement = self.altdriver.find_element('Resize Zone')
        positionFinalX = altElement.x
        positionFinalY = altElement.y 
        self.assertNotEqual(positionInitX, positionFinalX)
        self.assertNotEqual(positionInitY, positionFinalY)

    def test_find_element(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        plane = self.altdriver.find_element('Plane')
        capsule = self.altdriver.find_element('Capsule')
        self.assertEqual('Plane', plane.name)
        self.assertEqual('Capsule', capsule.name)

    def test_wait_for_element_with_text(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        text_to_wait_for = self.altdriver.find_element(
            'CapsuleInfo').get_text()
        capsule_info = self.altdriver.wait_for_element_with_text(
            'CapsuleInfo', text_to_wait_for, '', 1)
        self.assertEqual('CapsuleInfo', capsule_info.name)
        self.assertEqual(text_to_wait_for, capsule_info.get_text())

    def test_find_elements(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        planes = self.altdriver.find_elements("Plane")
        assert len(planes) == 2
        assert len(self.altdriver.find_elements(
            "something that does not exist")) == 0

    def test_find_element_where_name_contains(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        plane = self.altdriver.find_element_where_name_contains('Pla')
        self.assertTrue('Pla' in plane.name)

    # Fix in issue 184
    # def test_find_disabled_element_where_name_contains(self):
    #     self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
    #     self.altdriver.find_element_where_name_contains('Cube',enabled=False)

    def test_find_element_by_name_and_parent(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule_element = self.altdriver.find_element('Canvas/CapsuleInfo')
        assert capsule_element.name == 'CapsuleInfo'

    def test_find_element_by_component(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.assertEqual(self.altdriver.find_element_by_component(
            "Capsule").name, "Capsule")

    def test_find_elements_by_component(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.assertEqual(
            len(self.altdriver.find_elements_by_component("UnityEngine.MeshFilter")), 3)

    def test_get_component_property(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element(
            "Capsule").get_component_property("Capsule", "arrayOfInts")
        self.assertEqual(result, "[1,2,3]")

    def test_set_component_property(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element("Capsule").set_component_property(
            "Capsule", "arrayOfInts", "[2,3,4]")
        result = self.altdriver.find_element(
            "Capsule").get_component_property("Capsule", "arrayOfInts")
        self.assertEqual(result, "[2,3,4]")

    def test_call_component_method(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element("Capsule").call_component_method(
            "Capsule", "Jump", "setFromMethod")
        self.assertEqual(result, "null")
        self.altdriver.wait_for_element_with_text(
            'CapsuleInfo', 'setFromMethod')
        self.assertEqual('setFromMethod', self.altdriver.find_element(
            'CapsuleInfo').get_text())

    def test_pointer_enter_and_exit(self):
        self.altdriver.load_scene('Scene 3 Drag And Drop')

        alt_element = self.altdriver.find_element('Drop Image')
        color1 = alt_element.get_component_property('DropMe', 'highlightColor')
        alt_element.pointer_enter()
        color2 = alt_element.get_component_property('DropMe', 'highlightColor')
        self.assertNotEqual(color1, color2)

        alt_element.pointer_exit()
        color3 = alt_element.get_component_property('DropMe', 'highlightColor')
        self.assertNotEqual(color3, color2)
        self.assertEqual(color3, color1)

    def test_multiple_swipes(self):
        self.altdriver.load_scene('Scene 3 Drag And Drop')

        image1 = self.altdriver.find_element('Drag Image1')
        box1 = self.altdriver.find_element('Drop Box1')

        self.altdriver.swipe(image1.x, image1.y, box1.x, box1.y, 5)

        image2 = self.altdriver.find_element('Drag Image2')
        box2 = self.altdriver.find_element('Drop Box2')

        self.altdriver.swipe(image2.x, image2.y, box2.x, box2.y, 2)

        image3 = self.altdriver.find_element('Drag Image3')
        box1 = self.altdriver.find_element('Drop Box1')

        self.altdriver.swipe(image3.x, image3.y, box1.x, box1.y, 3)

        time.sleep(6)

        image_source = image1.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element(
            'Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

        image_source = image2.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element(
            'Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

    def test_multiple_swipe_and_waits(self):
        self.altdriver.load_scene('Scene 3 Drag And Drop')

        image2 = self.altdriver.find_element('Drag Image2')
        box2 = self.altdriver.find_element('Drop Box2')

        self.altdriver.swipe_and_wait(image2.x, image2.y, box2.x, box2.y, 2)

        image3 = self.altdriver.find_element('Drag Image3')
        box1 = self.altdriver.find_element('Drop Box1')

        self.altdriver.swipe_and_wait(image3.x, image3.y, box1.x, box1.y, 1)

        image1 = self.altdriver.find_element('Drag Image1')
        box1 = self.altdriver.find_element('Drop Box1')

        self.altdriver.swipe_and_wait(image1.x, image1.y, box1.x, box1.y, 3)

        image_source = image1.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element(
            'Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

        image_source = image2.get_component_property(
            'UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element(
            'Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

    def test_button_click_and_wait_with_swipe(self):
        button = self.altdriver.find_object(By.NAME, 'UIButton')
        self.altdriver.hold_button_and_wait(button.x, button.y, 1)
        capsule_info = self.altdriver.find_object(By.NAME, 'CapsuleInfo')
        time.sleep(1.4)
        text = capsule_info.get_text()
        self.assertEqual(text, "UIButton clicked to jump capsule!")

    def test_button_click_with_swipe(self):
        button = self.altdriver.find_object(By.NAME, 'UIButton')
        self.altdriver.hold_button(button.x, button.y,1)
        capsule_info = self.altdriver.find_object(By.NAME, 'CapsuleInfo')
        text = capsule_info.get_text()
        self.assertEqual(text, "UIButton clicked to jump capsule!")

    def test_multiple_swipe_and_waits_with_multipoint_swipe(self):
        altElement1 = self.altdriver.find_element('Drag Image1')
        altElement2 = self.altdriver.find_element('Drop Box1')

        multipointPositions = [altElement1.get_screen_position(), [altElement2.x, altElement2.y]]

        self.altdriver.multipoint_swipe_and_wait(multipointPositions, 2)
        time.sleep(2)

        altElement1 = self.altdriver.find_element('Drag Image1')
        altElement2 = self.altdriver.find_element('Drop Box1')
        altElement3 = self.altdriver.find_element('Drop Box2')
 
        positions = [
          [altElement1.x, altElement1.y], 
          [altElement2.x, altElement2.y], 
          [altElement3.x, altElement3.y]
        ]

        self.altdriver.multipoint_swipe_and_wait(positions, 3)
        imageSource = self.altdriver.find_element('Drag Image1').get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_element('Drop Image').get_component_property("UnityEngine.UI.Image", "sprite")
        self.assertNotEqual(imageSource, imageSourceDropZone)

        imageSource = self.altdriver.find_element('Drag Image2').get_component_property("UnityEngine.UI.Image", "sprite")
        imageSourceDropZone = self.altdriver.find_element('Drop').get_component_property("UnityEngine.UI.Image", "sprite")
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
            alt_element = self.altdriver.wait_for_element(
                "dlkasldkas", '', 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], "Element dlkasldkas not found after 1 seconds")

    def test_wait_forobject_to_not_exist_fail(self):
        try:
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            alt_element = self.altdriver.wait_for_element_to_not_be_present(
                "Capsule", '', 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Element Capsule still found after 1 seconds')

    def test_wait_for_object_with_text_wrong_text(self):
        try:
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            alt_element = self.altdriver.wait_for_element_with_text(
                "CapsuleInfo", "aaaaa", '', 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Element CapsuleInfo should have text `aaaaa` but has `Capsule Info` after 1 seconds')

    def test_wait_for_current_scene_to_be_a_non_existing_scene(self):
        try:
            alt_element = self.altdriver.wait_for_current_scene_to_be(
                "AltUnityDriverTestScenee", 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Scene AltUnityDriverTestScenee not loaded after 1 seconds')

    def test_get_bool(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        alt_element = self.altdriver.find_element('Capsule')
        text = alt_element.get_component_property('Capsule', 'TestBool')
        self.assertEqual('true', text)

    def test_call_static_method(self):
        self.altdriver.call_static_methods(
            "UnityEngine.PlayerPrefs", "SetInt", "Test?1", assembly="UnityEngine.CoreModule")
        a = int(self.altdriver.call_static_methods("UnityEngine.PlayerPrefs",
                                                   "GetInt", "Test?2", assembly="UnityEngine.CoreModule"))
        self.assertEquals(1, a)

    def test_call_method_with_multiple_definitions(self):
        capsule = self.altdriver.find_element("Capsule")
        capsule.call_component_method(
            "Capsule", "Test", "2", type_of_parameters="System.Int32")
        capsuleInfo = self.altdriver.find_element("CapsuleInfo")
        self.assertEquals("6", capsuleInfo.get_text())

    def test_tap_on_screen_where_there_are_no_objects(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        alt_element = self.altdriver.tap_at_coordinates(1, 1)
        self.assertIsNone(alt_element)

    def test_set_and_get_time_scale(self):
        self.altdriver.set_time_scale(0.1)
        time.sleep(1)
        time_scale = self.altdriver.get_time_scale()
        self.assertEquals(0.1, time_scale)
        self.altdriver.set_time_scale(1)

    def test_movement_cube(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        cube = self.altdriver.find_element("Player1")
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldY)
        self.altdriver.scroll_mouse(30, 1)
        self.altdriver.press_key('K', 1, 2)
        time.sleep(2)
        cube = self.altdriver.find_element("Player1")
        self.altdriver.press_key_and_wait('O', 1, 1)
        cubeFinalPosition = (cube.worldX, cube.worldY, cube.worldY)

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_camera_movement(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        cube = self.altdriver.find_element("Player1")
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldY)

        self.altdriver.press_key('W', 1, 2)
        time.sleep(2)
        cube = self.altdriver.find_element("Player1")
        cubeFinalPosition = (cube.worldX, cube.worldY, cube.worldY)

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_creating_stars(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        stars = self.altdriver.find_objects_which_contains(
            By.NAME, "Star", "Player2")
        self.assertEqual(1, len(stars))
        player = self.altdriver.find_objects_which_contains(
            By.NAME, "Player", "Player2")
        pressing_point_1 = self.altdriver.find_object(
            By.NAME, "PressingPoint1", "Player2")

        self.altdriver.move_mouse(
            int(pressing_point_1.x), int(pressing_point_1.y), 1)
        time.sleep(1.5)

        self.altdriver.press_key('Mouse0', 1, 0)
        pressing_point_2 = self.altdriver.find_object(
            By.NAME, "PressingPoint2", "Player2")
        self.altdriver.move_mouse_and_wait(
            int(pressing_point_1.x), int(pressing_point_2.y), 1)
        self.altdriver.press_key('Mouse0', 1, 0)

        stars = self.altdriver.find_objects_which_contains(By.NAME, "Star")
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
        altElement = self.altdriver.find_object(By.COMPONENT, "Capsule")
        self.assertTrue(altElement.name == "Capsule")

    def test_find_child(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.find_object(By.PATH, "//UIButton/*")
        self.assertTrue(altElement.name == "Text")

    def test_find_objects_by_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElements = self.altdriver.find_objects(By.TAG, "plane")
        self.assertEquals(2, len(altElements))
        for altElement in altElements:
            self.assertEquals("Plane", altElement.name)

    def test_find_objects_by_layer(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        time.sleep(1)
        altElements = self.altdriver.find_objects(By.LAYER, "Default")
        self.assertEquals(12, len(altElements))

    def test_find_objects_by_contains_name(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        time.sleep(1)
        altElements = self.altdriver.find_objects_which_contains(
            By.NAME, "Capsule")
        self.assertEquals(2, len(altElements), altElements)
        for altElement in altElements:
            self.assertTrue("Capsule" in altElement.name)

    def test_power_joystick(self):
        button_names = ['Horizontal', 'Vertical']
        keys_to_press = ['D', 'W']
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        axisName = self.altdriver.find_element("AxisName")
        axisValue = self.altdriver.find_element("AxisValue")
        i = 0
        for key in keys_to_press:
            self.altdriver.press_key_and_wait(key, 0.5, 0.1)
            self.assertEqual('0.5', axisValue.get_text())
            self.assertEqual(button_names[i], axisName.get_text())
            i = i+1

    def test_find_element_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element('Plane')
        self.altdriver.find_element('Capsule')

    def test_wait_for_element_with_text_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        text_to_wait_for = self.altdriver.find_element(
            'CapsuleInfo').get_text()
        self.altdriver.wait_for_element_with_text(
            'CapsuleInfo', text_to_wait_for, '', 1)

    def test_find_elements_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        planes = self.altdriver.find_elements("Plane")
        assert len(planes) == 2
        assert len(self.altdriver.find_elements(
            "something that does not exist")) == 0

    def test_find_element_where_name_contains_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element_where_name_contains('Pla')

    def test_find_element_by_name_and_parent_old(self):
        capsule_element = self.altdriver.find_element('Canvas/CapsuleInfo')
        assert capsule_element.name == 'CapsuleInfo'

    def test_find_element_by_component_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.assertEqual(self.altdriver.find_element_by_component(
            "Capsule").name, "Capsule")

    def test_find_elements_by_component_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.assertEqual(
            len(self.altdriver.find_elements_by_component("UnityEngine.MeshFilter")), 3)

    def test_get_component_property_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element(
            "Capsule").get_component_property("Capsule", "arrayOfInts")
        self.assertEqual(result, "[1,2,3]")

    def test_set_component_property_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element("Capsule").set_component_property(
            "Capsule", "arrayOfInts", "[2,3,4]")
        result = self.altdriver.find_element(
            "Capsule").get_component_property("Capsule", "arrayOfInts")
        self.assertEqual(result, "[2,3,4]")

    def test_call_component_method_old(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element("Capsule").call_component_method(
            "Capsule", "Jump", "setFromMethod")
        self.assertEqual(result, "null")
        self.altdriver.wait_for_element_with_text(
            'CapsuleInfo', 'setFromMethod')
        self.assertEqual('setFromMethod', self.altdriver.find_element(
            'CapsuleInfo').get_text())

    def test_wait_for_non_existing_object_old(self):
        try:
            alt_element = self.altdriver.wait_for_element(
                "dlkasldkas", '', 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], "Element dlkasldkas not found after 1 seconds")

    def test_wait_forobject_to_not_exist_fail_old(self):
        try:
            alt_element = self.altdriver.wait_for_element_to_not_be_present(
                "Capsule", '', 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Element Capsule still found after 1 seconds')

    def test_wait_for_object_with_text_wrong_text_old(self):
        try:
            alt_element = self.altdriver.wait_for_element_with_text(
                "CapsuleInfo", "aaaaa", '', 1, 0.5)
            self.assertEqual(False, True)
        except WaitTimeOutException as e:
            self.assertEqual(
                e.args[0], 'Element CapsuleInfo should have text `aaaaa` but has `Capsule Info` after 1 seconds')

    def test_call_method_with_assembly(self):
        capsule = self.altdriver.find_object(By.NAME, "Capsule")
        initialRotation = capsule.get_component_property(
            "UnityEngine.Transform", "rotation")
        capsule.call_component_method("UnityEngine.Transform", "Rotate", "10?10?10",
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

        self.assertEqual(22, len(list_of_elements), list_of_elements)
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

        self.assertEqual(27, len(list_of_elements))
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
            altElement = self.altdriver.find_object_which_contains(
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

    def test_wait_for_object_with_text(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElement = self.altdriver.wait_for_object_with_text(
            By.NAME, "CapsuleInfo", "Capsule Info")
        self.assertEqual(altElement.name, "CapsuleInfo")

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
        counterButton = self.altdriver.find_object(By.NAME, "ButtonCounter");
        counterButtonText = self.altdriver.find_object(By.NAME, "ButtonCounter/Text");
        counterButton.double_tap();
        time.sleep(0.5);
        self.assertEqual("2", counterButtonText.get_text());
        
    def test_custom_tap(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        counterButton = self.altdriver.find_object(By.NAME, "ButtonCounter");
        counterButtonText = self.altdriver.find_object(By.NAME, "ButtonCounter/Text");
        self.altdriver.tap_custom(counterButton.x, counterButton.y, 4);
        time.sleep(1);
        self.assertEqual("4", counterButtonText.get_text());



if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(PythonTests)
    result = unittest.TextTestRunner(verbosity=2).run(suite)
    sys.exit(not result.wasSuccessful())
