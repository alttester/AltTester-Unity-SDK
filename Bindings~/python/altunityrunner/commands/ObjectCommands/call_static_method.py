import json

from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.exceptions import InvalidParameterTypeException


class CallStaticMethod(BaseCommand):

    def __init__(self, connection, type_name, method_name, parameters, type_of_parameters, assembly=""):
        super().__init__(connection, "callComponentMethodForObject")

        self.type_name = type_name
        self.method_name = method_name
        self.parameters = parameters
        self.type_of_parameters = type_of_parameters
        self.assembly = assembly
        if not isinstance(parameters, (list, tuple)):
            raise InvalidParameterTypeException("parameters, Expected type list, got {}".format(type(parameters)))
        if not isinstance(type_of_parameters, (list, tuple)):
            raise InvalidParameterTypeException("type_of_parameters, Expected type list, got {}"
                                                .format(type(type_of_parameters)))

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "component": self.type_name,
            "method": self.method_name,
            "parameters": [json.dumps(p) for p in self.parameters],
            "typeofparameters": self.type_of_parameters,
            "assembly": self.assembly
        })

        return parameters

    def execute(self):
        return self.send()
