package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

public class AltEndTouch extends AltBaseCommand {
    private AltEndTouchParams params;

    public AltEndTouch(IMessageHandler messageHandler, AltEndTouchParams params) {
        super(messageHandler);
        this.params = params;
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
