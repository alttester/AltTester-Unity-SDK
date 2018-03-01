import os
from time import sleep
import unittest
from appium import webdriver
from altunityrunner import AltrunUnityDriver

PATH = lambda p: os.path.abspath(
    os.path.join(os.path.dirname(__file__), p)
)

class SampleAppiumTest(unittest.TestCase):
    altdriver = None
    platform = "android" # set to iOS to change platform

    def setUp(self):
        self.desired_caps = {}
        if (self.platform == "android"):
            self.setup_android()
        else: 
            self.setup_ios()
        self.driver = webdriver.Remote('http://localhost:4723/wd/hub', self.desired_caps)
        self.altdriver = AltrunUnityDriver(self.driver)


    def tearDown(self):
        self.altdriver.stop()
        self.driver.quit()

    def setup_android(self):
        self.desired_caps['platformName'] = 'Android'
        self.desired_caps['deviceName'] = 'device'
        self.desired_caps['app'] = PATH('../../../sampleGame.apk')

    def setup_ios(self):
        self.desired_caps['platformName'] = 'iOS'
        self.desired_caps['deviceName'] = 'iPhone8'
        self.desired_caps['automationName'] = 'XCUITest'
        self.desired_caps['app'] = PATH('../../../sampleGame.ipa')

    def test_simple_actions(self):
        self.altdriver.wait_for_current_scene_to_be('AltUnityDriverTestScene')

        self.altdriver.find_element('Plane')
        self.altdriver.find_element('Capsule')        
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule Info')

        # tap UIButton to make capsule jump
        self.altdriver.find_element('UIButton').tap()
        capsule_info = self.altdriver.wait_for_element_with_text('CapsuleInfo', 'UIButton clicked to jump capsule!')   
        assert capsule_info.get_text() == capsule_info.text
        
        # tap capsule to make it jump
        self.altdriver.find_element('Capsule').tap()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!')  

        # assert how many elements we have in the scene
        assert len(self.altdriver.find_elements("Plane")) == 2
        assert len(self.altdriver.find_elements("something that does not exist")) == 0

        # find element by component
        assert self.altdriver.find_element_by_component("Capsule").name == "Capsule" 

        # show use of find elements by component
        assert len(self.altdriver.find_elements_by_component("UnityEngine.MeshFilter")) == 3

        # assert values of different properties
        result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
        assert result == "[1,2,3]", "result was: " + result

        # get values of different properties
        self.altdriver.find_element("Capsule").set_component_property("Capsule", "arrayOfInts", "[2,3,4]")
        result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
        assert result == "[2,3,4]", "result was: " + result


        # check invoking a method
        result = self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
        assert result == "methodInvoked"

        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
        assert 'setFromMethod' == self.altdriver.find_element('CapsuleInfo').get_text()

if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(SampleAppiumTest)
    unittest.TextTestRunner(verbosity=2).run(suite)