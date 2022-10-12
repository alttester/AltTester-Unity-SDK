package ro.altom.alttester.altTesterExceptions;

public class ResponseFormatException extends AltException {
    public <T> ResponseFormatException(Class<T> type, String data) {
        super("Could not deserialize response data: `" + data + "` into " + type.getName());
    }
}