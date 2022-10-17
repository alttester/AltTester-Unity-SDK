import time

from loguru import logger

from alttester.by import By
from alttester.commands.base_command import Command
from alttester.commands.FindObjects.find_object import FindObject
from alttester.exceptions import NotFoundException, WaitTimeOutException, InvalidParameterTypeException


class WaitForObject(Command):

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
        alt_object = None

        while (t <= self.timeout):
            try:
                alt_object = FindObject(
                    self.connection,
                    self.by, self.value, self.camera_by, self.camera_value, self.enabled
                ).execute()

                break
            except NotFoundException:
                logger.debug("Waiting for element {}...", self.value)
                time.sleep(self.interval)
                t += self.interval

        if t >= self.timeout:
            raise WaitTimeOutException("Element {} not found after {} seconds".format(self.value, self.timeout))

        return alt_object
