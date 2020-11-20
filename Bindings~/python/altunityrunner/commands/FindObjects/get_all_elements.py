from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.by import By


class GetAllElements(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end, camera_by, camera_path, enabled):
        super(GetAllElements, self).__init__(
            socket, request_separator, request_end)
        self.camera_by = camera_by
        self.camera_path = camera_path
        self.enabled = enabled

    def execute(self):
        camera_path = self.set_path(self.camera_by, self.camera_path)
        if self.enabled == True:
            data = self.send_command(
                'findObjects', '//*', By.return_enum_string(self.camera_by), camera_path, 'true')
        else:
            data = self.send_command(
                'findObjects', '//*', By.return_enum_string(self.camera_by), camera_path, 'false')

        return self.get_alt_elements(data)
