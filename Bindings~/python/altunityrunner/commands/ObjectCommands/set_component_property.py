from altunityrunner.commands.base_command import BaseCommand


class SetComponentProperty(BaseCommand):
    def __init__(self, socket, request_separator, request_end, component_name, property_name, value, assembly_name, alt_object):
        super(SetComponentProperty, self).__init__(
            socket, request_separator, request_end)
        self.component_name = component_name
        self.property_name = property_name
        self.value = value
        self.assembly_name = assembly_name
        self.alt_object = alt_object

    def execute(self):
        property_info = '{"component":"' + self.component_name + '", "property":"' + \
            self.property_name + '"'+',"assembly":"' + self.assembly_name + '"}'
        data = self.send_command(
            'setObjectComponentProperty', self.alt_object, property_info, self.value)
        return self.handle_errors(data)
