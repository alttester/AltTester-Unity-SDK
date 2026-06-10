"""
    Copyright(C) 2026 Altom Consulting
"""

from alttester.commands.base_command import BaseCommand


class FetchText(BaseCommand):

    def __init__(self, connection, alt_object):
        super().__init__(connection, "getText")

        self.alt_object = alt_object

    @property  # _
    def _parameters(self):  # _
        parameters = super()._parameters  # _
        parameters.update(**{  # _
            "altObject": self.alt_object.to_json(),
        })

        return parameters

    def execute(self):
        return self.send()
