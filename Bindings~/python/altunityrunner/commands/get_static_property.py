from altunityrunner.commands.base_command import BaseCommand


class GetStaticProperty(BaseCommand):

    def __init__(self, connection, component_name, property_name, assembly_name, max_depth):
        super().__init__(connection, "getObjectComponentProperty")

        self.alt_object = None

        self.component_name = component_name
        self.property_name = property_name
        self.assembly_name = assembly_name
        self.max_depth = max_depth

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altUnityObject": self.alt_object,
            "component": self.component_name,
            "property": self.property_name,
            "assembly": self.assembly_name,
            "maxDepth": self.max_depth,
        })

        return parameters

    def execute(self):
        return self.send()
