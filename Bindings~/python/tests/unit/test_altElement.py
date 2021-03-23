from unittest.mock import MagicMock
from unittest import TestCase
from altunityrunner import AltElement
import altunityrunner


class CommandsTests(TestCase):
    def test_altElement_repr(self):
        element = AltElement(
            None, "{\"name\": \"ElementName\", \"id\": \"1\" , \"transformId\": \"100\"}")

        reconstructed = eval(repr(element), globals(), {"driver": None})

        self.assertEqual(element.alt_unity_driver,
                         reconstructed.alt_unity_driver)
        self.assertEqual(element.name, reconstructed.name)
        self.assertEqual(element.id, reconstructed.id)
        self.assertEqual(element.transformId, reconstructed.transformId)
