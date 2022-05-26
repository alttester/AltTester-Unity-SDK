import time

from loguru import logger

from altunityrunner.by import By
from altunityrunner.commands.base_command import Command
from altunityrunner.commands.FindObjects.find_object_which_contains import FindObjectWhichContains
from altunityrunner.exceptions import NotFoundException, WaitTimeOutException, InvalidParameterTypeException


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
