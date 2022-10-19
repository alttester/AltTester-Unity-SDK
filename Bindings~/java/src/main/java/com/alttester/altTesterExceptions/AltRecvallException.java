package com.alttester.altTesterExceptions;

public class AltRecvallException extends AltException {
    public AltRecvallException() {
    }

    public AltRecvallException(String message) {
        super(message);
    }

    public AltRecvallException(Throwable exception) {
        super(exception);
    }

    public AltRecvallException(String message, Throwable exception) {
        super(message, exception);
    }
}
