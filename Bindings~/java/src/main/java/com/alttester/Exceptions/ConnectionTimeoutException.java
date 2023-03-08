package com.alttester.Exceptions;

/**
 * Raised when the client connection timesout.
 */
public class ConnectionTimeoutException extends AltException {
    public ConnectionTimeoutException(String message, Throwable e) {
        super(message, e);
    }

    public ConnectionTimeoutException(Throwable e) {
        super(e);
    }
}
