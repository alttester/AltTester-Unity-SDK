package ro.altom.altunitytester.altUnityTesterExceptions;

public class AltUnityInvalidPathException extends AltUnityException {
    /**
     *
     */
    private static final long serialVersionUID = 3158967400312394991L;

    public AltUnityInvalidPathException() {
    }

    public AltUnityInvalidPathException(String message) {
        super(message);
    }

    public AltUnityInvalidPathException(Throwable exception) {
        super(exception);
    }

    public AltUnityInvalidPathException(String message, Throwable exception) {
        super(message, exception);
    }
}
