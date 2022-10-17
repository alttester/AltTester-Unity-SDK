package ro.altom.alttester.Commands.UnityCommand;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

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
