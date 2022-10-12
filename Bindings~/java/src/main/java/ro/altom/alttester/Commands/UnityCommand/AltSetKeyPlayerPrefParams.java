package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.AltDriver;

public class AltSetKeyPlayerPrefParams extends AltMessage{
    
    String keyName;
    int intValue;
    float floatValue;
    String stringValue;
    int keyType;

    AltSetKeyPlayerPrefParams(String keyName, int intValue){
        this.keyName = keyName;
        this.intValue = intValue;
        this.keyType = AltDriver.PlayerPrefsKeyType.Int.getVal();
    }

    AltSetKeyPlayerPrefParams(String keyName, float floatValue){
        this.keyName = keyName;
        this.floatValue = floatValue;
        this.keyType = AltDriver.PlayerPrefsKeyType.Float.getVal();
    }

    AltSetKeyPlayerPrefParams(String keyName, String stringValue){
        this.keyName = keyName;
        this.stringValue = stringValue;
        this.keyType = AltDriver.PlayerPrefsKeyType.String.getVal();
    }
}
