from alttester.commands.base_command import BaseCommand


class PointerEnter(BaseCommand):

    def __init__(self, connection, alt_object):
        super(PointerEnter, self).__init__(connection, "pointerEnterObject")

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
