package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.Commands.AltCommandReturningAltObjects;

public class AltBaseFindObject extends AltCommandReturningAltObjects {
    public AltBaseFindObject(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    protected String SetPath(AltUnityDriver.By by, String value)
    {
        String path = "";
        switch (by)
        {
            case TAG:
                path = "//*[@tag=" + value+"]";
                break;
            case LAYER:
                path = "//*[@layer=" + value+"]";
                break;
            case NAME:
                path = "//" + value;
                break;
            case COMPONENT:
                path = "//*[@component=" + value+"]";
                break;
            case PATH:
                path = value;
                break;
            case ID:
                path = "//*[@id=" + value+"]";
                break;
        }
        return path;
    }
    protected String SetPathContains(AltUnityDriver.By by, String value)
    {
        String path = "";
        switch (by)
        {
            case TAG:
                path = "//*[contains(@tag," + value + ")]";
                break;
            case LAYER:
                path = "//*[contains(@layer," + value + ")]";
                break;
            case NAME:
                path = "//*[contains(@name," + value+")]";
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
        }
        return path;
    }
}
