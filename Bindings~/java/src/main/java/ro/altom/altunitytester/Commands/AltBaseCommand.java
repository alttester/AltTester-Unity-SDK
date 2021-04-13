package ro.altom.altunitytester.Commands;

import org.apache.logging.log4j.LogManager;
import org.apache.logging.log4j.Logger;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

import java.io.IOException;
import java.nio.charset.StandardCharsets;

public class AltBaseCommand {

    protected static final Logger logger = LogManager.getLogger(AltBaseCommand.class);

    private final static int BUFFER_SIZE = 1024;
    private String messageId;
    public AltBaseSettings altBaseSettings;

    public AltBaseCommand(AltBaseSettings altBaseSettings) {
        this.altBaseSettings = altBaseSettings;
    }

    protected String recvall() {
        String receivedData = "";
        boolean streamIsFinished = false;
        int receivedZeroBytesCounter = 0;
        int receivedZeroBytesCounterLimit = 2;
        while (!streamIsFinished) {
            byte[] messageByte = new byte[BUFFER_SIZE];
            int bytesRead = 0;
            try {
                bytesRead = altBaseSettings.in.read(messageByte);
            } catch (IOException e) {
                throw new ConnectionException(e);
            }
            if (bytesRead > 0)
                receivedData += new String(messageByte, 0, bytesRead, StandardCharsets.UTF_8);
            else {
                if (receivedZeroBytesCounter < receivedZeroBytesCounterLimit) {
                    receivedZeroBytesCounter++;
                    continue;
                } else {
                    throw new ConnectionException(new Throwable("Received empty response"));
                }
            }
            if (receivedData.contains("::altend")) {
                streamIsFinished = true;
            }
        }

        logger.trace(receivedData);

        String[] parts = receivedData.split("altstart::|::response::|::altLog::|::altend", -1);// -1 limit to include
                                                                                               // Trailing empty strings
        if (parts.length != 5 || !parts[0].equals("") || !parts[4].equals("")) {

            throw new AltUnityRecvallMessageFormatException(String.format(
                    "Data received from socket doesn't have correct start and end control strings.\nGot:\n %s",
                    receivedData));
        }

        if (!parts[1].equals(messageId)) {
            throw new AltUnityRecvallMessageIdException(
                    "Response received does not match command send. Expected message id: " + messageId + ". Got "
                            + parts[1]);
        }
        receivedData = parts[2];
        String logData = parts[3];

        logger.debug("response: " + trimLogData(receivedData));
        if (logData != null && !logData.equals(""))
            logger.debug(logData);

        handleErrors(receivedData, logData);

        return receivedData;
    }

    protected void SendCommand(String... arguments) {
        send(createCommand(arguments));
    }

    private void send(String message) {
        altBaseSettings.out.print(message);
        altBaseSettings.out.flush();
        logger.debug("sent: {}", message);
    }

    private String createCommand(String[] arguments) {
        String command = String.join(altBaseSettings.RequestSeparator, arguments);
        messageId = Long.toString(System.currentTimeMillis());

        command = String.join(altBaseSettings.RequestSeparator, messageId, command) + altBaseSettings.RequestEnd;
        return command;
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

    protected void validateResponse(String expected, String received) {
        if (!expected.equals(received)) {
            throw new AltUnityInvalidServerResponse(expected, received);
        }
    }

    protected void handleErrors(String data) {
        handleErrors(data, "");
    }

    private void handleErrors(String data, String log) {
        String typeOfException = data.split(";")[0];
        if (!log.equals(""))
            log = "\n" + log;

        data = data + log;
        if ("error:notFound".equals(typeOfException)) {
            throw new NotFoundException(data);
        } else if ("error:propertyNotFound".equals(typeOfException)) {
            throw new PropertyNotFoundException(data);
        } else if ("error:methodNotFound".equals(typeOfException)) {
            throw new MethodNotFoundException(data);
        } else if ("error:componentNotFound".equals(typeOfException)) {
            throw new ComponentNotFoundException(data);
        } else if ("error:invalidParameterType".equals(typeOfException)) {
            throw new InvalidParameterTypeException(data);
        } else if ("error:assemblyNotFound".equals(typeOfException)) {
            throw new AssemblyNotFoundException(data);
        } else if ("error:couldNotPerformOperation".equals(typeOfException)) {
            throw new CouldNotPerformOperationException(data);
        } else if ("error:couldNotParseJsonString".equals(typeOfException)) {
            throw new CouldNotParseJsonStringException(data);
        } else if ("error:methodWithGivenParametersNotFound".equals(typeOfException)) {
            throw new MethodWithGivenParametersNotFoundException(data);
        } else if ("error:failedToParseMethodArguments".equals(typeOfException)) {
            throw new FailedToParseArgumentsException(data);
        } else if ("error:objectNotFound".equals(typeOfException)) {
            throw new ObjectWasNotFoundException(data);
        } else if ("error:propertyCannotBeSet".equals(typeOfException)) {
            throw new PropertyNotFoundException(data);
        } else if ("error:nullReferenceException".equals(typeOfException)) {
            throw new NullReferenceException(data);
        } else if ("error:unknownError".equals(typeOfException)) {
            throw new UnknownErrorException(data);
        } else if ("error:formatException".equals(typeOfException)) {
            throw new FormatException(data);
        } else if ("error:invalidPath".equals(typeOfException)) {
            throw new AltUnityInvalidPathException(data);
        }
    }

    public String vectorToJsonString(int x, int y) {
        return "{\"x\":" + x + ", \"y\":" + y + "}";
    }

    public String vectorToJsonString(int x, int y, int z) {
        return "{\"x\":" + x + ", \"y\":" + y + ", \"z\":" + z + "}";
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
