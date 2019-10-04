package ro.altom.altunitytester.Commands.FindObject;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

public class AltGetAllElements extends AltBaseFindObject {
    private AltGetAllElementsParameters altGetAllElementsParameters;
    public AltGetAllElements(AltBaseSettings altBaseSettings, AltGetAllElementsParameters altGetAllElementsParameters) {
        super(altBaseSettings);
        this.altGetAllElementsParameters = altGetAllElementsParameters;
    }
    public AltUnityObject[] Execute(){
        send(CreateCommand("findObjects", "//*",altGetAllElementsParameters.getCameraName(), String.valueOf(altGetAllElementsParameters.isEnabled())));
        return ReceiveListOfAltUnityObjects();
    }
}
