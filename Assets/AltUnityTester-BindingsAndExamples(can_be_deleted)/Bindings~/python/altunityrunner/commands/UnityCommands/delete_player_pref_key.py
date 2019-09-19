from altunityrunner.commands.base_command import BaseCommand

class DeletePlayerPrefKey(BaseCommand):
    def __init__(self, socket,request_separator,request_end,key_name):
        super().__init__(socket,request_separator,request_end)
        self.key_name=key_name
    
    def execute(self):
        print('Delete Player Pref for key: ' + self.key_name)        
        data = self.send_data(self.create_command('deleteKeyPlayerPref', self.key_name ))
        return self.handle_errors(data)