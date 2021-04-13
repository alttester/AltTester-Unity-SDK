from datetime import datetime
import re

from loguru import logger

from altunityrunner.by import By
from altunityrunner.altUnityExceptions import *


BUFFER_SIZE = 1024

EPOCH = datetime.utcfromtimestamp(0)


class BaseCommand(object):
    def __init__(self, socket, request_separator=';', request_end='&'):
        self.request_separator = request_separator
        self.request_end = request_end
        self.socket = socket
        self.messageId = ""

    def recvall(self):
        data = ''
        previousPart = ''
        receive_zero_bytes_counter = 0
        receive_zero_bytes_counter_limit = 2
        while True:
            part = self.socket.recv(BUFFER_SIZE)

            if not part:  # If received message is empty
                if receive_zero_bytes_counter < receive_zero_bytes_counter_limit:
                    receive_zero_bytes_counter += 1
                    continue
                else:
                    raise Exception('Server is not yet reachable')
            data += str(part.decode('utf-8'))
            partToSeeAltEnd = previousPart + str(part.decode('utf-8'))
            if '::altend' in partToSeeAltEnd:
                break
            previousPart = str(part.decode('utf-8'))

        logger.trace(data)

        parts = re.split("altstart::|::response::|::altLog::|::altend", data)

        if len(parts) != 5 or parts[0] or parts[4]:
            raise AltUnityRecvallMessageFormatException(
                "Data received from socket doesn't have correct start and end control strings.\nGot:\n{}".format(data))
        if parts[1] != self.messageId:
            raise AltUnityRecvallMessageIdException(
                "Response received does not match command send. Expected message id: {}. Got {}".format(self.messageId, parts[1]))

        data = parts[2]
        log = parts[3]

        logger.debug(f'response: {self._trim_log_data(data)}')
        if log:
            logger.debug(log)

        self._handle_errors(data, log)

        return data

    def _handle_errors(self, data, log):
        if log:
            data = data + "\n" + log + "\n"

        if ('error:' in data):
            if (data.startswith('error:notFound')):
                raise NotFoundException(data)
            elif (data.startswith('error:propertyNotFound')):
                raise PropertyNotFoundException(data)
            elif (data.startswith('error:methodNotFound')):
                raise MethodNotFoundException(data)
            elif (data.startswith('error:componentNotFound')):
                raise ComponentNotFoundException(data)
            elif (data.startswith('error:assemblyNotFound')):
                raise AssemblyNotFoundException(data)
            elif (data.startswith('error:couldNotPerformOperation')):
                raise CouldNotPerformOperationException(data)
            elif (data.startswith('error:couldNotParseJsonString')):
                raise CouldNotParseJsonStringException(data)
            elif (data.startswith('error:methodWithGivenParametersNotFound')):
                raise MethodWithGivenParametersNotFoundException(data)
            elif (data.startswith('error:invalidParameterType')):
                raise InvalidParameterTypeException(data)
            elif (data.startswith('error:failedToParseMethodArguments')):
                raise FailedToParseArgumentsException(data)
            elif (data.startswith('error:objectNotFound')):
                raise ObjectWasNotFoundException(data)
            elif (data.startswith('error:propertyCannotBeSet')):
                raise PropertyNotFoundException(data)
            elif (data.startswith('error:nullReferenceException')):
                raise NullReferenceException(data)
            elif (data.startswith('error:unknownError')):
                raise UnknownErrorException(data)
            elif (data.startswith('error:formatException')):
                raise FormatException(data)
            elif (data.startswith('error:invalidPath')):
                raise AltUnityInvalidPathException(data)

    def vector_to_json_string(self, x, y, z=None):
        if z is None:
            return '{"x":' + str(x) + ', "y":' + str(y) + '}'
        else:
            return '{"x":' + str(x) + ', "y":' + str(y) + ', "z":' + str(z) + '}'

    def positions_to_json_string(self, positions):
        json_positions = [self.vector_to_json_string(
            p[0], p[1]) for p in positions]
        return self.request_separator.join(json_positions)

    def send_command(self, *arguments):
        command = self._create_command(arguments)
        self._send_data(command)
        logger.debug("sent: " + command)

        if (arguments[0] == 'closeConnection'):
            return ''
        else:
            return self.recvall()

    def validate_response(self, expected, received):
        if expected != received:
            raise AltUnityInvalidServerResponse(expected, received)

    def _send_data(self, data):
        self.socket.send(data.encode('utf-8'))

    def _create_command(self, arguments):
        parts = [str(arg) for arg in arguments]
        self.messageId = str((datetime.utcnow() - EPOCH).total_seconds())
        parts.insert(0, self.messageId)
        return self.request_separator.join(parts) + self.request_end

    def set_path(self, by, value):
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
        if by == By.PATH:
            return value
        if by == By.TEXT:
            return "//*[@text={}]".format(value)

    def set_path_contains(self, by, value):
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
        if by == By.PATH:
            return value
        if by == By.TEXT:
            return "//*[contains(@text,{})]".format(value)

    def _trim_log_data(self, data, maxSize=10 * 1024):
        if len(data) < maxSize:
            return data
        return data[:maxSize] + "[...]"
