from altunityrunner.commands.base_command import BaseCommand


class GetServerVersion(BaseCommand):

    def __init__(self, socket,request_separator,request_end):
        super().__init__(socket,request_separator,request_end)
    
    def execute(self):
        serverVersion=self.send_data(self.create_command('getServerVersion'))
        serverVersion=self.handle_errors(serverVersion)
        f= open("Assets\AltUnityTester-BindingsAndExamples(can_be_deleted)\Bindings~\python\altunityrunner\commands\PythonServerVersion.txt","r")
        driverVersion= f.readline()
        if not driverVersion.Equals(serverVersion):
            raise Exception("Missmatch version. You are using different version of server and driver. Server version: " + serverVersion + " and Driver version: " + driverVersion);
