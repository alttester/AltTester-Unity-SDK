package ro.altom.altunitytester.altUnityTesterExceptions;

public class AltUnityInvalidServerResponse extends AltUnityException {
    public AltUnityInvalidServerResponse(String expected, String received) {
        super(String.format("Expected to get response '%s'; Got  '%s''", expected, received));
    }
}
