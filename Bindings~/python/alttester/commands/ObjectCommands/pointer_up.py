from alttester.commands.base_command import BaseCommand


class PointerUp(BaseCommand):

    def __init__(self, connection, alt_object):
        super().__init__(connection, "pointerUpFromObject")

        self.alt_object = alt_object

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altObject": self.alt_object.to_json(),
        })

        return parameters

    def execute(self):
        return self.send()
