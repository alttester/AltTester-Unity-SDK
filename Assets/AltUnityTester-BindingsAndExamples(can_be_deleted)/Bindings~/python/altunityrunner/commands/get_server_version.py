from altunityrunner.commands.base_command import BaseCommand
import os
THIS_FOLDER = os.path.dirname(os.path.abspath(__file__))
my_file = os.path.join(THIS_FOLDER, 'PythonServerVersion.txt')

class GetServerVersion(BaseCommand):

    def __init__(self, socket,request_separator,request_end):
        super().__init__(socket,request_separator,request_end)
    
    def execute(self):
        serverVersion=self.send_data(self.create_command('getServerVersion'))
        serverVersion=self.handle_errors(serverVersion)
        f= open(my_file,"r")
        driverVersion= f.readline()
        if not driverVersion==serverVersion:
            raise Exception("Mismatch version. You are using different version of server and driver. Server version: " + serverVersion + " and Driver version: " + driverVersion);
