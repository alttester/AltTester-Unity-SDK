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

from loguru import logger

from alttester.commands.base_command import BaseCommand


class SetTimeScale(BaseCommand):

    def __init__(self, connection, time_scale):
        super().__init__(connection, "setTimeScale")

        self.time_scale = time_scale

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "timeScale": str(self.time_scale),
        })

        return parameters

    def execute(self):
        logger.debug("Set time scale to: {}", self.time_scale)
        data = self.send()

        if (data == "Ok"):
            logger.debug("Time scale set to: {}", self.time_scale)
            return data

        return None
