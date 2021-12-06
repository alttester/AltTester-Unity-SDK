from altunityrunner.commands.base_command import validate_coordinates, BaseCommand


class Swipe(BaseCommand):

    def __init__(self, connection, start, end, duration, wait):
        super().__init__(connection, "swipe")

        self.start = validate_coordinates(start)
        self.end = validate_coordinates(end)
        self.duration = duration
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "start": self.start,
            "end": self.end,
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
