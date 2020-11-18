from altunityrunner.commands.base_command import BaseCommand
import time


class EnableLogging(BaseCommand):

    def __init__(self, socket, request_separator, request_end, log_flag):
        super(EnableLogging, self).__init__(
            socket, request_separator, request_end)
        self.log_flag = log_flag

    def execute(self):
        if(self.log_flag):
            self.send_command('enableLogging', 'true')
        else:
            self.send_command('enableLogging', 'false')
