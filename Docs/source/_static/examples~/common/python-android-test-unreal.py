import unittest

from alttester import *


class MyFirstTest(unittest.TestCase):

    alt_driver = None

    @classmethod
    def setUpClass(cls):
        AltReversePortForwarding.reverse_port_forwarding_android()
        cls.alt_driver = AltDriver()

    @classmethod
    def tearDownClass(cls):
        cls.alt_driver.stop()
        AltReversePortForwarding.remove_reverse_port_forwarding_android()

    def test_open_close_panel(self):
        self.alt_driver.load_scene("MainMenu")

        self.alt_driver.find_object(By.NAME, "Close Button").click()
        self.alt_driver.find_object(By.NAME, "Button").click()

        panel_element = self.alt_driver.wait_for_object(By.NAME, "Panel")
        self.assertTrue(panel_element.enabled)
