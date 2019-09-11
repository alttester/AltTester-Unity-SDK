from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
import time
class PressKey(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd, keyName,power,duration):
        super().__init__(socket,requestSeparator,requestEnd)
        self.keyName=keyName
        self.power=power
        self.duration=duration
    
    def execute(self):
        print ('Press key: ' + self.keyName)
        data = self.send_data(self.create_command('pressKeyboardKey',self.keyName,self.power, self.duration ))
        return self.handle_errors(data)