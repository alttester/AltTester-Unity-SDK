from altunityrunner.commands.base_command import Command
from altunityrunner.commands.FindObjects.find_objects import FindObjects
from altunityrunner.exceptions import InvalidParameterTypeException
from altunityrunner.by import By


class GetAllElements(Command):

    def __init__(self, connection, camera_by, camera_value, enabled):
        self.connection = connection

        if camera_by not in By:
            raise InvalidParameterTypeException(
                parameter_name="camera_by",
                expected_types=[By],
                received_type=type(camera_by)
            )

        self.camera_by = camera_by
        self.camera_value = camera_value
        self.enabled = enabled

    def execute(self):
        return FindObjects.run(self.connection, By.PATH, "//*", self.camera_by, self.camera_value, self.enabled)
