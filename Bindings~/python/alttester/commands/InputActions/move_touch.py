"""
    Copyright(C) 2023 Altom Consulting

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

from alttester.commands.base_command import validate_coordinates, BaseCommand


class MoveTouch(BaseCommand):

    def __init__(self, connection, finger_id, coordinates):
        super().__init__(connection, "moveTouch")

        self.coordinates = validate_coordinates(coordinates)
        self.finger_id = finger_id

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "fingerId": self.finger_id,
            "coordinates": self.coordinates
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)
