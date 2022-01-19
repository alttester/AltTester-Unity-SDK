package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

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
