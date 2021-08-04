from altunityrunner.commands.base_command import BaseCommand


class LoadScene(BaseCommand):

    def __init__(self, connection, scene_name, load_single):
        super().__init__(connection, "loadScene")

        self.scene_name = scene_name
        self.load_single = load_single

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "sceneName": self.scene_name,
            "loadSingle": self.load_single
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        data = self.recv()
        self.validate_response("Scene Loaded", data)

        return data
