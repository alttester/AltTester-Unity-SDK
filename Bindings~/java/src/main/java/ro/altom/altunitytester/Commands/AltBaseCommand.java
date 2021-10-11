package ro.altom.altunitytester.Commands;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltMessageResponse;
import ro.altom.altunitytester.CommandError;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

public class AltBaseCommand {

    protected static final Logger logger = LogManager.getLogger(AltBaseCommand.class);

    protected IMessageHandler messageHandler;

    public AltBaseCommand(IMessageHandler messageHandler) {
        this.messageHandler = messageHandler;
    }

    protected <T> T recvall(AltMessage altMessage, final Class<T> type) {

        AltMessageResponse<T> response = messageHandler.receive(altMessage, type);
        handleErrors(response.error);

        return response.data;
    }

    protected void SendCommand(AltMessage altMessage) {
        altMessage.setMessageId(Long.toString(System.currentTimeMillis()));
        messageHandler.send(altMessage);
    }

    protected void validateResponse(String expected, String received) {
        if (!expected.equals(received)) {
            throw new AltUnityInvalidServerResponse(expected, received);
        }
    }

    private void handleErrors(CommandError error) {
        if (error == null) {
            return;
        }

        switch (error.type) {
            case AltUnityErrors.errorNotFoundMessage:
                throw new NotFoundException(error.message);
            case AltUnityErrors.errorPropertyNotFoundMessage:
                throw new PropertyNotFoundException(error.message);
            case AltUnityErrors.errorMethodNotFoundMessage:
                throw new MethodNotFoundException(error.message);
            case AltUnityErrors.errorComponentNotFoundMessage:
                throw new ComponentNotFoundException(error.message);
            case AltUnityErrors.errorAssemblyNotFoundMessage:
                throw new AssemblyNotFoundException(error.message);
            case AltUnityErrors.errorCouldNotPerformOperationMessage:
                throw new CouldNotPerformOperationException(error.message);
            case AltUnityErrors.errorMethodWithGivenParametersNotFound:
                throw new MethodWithGivenParametersNotFoundException(error.message);
            case AltUnityErrors.errorFailedToParseArguments:
                throw new FailedToParseArgumentsException(error.message);
            case AltUnityErrors.errorInvalidParameterType:
                throw new InvalidParameterTypeException(error.message);
            case AltUnityErrors.errorObjectWasNotFound:
                throw new ObjectWasNotFoundException(error.message);
            case AltUnityErrors.errorPropertyNotSet:
                throw new PropertyNotFoundException(error.message);
            case AltUnityErrors.errorNullReferenceMessage:
                throw new NullReferenceException(error.message);
            case AltUnityErrors.errorUnknownError:
                throw new UnknownErrorException(error.message);
            case AltUnityErrors.errorFormatException:
                throw new FormatException(error.message);
            case AltUnityErrors.errorInvalidPath:
                throw new InvalidPathException(error.message);
            case AltUnityErrors.errorInvalidCommand:
                throw new InvalidCommandException(error.message);
            case AltUnityErrors.errorInputModule:
                throw new AltUnityInputModuleException(error.message);
            case AltUnityErrors.errorCameraNotFound:
                throw new CameraNotFoundException(error.message);
        }

        logger.error(error.type + " is not handled by driver.");
        throw new UnknownErrorException(error.message);
    }

    /**
     * Sleeps for certain amount of seconds.
     *
     * @param interval Seconds to sleep for.
     */
    protected void sleepFor(double interval) {
        long timeToSleep = (long) (interval * 1000);
        try {
            Thread.sleep(timeToSleep);
        } catch (InterruptedException e) {
            logger.warn("Could not sleep for " + timeToSleep + " ms");
        }
    }
}
