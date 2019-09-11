from altunityrunner.commands.base_command import BaseCommand
class PointerUp(BaseCommand):
    def __init__(self, socket,request_separator,request_end,alt_object):
        super().__init__(socket,request_separator,request_end)
        self.alt_object=alt_object
    
    def execute(self):
        data = self.alt_unity_driver.send_data(self.alt_unity_driver.create_command('pointerUpFromObject', alt_object ))
        return self.alt_unity_driver.handle_errors(data)