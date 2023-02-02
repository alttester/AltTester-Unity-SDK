package com.alttester.Commands.FindObject;

import com.alttester.AltDriver;
import com.alttester.IMessageHandler;
import com.alttester.Commands.AltCommandReturningAltObjects;

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
