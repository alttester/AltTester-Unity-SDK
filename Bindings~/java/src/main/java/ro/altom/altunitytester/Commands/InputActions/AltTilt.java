package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulates device rotation action in your game.
 */
public class AltTilt extends AltBaseCommand {
    /**
     * @param x Linear acceleration of a device on x
     */
    private int x;
    /**
     * @param y Linear acceleration of a device on y
     */
    private int y;
    /**
     * @param z Linear acceleration of a device on z
     */
    private int z;
    public AltTilt(AltBaseSettings altBaseSettings, int x, int y, int z) {
        super(altBaseSettings);
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public void Execute(){
        String accelerationString = vectorToJsonString(x, y, z);
        send(CreateCommand("tilt", accelerationString));
        String data = recvall();
        if (data.equals("OK")) {
            return;
        }
        handleErrors(data);
    }
}
