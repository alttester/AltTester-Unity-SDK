package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSetTimeScale extends AltBaseCommand {
    
    private AltSetTimescaleParams params;

    public AltSetTimeScale(IMessageHandler messageHandler, float timeScale) {
        super(messageHandler);
        params = new AltSetTimescaleParams(timeScale);
        params.setCommandName("setTimeScale");
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);
    }
}
