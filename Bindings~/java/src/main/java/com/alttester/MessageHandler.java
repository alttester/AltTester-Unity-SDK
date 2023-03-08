package com.alttester;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Queue;

import javax.websocket.Session;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import com.google.gson.Gson;
import com.google.gson.JsonParseException;

import com.alttester.Commands.AltCommands.NotificationType;
import com.alttester.Exceptions.AltErrors;
import com.alttester.Exceptions.AltException;
import com.alttester.Exceptions.AltInputModuleException;
import com.alttester.Exceptions.AltRecvallException;
import com.alttester.Exceptions.AssemblyNotFoundException;
import com.alttester.Exceptions.CameraNotFoundException;
import com.alttester.Exceptions.CommandResponseTimeoutException;
import com.alttester.Exceptions.ComponentNotFoundException;
import com.alttester.Exceptions.CouldNotPerformOperationException;
import com.alttester.Exceptions.FailedToParseArgumentsException;
import com.alttester.Exceptions.FormatException;
import com.alttester.Exceptions.InvalidCommandException;
import com.alttester.Exceptions.InvalidParameterTypeException;
import com.alttester.Exceptions.InvalidPathException;
import com.alttester.Exceptions.MethodNotFoundException;
import com.alttester.Exceptions.MethodWithGivenParametersNotFoundException;
import com.alttester.Exceptions.NotFoundException;
import com.alttester.Exceptions.NullReferenceException;
import com.alttester.Exceptions.ObjectWasNotFoundException;
import com.alttester.Exceptions.PropertyNotFoundException;
import com.alttester.Exceptions.ResponseFormatException;
import com.alttester.Exceptions.SceneNotFoundException;
import com.alttester.Exceptions.UnknownErrorException;
import com.alttester.Notifications.AltLoadSceneNotificationResultParams;
import com.alttester.Notifications.AltLogNotificationResultParams;
import com.alttester.Notifications.INotificationCallbacks;

public class MessageHandler implements IMessageHandler {
    private static Logger logger = LogManager.getLogger(MessageHandler.class);

    private Session session;
    private Queue<AltMessageResponse> responses = new LinkedList<AltMessageResponse>();
    private List<INotificationCallbacks> loadSceneNotificationList = new ArrayList<INotificationCallbacks>();
    private List<INotificationCallbacks> unloadSceneNotificationList = new ArrayList<INotificationCallbacks>();
    private List<INotificationCallbacks> logNotificationList = new ArrayList<INotificationCallbacks>();
    private List<INotificationCallbacks> applicationPausedNotificationList = new ArrayList<INotificationCallbacks>();
    private List<String> messageIdTimeout = new ArrayList<String>();
    private double commandTimeout = 60;
    private double delayAfterCommand = 0;

    public MessageHandler(final Session session) {
        this.session = session;
    }

    public final double getDelayAfterCommand() {
        return this.delayAfterCommand;
    }

    public final void setDelayAfterCommand(final double delay) {
        this.delayAfterCommand = delay;
    }

    public final <T> T receive(final AltMessage data, final Class<T> type) {
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

    public final void send(final AltMessage altMessage) {
        String message = new Gson().toJson(altMessage);
        session.getAsyncRemote().sendText(message);
        logger.debug("command sent: {}", trimLogData(message));
    }

    public final void onMessage(final String message) {
        logger.debug("response received: {}", trimLogData(message));

        AltMessageResponse response = new Gson().fromJson(message, AltMessageResponse.class);

        if (response.isNotification) {
            handleNotification(response);
        } else {
            responses.add(response);
        }
    }

    private void handleNotification(final AltMessageResponse message) {

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
            default:
                break;
        }
    }

    private void handleErrors(final CommandError error) {
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
            default:
                break;
        }

        logger.error(error.type + " is not handled by driver.");
        throw new UnknownErrorException(error.message);
    }

    public final void setCommandTimeout(final int timeout) {
        commandTimeout = timeout;
    }

    private String trimLogData(final String data) {
        return trimLogData(data, 1024 * 10);
    }

    private String trimLogData(final String data, final int maxSize) {
        if (data.length() > maxSize) {
            return data.substring(0, 10 * 1024) + "[...]";
        }
        return data;
    }

    public final void addNotificationListener(final NotificationType notificationType,
            final INotificationCallbacks callbacks,
            final boolean overwrite) {
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

    public final void removeNotificationListener(final NotificationType notificationType) {
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
