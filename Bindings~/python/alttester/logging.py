from enum import Enum, unique


@unique
class AltUnityLogger(Enum):
    File = 0
    Unity = 1
    Console = 2

    def __str__(self):
        return self.name


@unique
class AltUnityLogLevel(Enum):
    Trace = 0
    Debug = 1
    Info = 2
    Warn = 3
    Error = 4
    Fatal = 5
    Off = 6

    def __str__(self):
        return self.name
