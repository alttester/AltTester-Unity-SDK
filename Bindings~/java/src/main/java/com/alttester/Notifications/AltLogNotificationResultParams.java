package com.alttester.Notifications;

public class AltLogNotificationResultParams {

    public String message;
    public String stackTrace;
    public int level;

    public AltLogNotificationResultParams(String message, String stackTrace, int level) {
        this.message = message;
        this.stackTrace = stackTrace;
        this.level = level;
    }
}
