package com.alttester.Exceptions;

public class MethodWithGivenParametersNotFoundException extends AltException {
    public MethodWithGivenParametersNotFoundException() {
    }

    public MethodWithGivenParametersNotFoundException(String message) {
        super(message);
    }
}
