from altunityrunner.commands.base_command import BaseCommand


class MultipointSwipe(BaseCommand):

    def __init__(self, connection, positions, duration, wait):
        super().__init__(connection, "multipointSwipe")

        self.positions = positions
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "positions": self.positions_to_json(self.positions),
            "duration": self.duration,
            "wait": self.wait
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        if self.wait:
            data = self.recv()
            self.validate_response("Finished", data)

        return data
