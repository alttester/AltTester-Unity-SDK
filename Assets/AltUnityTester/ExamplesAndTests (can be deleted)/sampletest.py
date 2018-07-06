import os
import unittest
import sys
from appium import webdriver
from altunityrunner import AltrunUnityDriver

PATH = lambda p: os.path.abspath(
    os.path.join(os.path.dirname(__file__), p)
)

class SampleAppiumTest(unittest.TestCase):
    altdriver = None
    platform = "android" # set to `ios` or `android` to change platform

    @classmethod
    def setUpClass(cls):
        cls.desired_caps = {}
        if (cls.platform == "android"):
            cls.setup_android()
        else:
            cls.setup_ios()
        cls.driver = webdriver.Remote('http://localhost:4723/wd/hub', cls.desired_caps)
        cls.altdriver = AltrunUnityDriver(cls.driver, cls.platform)

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()
        cls.driver.quit()

    @classmethod
    def setup_android(cls):
        cls.desired_caps['platformName'] = 'Android'
        cls.desired_caps['deviceName'] = 'device'
        cls.desired_caps['app'] = PATH('../../../sampleGame.apk')

    @classmethod
    def setup_ios(cls):
        cls.desired_caps['platformName'] = 'iOS'
        cls.desired_caps['deviceName'] = 'iPhone5'
        cls.desired_caps['automationName'] = 'XCUITest'
        cls.desired_caps['app'] = PATH('../../../sampleGame.ipa')

    def test_wait_for_scene(self):
        self.altdriver.wait_for_current_scene_to_be('AltUnityDriverTestScene')

    def test_find_element(self):
        self.altdriver.find_element('Plane')
        self.altdriver.find_element('Capsule')
    
    def test_wait_for_element_with_text(self):
        text_to_wait_for = self.altdriver.find_element('CapsuleInfo').get_text()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', text_to_wait_for)

    def test_find_element_where_name_contains(self):
        self.altdriver.find_element_where_name_contains('Pla')

    def test_find_element_and_tap(self):
        # tap UIButton to make capsule jump
        self.altdriver.find_element('UIButton').tap()
        capsule_info = self.altdriver.wait_for_element_with_text('CapsuleInfo', 'UIButton clicked to jump capsule!')
        assert capsule_info.get_text() == capsule_info.text
        
        # tap capsule to make it jump
        self.altdriver.find_element('Capsule').tap()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!')

    def test_find_elements(self):
        assert len(self.altdriver.find_elements("Plane")) == 2
        assert len(self.altdriver.find_elements("something that does not exist")) == 0

    def test_find_element_by_component(self):
        assert self.altdriver.find_element_by_component("Capsule").name == "Capsule"

    def test_find_elements_by_component(self):
        assert len(self.altdriver.find_elements_by_component("UnityEngine.MeshFilter")) == 3

    def test_get_component_property(self):
        result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
        assert result == "[1,2,3]", "result was: " + result
    
    def test_set_component_property(self):
        self.altdriver.find_element("Capsule").set_component_property("Capsule", "arrayOfInts", "[2,3,4]")
        result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
        assert result == "[2,3,4]", "result was: " + result

    def test_cal_component_method(self):
        result = self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
        assert result == "methodInvoked"

        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
        assert 'setFromMethod' == self.altdriver.find_element('CapsuleInfo').get_text()

if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(SampleAppiumTest)
    result = unittest.TextTestRunner(verbosity=2).run(suite)
    sys.exit(not result.wasSuccessful())