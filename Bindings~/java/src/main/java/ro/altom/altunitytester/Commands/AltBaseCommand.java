package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.altUnityTesterExceptions.*;

import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.text.SimpleDateFormat;
import java.util.Date;

public class AltBaseCommand {

    protected static final org.slf4j.Logger log = org.slf4j.LoggerFactory.getLogger(AltBaseCommand.class);

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

        String[] parts = receivedData.split("altstart::|::response::|::altLog::|::altend", -1);// -1 limit to include
                                                                                               // Trailing empty strings
        if (parts.length != 5 || !parts[0].equals("") || !parts[4].equals("")) {

            throw new AltUnityRecvallMessageFormatException(
                    "Data received from socket doesn't have correct start and end control strings");
        }

        if (!parts[1].equals(messageId)) {
            throw new AltUnityRecvallMessageIdException(
                    "Response received does not match command send. Expected message id: " + messageId + ". Got "
                            + parts[1]);
        }
        receivedData = parts[2];
        String logData = parts[3];

        log.debug(trimLogData(receivedData));
        if (altBaseSettings.logEnabled) {
            SimpleDateFormat formatter = new SimpleDateFormat("MM-dd-yyyy HH:mm:ss");
            writeInLogFile(formatter.format(new Date()) + " : response received : " + trimLogData(receivedData));
            writeInLogFile(logData);
        }

        return receivedData;
    }

    private void writeInLogFile(String logMessages) {
        BufferedWriter writer = null;
        try {
            writer = new BufferedWriter(new FileWriter("AltUnityTesterLog.txt", true));
            writer.append(logMessages + System.lineSeparator());
            writer.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    protected void SendCommand(String... arguments) {
        send(createCommand(arguments));
    }

    private void send(String message) {
        log.info("Sending rpc message [{}]", message);
        altBaseSettings.out.print(message);
        altBaseSettings.out.flush();
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

    protected void handleErrors(String data) {
        String typeOfException = data.split(";")[0];
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
            log.warn("Could not sleep for " + timeToSleep + " ms");
        }
    }
}
