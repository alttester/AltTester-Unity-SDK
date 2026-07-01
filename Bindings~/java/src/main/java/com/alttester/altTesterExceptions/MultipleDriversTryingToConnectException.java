/*
    Copyright(C) 2026 Altom Consulting
*/
package com.alttester.altTesterExceptions;

public class MultipleDriversTryingToConnectException extends ConnectionException {
  public MultipleDriversTryingToConnectException(String message, Throwable e) {
    super(message, e);
  }

  public MultipleDriversTryingToConnectException(String message) {
    super(message);
  }

  public MultipleDriversTryingToConnectException(Throwable e) {
    super(e);
  }
}
