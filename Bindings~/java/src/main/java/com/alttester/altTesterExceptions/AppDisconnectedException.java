package com.alttester.altTesterExceptions;

public class AppDisconnectedException extends ConnectionException {
    public AppDisconnectedException(String message, Throwable e) {
        super(message, e);
    }

    public AppDisconnectedException(String message) {
        super(message);
    }

    public AppDisconnectedException(Throwable e) {
        super(e);
    }
}
