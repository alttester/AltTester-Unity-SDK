package ro.altom.altunitytester.Commands.InputActions;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.altUnityTesterExceptions.NotFoundException;

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

    public AltUnityObject Execute() {
        SendCommand("tapScreen", String.valueOf(x), String.valueOf(y));
        try {
            String data = recvall();
            return new Gson().fromJson(data, AltUnityObject.class);
        } catch (NotFoundException notFoundException) {
            return null;
        }
    }
}
