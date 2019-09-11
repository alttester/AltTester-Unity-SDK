from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements

class DeletePlayerPrefKey(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd,key_name):
        super().__init__(socket,requestSeparator,requestEnd)
        self.key_name=key_name
    
    def execute(self):
        print('Delete Player Pref for key: ' + self.key_name)        
        data = self.send_data(self.create_command('deleteKeyPlayerPref', self.key_name ))
        return self.handle_errors(data)