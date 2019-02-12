package ro.altom.altunitytester.altUnityTesterExceptions;

public class ConnectionException extends AltUnityException {
    public ConnectionException(String message, Throwable e) {
        super(message, e);
    }

    public ConnectionException(Throwable e) {
        super(e);
    }
}
