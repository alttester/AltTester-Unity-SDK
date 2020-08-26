# -*- coding: UTF-8

import os
import unittest
import sys
import json
import time
from altunityrunner import *


class MyFirstTest(unittest.TestCase):
    altdriver = None
    iProxyProcessID = -1

    @classmethod
    def setUpClass(cls):
        iOSPortForwarding = AltUnityiOSPortForwarding()
        cls.iProxyProcessID = iOSPortForwarding.forward_port_device()

        cls.altdriver = AltUnityDriver()

    @classmethod
    def tearDownClass(cls):
        cls.altdriver.stop()

        iOSPortForwarding = AltUnityiOSPortForwarding()
        iOSPortForwarding.kill_iproxy_process(cls.iProxyProcessID)

    def test_open_close_panel(self):
        self.altdriver.load_scene('Scene 2 Draggable Panel')

        self.altdriver.find_object(By.NAME, "Close Button").tap()
        self.altdriver.find_object(By.NAME, "Button").tap()

        panelElement = self.altdriver.wait_for_object(By.NAME, "Panel")
        self.assertTrue(panelElement.enabled)


if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(MyFirstTest)
    result = unittest.TextTestRunner(verbosity=2).run(suite)
    sys.exit(not result.wasSuccessful())
