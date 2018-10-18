package ro.altom.altunitytester.altUnityTesterExceptions;


public class AltUnityException extends RuntimeException {
    public AltUnityException() {
    }

    public AltUnityException(String message) {
        super(message);
    }

    public AltUnityException(Throwable exception) {
        super(exception);
    }
}

