package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSendActionAndEvaluateResult extends AltBaseCommand {
    /**
     * @param command The action command
     */
    private String command;
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;
    public AltSendActionAndEvaluateResult(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject, String command) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.command = command;
    }
    public AltUnityObject Execute(){
        String altObject = new Gson().toJson(altUnityObject);
        send(CreateCommand(command, altObject ));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }
}
