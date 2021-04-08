import json

from loguru import logger

from altunityrunner.altElement import AltElement
from altunityrunner.commands.base_command import BaseCommand


class CommandReturningAltElements(BaseCommand):
    def __init__(self, socket, request_separator, request_end):
        self.request_separator = request_separator
        self.request_end = request_end
        self.socket = socket

    def get_alt_element(self, data):
        if not data:
            return None

        alt_el = AltElement(self, data)

        logger.debug('Element {} found at x:{} y:{} mobileY:{}'.format(
            alt_el.name,
            alt_el.x,
            alt_el.y,
            alt_el.mobileY
        ))

        return alt_el

    def get_alt_elements(self, data):
        if not data:
            return None

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
            logger.debug('Element {} found at x:{} y:{} mobileY:{}'.format(
                alt_el.name,
                alt_el.x,
                alt_el.y,
                alt_el.mobileY
            ))

        return alt_elements
