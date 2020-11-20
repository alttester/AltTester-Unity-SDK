from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.commands.InputActions.tilt import Tilt
from loguru import logger
import time


class TiltAndWait(BaseCommand):
    def __init__(self, socket, request_separator, request_end, x, y, z, duration_in_secs):
        super(TiltAndWait, self).__init__(
            socket, request_separator, request_end)
        self.x = x
        self.y = y
        self.z = z
        self.duration_in_secs = duration_in_secs

    def execute(self):
        data = Tilt(self.socket, self.request_separator, self.request_end, self.x,
                    self.y, self.z, self.duration_in_secs).execute()
        self.handle_errors(data)
        logger.debug('Wait for tilt to finish')
        time.sleep(self.duration_in_secs)
        tilt_in_progress = True
        while tilt_in_progress:
            tilt_finished = self.send_command('actionFinished')
            self.handle_errors(tilt_finished)
            if tilt_finished == 'Yes':
                break
            elif tilt_finished != 'No':
                tilt_in_progress = False
        return self.handle_errors(data)
