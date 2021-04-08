from altunityrunner.altUnityExceptions import InvalidParameterTypeException


class AltUnityLogger(object):
    File = 0
    Unity = 1
    Console = 2

    @staticmethod
    def to_string(value):
        if value == 0 or value == "File":
            return "File"
        elif value == 1 or value == "Unity":
            return "Unity"
        elif value == 2 or value == "Console":
            return "Console"
        raise InvalidParameterTypeException(
            "Invalid AltUnityLogger type: " + str(value))


class AltUnityLogLevel(object):
    Trace = 0
    Debug = 1
    Info = 2
    Warn = 3
    Error = 4
    Fatal = 5
    Off = 6

    @staticmethod
    def to_string(value):
        if value == 0 or value == "Trace":
            return "Trace"
        elif value == 1 or value == "Debug":
            return "Debug"
        elif value == 2 or value == "Info":
            return "Info"
        elif value == 3 or value == "Warn":
            return "Warn"
        elif value == 4 or value == "Error":
            return "Error"
        elif value == 5 or value == "Fatal":
            return "Fatal"
        elif value == 6 or value == "Off":
            return "Off"

        raise InvalidParameterTypeException(
            "Invalid AltUnityLogLevel type: " + str(value))
