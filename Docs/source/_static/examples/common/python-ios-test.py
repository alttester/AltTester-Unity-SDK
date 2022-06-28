import unittest

from altunityrunner import *


class MyFirstTest(unittest.TestCase):

    altdriver = None

    @classmethod
    def setUpClass(cls):
        AltUnityPortForwarding.forward_ios()
        cls.altdriver = AltUnityDriver()

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()
        AltUnityPortForwarding.kill_all_iproxy_process()

    def test_open_close_panel(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        self.altdriver.find_object(By.NAME, "Close Button").tap()
        self.altdriver.find_object(By.NAME, "Button").tap()

        panel_element = self.altdriver.wait_for_object(By.NAME, "Panel")
        self.assertTrue(panel_element.enabled)
