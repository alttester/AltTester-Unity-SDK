from altunityrunner.commands.base_command import BaseCommand
class ClickEvent(BaseCommand):
    def __init__(self, socket,request_separator,request_end,alt_object):
        super().__init__(socket,request_separator,request_end)
        self.alt_object=alt_object
    
    def execute(self):
        data = self.send_data(self.create_command('clickObject',self.alt_object))
        return self.handle_errors(data)
