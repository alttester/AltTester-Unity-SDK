package ro.altom.altunitytester.altUnityTesterExceptions;

public class AltUnityInvalidServerResponse extends AltUnityException {
    /**
     *
     */
    private static final long serialVersionUID = -8402911747448385086L;

    public AltUnityInvalidServerResponse(String expected, String received) {
        super(String.format("Expected to get response '%s'; Got  '%s''", expected, received));
    }
}
