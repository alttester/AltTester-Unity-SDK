import unittest

from altunityrunner import *


class MyFirstTest(unittest.TestCase):
    altdriver = None

    @classmethod
    def setUpClass(cls):
        cls.altdriver = AltUnityDriver()

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()

    def test_open_close_panel(self):
        self.altdriver.load_scene("Scene 2 Draggable Panel")

        self.altdriver.find_object(By.NAME, "Close Button").tap()
        self.altdriver.find_object(By.NAME, "Button").tap()

        panel_element = self.altdriver.wait_for_object(By.NAME, "Panel")
        self.assertTrue(panel_element.enabled)
