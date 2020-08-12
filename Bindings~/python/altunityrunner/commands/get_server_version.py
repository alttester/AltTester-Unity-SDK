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
        serverVersion = self.send_data(self.create_command('getServerVersion'))
        if serverVersion == 'error:unknownError':
            write_warning(True)
            return "Version mismatch"

        serverVersion = self.handle_errors(serverVersion)

        if not VERSION == serverVersion:
            self.write_warning(False, serverVersion)
            return "Version mismatch! Server version was:"+str(serverVersion)
        else:
            return str(serverVersion)

    def write_warning(self, is_earlier, serverVersion=""):
        message = ""
        if is_earlier:
            message = "Version mismatch. You are using different versions of server and driver. Server version is earlier then 1.5.3 and Driver version: " + VERSION
        else:
            message = "Version mismatch. You are using different versions of server and driver. Server version: " + \
                serverVersion + " and Driver version: " + VERSION
        warnings.warn(message)
        super().write_to_log_file(message)
