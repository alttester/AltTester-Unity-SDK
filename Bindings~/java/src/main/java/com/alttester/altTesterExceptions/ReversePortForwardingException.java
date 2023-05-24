package com.alttester.altTesterExceptions;

public class ReversePortForwardingException extends AltException {

    /**
     *
     */
    private static final long serialVersionUID = -7629828251460910071L;

    public ReversePortForwardingException() {
    }

    public ReversePortForwardingException(String message) {
        super(message);
    }

    public ReversePortForwardingException(Throwable exception) {
        super(exception);
    }

    public ReversePortForwardingException(String message, Throwable exception) {
        super(message, exception);
    }
}
