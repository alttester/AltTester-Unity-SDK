import time

from loguru import logger

from altunityrunner.by import By
from altunityrunner.commands.base_command import Command
from altunityrunner.commands.FindObjects.find_object import FindObject
from altunityrunner.exceptions import NotFoundException, WaitTimeOutException, InvalidParameterTypeException


class WaitForObjectToNotBePresent(Command):

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

        while (t <= self.timeout):
            try:
                logger.debug("Waiting for element {} to not be present...", self.value)

                FindObject(
                    self.connection,
                    self.by, self.value, self.camera_by, self.camera_value, self.enabled
                ).execute()

                time.sleep(self.interval)
                t += self.interval
            except NotFoundException as ex:
                logger.debug(ex)
                break

        if t > self.timeout:
            logger.debug("WaitTimeOutException")
            raise WaitTimeOutException("Element {} still found after {} seconds".format(
                self.value,
                self.timeout
            ))
