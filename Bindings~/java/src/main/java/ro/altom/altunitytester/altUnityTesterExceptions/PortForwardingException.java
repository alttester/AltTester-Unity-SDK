package ro.altom.altunitytester.altUnityTesterExceptions;

public class PortForwardingException extends AltUnityException {

    /**
     *
     */
    private static final long serialVersionUID = -7629828251460910071L;

    public PortForwardingException() {
    }

    public PortForwardingException(String message) {
        super(message);
    }

    public PortForwardingException(Throwable exception) {
        super(exception);
    }

    public PortForwardingException(String message, Throwable exception) {
        super(message, exception);
    }
}
