/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.altTesterExceptions;

/** Raised when the client can not connect to the server. */
public class ConnectionException extends AltException {
  public ConnectionException(String message, Throwable cause) {
    super(message, cause);
  }

  public ConnectionException(String message) {
    this(message, null);
  }

  public ConnectionException(Throwable cause) {
    this(cause == null ? null : cause.getMessage(), cause);
  }
} // ConnectionException
