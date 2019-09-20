from altunityrunner.commands.base_command import BaseCommand

class DeletePlayerPref(BaseCommand):
    def __init__(self, socket,request_separator,request_end):
        super().__init__(socket,request_separator,request_end)
    
    def execute(self):
        print('Delete all Player Prefs')
        data = self.send_data(self.create_command('deletePlayerPref'))
        return self.handle_errors(data)