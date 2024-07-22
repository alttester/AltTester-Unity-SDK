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
from alttester.playerpref import PlayerPrefKeyType
from alttester.exceptions import InvalidParameterTypeException


class GetPlayerPrefKey(BaseCommand):

    def __init__(self, connection, key_name, key_type):
        super().__init__(connection, "getKeyPlayerPref")

        if key_type not in PlayerPrefKeyType and key_type not in PlayerPrefKeyType.value():
            raise InvalidParameterTypeException(
                parameter_name="key_type",
                expected_types=[PlayerPrefKeyType],
                received_type=type(key_type)
            )

        self.key_name = key_name
        self.key_type = key_type

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "keyName": self.key_name,
            "keyType": str(self.key_type)
        })

        return parameters

    def execute(self):
        return self.send()
