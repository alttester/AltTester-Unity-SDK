import json

from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.exceptions import InvalidParameterTypeException


class CallMethod(BaseCommand):

    def __init__(self, connection, component_name, method_name, alt_object=None, parameters=None,
                 type_of_parameters=None, assembly=""):
        super().__init__(connection, "callComponentMethodForObject")

        parameters = parameters if parameters is not None else []
        type_of_parameters = type_of_parameters if type_of_parameters is not None else []

        if not isinstance(parameters, (list, tuple)):
            raise InvalidParameterTypeException(
                parameter_name="parameters",
                expected_types=(list, tuple),
                received_type=type(parameters)
            )

        if not isinstance(type_of_parameters, (list, tuple)):
            raise InvalidParameterTypeException(
                parameter_name="type_of_parameters",
                expected_types=(list, tuple),
                received_type=type(type_of_parameters)
            )

        self.alt_object = alt_object
        self.component_name = component_name
        self.method_name = method_name
        self.parameters = [json.dumps(parameter) for parameter in parameters]
        self.type_of_parameters = type_of_parameters
        self.assembly = assembly

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "component": self.component_name,
            "method": self.method_name,
            "parameters": self.parameters,
            "typeofparameters": self.type_of_parameters,
            "assembly": self.assembly
        })

        if self.alt_object:
            parameters["altUnityObject"] = self.alt_object.to_json()

        return parameters

    def execute(self):
        return self.send()
