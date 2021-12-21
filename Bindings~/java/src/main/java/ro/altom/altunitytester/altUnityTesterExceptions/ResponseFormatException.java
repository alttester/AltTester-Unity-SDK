package ro.altom.altunitytester.altUnityTesterExceptions;

public class ResponseFormatException extends AltUnityException {
    public <T> ResponseFormatException(Class<T> type, String data) {
        super("Could not deserialize response data: `" + data + "` into " + type.getName());
    }
}