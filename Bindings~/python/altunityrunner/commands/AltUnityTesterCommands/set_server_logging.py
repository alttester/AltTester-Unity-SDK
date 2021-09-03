from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.logging import AltUnityLogger, AltUnityLogLevel
from altunityrunner.exceptions import InvalidParameterTypeException


class SetServerLogging(BaseCommand):

    def __init__(self, connection, logger, log_level):
        super().__init__(connection, "setServerLogging")

        if logger not in AltUnityLogger:
            raise InvalidParameterTypeException()

        if log_level not in AltUnityLogLevel:
            raise InvalidParameterTypeException()

        self.logger = logger
        self.log_level = log_level

    @property
    def _parameters(self):
        parameters = super()._parameters
        parameters.update(**{
            "logger": str(self.logger),
            "logLevel": str(self.log_level)
        })

        return parameters

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        return data
