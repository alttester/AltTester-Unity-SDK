"""
    Copyright(C) 2026 Altom Consulting

"""

from alttester.commands.base_command import BaseCommand


class UpdateText(BaseCommand):

    def __init__(self, connection, new_text, target_object, submit):
        super().__init__(connection, "setText")
        self.alt_object = target_object
        self.text = new_text
        self.submit = submit

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altObject": self.alt_object.to_json(),
            "value": self.text,
            "submit": self.submit
        })

        return parameters

    def execute(self):
        return self.send()
