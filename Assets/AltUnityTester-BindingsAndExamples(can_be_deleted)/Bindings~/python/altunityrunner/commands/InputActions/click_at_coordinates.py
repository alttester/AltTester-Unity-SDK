from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements

class ClickAtCoordinates(CommandReturningAltElements):
    def __init__(self, socket,requestSeparator,requestEnd, x, y):
        super().__init__(socket,requestSeparator,requestEnd)
        self.x=x
        self.y=y
    
    def execute(self):
        data = self.send_data(self.create_command("clickScreenOnXY",self.x,self.y ))
        print('Clicked at ' + str(self.x) + ', ' + str(self.y))
        return data
