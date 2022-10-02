from alttester.commands.base_command import BaseCommand


class ClickElement(BaseCommand):

    def __init__(self, connection, alt_object, count, interval, wait):
        super().__init__(connection, "clickElement")

        self.alt_object = alt_object
        self.count = count
        self.interval = interval
        self.wait = wait

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "altObject": self.alt_object.to_json(),
            "count": self.count,
            "interval": self.interval,
            "wait": self.wait
        })

        return parameters

    def execute(self):
        data = self.send()

        if self.wait:
            self.validate_response("Finished", self.recv())

        return data
