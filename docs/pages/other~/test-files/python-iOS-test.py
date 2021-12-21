# -*- coding: UTF-8

import os
import unittest
import sys
from altunityrunner import *


class MyFirstTest(unittest.TestCase):

    altUnityDriver = None

    @classmethod
    def setUpClass(cls):
        AltUnityPortForwarding.forward_ios()
        cls.altUnityDriver = AltUnityDriver()

    @classmethod
    def tearDownClass(cls):
        cls.altUnityDriver.stop()
        AltUnityPortForwarding.kill_all_iproxy_process()

    def test_open_close_panel(self):
        self.altUnityDriver.load_scene('Scene 2 Draggable Panel')

        self.altUnityDriver.find_object(By.NAME, "Close Button").tap()
        self.altUnityDriver.find_object(By.NAME, "Button").tap()

        panelElement = self.altUnityDriver.wait_for_object(By.NAME, "Panel")
        self.assertTrue(panelElement.enabled)


if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(MyFirstTest)
    result = unittest.TextTestRunner(verbosity=2).run(suite)
    sys.exit(not result.wasSuccessful())
