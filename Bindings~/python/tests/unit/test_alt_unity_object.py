import json

from altunityrunner.altUnityObject import AltUnityObject


class TestAltUnityObject:

    def test_repr(self):
        element = AltUnityObject(None, {"name": "ElementName", "id": "1", "transformId": "100"})
        reconstructed = eval(repr(element), globals(), {"altdriver": None})

        assert element._altdriver == reconstructed._altdriver
        assert element.name == reconstructed.name
        assert element.id == reconstructed.id
        assert element.transformId == reconstructed.transformId

    def test_str(self):
        data = {
            "name": "ElementName",
            "id": "1",
            "x": "10",
            "y": "20",
            "z": "30",
            "mobileY": "40",
            "type": "ElementType",
            "enabled": "true",
            "worldX": "100.0",
            "worldY": "200.0",
            "worldZ": "300.0",
            "transformParentId": "TransformParrentId",
            "transformId": "TransformId",
            "idCamera": "idCamera"
        }
        element = AltUnityObject(None, data)

        assert str(element) == json.dumps(data)

    def test_to_json(self):
        data = {
            "name": "ElementName",
            "id": "1",
            "x": "10",
            "y": "20",
            "z": "30",
            "mobileY": "40",
            "type": "ElementType",
            "enabled": "true",
            "worldX": "100.0",
            "worldY": "200.0",
            "worldZ": "300.0",
            "transformParentId": "TransformParrentId",
            "transformId": "TransformId",
            "idCamera": "idCamera"
        }

        element = AltUnityObject(None, data)
        assert data == element.to_json()
