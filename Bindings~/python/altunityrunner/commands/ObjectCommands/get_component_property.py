import json

from altunityrunner.commands.base_command import BaseCommand


class GetComponentProperty(BaseCommand):

    def __init__(self, socket, request_separator, request_end, component_name, property_name, assembly_name, max_depth,
                 alt_object):
        super(GetComponentProperty, self).__init__(socket, request_separator, request_end)
        self.component_name = component_name
        self.property_name = property_name
        self.assembly_name = assembly_name
        self.alt_object = alt_object
        self.max_depth = max_depth

    def execute(self):
        property_info = json.dumps({
            "component": self.component_name,
            "property": self.property_name,
            "assembly": self.assembly_name
        })

        return self.send_command("getObjectComponentProperty", self.alt_object, property_info, self.max_depth)
