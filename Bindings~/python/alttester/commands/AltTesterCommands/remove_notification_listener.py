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
        self.connection._notification_handler.remove_notification_listener(
            self.notification_type)
        data = self.send()
        self.validate_response("Ok", data)

        return data
