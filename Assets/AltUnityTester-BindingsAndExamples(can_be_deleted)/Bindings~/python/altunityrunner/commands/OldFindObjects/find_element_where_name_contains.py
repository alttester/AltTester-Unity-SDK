from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
class FindElementWhereNameContains(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver,value,camera_name,enabled):
        super().__init__(socket,request_separator,request_end,appium_driver)
        self.value=value
        self.camera_name=camera_name
        self.enabled=enabled
    
    def execute(self):
        if self.enabled==True:
            data = self.send_data(self.create_command('findObjectWhereNameContains', self.value , self.camera_name,'true' ))
        else:
            data = self.send_data(self.create_command('findObjectWhereNameContains', self.value , self.camera_name,'false' ))
        return self.get_alt_element(data)