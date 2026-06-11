"""
    Copyright(C) 2026 Altom Consulting
"""

import time
import json
from loguru import logger
from alttester.commands.base_command import Command
from alttester.exceptions import WaitTimeOutException


class WaitForVisualElementProperty(Command):
    def __init__(
        self,
        property_name,
        property_value,
        altObject,
        timeout=20,
        interval=0.5,
        get_property_as_string=False,
    ):
        self.property_name = property_name
        self.property_value = property_value
        self.timeout = timeout
        self.interval = interval
        self.get_property_as_string = get_property_as_string
        self.altObject = altObject

    def execute(self):
        t = 0
        exception = None
        property_found = None
        while t <= self.timeout:
            try:
                property_found = self.altObject.get_visual_element_property(
                    self.property_name
                )
                if not self.get_property_as_string and property_found == self.property_value:
                    return property_found
                if (str(property_found) == "0.0" and str(self.property_value) == "0"):
                    return property_found
                if self.get_property_as_string and \
                   (
                       str(property_found).replace(" ", "") == str(self.property_value).replace(" ", "") or
                       str(json.dumps(property_found)).replace(" ", "") == str(
                        self.property_value).replace(" ", "")
                   ):

                    return property_found
            except Exception as ex:
                exception = ex

            logger.debug("Waiting for property {}...", self.property_name)
            time.sleep(self.interval)
            t += self.interval
        if exception:
            raise WaitTimeOutException(
                "After {} seconds, exception was: {} for  property {}".format(
                    self.timeout, exception, self.property_name
                )
            )
        raise WaitTimeOutException(
            "Property {} was {}, not {} as expected, after {} seconds".format(
                self.property_name, property_found, self.property_value, self.timeout
            )
        )
