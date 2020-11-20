from os import getenv
from altunityrunner.altUnityExceptions import *
from altunityrunner.by import By
from loguru import logger
from datetime import datetime
import re
import json
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
            partToSeeAltEnd = previousPart+str(part.decode('utf-8'))
            if '::altend' in partToSeeAltEnd:
                break
            previousPart = str(part.decode('utf-8'))

        parts = re.split("altstart::|::response::|::altLog::|::altend", data)

        if len(parts) != 5 or parts[0] or parts[4]:
            raise AltUnityRecvallMessageFormatException(
                "Data received from socket doesn't have correct start and end control strings")
        if parts[1] != self.messageId:
            raise AltUnityRecvallMessageIdException(
                "Response received does not match command send. Expected message id: " + self.messageId + ". Got " + parts[1])

        data = parts[2]
        log = parts[3]

        logger.debug(f'Received data was: {self._trim_log_data(data)}')

        self.write_to_log_file(datetime.now().strftime(
            "%m/%d/%Y %H:%M:%S")+": response received: "+self._trim_log_data(data))
        self.write_to_log_file(log)

        return data

    def write_to_log_file(self, message):
        with open("AltUnityTesterLog.txt", "a", encoding="utf-8") as f:
            f.write(message+"\n")

    def handle_errors(self, data):
        if ('error' in data):
            if ('error:notFound' in data):
                raise NotFoundException(data)
            elif ('error:propertyNotFound' in data):
                raise PropertyNotFoundException(data)
            elif ('error:methodNotFound' in data):
                raise MethodNotFoundException(data)
            elif ('error:componentNotFound' in data):
                raise ComponentNotFoundException(data)
            elif ('error:assemblyNotFound' in data):
                raise AssemblyNotFoundException(data)
            elif ('error:couldNotPerformOperation' in data):
                raise CouldNotPerformOperationException(data)
            elif ('error:couldNotParseJsonString' in data):
                raise CouldNotParseJsonStringException(data)
            elif ('error:methodWithGivenParametersNotFound' in data):
                raise MethodWithGivenParametersNotFoundException(data)
            elif ('error:invalidParameterType' in data):
                raise InvalidParameterTypeException(data)
            elif ('error:failedToParseMethodArguments' in data):
                raise FailedToParseArgumentsException(data)
            elif ('error:objectNotFound' in data):
                raise ObjectWasNotFoundException(data)
            elif ('error:propertyCannotBeSet' in data):
                raise PropertyNotFoundException(data)
            elif ('error:nullReferenceException' in data):
                raise NullReferenceException(data)
            elif ('error:unknownError' in data):
                raise UnknownErrorException(data)
            elif ('error:formatException' in data):
                raise FormatException(data)
        else:
            return data

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
        self._send_data(self._create_command(arguments))
        if (arguments[0] == 'closeConnection'):
            return ''
        else:
            return self.recvall()

    def _send_data(self, data):
        self.socket.send(data.encode('utf-8'))

    def _create_command(self, arguments):
        parts = [str(arg) for arg in arguments]
        self.messageId = str((datetime.utcnow() - EPOCH).total_seconds())
        parts.insert(0, self.messageId)
        return self.request_separator.join(parts) + self.request_end

    def set_path(self, by, value):
        if by == By.TAG:
            return "//*[@tag="+str(value)+"]"
        if by == By.COMPONENT:
            return "//*[@component="+str(value)+"]"
        if by == By.LAYER:
            return "//*[@layer="+str(value)+"]"
        if by == By.NAME:
            return "//"+str(value)
        if by == By.ID:
            return "//*[@id="+str(value)+"]"
        if by == By.PATH:
            return value

    def set_path_contains(self, by, value):
        if by == By.TAG:
            return "//*[contains(@tag,"+str(value)+")]"
        if by == By.COMPONENT:
            return "//*[contains(@component,"+str(value)+")]"
        if by == By.LAYER:
            return "//*[contains(@layer,"+str(value)+")]"
        if by == By.NAME:
            return "//*[contains(@name,"+str(value)+")]"
        if by == By.ID:
            return "//*[contains(@id,"+str(value)+")]"
        if by == By.PATH:
            return value

    def _trim_log_data(self, data, maxSize=10 * 1024):
        if len(data) < maxSize:
            return data
        return data[:maxSize] + "[...]"
