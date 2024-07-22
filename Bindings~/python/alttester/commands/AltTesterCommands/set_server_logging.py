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

from alttester.commands.base_command import BaseCommand
from alttester.logging import AltLogger, AltLogLevel
from alttester.exceptions import InvalidParameterTypeException


class SetServerLogging(BaseCommand):

    def __init__(self, connection, logger, log_level):
        super().__init__(connection, "setServerLogging")

        if logger not in AltLogger:
            raise InvalidParameterTypeException(
                parameter_name='logger',
                expected_types=[AltLogger],
                received_type=type(logger)
            )

        if log_level not in AltLogLevel:
            raise InvalidParameterTypeException(
                parameter_name='log_level',
                expected_types=[AltLogLevel],
                received_type=type(log_level)
            )

        self.logger = logger
        self.log_level = log_level

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "logger": str(self.logger),
            "logLevel": str(self.log_level)
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        return data
