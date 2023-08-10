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

import time
from loguru import logger
from alttester.commands.base_command import Command
from alttester.exceptions import NotFoundException, WaitTimeOutException


class WaitForComponentProperty(Command):
    def __init__(self, component_name, property_name,
                 property_value, assembly, altObject, timeout=20, interval=0.5):
        self.component_name = component_name
        self.property_name = property_name
        self.property_value = property_value
        self.assembly = assembly
        self.timeout = timeout
        self.interval = interval
        self.altObject = altObject

    def execute(self):
        t = 0
        while (t <= self.timeout):
            try:
                property_found = self.altObject.get_component_property(
                    self.component_name, self.property_name, self.assembly)
                if (property_found == self.property_value):
                    break
            except NotFoundException:
                logger.debug("Waiting for property {}...", self.property_name)
                time.sleep(self.interval)
                t += self.interval

        if t >= self.timeout:
            raise WaitTimeOutException("Property {} not found after {} seconds"
                                       .format(
                                           self.property_name, self.timeout))

        return property_found
