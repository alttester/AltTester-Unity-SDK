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

import abc
import json
import time
from loguru import logger

import alttester.exceptions as exceptions
from alttester.by import By
from datetime import datetime, timezone
import threading


EPOCH = datetime(1970, 1, 1, tzinfo=timezone.utc)
lock = threading.Lock()


def validate_coordinates_3(coordinates):
    if isinstance(coordinates, (list, tuple)):
        if len(coordinates) != 3:
            raise exceptions.InvalidParameterValueException(
                "ValueError: coordinates must have items for x,y and z.")

        return {
            "x": coordinates[0],
            "y": coordinates[1],
            "z": coordinates[2]
        }
    elif isinstance(coordinates, dict):
        if "x" not in coordinates or "y" not in coordinates or "z" not in coordinates:
            raise exceptions.InvalidParameterValueException(
                "ValueError: coordinates must have an x,y and z key.")

        return coordinates
    else:
        raise exceptions.InvalidParameterTypeException(
            parameter_name="coordinates",
            expected_types=(list, tuple, dict),
            received_type=type(coordinates)
        )


def validate_coordinates(coordinates):
    if isinstance(coordinates, (list, tuple)):
        if len(coordinates) != 2:
            raise exceptions.InvalidParameterValueException(
                "ValueError: coordinates must have two items for x and y.")

        return {
            "x": coordinates[0],
            "y": coordinates[1]
        }
    elif isinstance(coordinates, dict):
        if "x" not in coordinates or "y" not in coordinates:
            raise exceptions.InvalidParameterValueException(
                "ValueError: coordinates must have an x and y key.")

        return coordinates
    else:
        raise exceptions.InvalidParameterTypeException(
            parameter_name="coordinates",
            expected_types=(list, tuple, dict),
            received_type=type(coordinates)
        )


class Command(metaclass=abc.ABCMeta):
    """An abstract class that defines the command protocol and contains utils methods for the commands."""

    delay_after_command = 0

    def get_path(self, by, value):
        if by == By.TAG:
            return "//*[@tag={}]".format(value)
        if by == By.COMPONENT:
            return "//*[@component={}]".format(value)
        if by == By.LAYER:
            return "//*[@layer={}]".format(value)
        if by == By.NAME:
            return "//{}".format(value)
        if by == By.ID:
            return "//*[@id={}]".format(value)
        if by == By.TEXT:
            return "//*[@text={}]".format(value)
        if by == By.PATH:
            return value

    def get_path_contains(self, by, value):
        if by == By.TAG:
            return "//*[contains(@tag,{})]".format(value)
        if by == By.COMPONENT:
            return "//*[contains(@component,{})]".format(value)
        if by == By.LAYER:
            return "//*[contains(@layer,{})]".format(value)
        if by == By.NAME:
            return "//*[contains(@name,{})]".format(value)
        if by == By.ID:
            return "//*[contains(@id,{})]".format(value)
        if by == By.TEXT:
            return "//*[contains(@text,{})]".format(value)
        if by == By.PATH:
            return value

    def vector_to_json(self, x, y, z=None):
        if z is None:
            return {"x": x, "y": y}
        else:
            return {"x": x, "y": y, "z": z}

    def positions_to_json(self, positions):
        return [self.vector_to_json(x, y) for x, y in positions]

    @classmethod
    def get_delay_after_command(cls):
        return cls.delay_after_command

    @classmethod
    def set_delay_after_command(cls, delay):
        cls.delay_after_command = delay

    @abc.abstractmethod
    def execute(self):
        """Execute the command."""

    @classmethod
    def run(cls, *args, **kwargs):
        """Run the command."""

        command = cls(*args, **kwargs)
        response = command.execute()
        time.sleep(cls.delay_after_command)

        return response


class BaseCommand(Command):
    """"A base command that sends and receive data from the AltTester."""

    def __init__(self, connection, command_name):
        self.connection = connection
        self.command_name = command_name
        with lock:
            self.message_id = str(
                (datetime.now(timezone.utc) - EPOCH).microseconds)

    @property
    def _parameters(self):
        """Returns command parammeters that will be sent to the AltTester."""

        return {
            "commandName": self.command_name,
            "messageId": self.message_id
        }

    def handle_response(self, response):
        error = response.get("error")

        if error:
            logger.error("Response error: {} - {}",
                         error.get("type"), error.get("message"))
            logger.error("Trace: {}", error.get("trace"))
            self.handle_errors(error)

        data = response.get("data")
        if data:
            logger.debug("Response data: {}", data)

    def handle_errors(self, error):
        error_map = {
            "ALTTESTERNotAddedAsDefineVariable": exceptions.AltTesterInputModuleException,
            "notFound": exceptions.NotFoundException,
            "sceneNotFound": exceptions.SceneNotFoundException,
            "objectNotFound": exceptions.ObjectNotFoundException,
            "cameraNotFound": exceptions.CameraNotFoundException,
            "propertyNotFound": exceptions.PropertyNotFoundException,
            "methodNotFound": exceptions.MethodNotFoundException,
            "methodWithGivenParametersNotFound": exceptions.MethodWithGivenParametersNotFoundException,
            "componentNotFound": exceptions.ComponentNotFoundException,
            "assemblyNotFound": exceptions.AssemblyNotFoundException,
            "propertyCannotBeSet": exceptions.PropertyCannotBeSetException,
            "couldNotPerformOperation": exceptions.CouldNotPerformOperationException,
            "couldNotParseJsonString": exceptions.CouldNotParseJsonStringException,
            "failedToParseMethodArguments": exceptions.FailedToParseArgumentsException,
            "formatException": exceptions.FormatException,
            "invalidParameterType": exceptions.InvalidParameterTypeException,
            "invalidCommand": exceptions.InvalidCommandException,
            "invalidPath": exceptions.InvalidPathException,
            "nullReferenceException": exceptions.NotFoundException,
            "unknownError": exceptions.UnknownErrorException
        }

        exception = error_map.get(
            error.get("type"), exceptions.UnknownErrorException)
        if exception == exceptions.InvalidCommandException:
            raise exception("Invalid command exception. You may want to set the Managed\
                            Stripping Level to `Minimal` from Player Settings -> Other Settings -> Optimization.")
        raise exception(error.get("message"))

    def validate_response(self, expected, received):
        if expected != received.strip('\"'):
            raise exceptions.AltTesterInvalidServerResponse(expected, received)

    def send(self):
        """Send a command to the AltTester® and return the response."""

        self.connection.send(self._parameters)
        response = self.connection.recv()
        self.handle_response(response)
        data = response.get("data")
        time.sleep(0.1)
        if data is None:
            return data
        return json.loads(data)

    def recv(self):
        """Wait for a response from the AltTester."""

        response = self.connection.recv()
        self.handle_response(response)

        data = response.get("data")
        if data is None:
            return data
        return json.loads(data)
