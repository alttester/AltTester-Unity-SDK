"""
    Copyright(C) 2026 Altom Consulting
"""


from alttester.commands.base_command import BaseCommand


class SetTimeScale(BaseCommand):

    def __init__(self, connection, time_scale):
        super().__init__(connection, "setTimeScale")

        self.time_scale = time_scale

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "timeScale": str(self.time_scale),
        })

        return parameters

    def execute(self):
        data = self.send()

        if (data == "Ok"):
            return data

        return None
