"""This module contains the set of AltTester's exceptions."""


class AltException(Exception):
    """Base exception class for AltTester."""


class ConnectionError(AltException):
    """Raised when the client can not connect to the server."""


class ConnectionTimeoutError(ConnectionError):
    """Raised when the client connection timesout."""


class AltTesterInvalidServerResponse(AltException):
    """Raised when the server responds with an invalid respose."""

    def __init__(self, expected, received):
        super().__init__("Expected to get response {}; got {}".format(expected, received))


class InvalidParameterTypeException(TypeError, AltException):
    """Raised when an function or method receives an parameter that has the inappropriate type."""

    def __init__(self, parameter_name, expected_types, received_type):
        expected_types = [expected_type.__name__ for expected_type in expected_types]
        expected_types = ", ".join(expected_types)

        super().__init__(
            "TypeError: {} must be {}; not {}.".format(parameter_name, expected_types, received_type.__name__)
        )


class InvalidParameterValueException(ValueError, AltException):
    """Raised when an function or method receives an parameter that has the right type but an inappropriate value."""


class NotFoundException(AltException):
    """Raised when a object, camera, component, property, method or assembly is not found."""


class SceneNotFoundException(NotFoundException):
    """Raised when a scene is not found."""


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


class CouldNotPerformOperationException(AltException):
    """Raised when an opperation could not be performed."""


class CouldNotParseJsonStringException(AltException):
    """Raised when AltTester could not parse an JSON command."""


class NullReferenceException(AltException):
    """Raised when there is an attempt to dereference a null object reference."""


class FailedToParseArgumentsException(AltException):
    """Raised when a method arguments could not be parsed by AltTester."""


class WaitTimeOutException(AltException):
    """Raised when a wait command times out."""


class CommandResponseTimeoutException(AltException):
    """Raised when a command does't send a response in the given time."""


class PropertyCannotBeSetException(AltException):
    """Raised when a property could not be found or it's value could not be updated."""


class FormatException(AltException):
    pass


class InvalidPathException(AltException):
    """Raised when a command recives an invalid path."""


class AltTesterInputModuleException(AltException):
    pass


class UnknownErrorException(AltException):
    """Raised when an unexpected error occurred."""
