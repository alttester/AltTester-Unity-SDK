from altunityrunner.commands.base_command import BaseCommand

class MoveMouse(BaseCommand):
    def __init__(self, socket,request_separator,request_end, x, y, duration):
        super().__init__(socket,request_separator,request_end)
        self.x=x
        self.y=y
        self.duration=duration
    
    def execute(self):
        location = self.vector_to_json_string(self.x, self.y)
        print ('Move mouse to: ' + location)
        data = self.send_data(self.create_command('moveMouse', location, self.duration ))
        return self.handle_errors(data)