package ro.altom.altunitytester.altUnityTesterExceptions;

public class CameraNotFoundException extends AltUnityException {
    private static final long serialVersionUID = 4896864591826296908L;

    public CameraNotFoundException() {
    }

    public CameraNotFoundException(String message) {
        super(message);
    }
}
