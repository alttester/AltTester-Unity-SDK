from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from altunityrunner.by import By


class FindObjectsWhichContain(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end, by, value, camera_by, camera_path, enabled):
        super(FindObjectsWhichContain, self).__init__(
            socket, request_separator, request_end)
        self.by = by
        self.value = value
        self.camera_by = camera_by
        self.camera_path = camera_path
        self.enabled = enabled

    def execute(self):
        path = self.set_path_contains(self.by, self.value)
        camera_path = self.set_path(self.camera_by, self.camera_path)
        if self.enabled == True:
            data = self.send_command(
                'findObjects', path, By.return_enum_string(self.camera_by), camera_path, 'true')
        else:
            data = self.send_command(
                'findObjects', path, By.return_enum_string(self.camera_by), camera_path, 'false')
        return self.get_alt_elements(data)
