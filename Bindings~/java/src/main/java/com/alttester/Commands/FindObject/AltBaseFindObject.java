/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.Commands.FindObject;

import com.alttester.AltDriver;
import com.alttester.AltObject;
import com.alttester.Commands.AltCommandReturningAltObjects;
import com.alttester.IMessageHandler;

public class AltBaseFindObject extends AltCommandReturningAltObjects {
  public AltBaseFindObject(IMessageHandler messageHandler) {
    super(messageHandler);
  }

  protected String SetPath(AltDriver.By by, String value) {
    String path = "";
    switch (by) {
      case TAG:
        path = "//*[@tag=" + value + "]";
        break;
      case LAYER:
        path = "//*[@layer=" + value + "]";
        break;
      case NAME:
        path = "//" + value;
        break;
      case COMPONENT:
        path = "//*[@component=" + value + "]";
        break;
      case PATH:
        path = value;
        break;
      case ID:
        path = "//*[@id=" + value + "]";
        break;
      case TEXT:
        path = "//*[@text=" + value + "]";
        break;
    }
    return path;
  }

  protected String SetPathFromObject(AltObject obj, AltDriver.By by, String value) {
    String path = "";
    switch (by) {
      case TAG:
        path = "//*[@id=" + obj.id + "]//*[@tag=" + value + "]";
        break;
      case LAYER:
        path = "//*[@id=" + obj.id + "]//*[@layer=" + value + "]";
        break;
      case NAME:
        path = "//*[@id=" + obj.id + "]//" + value;
        break;
      case COMPONENT:
        path = "//*[@id=" + obj.id + "]//*[@component=" + value + "]";
        break;
      case PATH:
        path = "//*[@id=" + obj.id + "]" + value;
        break;
      case ID:
        path = "//*[@id=" + obj.id + "]//*[@id=" + value + "]";
        break;
      case TEXT:
        path = "//*[@id=" + obj.id + "]//*[@text=" + value + "]";
        break;
    }
    return path;
  }

  protected String SetPathContains(AltDriver.By by, String value) {
    String path = "";
    switch (by) {
      case TAG:
        path = "//*[contains(@tag," + value + ")]";
        break;
      case LAYER:
        path = "//*[contains(@layer," + value + ")]";
        break;
      case NAME:
        path = "//*[contains(@name," + value + ")]";
        break;
      case COMPONENT:
        path = "//*[contains(@component," + value + ")]";
        break;
      case PATH:
        path = value;
        break;
      case ID:
        path = "//*[contains(@id," + value + ")]";
        break;
      case TEXT:
        path = "//*[contains(@text," + value + ")]";
        break;
    }
    return path;
  }
}
