package ro.altom.altunitytester.altUnityTesterExceptions;

/**
 * Raised when the client can not connect to the server.
 */
public class ConnectionException extends AltUnityException {
    public ConnectionException(String message, Throwable e) {
        super(message, e);
    }

    public ConnectionException(Throwable e) {
        super(e);
    }
}
