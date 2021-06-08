from altunityrunner.commands.base_command import BaseCommand


class UnloadScene(BaseCommand):

    def __init__(self, socket, request_separator, request_end, scene_name):
        super(UnloadScene, self).__init__(socket, request_separator, request_end)
        self.scene_name = scene_name

    def execute(self):
        data = self.send_command("unloadScene", self.scene_name)
        self.validate_response("Ok", data)

        data = self.recvall()
        self.validate_response("Scene Unloaded", data)

        return data
