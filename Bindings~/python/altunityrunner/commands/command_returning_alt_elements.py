import json

from loguru import logger

from altunityrunner.altElement import AltElement
from altunityrunner.commands.base_command import BaseCommand


class CommandReturningAltElements(BaseCommand):

    def __init__(self, socket, request_separator, request_end):
        super(CommandReturningAltElements, self).__init__(socket, request_separator, request_end)

    def get_alt_element(self, data):
        if not data:
            return None

        alt_element = AltElement(self, data)

        logger.debug("Element {} found at x:{} y:{} mobileY:{}".format(
            alt_element.name,
            alt_element.x,
            alt_element.y,
            alt_element.mobileY
        ))

        return alt_element

    def get_alt_elements(self, data):
        if not data:
            return None

        alt_elements = []
        elements = []

        try:
            elements = json.loads(data)
        except Exception:
            raise Exception("Couldn't parse json data: " + data)

        for element in elements:
            alt_element = AltElement(self, json.dumps(element))
            alt_elements.append(alt_element)

            logger.debug("Element {} found at x:{} y:{} mobileY:{}".format(
                alt_element.name,
                alt_element.x,
                alt_element.y,
                alt_element.mobileY
            ))

        return alt_elements
