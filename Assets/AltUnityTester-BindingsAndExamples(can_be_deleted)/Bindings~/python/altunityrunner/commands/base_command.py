from altunityrunner.altUnityExceptions import *
from altunityrunner.by import By
from datetime import datetime
import json
BUFFER_SIZE = 1024
class BaseCommand(object):
    def __init__(self, socket,request_separator=';',request_end='&'):
        self.request_separator=request_separator
        self.request_end=request_end
        self.socket=socket

    def recvall(self):
        data = ''
        previousPart=''
        while True:
            part = self.socket.recv(BUFFER_SIZE)
            data += str(part.decode('ascii'))
            partToSeeAltEnd=previousPart+str(part)
            if '::altend' in partToSeeAltEnd:
                break
            previousPart=str(part)
        try:
            data = data.split('altstart::')[1].split('::altend')[0]
            splitted_string=data.split('::altLog::')
            self.write_to_log_file(splitted_string[1])
            data=splitted_string[0]
            self.write_to_log_file(datetime.now().strftime("%m/%d/%Y %H:%M:%S")+": response received: "+data)
        except:
            print('Data received from socket doesn not have correct start and end control strings')
            return ''
        print('Received data was: ' + data)
        return data
    
    def write_to_log_file(self,message):
        f = open("AltUnityTesterLog.txt", "a")
        f.write(message+"\n")
        f.close()

    def handle_errors(self, data):
        if ('error' in data):
            if ('error:notFound' in data):
                raise  NotFoundException(data)
            elif ('error:propertyNotFound' in data): 
                raise  PropertyNotFoundException(data)
            elif ('error:methodNotFound' in data): 
                raise  MethodNotFoundException(data)
            elif ('error:componentNotFound' in data): 
                raise  ComponentNotFoundException(data)
            elif ('error:couldNotPerformOperation' in data): 
                raise  CouldNotPerformOperationException(data)
            elif ('error:couldNotParseJsonString' in data): 
                raise  CouldNotParseJsonStringException(data)
            elif ('error:incorrectNumberOfParameters' in data): 
                raise  IncorrectNumberOfParametersException(data)
            elif ('error:failedToParseMethodArguments' in data): 
                raise  FailedToParseArgumentsException(data)
            elif ('error:objectNotFound' in data): 
                raise  ObjectWasNotFoundException(data)
            elif ('error:propertyCannotBeSet' in data): 
                raise  PropertyNotFoundException(data)
            elif ('error:nullReferenceException' in data): 
                raise  NullReferenceException(data)
            elif ('error:unknownError' in data): 
                raise  UnknownErrorException(data)
            elif ('error:formatException' in data): 
                raise  FormatException(data)
        else:
            return data
    def vector_to_json_string(self, x, y, z=None):
        if z is None:
            return '{"x":' + str(x) + ', "y":' + str(y) + '}'
        else:
            return '{"x":' + str(x) + ', "y":' + str(y) +', "z":' + str(z) + '}'
    def send_data(self, data):
        self.socket.send(data.encode('ascii'))
        if ('closeConnection' in data):
            return ''
        else:
            return self.recvall()

    def create_command(self,*arguments):
        command=''
        for argument in arguments:
            command+=str(argument)+self.request_separator
        command+=self.request_end
        return command
    
    def set_path(self,by,value):
        if by==By.TAG:
            return "//*[@tag="+str(value)+"]"
        if by==By.COMPONENT:
            return "//*[@component="+str(value)+"]"
        if by==By.LAYER:
            return "//*[@layer="+str(value)+"]"
        if by==By.NAME:
            return "//"+str(value)
        if by==By.ID:
            return "//*[@id="+str(value)+"]"
        if by==By.PATH:
            return value

    def set_path_contains(self,by,value):
        if by==By.TAG:
            return "//*[contains(@tag,"+str(value)+")]"
        if by==By.COMPONENT:
            return "//*[contains(@component,"+str(value)+")]"
        if by==By.LAYER:
            return "//*[contains(@layer,"+str(value)+")]"
        if by==By.NAME:
            return "//*[contains(@name,"+str(value)+")]"
        if by==By.ID:
            return "//*[contains(@id,"+str(value)+")]"
        if by==By.PATH:
            return value