from altunityrunner.altUnityExceptions import InvalidParameterTypeException
from altunityrunner.commands.ObjectCommands.call_component_method import CallComponentMethodForObject
import pytest


class TestComponentParameters:

    def test_repr(self):
        element = CallComponentMethodForObject(None, "AltUnityScript", "methodName", [1, "stringparam", 0.5, [
                                               1, 2, 3]], "assemblyName", ["System.Stringgg"], None)

        assert element.component_name == "AltUnityScript"
        assert element.method_name == "methodName"
        assert element.parameters == [1, "stringparam", 0.5, [1, 2, 3]]
        assert element.assembly_name == "assemblyName"
        assert element.type_of_parameters == ["System.Stringgg"]

    def test_with_invalid_typeofparameters(self):
        with pytest.raises(InvalidParameterTypeException):
            CallComponentMethodForObject(None, "AltUnityScript", "methodName", [1, "stringparam", 0.5, [
                1, 2, 3]], "assemblyName", "System.Stringgg", None)

    def test_with_invalid_parameters(self):
        with pytest.raises(InvalidParameterTypeException):
            CallComponentMethodForObject(None, "AltUnityScript", "methodName",
                                         "1?stringparam?0.5?[1,2,3]", "assemblyName", ["System.Stringgg"], None)
