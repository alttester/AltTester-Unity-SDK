from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.commands.InputActions.move_mouse import MoveMouse
import time
class MoveMouseAndWait(BaseCommand):
    def __init__(self, socket,request_separator,request_end, x, y, duration):
        super().__init__(socket,request_separator,request_end)
        self.x=x
        self.y=y
        self.duration=duration
    
    def execute(self):
        data = MoveMouse(self.socket,self.request_separator,self.request_end,self.x, self.y, self.duration).execute()
        self.handle_errors(data)
        print('Wait for move mouse to finish')
        time.sleep(self.duration)
        action_in_progress = True
        while action_in_progress:
            action_finished = self.send_data(self.create_command('actionFinished'))
            self.handle_errors(action_finished)
            if action_finished is 'Yes':
                break
            elif action_finished != 'No':
                action_in_progress = False
        return self.handle_errors(data)