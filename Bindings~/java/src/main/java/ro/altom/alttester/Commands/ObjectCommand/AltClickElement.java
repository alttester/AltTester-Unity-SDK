package ro.altom.alttester.Commands.ObjectCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltObject;
import ro.altom.alttester.Commands.AltCommandReturningAltObjects;

public class AltClickElement extends AltCommandReturningAltObjects {
    /**
     * @param command The parameters
     */
    private AltTapClickElementParams params;

    /**
     * @param parameters
     * @param messageHandler
     */

    public AltClickElement(IMessageHandler messageHandler, AltTapClickElementParams parameters) {
        super(messageHandler);
        this.params = parameters;
        this.params.setCommandName("clickElement");
    }

    public AltObject Execute() {
        SendCommand(params);
        AltObject obj = ReceiveAltObject(params);

        if (params.getWait()) {
            String data = recvall(params, String.class);
            validateResponse("Finished", data);
        }

        return obj;
    }
}
