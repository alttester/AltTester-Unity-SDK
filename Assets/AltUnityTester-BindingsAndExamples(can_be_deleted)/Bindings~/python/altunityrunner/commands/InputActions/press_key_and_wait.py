from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.commands.InputActions.press_key import PressKey
import time
class PressKeyAndWait(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd, keyName,power,duration):
        super().__init__(socket,requestSeparator,requestEnd)
        self.keyName=keyName
        self.power=power
        self.duration=duration
    
    def execute(self):
        data = PressKey(self.socket,self.requestSeparator,self.requestEnd,self.keyName,self.power,self.duration).execute()
        self.handle_errors(data)
        print('Wait for press key to finish')
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