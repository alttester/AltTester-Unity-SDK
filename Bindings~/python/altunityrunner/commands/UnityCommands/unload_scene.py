from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class UnloadScene(BaseCommand):
    def __init__(self, socket, request_separator, request_end, scene_name):
        super(UnloadScene, self).__init__(
            socket, request_separator, request_end)
        self.scene_name = scene_name

    def execute(self):
        data = self.send_command(
            'unloadScene', self.scene_name)
        if (data == 'Ok'):
            data = self.recvall()
            if (data == "Scene Unloaded"):
                logger.debug('Scene Unloaded: ' + self.scene_name)
                return data
        return self.handle_errors(data)
