from altunityrunner.commands.base_command import BaseCommand


class MultipointSwipe(BaseCommand):

    def __init__(self, connection, positions, duration):
        super().__init__(connection, "multipointSwipeChain")

        self.positions = positions
        self.duration = duration

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "positions": self.positions_to_json(self.positions),
            "duration": self.duration
        })

        return parameters

    def execute(self):
        return self.send()
