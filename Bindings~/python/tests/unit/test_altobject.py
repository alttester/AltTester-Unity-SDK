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

import json

from alttester.altobject import AltObject


class TestAltObject:

    def test_repr(self):
        element = AltObject(
            None, {"name": "ElementName", "id": "1", "transformId": "100"})
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
        element = AltObject(None, data)

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

        element = AltObject(None, data)
        assert data == element.to_json()
