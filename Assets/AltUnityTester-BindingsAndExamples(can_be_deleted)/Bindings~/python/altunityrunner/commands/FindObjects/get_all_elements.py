from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
class GetAllElements(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver,camera_name,enabled):
        super().__init__(socket,request_separator,request_end,appium_driver)
        self.camera_name=camera_name
        self.enabled=enabled
    
    def execute(self):
        if self.enabled==True:
            data = self.send_data(self.create_command('findObjects','//*', self.camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjects','//*', self.camera_name ,'false'))

        return self.get_alt_elements(data)