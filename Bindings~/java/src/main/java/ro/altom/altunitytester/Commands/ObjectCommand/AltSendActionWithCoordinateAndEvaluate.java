package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSendActionWithCoordinateAndEvaluate extends AltBaseCommand {
    private AltUnityObject altUnityObject;
    private int x;
    private int y;
    private String command;
    public AltSendActionWithCoordinateAndEvaluate(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject, int x, int y, String command) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.x = x;
        this.y = y;
        this.command = command;
    }
    public AltUnityObject Execute(){
        String positionString = vectorToJsonString(x, y);
        String altObject = new Gson().toJson(altUnityObject);
        send(CreateCommand(command ,positionString, altObject ));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        handleErrors(data);
        return null;
    }
}
