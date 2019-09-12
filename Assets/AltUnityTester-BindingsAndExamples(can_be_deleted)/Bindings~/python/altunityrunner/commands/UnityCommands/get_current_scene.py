from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements

class GetCurrentScene(CommandReturningAltElements):
    def __init__(self, socket,request_separator,request_end,appium_driver):
        super().__init__(socket,request_separator,request_end,appium_driver)
    def execute(self):
        data = self.send_data(self.create_command('getCurrentScene'))
        if (data != '' and 'error:' not in data):
            alt_el = self.get_alt_element(data)
            print('Current scene is ' + alt_el.name)
            return alt_el.name
        return self.handle_errors(data)
