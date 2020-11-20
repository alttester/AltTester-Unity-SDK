from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class LoadScene(BaseCommand):
    def __init__(self, socket, request_separator, request_end, scene_name, load_single):
        super(LoadScene, self).__init__(socket, request_separator, request_end)
        self.scene_name = scene_name
        self.load_single = load_single

    def execute(self):
        data = self.send_command(
            'loadScene', self.scene_name, self.load_single)
        if (data == 'Ok'):
            data = self.recvall()
            if (data == "Scene Loaded"):
                logger.debug('Scene loaded: ' + self.scene_name)
                return data
        return None
