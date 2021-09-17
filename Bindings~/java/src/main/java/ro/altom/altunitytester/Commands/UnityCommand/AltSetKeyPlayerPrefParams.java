package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityDriver;

public class AltSetKeyPlayerPrefParams extends AltMessage{
    
    String keyName;
    int intValue;
    float floatValue;
    String stringValue;
    int keyType;

    AltSetKeyPlayerPrefParams(String keyName, int intValue){
        this.keyName = keyName;
        this.intValue = intValue;
        this.keyType = AltUnityDriver.PlayerPrefsKeyType.Int.getVal();
    }

    AltSetKeyPlayerPrefParams(String keyName, float floatValue){
        this.keyName = keyName;
        this.floatValue = floatValue;
        this.keyType = AltUnityDriver.PlayerPrefsKeyType.Float.getVal();
    }

    AltSetKeyPlayerPrefParams(String keyName, String stringValue){
        this.keyName = keyName;
        this.stringValue = stringValue;
        this.keyType = AltUnityDriver.PlayerPrefsKeyType.String.getVal();
    }
}
