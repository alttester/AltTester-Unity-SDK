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

    @classmethod
    def teardown_class(cls):
        cls.altdriver.stop()
        cls.driver.quit()

    def test_devicefarm(self):
        print("test1")
        self.altdriver.wait_for_current_scene_to_be("Scene 1 AltUnityDriverTestScene")
        screenshot_folder = os.getenv('SCREENSHOT_PATH', '/tmp')
        self.driver.save_screenshot(screenshot_folder + '/devicefarm.png')
        sleep(5)

    def test_devicefarm2(self):
        print("test2")
        self.altdriver.find_element("Capsule").tap()
        screenshot_folder = os.getenv('SCREENSHOT_PATH', '/tmp')
        self.driver.save_screenshot(screenshot_folder + '/devicefarm2.png')
        sleep(5)

# Start of script
if __name__ == '__main__':
    unittest.main()
    # suite = unittest.TestLoader().loadTestsFromTestCase(DeviceFarmAppiumTests)
    # unittest.TextTestRunner(verbosity=2).run(suite)