package ro.altom.altunitytester.altUnityTesterExceptions;

/**
 * Raised when the client connection timesout.
 */
public class ConnectionTimeoutException extends AltUnityException {
    public ConnectionTimeoutException(String message, Throwable e) {
        super(message, e);
    }

    public ConnectionTimeoutException(Throwable e) {
        super(e);
    }
}
