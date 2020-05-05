package ro.altom.altunitytester.Commands.InputActions;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate a tap action on the screen at the given coordinates.
 */
public class AltTapScreen extends AltBaseCommand {
    /**
     * @param x x coordinate of the screen
     */
    private int x;
    /**
     * @param y y coordinate of the screen
     */
    private int y;
    public AltTapScreen(AltBaseSettings altBaseSettings, int x, int y) {
        super(altBaseSettings);
        this.x = x;
        this.y = y;
    }
    public AltUnityObject Execute(){
        send(CreateCommand("tapScreen", String.valueOf(x), String.valueOf(y)));
        String data = recvall();
        if (!data.contains("error:")) {
            return new Gson().fromJson(data, AltUnityObject.class);
        }
        if(data.contains("error:notFound"))
            return null;
        handleErrors(data);
        return null;
    }
}
