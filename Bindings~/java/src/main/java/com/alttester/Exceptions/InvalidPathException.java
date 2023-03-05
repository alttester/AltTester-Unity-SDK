package com.alttester.Exceptions;

public class InvalidPathException extends AltException {
    public InvalidPathException() {
    }

    public InvalidPathException(String message) {
        super(message);
    }

    public InvalidPathException(Throwable exception) {
        super(exception);
    }

    public InvalidPathException(String message, Throwable exception) {
        super(message, exception);
    }
}
