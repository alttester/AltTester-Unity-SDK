package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSetKeyPlayerPref extends AltBaseCommand {

    String keyName;
    int intValue;
    float floatValue;
    String stringValue;
    int option;
    public AltSetKeyPlayerPref(AltBaseSettings altBaseSettings,String keyName,int intValue) {
        super(altBaseSettings);
        this.keyName=keyName;
        this.intValue=intValue;
        option=1;
    }
    public AltSetKeyPlayerPref(AltBaseSettings altBaseSettings,String keyName,float floatValue) {
        super(altBaseSettings);
        this.keyName=keyName;
        this.floatValue=floatValue;
        option=2;
    }
    public AltSetKeyPlayerPref(AltBaseSettings altBaseSettings,String keyName,String stringValue) {
        super(altBaseSettings);
        this.keyName=keyName;
        this.stringValue=stringValue;
        option=3;
    }

    public void Execute(){
        switch (option){
            case 1:
                send(CreateCommand("setKeyPlayerPref", keyName, String.valueOf(intValue), String.valueOf(AltUnityDriver.PlayerPrefsKeyType.IntType)));
                break;
            case 2:
                send(CreateCommand("setKeyPlayerPref", keyName, String.valueOf(floatValue), String.valueOf(AltUnityDriver.PlayerPrefsKeyType.FloatType)));
                break;
            case 3:
                send(CreateCommand("setKeyPlayerPref", keyName, String.valueOf(stringValue), String.valueOf(AltUnityDriver.PlayerPrefsKeyType.StringType)));
                break;
        }
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }

}
