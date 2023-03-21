package com.alttester.altTesterExceptions;

public class NoAppConnectedException extends ConnectionException {
    public NoAppConnectedException(String message, Throwable e) {
        super(message, e);
    }

    public NoAppConnectedException(String message) {
        super(message);
    }

    public NoAppConnectedException(Throwable e) {
        super(e);
    }
}
