from altunityrunner.commands.base_command import BaseCommand


class UnloadScene(BaseCommand):

    def __init__(self, connection, scene_name):
        super(UnloadScene, self).__init__(connection, "unloadScene")

        self.scene_name = scene_name

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "sceneName": self.scene_name,
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        data = self.recv()
        self.validate_response("Scene Unloaded", data)

        return data
