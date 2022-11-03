package com.alttester.altTesterExceptions;

public class CommandResponseTimeoutException extends AltException {
    public CommandResponseTimeoutException() {
    }

    public CommandResponseTimeoutException(String message) {
        super(message);
    }
}
