from altunityrunner.commands.base_command import BaseCommand
class GetAllElements(BaseCommand):
    def __init__(self, socket,requestSeparator,requestEnd,camera_name,enabled):
        super().__init__(socket,requestSeparator,requestEnd)
        self.camera_name=camera_name
        self.enabled=enabled
    
    def execute(self):
        if enabled==True:
            data = self.send_data(self.create_command('findAllObjects','//*', self.camera_name ,'true'))
        else:
            data = self.send_data(self.create_command('findAllObjects','//*', self.camera_name ,'false'))

        return self.get_alt_elements(data)