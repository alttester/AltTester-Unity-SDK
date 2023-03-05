package com.alttester.Exceptions;

public class UnknownErrorException extends AltException {
    public UnknownErrorException() {
    }

    public UnknownErrorException(String message) {
        super(message);
    }
}
