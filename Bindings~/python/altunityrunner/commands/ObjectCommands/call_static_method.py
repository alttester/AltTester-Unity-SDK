from altunityrunner.commands.base_command import BaseCommand


class CallStaticMethod(BaseCommand):

    def __init__(self, connection, type_name, method_name, parameters, type_of_parameters="", assembly=""):
        super().__init__(connection, "callComponentMethodForObject")

        self.type_name = type_name
        self.method_name = method_name
        self.parameters = parameters
        self.type_of_parameters = type_of_parameters
        self.assembly = assembly

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "component": self.type_name,
            "method": self.method_name,
            "parameters": self.parameters,
            "typeofparameters": self.type_of_parameters,
            "assembly": self.assembly
        })

        return parameters

    def execute(self):
        return self.send()
