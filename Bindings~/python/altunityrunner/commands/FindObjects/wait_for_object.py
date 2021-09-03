import time

from loguru import logger

from altunityrunner.by import By
from altunityrunner.commands.base_command import Command
from altunityrunner.commands.FindObjects.find_object import FindObject
from altunityrunner.exceptions import NotFoundException, WaitTimeOutException, InvalidParameterTypeException


class WaitForObject(Command):

    def __init__(self, connection, by, value, camera_by, camera_path, timeout, interval, enabled):
        self.connection = connection

        if by not in By:
            raise InvalidParameterTypeException()

        if camera_by not in By:
            raise InvalidParameterTypeException()

        self.by = by
        self.value = value
        self.camera_by = camera_by
        self.camera_path = camera_path
        self.timeout = timeout
        self.interval = interval
        self.enabled = enabled

    def execute(self):
        t = 0
        alt_element = None

        while (t <= self.timeout):
            try:
                alt_element = FindObject.run(
                    self.connection,
                    self.by, self.value, self.camera_by, self.camera_path, self.enabled
                )

                break
            except NotFoundException:
                logger.debug("Waiting for element {}...".format(self.value))
                time.sleep(self.interval)
                t += self.interval

        if t >= self.timeout:
            raise WaitTimeOutException("Element {} not found after {} seconds".format(self.value, self.timeout))

        return alt_element
