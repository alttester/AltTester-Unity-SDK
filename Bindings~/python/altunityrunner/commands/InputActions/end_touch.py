from altunityrunner.commands.base_command import BaseCommand


class EndTouch(BaseCommand):

    def __init__(self, connection, finger_id):
        super(EndTouch, self).__init__(connection, "endTouch")

        self.finger_id = finger_id

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "fingerId": self.finger_id,
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)
