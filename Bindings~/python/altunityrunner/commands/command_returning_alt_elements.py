from altunityrunner.altElement import AltElement
from altunityrunner.commands.base_command import BaseCommand
from loguru import logger
import json


class CommandReturningAltElements(BaseCommand):
    def __init__(self, socket, request_separator, request_end):
        self.request_separator = request_separator
        self.request_end = request_end
        self.socket = socket

    def get_alt_element(self, data):
        logger.debug(data)
        if data != '' and 'error:' not in data:
            alt_el = AltElement(self, data)

            logger.debug('Element ' + alt_el.name + ' found at x:' + str(alt_el.x) +
                         ' y:' + str(alt_el.y) + ' mobileY:' + str(alt_el.mobileY))
            return alt_el
        logger.debug("handle errors")
        self.handle_errors(data)
        logger.debug("return None")
        return None

    def get_alt_elements(self, data):
        if data != '' and 'error:' not in data:
            alt_elements = []
            elements = []
            try:
                elements = json.loads(data)
            except:
                raise Exception("Couldn't parse json data: " + data)

            alt_el = None
            for i in range(0, len(elements)):
                alt_el = AltElement(self, json.dumps(elements[i]))

                alt_elements.append(alt_el)
                logger.debug('Element ' + alt_el.name + ' found at x:' + str(alt_el.x) +
                             ' y:' + str(alt_el.y) + ' mobileY:' + str(alt_el.mobileY))
            return alt_elements

        self.handle_errors(data)
        return None
