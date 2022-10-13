package ro.altom.alttester;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;
import java.lang.Thread;

import javax.websocket.Session;
import com.google.gson.Gson;
import com.google.gson.JsonParseException;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import ro.altom.alttester.Commands.AltCommands.NotificationType;
import ro.altom.alttester.Notifications.AltLoadSceneNotificationResultParams;
import ro.altom.alttester.Notifications.AltLogNotificationResultParams;
import ro.altom.alttester.Notifications.INotificationCallbacks;
import ro.altom.alttester.altTesterExceptions.AltErrors;
import ro.altom.alttester.altTesterExceptions.AltException;
import ro.altom.alttester.altTesterExceptions.AltInputModuleException;
import ro.altom.alttester.altTesterExceptions.AltRecvallException;
import ro.altom.alttester.altTesterExceptions.AssemblyNotFoundException;
import ro.altom.alttester.altTesterExceptions.CameraNotFoundException;
import ro.altom.alttester.altTesterExceptions.ComponentNotFoundException;
import ro.altom.alttester.altTesterExceptions.CouldNotPerformOperationException;
import ro.altom.alttester.altTesterExceptions.FailedToParseArgumentsException;
import ro.altom.alttester.altTesterExceptions.FormatException;
import ro.altom.alttester.altTesterExceptions.InvalidCommandException;
import ro.altom.alttester.altTesterExceptions.InvalidParameterTypeException;
import ro.altom.alttester.altTesterExceptions.InvalidPathException;
import ro.altom.alttester.altTesterExceptions.MethodNotFoundException;
import ro.altom.alttester.altTesterExceptions.MethodWithGivenParametersNotFoundException;
import ro.altom.alttester.altTesterExceptions.NotFoundException;
import ro.altom.alttester.altTesterExceptions.NullReferenceException;
import ro.altom.alttester.altTesterExceptions.ObjectWasNotFoundException;
import ro.altom.alttester.altTesterExceptions.PropertyNotFoundException;
import ro.altom.alttester.altTesterExceptions.ResponseFormatException;
import ro.altom.alttester.altTesterExceptions.SceneNotFoundException;
import ro.altom.alttester.altTesterExceptions.UnknownErrorException;
import ro.altom.alttester.altTesterExceptions.CommandResponseTimeoutException;

public class MessageHandler implements IMessageHandler {
    private Session session;
    private Queue<AltMessageResponse> responses = new LinkedList<AltMessageResponse>();
    private static final Logger logger = LogManager.getLogger(MessageHandler.class);
    private List<INotificationCallbacks> loadSceneNotificationList = new ArrayList<INotificationCallbacks>();
    private List<INotificationCallbacks> unloadSceneNotificationList = new ArrayList<INotificationCallbacks>();
    private List<INotificationCallbacks> logNotificationList = new ArrayList<INotificationCallbacks>();
    private List<INotificationCallbacks> applicationPausedNotificationList = new ArrayList<INotificationCallbacks>();
    private List<String> messageIdTimeout = new ArrayList<String>();
    private double commandTimeout = 60;
    private double delayAfterCommand = 0;

    public MessageHandler(Session session) {
        this.session = session;
    }

    public double getDelayAfterCommand() {
        return this.delayAfterCommand;
    }

    public void setDelayAfterCommand(double delay) {
        this.delayAfterCommand = delay;
    }

