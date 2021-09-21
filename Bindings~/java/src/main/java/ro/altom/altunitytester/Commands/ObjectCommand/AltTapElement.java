package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltTapElement extends AltBaseCommand {
    /**
     * @param command The parameters
     */
    private AltTapClickElementParameters parameters;
    /**
     * @param altUnityObject The game object
     */

    public AltTapElement(IMessageHandler messageHandler,
            AltTapClickElementParameters parameters) {
        super(messageHandler);
        this.parameters = parameters;
        this.parameters.setCommandName("tapElement");
    }

    public AltUnityObject Execute() {
        SendCommand(parameters);
        AltUnityObject obj = recvall(parameters, AltUnityObject.class);
        obj.setMesssageHandler(messageHandler);

        if (parameters.getWait()) {
            String data = recvall(parameters, String.class);
            validateResponse("Finished", data);
        }

        return obj;
    }
}
