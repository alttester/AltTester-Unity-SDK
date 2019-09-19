from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.by import By
class FindObjects(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver,by,value,camera_name,enabled):
        super().__init__(socket,request_separator,request_end,appium_driver)
        self.by=by
        self.value=value
        self.camera_name=camera_name
        self.enabled=enabled
    
    def execute(self):
        path=self.set_path(self.by,self.value)
        if self.enabled==True:
            data = self.send_data(self.create_command('findObjects', path , self.camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findObjects', path , self.camera_name ,'false'))
        return self.get_alt_elements(data)