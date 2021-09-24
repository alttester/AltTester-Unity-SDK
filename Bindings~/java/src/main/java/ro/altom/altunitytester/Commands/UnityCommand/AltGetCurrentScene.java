package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltGetCurrentScene extends AltBaseCommand {
    public AltGetCurrentScene(IMessageHandler messageHandler) {
        super(messageHandler);
    }

    public String Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getCurrentScene");
        SendCommand(altMessage);
        AltUnityObject scene = recvall(altMessage, AltUnityObject.class);
        return scene.name;
    }
}
