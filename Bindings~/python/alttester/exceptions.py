"""
    Copyright(C) 2024 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
"""

"""This module contains the set of AltTester's exceptions."""


class AltException(Exception):
    """Base exception class for AltTester."""


class ConnectionError(AltException):
    """Raised when the client can not connect to the server. Used as base class for all connection exceptions."""


class ConnectionTimeoutError(ConnectionError):
    """Raised when the client connection timeouts."""


class NoAppConnected(ConnectionError):
    """Raised when the client tries to connect to a server without an app."""


class AppDisconnectedError(ConnectionError):
    """Raised when the app closed the connection or unexpectedly disconnected."""


class MultipleDriverError(ConnectionError):
    """ Raised when the client tries to connect to a server with a driver already connected.
      Free accounts are limited to a single driver connection at a time."""


class MultipleDriversTryingToConnectException(ConnectionError):
    """Raised when the client tries to connect to a server at the same time with another driver"""


class MaxNoOfConnectionsDriversExceededException(ConnectionError):
    """Raised when the client tries to connect to a server but the limit of drivers connected
        is exceeded"""


class AltTesterInvalidServerResponse(AltException):
    """Raised when the server responds with an invalid response."""

    def __init__(self, expected, received):
        super().__init__("Expected to get response {}; got {}".format(expected, received))


class InvalidParameterTypeException(TypeError, AltException):
    """Raised when an function or method receives an parameter that has the inappropriate type."""

    def __init__(self, parameter_name, expected_types, received_type):
        expected_types = [
            expected_type.__name__ for expected_type in expected_types]
        expected_types = ", ".join(expected_types)

        super().__init__(
            "TypeError: {} must be {}; not {}.".format(
                parameter_name, expected_types, received_type.__name__)
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
    """Raised when an operation could not be performed."""


class CouldNotParseJsonStringException(AltException):
    """Raised when AltTester® could not parse an JSON command."""


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
    """Raised when a command receives an invalid path."""


class InvalidCommandException(AltException):
    """Raised when a command is invalid."""


class AltTesterInputModuleException(AltException):
    pass


class UnknownErrorException(AltException):
    """Raised when an unexpected error occurred."""
