package com.alttester.altTesterExceptions;

public class MultipleDriversException extends ConnectionException {
    public MultipleDriversException(String message, Throwable e) {
        super(message, e);
    }

    public MultipleDriversException(String message) {
        super(message);
    }

    public MultipleDriversException(Throwable e) {
        super(e);
    }
}
