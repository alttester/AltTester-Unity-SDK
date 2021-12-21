package ro.altom.altunitytester.Notifications;

public class AltUnityLogNotificationResultParams {
    
    public String message;
    public String stackTrace;
    public int level;

    public AltUnityLogNotificationResultParams(String message, String stackTrace, int level) {
        this.message = message;
        this.stackTrace = stackTrace;
        this.level = level;
    }
}
