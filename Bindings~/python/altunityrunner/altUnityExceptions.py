
class AltUnityException(Exception):
    """Base exception class for AltUnity."""


class ConnectionError(AltUnityException):
    """Raised when the client can not connect to the server."""


class AltUnityInvalidServerResponse(AltUnityException):
    """Raised when the server responds with an invalid respose."""

    def __init__(self, expected, received):
        super().__init__("Expected to get response {}; Got {}".format(expected, received))


class InvalidParameterTypeException(AltUnityException):
    """Raised when an function or method receives an parameter that has the inappropriate type."""


class NotFoundException(AltUnityException):
    """Raised when a object, camera, component, property, method or assembly is not found."""


class ObjectNotFoundException(NotFoundException):
    """Raised when a object is not found."""


class CameraNotFoundException(NotFoundException):
    """Raised when a camera is not found."""


class PropertyNotFoundException(NotFoundException):
    """Raised when a property is not found."""


class MethodNotFoundException(NotFoundException):
    """Raised when a method is not found."""


class MethodWithGivenParametersNotFoundException(NotFoundException):
    """Raised when a method with the given parameters is not found."""


class ComponentNotFoundException(NotFoundException):
    """Raised when a component is not found."""


class AssemblyNotFoundException(NotFoundException):
    """Raised when an assembly is not found."""


class CouldNotPerformOperationException(AltUnityException):
    pass


class CouldNotParseJsonStringException(AltUnityException):
    pass


class FailedToParseArgumentsException(AltUnityException):
    pass


class PropertyCannotBeSetException(AltUnityException):
    pass


class NullReferenceException(AltUnityException):
    pass


class UnknownErrorException(AltUnityException):
    pass


class FormatException(AltUnityException):
    pass


class WaitTimeOutException(AltUnityException):
    pass


class AltUnityInvalidPathException(AltUnityException):
    pass


class AltUnityInputModuleException(AltUnityException):
    pass
