from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.commands.InputActions.scroll_mouse import ScrollMouse
import time
class ScrollMouseAndWait(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd, speed, duration):
        super().__init__(socket,requestSeparator,requestEnd)
        self.speed=speed
        self.duration=duration
    
    def execute(self):
        data = ScrollMouse(self.socket,self.requestSeparator,self.requestEnd,self.speed, self.duration).execute()
        self.handle_errors(data)
        print('Wait for scroll mouse to finish')
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