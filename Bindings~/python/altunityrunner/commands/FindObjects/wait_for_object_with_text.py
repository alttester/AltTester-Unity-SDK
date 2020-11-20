from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.altUnityExceptions import WaitTimeOutException
from altunityrunner.commands.FindObjects.find_object import FindObject
from loguru import logger
import time


class WaitForObjectWithText(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end, by, value, text, camera_by, camera_path, timeout, interval, enabled):
        super(WaitForObjectWithText, self).__init__(
            socket, request_separator, request_end)
        self.by = by
        self.value = value
        self.text = text
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
                alt_element = FindObject(self.socket, self.request_separator, self.request_end,
                                         self.by, self.value, self.camera_by, self.camera_path, self.enabled).execute()
                if alt_element.get_text() == self.text:
                    break
                raise Exception('Not the wanted text')
            except Exception as e:
                logger.debug('Waiting for element ' +
                             str(self.value) + ' to have text ' + str(self.text))
                time.sleep(self.interval)
                t += self.interval
        if t >= self.timeout:
            raise WaitTimeOutException('Element ' + str(self.value) + ' should have text `' + str(self.text) +
                                       '` but has `' + alt_element.get_text() + '` after ' + str(self.timeout) + ' seconds')
        return alt_element
