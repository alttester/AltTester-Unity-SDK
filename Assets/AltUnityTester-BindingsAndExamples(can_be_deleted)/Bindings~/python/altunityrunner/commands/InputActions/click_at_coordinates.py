from altunityrunner.commands.base_command import BaseCommand

class ClickAtCoordinates(BaseCommand):
    def __init__(self, socket,request_separator,request_end, x, y):
        super().__init__(socket,request_separator,request_end)
        self.x=x
        self.y=y
    
    def execute(self):
        data = self.send_data(self.create_command("clickScreenOnXY",self.x,self.y ))
        print('Clicked at ' + str(self.x) + ', ' + str(self.y))
        return data
