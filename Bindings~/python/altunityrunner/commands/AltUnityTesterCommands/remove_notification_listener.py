from altunityrunner.commands.Notifications.notification_type import NotificationType
from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.exceptions import InvalidParameterTypeException


class RemoveNotificationListener(BaseCommand):

    def __init__(self, connection, notification_type):
        super().__init__(connection, "deactivateNotification")

        if notification_type not in NotificationType:
            raise InvalidParameterTypeException(
                parameter_name='notification_type',
                expected_types=[NotificationType],
                received_type=type(notification_type)
            )
        self.notification_type = notification_type

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "notificationType": str(int(self.notification_type)),
        })

        return parameters

    def execute(self):
        self.connection.remove_notification_listener(self.notification_type)
        data = self.send()
        self.validate_response("Ok", data)

        return data
