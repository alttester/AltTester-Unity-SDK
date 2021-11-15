from loguru import logger

from altunityrunner.commands.base_command import BaseCommand


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
        logger.debug("Set time scale to: {}", self.time_scale)
        data = self.send()

        if (data == "Ok"):
            logger.debug("Time scale set to: {}", self.time_scale)
            return data

        return None
