import os
import unittest
import sys
import json
import time
from altunityrunner import *
PATH = lambda p: os.path.abspath(
    os.path.join(os.path.dirname(__file__), p)
)

class PythonTests(unittest.TestCase):
    altdriver = None
    platform = "android" # set to `ios` or `android` to change platform

    @classmethod
    def setUpClass(cls):
        cls.altdriver = AltrunUnityDriver(None, 'android', TCP_FWD_PORT=13000)

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()

    def test_tap_ui_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element('UIButton').tap()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'UIButton clicked to jump capsule!','',1)

    def test_tap_object(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule_element = self.altdriver.find_element('Capsule')
        capsule_element.tap()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!','',1)


    def test_tap_at_coordinates(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        capsule_element = self.altdriver.find_element('Capsule')
        self.altdriver.tap_at_coordinates(capsule_element.x, capsule_element.y)
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!','',1)

    def test_load_and_wait_for_scene(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.wait_for_current_scene_to_be('Scene 1 AltUnityDriverTestScene',1)
        self.altdriver.load_scene('Scene 2 Draggable Panel')
        self.altdriver.wait_for_current_scene_to_be('Scene 2 Draggable Panel',1)

    def test_find_element(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element('Plane')
        self.altdriver.find_element('Capsule')

    def test_wait_for_element_with_text(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        text_to_wait_for = self.altdriver.find_element('CapsuleInfo').get_text()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', text_to_wait_for,'',1)   

    def test_find_elements(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        planes = self.altdriver.find_elements("Plane")
        assert len(planes) == 2
        assert len(self.altdriver.find_elements("something that does not exist")) == 0

    def test_find_element_where_name_contains(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element_where_name_contains('Pla')

    def test_find_element_by_name_and_parent(self):
        capsule_element = self.altdriver.find_element('Canvas/CapsuleInfo')
        assert capsule_element.name == 'CapsuleInfo'

    def test_find_element_by_component(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.assertEqual(self.altdriver.find_element_by_component("Capsule").name, "Capsule")

    def test_find_elements_by_component(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.assertEqual(len(self.altdriver.find_elements_by_component("UnityEngine.MeshFilter")), 3)

    def test_get_component_property(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
        self.assertEqual(result,"[1,2,3]")
     

    def test_set_component_property(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.find_element("Capsule").set_component_property("Capsule", "arrayOfInts", "[2,3,4]")
        result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
        self.assertEqual(result,"[2,3,4]")

    def test_call_component_method(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
        self.assertEqual(result,"null")
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
        self.assertEqual('setFromMethod', self.altdriver.find_element('CapsuleInfo').get_text())

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

        image_source = image1.get_component_property('UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element('Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

        image_source = image2.get_component_property('UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element('Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
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

        image_source = image1.get_component_property('UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element('Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)

        image_source = image2.get_component_property('UnityEngine.UI.Image', 'sprite')
        image_source_drop_zone = self.altdriver.find_element('Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
        self.assertNotEqual(image_source, image_source_drop_zone)


    def test_set_player_pref_keys_int(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.delete_player_prefs()
        self.altdriver.set_player_pref_key('test', 1, PlayerPrefKeyType.Int)
        value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.Int)
        self.assertEqual(int(value), 1)

    def test_set_player_pref_keys_float(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.delete_player_prefs()
        self.altdriver.set_player_pref_key('test', 1.3, PlayerPrefKeyType.Float)
        value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.Float)
        self.assertEqual(float(value), 1.3)

    def test_set_player_pref_keys_string(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        self.altdriver.delete_player_prefs()
        self.altdriver.set_player_pref_key('test', 'string value', PlayerPrefKeyType.String)
        value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.String)
        self.assertEqual(value, 'string value')
 
    def test_wait_for_non_existing_object(self):
        try:
            alt_element = self.altdriver.wait_for_element("dlkasldkas",'',1,0.5)
            self.assertEqual(False,True)
        except WaitTimeOutException as e:
            self.assertEqual(e.args[0],"Element dlkasldkas not found after 1 seconds")
    
    def test_wait_forobject_to_not_exist_fail(self):
            try:
                alt_element = self.altdriver.wait_for_element_to_not_be_present("Capsule",'',1,0.5)
                self.assertEqual(False,True)
            except WaitTimeOutException as e:
                self.assertEqual(e.args[0],'Element Capsule still found after 1 seconds')
    
    def test_wait_for_object_with_text_wrong_text(self):
            try:
                alt_element = self.altdriver.wait_for_element_with_text("CapsuleInfo","aaaaa",'',1,0.5)
                self.assertEqual(False,True)
            except WaitTimeOutException as e:
                self.assertEqual(e.args[0],'Element CapsuleInfo should have text `aaaaa` but has `Capsule Info` after 1 seconds')
    
    def test_wait_for_current_scene_to_be_a_non_existing_scene(self):
            try:
                alt_element = self.altdriver.wait_for_current_scene_to_be("AltUnityDriverTestScenee",1,0.5)
                self.assertEqual(False,True)
            except WaitTimeOutException as e:
                self.assertEqual(e.args[0],'Scene AltUnityDriverTestScenee not loaded after 1 seconds')

   
    def test_get_bool(self):
        alt_element=self.altdriver.find_element('Capsule')
        text=alt_element.get_component_property('Capsule','TestBool')
        self.assertEqual('true',text)
        

    def test_call_static_method(self):
        self.altdriver.call_static_methods("UnityEngine.PlayerPrefs", "SetInt","Test?1",assembly="UnityEngine.CoreModule")
        a=int(self.altdriver.call_static_methods("UnityEngine.PlayerPrefs", "GetInt", "Test?2",assembly="UnityEngine.CoreModule"))
        self.assertEquals(1,a)

    def test_call_method_with_multiple_definitions(self):
        capsule=self.altdriver.find_element("Capsule")
        capsule.call_component_method("Capsule", "Test","2",type_of_parameters="System.Int32")
        capsuleInfo=self.altdriver.find_element("CapsuleInfo")
        self.assertEquals("6",capsuleInfo.get_text())
    
    def test_tap_on_screen_where_there_are_no_objects(self):
        alt_element=self.altdriver.tap_at_coordinates(1,1)
        self.assertIsNone(alt_element)

    def test_set_and_get_time_scale(self):
        self.altdriver.set_time_scale(0.1)
        time.sleep(1)
        time_scale=self.altdriver.get_time_scale()
        self.assertEquals(0.1, time_scale)
        self.altdriver.set_time_scale(1)
    
   
    def test_movement_cube(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")
       

        cube = self.altdriver.find_element("Player1")
        cubeInitialPostion = (cube.worldX, cube.worldY, cube.worldY)
        self.altdriver.scroll_mouse(30, 1)
        self.altdriver.press_key('K',1, 2)
        time.sleep(2)
        cube = self.altdriver.find_element("Player1")
        self.altdriver.press_key_and_wait('O',1, 1)
        cubeFinalPosition = (cube.worldX, cube.worldY, cube.worldY)

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_camera_movement(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")


        cube = self.altdriver.find_element("Player1")
        cubeInitialPostion =(cube.worldX, cube.worldY, cube.worldY)

        self.altdriver.press_key('W',1, 2)
        time.sleep(2)
        cube = self.altdriver.find_element("Player1")
        cubeFinalPosition =(cube.worldX, cube.worldY, cube.worldY)

        self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

    def test_creating_stars(self):
        self.altdriver.load_scene("Scene 5 Keyboard Input")

        stars = self.altdriver.find_elements_where_name_contains("Star","Player2")
        self.assertEqual(1, len(stars))
        player = self.altdriver.find_elements_where_name_contains("Player","Player2")

        self.altdriver.move_mouse(int(stars[0].x),int(player[0].y)+500, 1)
        time.sleep(1.5)

        self.altdriver.press_key('Mouse0', 1,0)
        self.altdriver.move_mouse_and_wait(int(stars[0].x),int(player[0].y)-500, 1)
        self.altdriver.press_key('Mouse0', 1,0)

        stars = self.altdriver.find_elements_where_name_contains("Star")
        self.assertEqual(3, len(stars))

    def test_power_joystick(self):
        button_names=['Horizontal','Vertical']
        keys_to_press=['D','W']
        self.altdriver.load_scene("Scene 5 Keyboard Input")
        axisName = self.altdriver.find_element("AxisName")
        axisValue = self.altdriver.find_element("AxisValue")
        i = 0
        for key in keys_to_press:
            self.altdriver.press_key_and_wait(key,0.5, 0.1)
            self.assertEqual('0.5', axisValue.get_text())
            self.assertEqual(button_names[i], axisName.get_text())
            i=i+1
       
if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(PythonTests)
    result = unittest.TextTestRunner(verbosity=2).run(suite)
    sys.exit(not result.wasSuccessful())