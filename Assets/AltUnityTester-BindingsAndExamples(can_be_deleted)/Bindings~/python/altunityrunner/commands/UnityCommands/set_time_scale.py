from altunityrunner.commands.base_command import BaseCommand

class SetTimeScale(BaseCommand):
    def __init__(self, socket,request_separator,request_end,time_scale):
        super().__init__(socket,request_separator,request_end)
        self.time_scale=time_scale
    
    def execute(self):
        print('Set time scale to: ' + str(self.time_scale))
        data = self.send_data(self.create_command('setTimeScale', str(self.time_scale)))
        if (data == 'Ok'):
            print('Time scale set to: ' + str(self.time_scale))
            return data
        return None