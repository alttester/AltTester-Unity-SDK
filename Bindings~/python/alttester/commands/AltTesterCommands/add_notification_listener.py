"""
    Copyright(C) 2026 Altom Consulting
"""

from alttester.commands.Notifications.notification_type import NotificationType
from alttester.commands.base_command import BaseCommand
from alttester.exceptions import InvalidParameterTypeException


class AddNotificationListener(BaseCommand):

    def __init__(self, connection, notification_type, notification_callback, overwrite=False):
        super().__init__(connection, "activateNotification")

        if notification_type not in NotificationType:
            raise InvalidParameterTypeException(
                parameter_name='notification_type',
                expected_types=[NotificationType],
                received_type=type(notification_type)
            )
        self.notification_type = notification_type

        if not callable(notification_callback):
            raise InvalidParameterTypeException(
                parameter_name='notification_callback',
                expected_types=[callable],
                received_type=type(notification_callback)
            )
        self.notification_callback = notification_callback

        if not isinstance(overwrite, bool):
            raise InvalidParameterTypeException(
                parameter_name='overwrite',
                expected_types=[bool],
                received_type=type(overwrite)
            )
        self.overwrite = overwrite

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "notificationType": str(int(self.notification_type)),
        })

        return parameters

    def execute(self):
        self.connection._notification_handler.add_notification_listener(
            self.notification_type,
            self.notification_callback,
            self.overwrite
        )
        data = self.send()
        self.validate_response("Ok", data)

        return data
