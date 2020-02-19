from altunityrunner.commands.base_command import BaseCommand
class LoadScene(BaseCommand):
    def __init__(self, socket,request_separator,request_end,scene_name):
        super(LoadScene, self).__init__(socket,request_separator,request_end)
        self.scene_name=scene_name
    
    def execute(self):
        data = self.send_data(self.create_command('loadScene', self.scene_name))
        if (data == 'Ok'):
            data=self.recvall()
            if (data =="Scene Loaded"):
                print('Scene loaded: ' + self.scene_name)
                return data
        return None