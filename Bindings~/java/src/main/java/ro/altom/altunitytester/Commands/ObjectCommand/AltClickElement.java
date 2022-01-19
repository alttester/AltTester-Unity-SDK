package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltCommandReturningAltObjects;

public class AltClickElement extends AltCommandReturningAltObjects {
    /**
     * @param command The parameters
     */
    private AltTapClickElementParams params;

    /**
     * @param altUnityObject The game object
     */

    public AltClickElement(IMessageHandler messageHandler, AltTapClickElementParams parameters) {
        super(messageHandler);
        this.params = parameters;
        this.params.setCommandName("clickElement");
    }

    public AltUnityObject Execute() {
        SendCommand(params);
        AltUnityObject obj = ReceiveAltUnityObject(params);

        if (params.getWait()) {
            String data = recvall(params, String.class);
            validateResponse("Finished", data);
        }

        return obj;
    }
}
