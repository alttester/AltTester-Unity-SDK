class AltUnityException(Exception):
    def __init__(self,message):
        super().__init__(message)
           
class NotFoundException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class PropertyNotFoundException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class MethodNotFoundException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class ComponentNotFoundException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class CouldNotPerformOperationException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class IncorrectNumberOfParametersException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class CouldNotParseJsonStringException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class FailedToParseArgumentsException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class ObjectWasNotFoundException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class PropertyCannotBeSetException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class NullRefferenceException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class UnknownErrorException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class FormatException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)

class WaitTimeOutException(AltUnityException):
    def __init__(self,message):
        super().__init__(message)
