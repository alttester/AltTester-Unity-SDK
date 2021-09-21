import base64

from altunityrunner.commands.base_command import BaseCommand


class GetPNGScreenshot(BaseCommand):

    def __init__(self, connection, path):
        super().__init__(connection, "getPNGScreenshot")
        self.path = path

    def execute(self):
        data = self.send()
        self.validate_response("Ok", data)

        data = self.recv()
        screenshot_data = base64.b64decode(data)

        with open(self.path, "wb") as fp:
            fp.write(screenshot_data)
