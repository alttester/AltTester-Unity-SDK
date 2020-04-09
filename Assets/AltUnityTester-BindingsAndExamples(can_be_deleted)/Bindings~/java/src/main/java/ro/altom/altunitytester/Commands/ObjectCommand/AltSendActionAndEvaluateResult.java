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
    private String parameter;
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;

    public AltSendActionAndEvaluateResult(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject,
            String command) {
        this(altBaseSettings, altUnityObject, command, null);
    }

    public AltSendActionAndEvaluateResult(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject,
            String command, String parameter) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.command = command;
        this.parameter = parameter;
    }

    public AltUnityObject Execute() {
        String altObject = new Gson().toJson(altUnityObject);
        String strCommand = parameter == null ? CreateCommand(command, altObject)
                : CreateCommand(command, altObject, parameter);

        send(strCommand);
        String data = recvall();
        if (!data.contains("error:")) {
            AltUnityObject obj = new Gson().fromJson(data, AltUnityObject.class);
            obj.setAltBaseSettings(altBaseSettings);
            return obj;
        }
        handleErrors(data);
        return null;
    }
}
