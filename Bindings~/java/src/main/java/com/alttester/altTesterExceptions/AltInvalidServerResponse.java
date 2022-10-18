package com.alttester.altTesterExceptions;

public class AltInvalidServerResponse extends AltException {
    public AltInvalidServerResponse(String expected, String received) {
        super(String.format("Expected to get response '%s'; Got  '%s''", expected, received));
    }
}
