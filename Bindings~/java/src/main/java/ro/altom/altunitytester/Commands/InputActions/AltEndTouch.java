package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltEndTouch extends AltBaseCommand {
    private int fingerId;

    public AltEndTouch(AltBaseSettings altBaseSettings, int fingerId) {
        super(altBaseSettings);
        this.fingerId = fingerId;
    }

    public void Execute() {
        SendCommand("endTouch", String.valueOf(fingerId));
        String data = recvall();
        validateResponse("Ok", data);
    }
}
