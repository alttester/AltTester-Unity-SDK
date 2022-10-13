package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

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
