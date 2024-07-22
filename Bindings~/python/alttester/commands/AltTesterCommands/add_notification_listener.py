"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

from alttester.commands.Notifications.notification_type import NotificationType
from alttester.commands.base_command import BaseCommand
from alttester.exceptions import InvalidParameterTypeException


class AddNotificationListener(BaseCommand):

    def __init__(self, connection, notification_type, notification_callback, overwrite):
        super().__init__(connection, "activateNotification")

        if notification_type not in NotificationType:
            raise InvalidParameterTypeException(
                parameter_name='notification_type',
                expected_types=[NotificationType],
                received_type=type(notification_type)
            )
        self.notification_type = notification_type

        if type(notification_callback) == "method":
            raise InvalidParameterTypeException(
                parameter_name='notification_callback',
                expected_types=[callable],
                received_type=type(notification_callback)
            )
        self.notification_callback = notification_callback

        if type(overwrite) == "bool":
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
