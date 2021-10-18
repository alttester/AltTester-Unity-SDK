import os
import time
import unittest

from appium import webdriver

from altunityrunner import AltUnityDriver, By


def PATH(p):
    return os.path.abspath(os.path.join(os.path.dirname(__file__), p))


class SampleAppiumTest(unittest.TestCase):
    altdriver = None
    platform = "android"  # set to `ios` or `android` to change platform
    iProxyProcessID = -1

    @classmethod
    def setUpClass(cls):
        cls.desired_caps = {}

        if cls.platform == "android":
            cls.setup_android()
        else:
            cls.setup_ios()

        cls.driver = webdriver.Remote('http://localhost:4723/wd/hub', cls.desired_caps)
        time.sleep(10)

        cls.altdriver = AltUnityDriver(port=13010, timeout=None, enable_logging=True)

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()
        cls.driver.quit()

    @classmethod
    def setup_android(cls):
        cls.desired_caps['platformName'] = 'Android'
        cls.desired_caps['deviceName'] = 'device'
        cls.desired_caps['app'] = PATH('../../../../sampleGame.apk')

    @classmethod
    def setup_ios(cls):
        cls.desired_caps['platformName'] = 'iOS'
        cls.desired_caps['deviceName'] = 'iPhone5'
        cls.desired_caps['automationName'] = 'XCUITest'
        cls.desired_caps['app'] = PATH('../../../../sampleGame.ipa')

    def test_find_object_and_tap(self):
        # tap UIButton to make capsule jump
        self.altdriver.find_object(By.NAME, 'UIButton').tap()
        capsule_info = self.altdriver.wait_for_object(By.PATH, '//CapsuleInfo[@text=UIButton clicked to jump capsule!]')
        self.assertEqual('UIButton clicked to jump capsule!', capsule_info.get_text())

        # tap capsule to make it jump
        self.altdriver.find_object(By.NAME, 'Capsule').tap()
        capsule_info = self.altdriver.wait_for_object(By.PATH, '//CapsuleInfo[@text=Capsule was clicked to jump!]')
        self.assertEqual('Capsule was clicked to jump!', capsule_info.get_text())
