package ro.altom.altunitytester.Commands.InputActions;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltClickScreen extends AltBaseCommand {
    private float x;
    private float y;
    public AltClickScreen(AltBaseSettings altBaseSettings, float x, float y) {
        super(altBaseSettings);
        this.x = x;
        this.y = y;
    }
    public AltUnityObject Execute(){
        send(CreateCommand("clickScreenOnXY", String.valueOf(x), String.valueOf(y)));
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, AltUnityObject.class));
        }
        handleErrors(data);
        return null;
    }
}
