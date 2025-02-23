"""
    Copyright(C) 2025 Altom Consulting

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

from alttester.commands.base_command import BaseCommand


class EndTouch(BaseCommand):

    def __init__(self, connection, finger_id):
        super(EndTouch, self).__init__(connection, "endTouch")

        self.finger_id = finger_id

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "fingerId": self.finger_id,
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)
