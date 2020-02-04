from altunityrunner.commands.base_command import BaseCommand
import subprocess
import re
import os
from altunityrunner.__version__ import VERSION
import warnings
THIS_FOLDER = os.path.dirname(os.path.abspath(__file__))
my_file = os.path.join(THIS_FOLDER, 'PythonServerVersion.txt')

class GetServerVersion(BaseCommand):

    def __init__(self, socket,request_separator,request_end):
        super().__init__(socket,request_separator,request_end)
    
    def execute(self):
        serverVersion=self.send_data(self.create_command('getServerVersion'))
        serverVersion=self.handle_errors(serverVersion)
        
        if not VERSION==serverVersion:
            message="Version mismatch. You are using different version of server and driver. Server version: " + serverVersion + " and Driver version: " + VERSION
            warnings.warn(message)
            super().write_to_log_file(message)
            return "Version mismatch"
        else:
            return "Ok"
