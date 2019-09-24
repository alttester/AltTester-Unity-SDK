from altunityrunner.commands.base_command import BaseCommand
import time
class EnableDebugging(BaseCommand):

    def __init__(self, socket,request_separator,request_end,debug_flag):
        super().__init__(socket,request_separator,request_end)
        self.debug_flag=debug_flag
    
    def execute(self):
        if(self.debug_flag):
            self.send_data(self.create_command('enableDebug','true'))
        else:
            self.send_data(self.create_command('enableDebug','false')) 