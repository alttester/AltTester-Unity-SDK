package com.alttester.Exceptions;

public class AltException extends RuntimeException {
    public AltException() {
    }

    public AltException(String message) {
        super(message);
    }

    public AltException(Throwable exception) {
        super(exception);
    }

    public AltException(String message, Throwable exception) {
        super(message, exception);
    }
}
