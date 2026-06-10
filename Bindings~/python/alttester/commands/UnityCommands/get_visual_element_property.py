"""
    Copyright(C) 2026 Altom Consulting
"""

from alttester.commands.base_command import BaseCommand


class GetVisualElementProperty(BaseCommand):

    def __init__(self, connection, property_name, alt_object):
        super().__init__(connection, "getVisualElementProperty")

        self.alt_object = alt_object
        self.property_name = property_name

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altObject": self.alt_object.to_json(),
            "property": self.property_name,
        })

        return parameters

    def execute(self):
        return self.send()
