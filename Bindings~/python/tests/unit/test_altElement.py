import unittest
import json

import altunityrunner
from altunityrunner import AltElement


class AltElementTests(unittest.TestCase):

    def test_repr(self):
        element = AltElement(None, '{"name": "ElementName", "id": "1" , "transformId": "100"}')
        reconstructed = eval(repr(element), globals(), {"driver": None})

        self.assertEqual(element.alt_unity_driver, reconstructed.alt_unity_driver)
        self.assertEqual(element.name, reconstructed.name)
        self.assertEqual(element.id, reconstructed.id)
        self.assertEqual(element.transformId, reconstructed.transformId)

    def test_str(self):
        json_data = '{"name": "ElementName", "id": "1" , "transformId": "100"}'
        element = AltElement(None, json_data)

        self.assertEqual(str(element), element.toJSON())

    def test_toJSON(self):
        data = {
            "name": "ElementName",
            "id": "1",
            "x": "10",
            "y": "20",
            "z": "30",
            "mobileY": "40",
            "type": "ElementType",
            "enabled": "true",
            "worldX": "100",
            "worldY": "200",
            "worldZ": "300",
            "parentId": "0",
            "transformParentId": "TransformParrentId",
            "transformId": "TransformId",
            "idCamera": "idCamera"
        }

        json_data = json.dumps(data)
        element = AltElement(None, json_data)

        self.assertEqual(element.toJSON(), json_data)
