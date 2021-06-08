import time

from loguru import logger

from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import WaitTimeOutException
from altunityrunner.commands.FindObjects.find_object_which_contains import FindObjectWhichContains


class WaitForObjectWhichContains(CommandReturningAltElements):

    def __init__(self, socket, request_separator, request_end, by, value, camera_by, camera_path, timeout, interval,
                 enabled):
        super(WaitForObjectWhichContains, self).__init__(socket, request_separator, request_end)
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
                alt_element = FindObjectWhichContains(
                    self.socket, self.request_separator, self.request_end,
                    self.by, self.value, self.camera_by, self.camera_path, self.enabled
                ).execute()

                break
            except Exception:
                logger.debug("Waiting for element where name contains {}...".format(self.value))
                time.sleep(self.interval)
                t += self.interval
        if t >= self.timeout:
            raise WaitTimeOutException("Element where name contains {} not found after {} seconds".format(
                self.value,
                self.timeout
            ))

        return alt_element
