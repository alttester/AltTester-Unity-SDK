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


class LoadScene(BaseCommand):

    def __init__(self, connection, scene_name, load_single):
        super().__init__(connection, "loadScene")

        self.scene_name = scene_name
        self.load_single = load_single

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "sceneName": self.scene_name,
            "loadSingle": self.load_single
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        data = self.recv()
        self.validate_response("Scene Loaded", data)

        return data
