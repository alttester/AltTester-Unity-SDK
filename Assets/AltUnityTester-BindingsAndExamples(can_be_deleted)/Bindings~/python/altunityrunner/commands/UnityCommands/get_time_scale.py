from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements

class GetTimeScale(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd):
        super().__init__(socket,requestSeparator,requestEnd)
    
    def execute(self):
        print('Get time scale')
        data = self.send_data(self.create_command('getTimeScale'))
        if (data != '' and 'error:' not in data):
            print('Got time scale: ' + data)
            return float(data)
        return None