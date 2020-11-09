package ro.altom.altunitytester.altUnityTesterExceptions;

public class ConnectionException extends AltUnityException {
    /**
     *
     */
    private static final long serialVersionUID = 326835660345791896L;

    public ConnectionException(String message, Throwable e) {
        super(message, e);
    }

    public ConnectionException(Throwable e) {
        super(e);
    }
}
