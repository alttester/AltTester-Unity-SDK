/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.FindObject;

import com.alttester.AltMessage;
import com.alttester.AltObject;

public class AltWaitForVisualElementPropertyParams extends AltMessage {
  public static class Builder {
    private String propertyName;
    private double timeout = 20;
    private double interval = 0.5;
    private AltObject altObject;

    public Builder(String propertyName) {
      this.propertyName = propertyName;
    }

    public AltWaitForVisualElementPropertyParams.Builder withTimeout(double timeout) {
      this.timeout = timeout;
      return this;
    }

    public AltWaitForVisualElementPropertyParams.Builder withInterval(double interval) {
      this.interval = interval;
      return this;
    }

    public AltWaitForVisualElementPropertyParams build() {
      AltWaitForVisualElementPropertyParams AltWaitForVisualElementPropertyParams = new AltWaitForVisualElementPropertyParams();
      AltWaitForVisualElementPropertyParams.propertyName = this.propertyName;
      AltWaitForVisualElementPropertyParams.timeout = this.timeout;
      AltWaitForVisualElementPropertyParams.interval = this.interval;
      AltWaitForVisualElementPropertyParams.altObject = this.altObject;

      return AltWaitForVisualElementPropertyParams;
    }
  }

  private AltWaitForVisualElementPropertyParams() {
  }

  private String propertyName;
  private double timeout = 20;
  private double interval = 0.5;
  private AltObject altObject;

  public String getPropertyName() {
    return propertyName;
  }

  public void setPropertyName(String propertyName) {
    this.propertyName = propertyName;
  }

  public AltObject getAltObject() {
    return altObject;
  }

  public void setAltObject(AltObject altObject) {
    this.altObject = altObject;
  }

  public double getTimeout() {
    return timeout;
  }

  public void setTimeout(double timeout) {
    this.timeout = timeout;
  }

  public double getInterval() {
    return interval;
  }

  public void setInterval(double interval) {
    this.interval = interval;
  }
}
