from altunityrunner.commands.base_command import BaseCommand
from loguru import logger
import json
from typing import List


class GetAllComponents(BaseCommand):
    def __init__(self, socket, request_separator, request_end, alt_object) -> None:
        super(GetAllComponents, self).__init__(
            socket, request_separator, request_end)
        self.alt_object = alt_object

    def execute(self) -> List[dict]:
        data = self.send_command(
            'getAllComponents', str(self.alt_object.id))
        self.handle_errors(data)
        try:
            return json.loads(data)
        except json.JSONDecodeError:
            logger.error(f'Cannot parse the {data}, this is not JSON.')
            raise
