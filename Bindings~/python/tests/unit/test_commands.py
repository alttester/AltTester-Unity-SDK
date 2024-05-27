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

import pytest

from alttester.exceptions import InvalidParameterTypeException
from alttester.commands import CallMethod


class TestCallMethod:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.connection = None

    def test_constructor(self):
        command = CallMethod(
            self.connection,
            'AltScript',
            'methodName',
            assembly='assemblyName',
            parameters=[1, 'string_param', 0.5, [1, 2, 3]],
            type_of_parameters=['System.String'],
        )

        assert command.component_name == 'AltScript'
        assert command.method_name == 'methodName'
        assert command.parameters == [
            '1', '"string_param"', '0.5', '[1, 2, 3]']
        assert command.type_of_parameters == ['System.String']
        assert command.assembly == 'assemblyName'

    def test_with_invalid_type_of_parameters(self):
        with pytest.raises(InvalidParameterTypeException):
            CallMethod(
                self.connection,
                'AltScript',
                'methodName',
                parameters=[1, 'stringparam', 0.5, [1, 2, 3]],
                type_of_parameters='System.String'
            )

    def test_with_invalid_parameters(self):
        with pytest.raises(InvalidParameterTypeException):
            CallMethod(
                self.connection,
                "AltScript",
                "methodName",
                parameters="1?stringparam?0.5?[1,2,3]",
                type_of_parameters=["System.String"]
            )
