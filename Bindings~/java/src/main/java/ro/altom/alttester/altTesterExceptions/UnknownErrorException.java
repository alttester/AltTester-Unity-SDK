package ro.altom.alttester.altTesterExceptions;

public class UnknownErrorException extends AltException {
    public UnknownErrorException() {
    }

    public UnknownErrorException(String message) {
        super(message);
    }
}
