package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltGetTimeScale extends AltBaseCommand {

    public AltGetTimeScale(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public float Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getTimeScale");
        SendCommand(altMessage);
        return recvall(altMessage, Float.class);
    }
}
