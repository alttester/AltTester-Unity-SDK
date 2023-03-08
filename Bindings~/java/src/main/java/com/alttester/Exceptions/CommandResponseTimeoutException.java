package com.alttester.Exceptions;

public class CommandResponseTimeoutException extends AltException {
    public CommandResponseTimeoutException() {
    }

    public CommandResponseTimeoutException(String message) {
        super(message);
    }
}
