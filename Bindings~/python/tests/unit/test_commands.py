import pytest

from altunityrunner.exceptions import InvalidParameterTypeException
from altunityrunner.commands import CallMethod


class TestCallMethod:

    @pytest.fixture(autouse=True)
    def setup(self):
        self.connection = None

    def test_constructor(self):
        command = CallMethod(
            self.connection,
            'AltUnityScript',
            'methodName',
            parameters=[1, 'string_param', 0.5, [1, 2, 3]],
            type_of_parameters=['System.String'],
            assembly='assemblyName'
        )

        assert command.component_name == 'AltUnityScript'
        assert command.method_name == 'methodName'
        assert command.parameters == ['1', '"string_param"', '0.5', '[1, 2, 3]']
        assert command.type_of_parameters == ['System.String']
        assert command.assembly == 'assemblyName'

    def test_with_invalid_type_of_parameters(self):
        with pytest.raises(InvalidParameterTypeException):
            CallMethod(
                self.connection,
                'AltUnityScript',
                'methodName',
                parameters=[1, 'stringparam', 0.5, [1, 2, 3]],
                type_of_parameters='System.String'
            )

    def test_with_invalid_parameters(self):
        with pytest.raises(InvalidParameterTypeException):
            CallMethod(
                self.connection,
                "AltUnityScript",
                "methodName",
                parameters="1?stringparam?0.5?[1,2,3]",
                type_of_parameters=["System.String"]
            )
