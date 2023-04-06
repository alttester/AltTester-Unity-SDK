import time
from loguru import logger
from alttester.commands.base_command import Command
from alttester.exceptions import NotFoundException, WaitTimeOutException


class WaitForComponentProperty(Command):
    def __init__(self, component_name, property_name,
                 property_value, assembly, altObject, timeout=20, interval=0.5):
        self.component_name = component_name
        self.property_name = property_name
        self.property_value = property_value
        self.assembly = assembly
        self.timeout = timeout
        self.interval = interval
        self.altObject = altObject

    def execute(self):
        t = 0
        while (t <= self.timeout):
            try:
                property_found = self.altObject.get_component_property(
                    self.component_name, self.property_name, self.assembly)
                if (property_found == self.property_value):
                    break
            except NotFoundException:
                logger.debug("Waiting for property {}...", self.property_name)
                time.sleep(self.interval)
                t += self.interval

        if t >= self.timeout:
            raise WaitTimeOutException("Property {} not found after {} seconds"
                                       .format(
                                           self.property_name, self.timeout))

        return property_found
