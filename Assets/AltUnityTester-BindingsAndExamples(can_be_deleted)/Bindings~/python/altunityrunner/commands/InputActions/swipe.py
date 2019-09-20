from altunityrunner.commands.base_command import BaseCommand
class Swipe(BaseCommand):
    def __init__(self, socket,request_separator,request_end,x_start, y_start, x_end, y_end, duration_in_secs):
        super().__init__(socket,request_separator,request_end)
        self.x_start=x_start
        self.y_start=y_start
        self.x_end=x_end
        self.y_end=y_end
        self.duration_in_secs=duration_in_secs
    
    def execute(self):
        start_position = self.vector_to_json_string(self.x_start, self.y_start)
        end_position = self.vector_to_json_string(self.x_end, self.y_end)
        print('Swipe from ' + start_position + ' to ' + end_position + ' with duration: ' + str(self.duration_in_secs) + ' secs')
        data = self.send_data(self.create_command('movingTouch', start_position , end_position , str(self.duration_in_secs) ))
        return self.handle_errors(data)