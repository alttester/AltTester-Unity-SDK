from altunityrunner.commands.base_command import BaseCommand


class CallComponentMethodForObject(BaseCommand):
    def __init__(self, socket, request_separator, request_end, component_name, method_name, parameters, assembly_name, type_of_parameters, alt_object):
        super(CallComponentMethodForObject, self).__init__(
            socket, request_separator, request_end)
        self.component_name = component_name
        self.method_name = method_name
        self.parameters = parameters
        self.assembly_name = assembly_name
        self.type_of_parameters = type_of_parameters
        self.alt_object = alt_object

    def execute(self):
        action_info = '{"component":"' + self.component_name + '", "method":"' + self.method_name + '", "parameters":"' + \
            self.parameters + '"'+',"assembly":"' + self.assembly_name + \
            '", "typeofparameters":"' + self.type_of_parameters + '"}'
        data = self.send_command(
            'callComponentMethodForObject', self.alt_object, action_info)
        return self.handle_errors(data)
