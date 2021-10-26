package ro.altom.altunitytester.altUnityTesterExceptions;

public class AltUnityRecvallException extends AltUnityException {
    public AltUnityRecvallException() {
    }

    public AltUnityRecvallException(String message) {
        super(message);
    }

    public AltUnityRecvallException(Throwable exception) {
        super(exception);
    }

    public AltUnityRecvallException(String message, Throwable exception) {
        super(message, exception);
    }
}
