from altunityrunner.commands.base_command import BaseCommand
from loguru import logger


class Tilt(BaseCommand):
    def __init__(self, socket, request_separator, request_end, x, y, z):
        super(Tilt, self).__init__(socket, request_separator, request_end)
        self.x = x
        self.y = y
        self.z = z

    def execute(self):
        acceleration = self.vector_to_json_string(self.x, self.y, self.z)
        logger.debug('Tilt with acceleration: ' + acceleration)
        data = self.send_data(self.create_command('tilt', acceleration))
        return self.handle_errors(data)
