package ro.altom.altunitytester.altUnityTesterExceptions;

public class AltUnityException extends RuntimeException {
    /**
     *
     */
    private static final long serialVersionUID = -4049659159406476777L;

    public AltUnityException() {
    }

    public AltUnityException(String message) {
        super(message);
    }

    public AltUnityException(Throwable exception) {
        super(exception);
    }

    public AltUnityException(String message, Throwable exception) {
        super(message, exception);
    }
}
