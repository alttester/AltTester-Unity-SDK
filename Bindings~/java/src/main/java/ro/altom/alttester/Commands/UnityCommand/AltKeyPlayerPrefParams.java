package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.AltDriver;

public class AltKeyPlayerPrefParams extends AltMessage{
    
    private String keyName;
    private int keyType;

    AltKeyPlayerPrefParams(String keyName){
        this.setKeyName(keyName);
    }

    AltKeyPlayerPrefParams(String keyName, AltDriver.PlayerPrefsKeyType keyType){
        this.keyName = keyName;
        this.keyType = keyType.getVal();
    }

    public int getType() {
        return keyType;
    }

    public void setType(AltDriver.PlayerPrefsKeyType keyType) {
        this.keyType = keyType.getVal();
    }

    public String getKeyName() {
        return keyName;
    }

    public void setKeyName(String keyName) {
        this.keyName = keyName;
    }
}
