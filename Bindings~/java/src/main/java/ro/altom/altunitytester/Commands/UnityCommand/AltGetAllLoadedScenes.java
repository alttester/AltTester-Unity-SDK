package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltGetAllLoadedScenes extends AltBaseCommand {
    public AltGetAllLoadedScenes(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public String[] Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getAllLoadedScenes");
        SendCommand(altMessage);
        return recvall(altMessage, String[].class);
    }
}
