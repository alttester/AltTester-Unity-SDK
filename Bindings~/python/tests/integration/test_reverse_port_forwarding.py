"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

import unittest

from alttester import By, AltReversePortForwarding, AltDriver


@unittest.skip
class TestReversePortForwarding(unittest.TestCase):

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
        self.alt_driver.load_scene("Scene 2 Draggable Panel")

        self.alt_driver.find_object(By.NAME, "Close Button").tap()
        self.alt_driver.find_object(By.NAME, "Button").tap()

        panel_element = self.alt_driver.wait_for_object(By.NAME, "Panel")
        self.assertTrue(panel_element.enabled)
