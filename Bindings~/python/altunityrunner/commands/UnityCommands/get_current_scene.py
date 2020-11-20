from altunityrunner.commands.command_returning_alt_elements import CommandReturningAltElements
from loguru import logger


class GetCurrentScene(CommandReturningAltElements):
    def __init__(self, socket, request_separator, request_end):
        super(GetCurrentScene, self).__init__(
            socket, request_separator, request_end)

    def execute(self):
        data = self.send_command('getCurrentScene')
        if (data != '' and 'error:' not in data):
            alt_el = self.get_alt_element(data)
            logger.debug('Current scene is ' + alt_el.name)
            return alt_el.name
        return self.handle_errors(data)
