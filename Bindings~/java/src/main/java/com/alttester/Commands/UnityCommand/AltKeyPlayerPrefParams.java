package com.alttester.Commands.UnityCommand;

import com.alttester.AltMessage;
import com.alttester.AltDriver;

public class AltKeyPlayerPrefParams extends AltMessage {

    private String keyName;
    private int keyType;

    AltKeyPlayerPrefParams(String keyName) {
        this.setKeyName(keyName);
    }

    AltKeyPlayerPrefParams(String keyName, AltDriver.PlayerPrefsKeyType keyType) {
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
