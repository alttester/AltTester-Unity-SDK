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

import time

from loguru import logger

from alttester.commands.base_command import Command
from alttester.exceptions import WaitTimeOutException
from alttester.commands.UnityCommands.get_current_scene import GetCurrentScene


class WaitForCurrentSceneToBe(Command):

    def __init__(self, connection, scene_name, timeout, interval):
        self._connection = connection

        self.scene_name = scene_name
        self.timeout = timeout
        self.interval = interval

    def execute(self):
        t = 0
        current_scene = ""

        while t <= self.timeout:
            logger.debug("Waiting for scene to be {}...", self.scene_name)
            current_scene = GetCurrentScene(self._connection).execute()

            if current_scene == self.scene_name:
                return
            time.sleep(self.interval)
            t += self.interval
        if t >= self.timeout:
            raise WaitTimeOutException(
                "Scene {} not loaded after {} seconds".format(self.scene_name, self.timeout))
