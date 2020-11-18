from altunityrunner.commands.InputActions.multi_point_swipe import MultipointSwipe
from altunityrunner.commands.base_command import BaseCommand
from loguru import logger
import time


class MultipointSwipeAndWait(BaseCommand):
    def __init__(self, socket, request_separator, request_end, positions, duration_in_secs):
        super(MultipointSwipeAndWait, self).__init__(
            socket, request_separator, request_end)
        self.positions = positions
        self.duration_in_secs = duration_in_secs

    def execute(self):
        data = MultipointSwipe(self.socket, self.request_separator,
                               self.request_end, self.positions, self.duration_in_secs).execute()
        self.handle_errors(data)
        logger.debug('Wait for moving touch to finish')
        time.sleep(self.duration_in_secs)
        swipe_in_progress = True
        while swipe_in_progress:
            swipe_finished = self.send_command('actionFinished')
            self.handle_errors(swipe_finished)
            if swipe_finished == 'Yes':
                break
            elif swipe_finished != 'No':
                swipe_in_progress = False
        return self.handle_errors(data)
