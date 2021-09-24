from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.exceptions import InvalidParameterTypeException
from altunityrunner.by import By


class FindObjectsWhichContain(BaseCommand):

    def __init__(self, connection, by, value, camera_by, camera_value, enabled):
        super().__init__(connection, "findObjects")

        if by not in By:
            raise InvalidParameterTypeException(
                parameter_name="by",
                expected_types=[By],
                received_type=type(by)
            )

        if camera_by not in By:
            raise InvalidParameterTypeException(
                parameter_name="camera_by",
                expected_types=[By],
                received_type=type(camera_by)
            )

        self.by = by
        self.value = value
        self.camera_by = camera_by
        self.camera_value = camera_value
        self.enabled = enabled

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "by": str(self.by),
            "path": self.get_path_contains(self.by, self.value),
            "cameraBy": str(self.camera_by),
            "cameraPath": self.get_path(self.camera_by, self.camera_value),
            "enabled": self.enabled
        })

        return parameters

    def execute(self):
        return self.send()