    public <T> T receive(AltMessage data, Class<T> type) {
        double time = 0;
        double delay = 0.1;
        long sleepDelay = (long) (delay * 1000);
        while (true) {
            while (responses.isEmpty() && session.isOpen() && commandTimeout >= time) {
                try {
                    Thread.sleep(sleepDelay);
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
                time += delay;
            }
            if (commandTimeout < time && session.isOpen()) {
                messageIdTimeout.add(data.messageId());
                throw new CommandResponseTimeoutException();
            }
            if (!session.isOpen()) {
                throw new AltException("Driver disconnected");
            }
            AltMessageResponse responseMessage = responses.remove();
            if (messageIdTimeout.contains(responseMessage.messageId)) {
                continue;
            }
            if ((responseMessage.error == null || responseMessage.error.type != AltErrors.errorInvalidCommand)
                    && (!responseMessage.messageId.equals(data.messageId())
                            || !responseMessage.commandName.equals(data.getCommandName()))) {
                throw new AltRecvallException(
                        String.format("Response received does not match command send. Expected %s:%s. Got %s:%s",
                                data.getCommandName(), data.messageId(), responseMessage.commandName,
                                responseMessage.messageId));
            }
            handleErrors(responseMessage.error);
            try {
                T response = new Gson().fromJson(responseMessage.data, type);
                return response;
            } catch (JsonParseException ex) {
                throw new ResponseFormatException(type, responseMessage.data);

            }
        }
    }

    public void send(AltMessage altMessage) {
        String message = new Gson().toJson(altMessage);
        session.getAsyncRemote().sendText(message);
        logger.debug("command sent: {}", trimLogData(message));
    }

    public void onMessage(String message) {
        logger.debug("response received: {}", trimLogData(message));

        AltMessageResponse response = new Gson().fromJson(message, AltMessageResponse.class);

        if (response.isNotification) {
            handleNotification(response);
        } else {
            responses.add(response);
        }
    }

    private void handleNotification(AltMessageResponse message) {

        switch (message.commandName) {
            case "loadSceneNotification":
                AltLoadSceneNotificationResultParams data = new Gson().fromJson(message.data,
                        AltLoadSceneNotificationResultParams.class);
                for (INotificationCallbacks callback : loadSceneNotificationList) {
                    callback.SceneLoadedCallBack(data);
                }
                break;
            case "unloadSceneNotification":
                String sceneName = new Gson().fromJson(message.data, String.class);
                for (INotificationCallbacks callback : unloadSceneNotificationList) {
                    callback.SceneUnloadedCallBack(sceneName);
                }
                break;
            case "logNotification":
                AltLogNotificationResultParams data1 = new Gson().fromJson(message.data,
                        AltLogNotificationResultParams.class);
                for (INotificationCallbacks callback : logNotificationList) {
                    callback.LogCallBack(data1);
                }
                break;
            case "applicationPausedNotification":
                Boolean data2 = new Gson().fromJson(message.data, Boolean.class);
                for (INotificationCallbacks callback : applicationPausedNotificationList) {
                    callback.ApplicationPausedCallBack(data2);
                }
                break;
        }
    }

    private void handleErrors(CommandError error) {
        if (error == null) {
            return;
        }
        logger.error(error.type + ": " + error.message);
        logger.error(error.trace);

        switch (error.type) {
            case AltErrors.errorNotFound:
                throw new NotFoundException(error.message);
            case AltErrors.errorSceneNotFound:
                throw new SceneNotFoundException(error.message);
            case AltErrors.errorPropertyNotFound:
                throw new PropertyNotFoundException(error.message);
            case AltErrors.errorMethodNotFound:
                throw new MethodNotFoundException(error.message);
            case AltErrors.errorComponentNotFound:
                throw new ComponentNotFoundException(error.message);
            case AltErrors.errorAssemblyNotFound:
                throw new AssemblyNotFoundException(error.message);
            case AltErrors.errorCouldNotPerformOperation:
                throw new CouldNotPerformOperationException(error.message);
            case AltErrors.errorMethodWithGivenParametersNotFound:
                throw new MethodWithGivenParametersNotFoundException(error.message);
            case AltErrors.errorFailedToParseArguments:
                throw new FailedToParseArgumentsException(error.message);
            case AltErrors.errorInvalidParameterType:
                throw new InvalidParameterTypeException(error.message);
            case AltErrors.errorObjectNotFound:
                throw new ObjectWasNotFoundException(error.message);
            case AltErrors.errorPropertyNotSet:
                throw new PropertyNotFoundException(error.message);
            case AltErrors.errorNullReference:
                throw new NullReferenceException(error.message);
            case AltErrors.errorUnknownError:
                throw new UnknownErrorException(error.message);
            case AltErrors.errorFormatException:
                throw new FormatException(error.message);
            case AltErrors.errorInvalidPath:
                throw new InvalidPathException(error.message);
            case AltErrors.errorInvalidCommand:
                throw new InvalidCommandException(error.message);
            case AltErrors.errorInputModule:
                throw new AltInputModuleException(error.message);
            case AltErrors.errorCameraNotFound:
                throw new CameraNotFoundException(error.message);
        }

        logger.error(error.type + " is not handled by driver.");
        throw new UnknownErrorException(error.message);
    }

    public void setCommandTimeout(int timeout) {
        commandTimeout = timeout;
    }

    private String trimLogData(String data) {
        return trimLogData(data, 1024 * 10);
    }

    private String trimLogData(String data, int maxSize) {
        if (data.length() > maxSize) {
            return data.substring(0, 10 * 1024) + "[...]";
        }
        return data;
    }

    public void addNotificationListener(NotificationType notificationType, INotificationCallbacks callbacks,
            boolean overwrite) {
        switch (notificationType) {
            case LOADSCENE:
                if (overwrite) {
                    loadSceneNotificationList.clear();
                }
                loadSceneNotificationList.add(callbacks);
                break;
            case UNLOADSCENE:
                if (overwrite) {
                    unloadSceneNotificationList.clear();
                }
                unloadSceneNotificationList.add(callbacks);
                break;
            case LOG:
                if (overwrite) {
                    logNotificationList.clear();
                }
                logNotificationList.add(callbacks);
                break;
            case APPLICATION_PAUSED:
                if (overwrite) {
                    applicationPausedNotificationList.clear();
                }
                applicationPausedNotificationList.add(callbacks);
                break;
            default:
                break;

        }
    }

    public void removeNotificationListener(NotificationType notificationType) {
        switch (notificationType) {
            case LOADSCENE:
                loadSceneNotificationList.clear();
                break;
            case UNLOADSCENE:
                unloadSceneNotificationList.clear();
                break;
            case LOG:
                logNotificationList.clear();
                break;
            case APPLICATION_PAUSED:
                applicationPausedNotificationList.clear();
                break;
            default:
                break;

        }
    }
}
