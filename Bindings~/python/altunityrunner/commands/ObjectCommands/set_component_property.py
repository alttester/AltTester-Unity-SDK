import json
from altunityrunner.commands.base_command import BaseCommand


class SetComponentProperty(BaseCommand):

    def __init__(self, connection, component_name, property_name, value, assembly_name, alt_object):
        super().__init__(connection, "setObjectComponentProperty")

        self.alt_object = alt_object

        self.component_name = component_name
        self.property_name = property_name
        self.value = json.dumps(value)
        self.assembly_name = assembly_name

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altUnityObject": self.alt_object.to_json(),
            "component": self.component_name,
            "property": self.property_name,
            "assembly": self.assembly_name,
            "value": self.value,
        })

        return parameters

    def execute(self):
        return self.send()
