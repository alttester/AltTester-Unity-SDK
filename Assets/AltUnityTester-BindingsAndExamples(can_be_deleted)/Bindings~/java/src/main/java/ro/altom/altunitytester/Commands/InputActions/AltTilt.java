package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltTilt extends AltBaseCommand {
    private int x;
    private int y;
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
