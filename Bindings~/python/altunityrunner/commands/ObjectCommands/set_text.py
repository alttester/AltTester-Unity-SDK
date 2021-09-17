from altunityrunner.commands.base_command import BaseCommand


class SetText(BaseCommand):

    def __init__(self, connection, text_value, alt_object, submit):
        super().__init__(connection, "setText")

        self.alt_object = alt_object
        self.text = text_value
        self.submit = submit

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altUnityObject": self.alt_object.to_json(),
            "value": self.text,
            "submit": self.submit
        })

        return parameters

    def execute(self):
        return self.send()
