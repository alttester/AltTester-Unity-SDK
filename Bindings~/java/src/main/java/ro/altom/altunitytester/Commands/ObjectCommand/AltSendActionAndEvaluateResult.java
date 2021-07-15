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
    private String[] parameters;
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;

    public AltSendActionAndEvaluateResult(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject,
            String command, String... parameters) {
                super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.command = command;
        this.parameters = parameters;
    }

    public AltUnityObject Execute() {
        String altObject = new Gson().toJson(altUnityObject);
        
        String[] args=  new String[parameters.length+2];
        args[0] = command;
        args[1] = altObject;
        for ( int i =0 ; i< parameters.length; i++ )
        {
            args[i+2] = parameters[i];
        }
        SendCommand(args);

        String data = recvall();

        AltUnityObject obj = new Gson().fromJson(data, AltUnityObject.class);
        obj.setAltBaseSettings(altBaseSettings);
        return obj;
    }
}
