
# For more info on the setup, check out our docs https://alttester.com/docs/sdk/latest/pages/get-started.html#write-and-execute-first-test-for-your-app
import unittest
from alttester import By, AltKeyCode, AltDriver
import loguru


class MyTests(unittest.TestCase):

    sky_atmosphere_path = "/SkyAtmosphere"

    @classmethod
    def setUp(self):
        self.alt_driver = AltDriver(
            host="127.0.0.1", port=13000, app_name="__default__", enable_logging=True)
        # You might want to load the scene here
        # self.alt_driver.load_scene("Scene1", True)

    @classmethod
    def tearDown(self):
        self.alt_driver.stop()

    def test(self):
        sky_atmosphere = self.alt_driver.wait_for_object(
            By.PATH, self.sky_atmosphere_path, timeout=20)
