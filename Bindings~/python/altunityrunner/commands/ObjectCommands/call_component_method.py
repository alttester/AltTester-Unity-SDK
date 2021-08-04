from altunityrunner.commands.base_command import BaseCommand


class CallComponentMethodForObject(BaseCommand):

    def __init__(self, connection, component_name, method_name, parameters, assembly_name, type_of_parameters,
                 alt_object):
        super().__init__(connection, "callComponentMethodForObject")

        self.alt_object = alt_object

        self.component_name = component_name
        self.method_name = method_name
        self.parameters = parameters
        self.assembly_name = assembly_name
        self.type_of_parameters = type_of_parameters

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altUnityObject": self.alt_object.to_json(),
            "component": self.component_name,
            "method": self.method_name,
            "parameters": self.parameters,
            "assembly": self.assembly_name,
            "typeOfParameters": self.type_of_parameters
        })

        return parameters

    def execute(self):
        return self.send()
