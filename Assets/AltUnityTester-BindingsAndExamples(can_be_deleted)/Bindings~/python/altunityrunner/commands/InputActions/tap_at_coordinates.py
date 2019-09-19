from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
class TapAtCoordinates(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver,x,y):
        super().__init__(socket,request_separator,request_end,appium_driver)
        self.x=x
        self.y=y
    
    def execute(self):
        data=self.send_data(self.create_command('tapScreen',self.x, self.y))
        if 'error:notFound' in data:
            return None
        return self.get_alt_element(data)