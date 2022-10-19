package com.alttester.Commands.ObjectCommand;

import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.Commands.AltBaseCommand;

public class AltTapElement extends AltBaseCommand {
    /**
     * @param command The parameters
     */
    private AltTapClickElementParams parameters;

    /**
     * @param messageHandler - Message
     * @param parameters     - int count , float interval , boolean wait
     */

    public AltTapElement(IMessageHandler messageHandler,
            AltTapClickElementParams parameters) {
        super(messageHandler);
        this.parameters = parameters;
        this.parameters.setCommandName("tapElement");
    }

    public AltObject Execute() {
        SendCommand(parameters);
        AltObject obj = recvall(parameters, AltObject.class);
        obj.setMesssageHandler(messageHandler);

        if (parameters.getWait()) {
            String data = recvall(parameters, String.class);
            validateResponse("Finished", data);
        }

        return obj;
    }
}
