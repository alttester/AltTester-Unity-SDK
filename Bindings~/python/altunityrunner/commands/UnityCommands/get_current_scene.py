from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements


class GetCurrentScene(CommandReturningAltElements):

    def __init__(self, socket, request_separator, request_end):
        super(GetCurrentScene, self).__init__(socket, request_separator, request_end)

    def execute(self):
        data = self.send_command("getCurrentScene")
        alt_element = self.get_alt_element(data)

        return alt_element.name
