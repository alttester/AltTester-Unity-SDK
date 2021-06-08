import json

from loguru import logger

from altunityrunner.commands.base_command import BaseCommand


class GetAllComponents(BaseCommand):

    def __init__(self, socket, request_separator, request_end, alt_object):
        super(GetAllComponents, self).__init__(socket, request_separator, request_end)
        self.alt_object = alt_object

    def execute(self):
        data = self.send_command("getAllComponents", str(self.alt_object.id))

        try:
            return json.loads(data)
        except json.JSONDecodeError:
            logger.exception("Cannot parse the {}, this is not JSON.".format(data))
            raise
