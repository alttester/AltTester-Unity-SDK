package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.IMessageHandler;

public class GetServerVersionCommand extends AltBaseCommand {
    public GetServerVersionCommand(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public String Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getServerVersion");
        SendCommand(altMessage);
        return recvall(altMessage, String.class);
    }
}
