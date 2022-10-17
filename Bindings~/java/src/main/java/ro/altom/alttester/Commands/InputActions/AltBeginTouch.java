package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

public class AltBeginTouch extends AltBaseCommand {
    private AltBeginTouchParams params;

    public AltBeginTouch(IMessageHandler messageHandler, AltBeginTouchParams params) {
        super(messageHandler);
        this.params = params;
    }

    public int Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        return Integer.parseInt(data);
    }
}
