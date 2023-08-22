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

from alttester.commands.base_command import BaseCommand


class SetStaticProperty(BaseCommand):

    def __init__(self, connection, component_name, property_name, assembly_name, value):
        super().__init__(connection, "setObjectComponentProperty")

        self.alt_object = None

        self.component_name = component_name
        self.property_name = property_name
        self.assembly_name = assembly_name
        self.value = value

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altObject": self.alt_object,
            "component": self.component_name,
            "property": self.property_name,
            "assembly": self.assembly_name,
            "value": self.value
        })

        return parameters

    def execute(self):
        return self.send()
