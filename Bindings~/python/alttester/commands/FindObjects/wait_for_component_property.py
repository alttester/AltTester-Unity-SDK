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
from alttester.exceptions import WaitTimeOutException


class WaitForComponentProperty(Command):
    def __init__(
        self,
        component_name,
        property_name,
        property_value,
        assembly,
        altObject,
        timeout=20,
        interval=0.5,
    ):
        self.component_name = component_name
        self.property_name = property_name
        self.property_value = property_value
        self.assembly = assembly
        self.timeout = timeout
        self.interval = interval
        self.altObject = altObject

    def execute(self):
        t = 0
        exception = None
        property_found = None
        while t <= self.timeout:
            try:
                property_found = self.altObject.get_component_property(
                    self.component_name, self.property_name, self.assembly
                )
                if property_found == self.property_value:
                    return property_found
            except Exception as ex:
                exception = ex

            logger.debug("Waiting for property {}...", self.property_name)
            time.sleep(self.interval)
            t += self.interval
        if exception:
            raise WaitTimeOutException(
                "After {} seconds, exception was: {} for component: {} and property {}".format(
                    self.timeout, exception, self.component_name, self.property_name
                )
            )
        raise WaitTimeOutException(
            "Property {} was {}, not {} as expected, after {} seconds".format(
                self.property_name, property_found, self.property_value, self.timeout
            )
        )
