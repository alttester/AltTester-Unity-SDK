from altunityrunner.commands.base_command import BaseCommand
from loguru import logger
import time


class PressKey(BaseCommand):
    def __init__(self, socket, request_separator, request_end, keyName, power, duration):
        super(PressKey, self).__init__(socket, request_separator, request_end)
        self.keyName = keyName
        self.power = power
        self.duration = duration

    def execute(self):
        logger.debug('Press key: ' + self.keyName)
        data = self.send_command(
            'pressKeyboardKey', self.keyName, self.power, self.duration)
        return self.handle_errors(data)
