package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

public class AltSetTimeScale extends AltBaseCommand {

    private AltSetTimeScaleParams params;

    public AltSetTimeScale(IMessageHandler messageHandler, AltSetTimeScaleParams params) {
        super(messageHandler);
        this.params = params;
        params.setCommandName("setTimeScale");
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
