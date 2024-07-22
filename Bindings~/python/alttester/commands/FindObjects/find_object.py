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

from alttester.commands.base_command import BaseCommand
from alttester.exceptions import InvalidParameterTypeException
from alttester.by import By


class FindObject(BaseCommand):

    def __init__(self, connection, by, value, camera_by, camera_value, enabled):
        super().__init__(connection, "findObject")

        if by not in By:
            raise InvalidParameterTypeException(
                parameter_name="by",
                expected_types=[By],
                received_type=type(by)
            )

        if camera_by not in By:
            raise InvalidParameterTypeException(
                parameter_name="camera_by",
                expected_types=[By],
                received_type=type(camera_by)
            )

        self.by = by
        self.value = value
        self.camera_by = camera_by
        self.camera_value = camera_value
        self.enabled = enabled

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "by": str(self.by),
            "path": self.get_path(self.by, self.value),
            "cameraBy": str(self.camera_by),
            "cameraPath": self.get_path(self.camera_by, self.camera_value),
            "enabled": self.enabled,
        })

        return parameters

    def execute(self):
        return self.send()
