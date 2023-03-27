package com.alttester.altTesterExceptions;

/**
 * Raised when the client can not connect to the server.
 */
public class ConnectionException extends AltException {
    public ConnectionException(String message, Throwable e) {
        super(message, e);
    }

    public ConnectionException(String message) {
        super(message);
    }

    public ConnectionException(Throwable e) {
        super(e);
    }
}
