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

import base64

from alttester.commands.base_command import BaseCommand


class GetPNGScreenshot(BaseCommand):

    def __init__(self, connection, path):
        super().__init__(connection, "getPNGScreenshot")
        self.path = path

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        data = self.recv()
        screenshot_data = base64.b64decode(data)

        with open(self.path, "wb") as fp:
            fp.write(screenshot_data)
