from altunityrunner.commands.base_command import BaseCommand

class GetTimeScale(BaseCommand):
    def __init__(self, socket,request_separator,request_end):
        super().__init__(socket,request_separator,request_end)
    
    def execute(self):
        print('Get time scale')
        data = self.send_data(self.create_command('getTimeScale'))
        if (data != '' and 'error:' not in data):
            print('Got time scale: ' + data)
            return float(data)
        return None