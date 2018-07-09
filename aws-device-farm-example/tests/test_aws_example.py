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
    def setup_class(cls):
        cls.desired_caps = {}
        if (cls.platform == "android"):
            cls.setup_android()
        else:
            cls.setup_ios()
        cls.driver = webdriver.Remote('http://localhost:4723/wd/hub', cls.desired_caps)
        cls.altdriver = AltrunUnityDriver(cls.driver, cls.platform)

    @classmethod
    def teardown_class(cls):
        cls.altdriver.stop()
        cls.driver.quit()

    @classmethod
    def setup_android(cls):
        cls.desired_caps['platformName'] = 'Android'
        cls.desired_caps['deviceName'] = 'device'

    @classmethod
    def setup_ios(cls):
        cls.desired_caps['platformName'] = 'iOS'
        cls.desired_caps['deviceName'] = 'iPhone5'
        cls.desired_caps['automationName'] = 'XCUITest'

    def test_unity_elements(self):
        screenshot_folder = os.getenv('SCREENSHOT_PATH', '/temp')

        self.altdriver.wait_for_current_scene_to_be('AltUnityDriverTestScene')
        self.altdriver.find_element('Plane')
        self.altdriver.find_element('Capsule')
        text_to_wait_for = self.altdriver.find_element('CapsuleInfo').get_text()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', text_to_wait_for)
        self.altdriver.find_element_where_name_contains('Pla')

        # tap UIButton to make capsule jump
        self.altdriver.find_element('UIButton').tap()
        capsule_info = self.altdriver.wait_for_element_with_text('CapsuleInfo', 'UIButton clicked to jump capsule!')
        assert capsule_info.get_text() == capsule_info.text
        self.driver.save_screenshot(screenshot_folder + "/screenshot_after_button_press.png")
        
        # tap capsule to make it jump
        self.altdriver.find_element('Capsule').tap()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!')
        self.driver.save_screenshot(screenshot_folder + "/screenshot_after_capsule_press.png")


if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(SampleAppiumTest)
    result = unittest.TextTestRunner(verbosity=2).run(suite)
    sys.exit(not result.wasSuccessful())