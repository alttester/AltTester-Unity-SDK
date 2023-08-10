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

from alttester.by import By
from alttester.commands.base_command import Command
from alttester.commands.FindObjects.find_object_which_contains import FindObjectWhichContains
from alttester.exceptions import NotFoundException, WaitTimeOutException, InvalidParameterTypeException


class WaitForObjectWhichContains(Command):

    def __init__(self, connection, by, value, camera_by, camera_value, timeout, interval, enabled):
        self.connection = connection

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
        self.timeout = timeout
        self.interval = interval
        self.enabled = enabled

    def execute(self):
        t = 0
        alt_unity_object = None

        while (t <= self.timeout):
            try:
                alt_unity_object = FindObjectWhichContains(
                    self.connection,
                    self.by, self.value, self.camera_by, self.camera_value, self.enabled
                ).execute()

                break
            except NotFoundException:
                logger.debug("Waiting for element where name contains {}...", self.value)
                time.sleep(self.interval)
                t += self.interval

        if t >= self.timeout:
            raise WaitTimeOutException("Element where name contains {} not found after {} seconds".format(
                self.value,
                self.timeout
            ))

        return alt_unity_object
