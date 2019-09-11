from altunityrunner.commands.base_command import BaseCommand
import time
class CloseConnection(BaseCommand):
    def __init__(self, socket,requestSeparator,requestEnd):
        super().__init__(socket,requestSeparator,requestEnd)
    
    def execute(self):
        data = self.send_data(self.create_command('closeConnection'))
        print('Sent close connection command...')
        time.sleep(1)
        self.socket.close()
        print('Socket closed.')  