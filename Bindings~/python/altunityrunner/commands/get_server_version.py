from altunityrunner.commands.base_command import BaseCommand
import subprocess
import re
import os
from altunityrunner.__version__ import VERSION
import warnings
THIS_FOLDER = os.path.dirname(os.path.abspath(__file__))
my_file = os.path.join(THIS_FOLDER, 'PythonServerVersion.txt')


class GetServerVersion(BaseCommand):

    def __init__(self, socket, request_separator, request_end):
        super(GetServerVersion, self).__init__(
            socket, request_separator, request_end)

    def execute(self):
        serverVersion = self.send_command('getServerVersion')
        self.handle_errors(serverVersion)
        return serverVersion
