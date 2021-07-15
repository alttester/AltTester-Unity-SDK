package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltTapElement extends AltBaseCommand {
    /**
     * @param command The parameters
     */
    private AltTapClickElementParameters parameters;
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;

    public AltTapElement(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject,
            AltTapClickElementParameters parameters) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.parameters = parameters;
    }

    public AltUnityObject Execute() {
        String altObject = new Gson().toJson(altUnityObject);

        SendCommand("tapElement", altObject, String.valueOf(parameters.getCount()),
                String.valueOf(parameters.getInterval()), String.valueOf(parameters.getWait()));

        String data = recvall();
        AltUnityObject obj = new Gson().fromJson(data, AltUnityObject.class);
        obj.setAltBaseSettings(altBaseSettings);

        if (parameters.getWait()) {
            data = recvall();
            validateResponse("Finished", data);
        }

        return obj;
    }
}
