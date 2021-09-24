package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityDriver;

public class AltKeyPlayerPrefParams extends AltMessage{
    
    private String keyName;
    private int keyType;

    AltKeyPlayerPrefParams(String keyName){
        this.setKeyName(keyName);
    }

    AltKeyPlayerPrefParams(String keyName, AltUnityDriver.PlayerPrefsKeyType keyType){
        this.keyName = keyName;
        this.keyType = keyType.getVal();
    }

    public int getType() {
        return keyType;
    }

    public void setType(AltUnityDriver.PlayerPrefsKeyType keyType) {
        this.keyType = keyType.getVal();
    }

    public String getKeyName() {
        return keyName;
    }

    public void setKeyName(String keyName) {
        this.keyName = keyName;
    }
}
