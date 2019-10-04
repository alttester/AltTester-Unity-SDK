package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltFindObject extends AltBaseFindObject {
    private AltFindObjectsParameters altFindObjectsParameters;
    public AltFindObject(AltBaseSettings altBaseSettings, AltFindObjectsParameters altFindObjectsParameters) {
        super(altBaseSettings);
        this.altFindObjectsParameters = altFindObjectsParameters;
    }
    public AltUnityObject Execute(){
        if(altFindObjectsParameters.isEnabled() && altFindObjectsParameters.getBy()== AltUnityDriver.By.NAME){
            send(CreateCommand("findActiveObjectByName", altFindObjectsParameters.getValue(), altFindObjectsParameters.getCameraName(), String.valueOf(altFindObjectsParameters.isEnabled())));
        }else{
            String path= SetPath(altFindObjectsParameters.getBy(),altFindObjectsParameters.getValue());
            send(CreateCommand("findObject", path, altFindObjectsParameters.getCameraName(), String.valueOf(altFindObjectsParameters.isEnabled())));
        }
        return ReceiveAltUnityObject();
    }
}
