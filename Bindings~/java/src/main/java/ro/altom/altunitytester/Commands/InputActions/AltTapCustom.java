package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltTapCustom extends AltBaseCommand {
    private int x;
    private int y;
    private int count;
    private float interval;
    public AltTapCustom(AltBaseSettings altBaseSettings, int x, int y, int count, float interval) {
        super(altBaseSettings);
        this.x = x;
        this.y = y;
        this.count = count;
        this.interval = interval;
    }
    public void Execute() {
        String position = vectorToJsonString(x, y);
        send(CreateCommand("tapCustom", position, String.valueOf(count), String.valueOf(interval)));
        String data = recvall();
        if (!data.contains("error:")) {
            return;
        }
        handleErrors(data);
    }
}