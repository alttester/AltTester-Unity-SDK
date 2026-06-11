"""
    Copyright(C) 2026 Altom Consulting
"""


from alttester.commands.base_command import BaseCommand


class GetTimeScale(BaseCommand):

    def __init__(self, connection):
        super().__init__(connection, "getTimeScale")

    def execute(self):
        data = self.send()

        return float(data)
