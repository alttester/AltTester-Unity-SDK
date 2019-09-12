class AltUnityException(Exception):
    def __init__(self,message):
        super(AltUnityException,self).__init__(message)
           
class NotFoundException(AltUnityException):
    def __init__(self,message):
        super(NotFoundException,self).__init__(message)

class PropertyNotFoundException(AltUnityException):
    def __init__(self,message):
        super(PropertyNotFoundException,self).__init__(message)

class MethodNotFoundException(AltUnityException):
    def __init__(self,message):
        super(MethodNotFoundException,self).__init__(message)

class ComponentNotFoundException(AltUnityException):
    def __init__(self,message):
        super(ComponentNotFoundException,self).__init__(message)

class CouldNotPerformOperationException(AltUnityException):
    def __init__(self,message):
        super(CouldNotPerformOperationException,self).__init__(message)

class IncorrectNumberOfParametersException(AltUnityException):
    def __init__(self,message):
        super(IncorrectNumberOfParametersException,self).__init__(message)

class CouldNotParseJsonStringException(AltUnityException):
    def __init__(self,message):
        super(CouldNotParseJsonStringException,self).__init__(message)

class FailedToParseArgumentsException(AltUnityException):
    def __init__(self,message):
        super(FailedToParseArgumentsException,self).__init__(message)

class ObjectWasNotFoundException(AltUnityException):
    def __init__(self,message):
        super(ObjectWasNotFoundException,self).__init__(message)

class PropertyCannotBeSetException(AltUnityException):
    def __init__(self,message):
        super(PropertyCannotBeSetException,self).__init__(message)

class NullReferenceException(AltUnityException):
    def __init__(self,message):
        super(NullReferenceException,self).__init__(message)

class UnknownErrorException(AltUnityException):
    def __init__(self,message):
        super(UnknownErrorException,self).__init__(message)

class FormatException(AltUnityException):
    def __init__(self,message):
        super(FormatException,self).__init__(message)

class WaitTimeOutException(AltUnityException):
    def __init__(self,message):
        super(WaitTimeOutException,self).__init__(message)
