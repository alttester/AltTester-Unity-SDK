import json

from altunityrunner import AltElement


class TestAltElement:

    def test_repr(self):
        element = AltElement(None, {"name": "ElementName", "id": "1" , "transformId": "100"})
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
            "worldX": "100",
            "worldY": "200",
            "worldZ": "300",
            "transformParentId": "TransformParrentId",
            "transformId": "TransformId",
            "idCamera": "idCamera"
        }
        element = AltElement(None, data)

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
            "worldX": "100",
            "worldY": "200",
            "worldZ": "300",
            "transformParentId": "TransformParrentId",
            "transformId": "TransformId",
            "idCamera": "idCamera"
        }

        element = AltElement(None, data)
        assert data == element.to_json()