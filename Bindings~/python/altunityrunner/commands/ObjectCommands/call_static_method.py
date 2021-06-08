import json

from altunityrunner.commands.base_command import BaseCommand


class CallStaticMethod(BaseCommand):

    def __init__(self, socket, request_separator, request_end, type_name, method_name, parameters,
                 type_of_parameters='', assembly=''):
        super(CallStaticMethod, self).__init__(socket, request_separator, request_end)
        self.type_name = type_name
        self.method_name = method_name
        self.parameters = parameters
        self.type_of_parameters = type_of_parameters
        self.assembly = assembly

    def execute(self):
        action_info = json.dumps({
            "component": self.type_name,
            "method": self.method_name,
            "parameters": self.parameters,
            "typeofparameters": self.type_of_parameters,
            "assembly": self.assembly
        })

        return self.send_command("callComponentMethodForObject", "", action_info)
