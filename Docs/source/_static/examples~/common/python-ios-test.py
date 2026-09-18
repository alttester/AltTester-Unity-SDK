# On iOS there is no driver-side reverse port forwarding: IProxy cannot set it up.
# Connect over the network instead - see "In case of iOS" in Advanced Usage for the
# personal-hotspot-over-USB workaround. The test code itself is unchanged; only the
# IP configured in the instrumented app differs.
import unittest
from alttester import AltDriver, By


class MyFirstTest(unittest.TestCase):
    alt_driver = None

    @classmethod
    def setUpClass(cls):
        cls.alt_driver = AltDriver()

    @classmethod
    def tearDownClass(cls):
        cls.alt_driver.stop()

    def test_start_game(self):
        self.alt_driver.load_scene("Scene 2 Draggable Panel")

        self.alt_driver.find_object(By.NAME, "Close Button").tap()
        self.alt_driver.find_object(By.NAME, "Button").tap()

        panel_element = self.alt_driver.wait_for_object(By.NAME, "Panel")
        self.assertTrue(panel_element.enabled)
