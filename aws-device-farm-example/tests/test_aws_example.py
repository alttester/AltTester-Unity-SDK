from altunityrunner import AltrunUnityDriver
import os
import unittest
from appium import webdriver
from time import sleep


class DeviceFarmAppiumTests(unittest.TestCase):

    @classmethod
    def setup_class(self):
        desired_caps = {}
        self.driver = webdriver.Remote('http://127.0.0.1:4723/wd/hub', desired_caps)
        self.altdriver = AltrunUnityDriver(self.driver, "android")
        self.screenshot_folder = os.getenv('SCREENSHOT_PATH', '/tmp')

    @classmethod
    def teardown_class(self):
        self.altdriver.stop()
        self.driver.quit()

    def test_01_wait_for_scene(self):
        self.altdriver.wait_for_current_scene_to_be("Scene 1 AltUnityDriverTestScene")
        self.driver.save_screenshot(self.screenshot_folder + '/scene_loaded.png')

    def test_02_find_elements(self):
        self.altdriver.find_element('Plane')
        self.altdriver.find_element('Capsule')
        self.driver.save_screenshot(self.screenshot_folder + '/after_capsule_tap.png')

    def test_03_tap_elements(self):
        self.altdriver.find_element('UIButton').tap()
        capsule_info = self.altdriver.wait_for_element_with_text('CapsuleInfo', 'UIButton clicked to jump capsule!')
        assert capsule_info.get_text() == 'UIButton clicked to jump capsule!'
        self.driver.save_screenshot(self.screenshot_folder + "/after_button_press.png")
        
        # tap capsule to make it jump
        self.altdriver.find_element('Capsule').tap()
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!')
        self.driver.save_screenshot(self.screenshot_folder + "/after_capsule_press.png")

# Start of script
if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(DeviceFarmAppiumTests)
    unittest.TextTestRunner(verbosity=2).run(suite)