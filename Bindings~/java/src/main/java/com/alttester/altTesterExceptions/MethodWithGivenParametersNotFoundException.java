package com.alttester.altTesterExceptions;

public class MethodWithGivenParametersNotFoundException extends AltException {
    public MethodWithGivenParametersNotFoundException() {
    }

    public MethodWithGivenParametersNotFoundException(String message) {
        super(message);
    }
}
