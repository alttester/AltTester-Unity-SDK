from altunityrunner.commands.base_command import BaseCommand
from altunityrunner.logging import AltUnityLogger, AltUnityLogLevel


class SetServerLogging(BaseCommand):

    def __init__(self, socket, request_separator, request_end, logger, log_level):
        super(SetServerLogging, self).__init__(socket, request_separator, request_end)
        self.logger = AltUnityLogger.to_string(logger)
        self.log_level = AltUnityLogLevel.to_string(log_level)

    def execute(self):
        data = self.send_command("setServerLogging", self.logger, self.log_level)
        self.validate_response("Ok", data)
