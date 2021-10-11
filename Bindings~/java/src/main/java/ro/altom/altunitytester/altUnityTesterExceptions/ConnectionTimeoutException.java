package ro.altom.altunitytester.altUnityTesterExceptions;

/**
 * Raised when the client connection timesout.
 */
public class ConnectionTimeoutException extends AltUnityException {
    /**
     * 
     */
    private static final long serialVersionUID = 326835660345791896L;

    public ConnectionTimeoutException(String message, Throwable e) {
        super(message, e);
    }

    public ConnectionTimeoutException(Throwable e) {
        super(e);
    }
}
