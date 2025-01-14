"""
    Copyright(C) 2025 Altom Consulting

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

from AltTesterLibrary.AltTesterKeywords import AltTesterKeywords
from AltTesterLibrary.version import VERSION

__version__ = VERSION


class AltTesterLibrary(AltTesterKeywords):

    """ A keyword library for Robot Framework. It provides keywords for
        performing various operations using AltTester® to interact with 
        Unity games and apps. See http://alttester.com/tools/
        for more details.

        This library uses the AltTester® Driver Python Library from
            https://pypi.org/project/AltTester-Driver/

    """

    ROBOT_LIBRARY_SCOPE = 'GLOBAL'
    ROBOT_LIBRARY_VERSION = __version__
