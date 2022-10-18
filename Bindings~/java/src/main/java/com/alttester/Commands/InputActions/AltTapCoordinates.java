package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltTapCoordinates extends AltBaseCommand {
    private AltTapClickCoordinatesParams parameters;

    public AltTapCoordinates(IMessageHandler messageHandler, AltTapClickCoordinatesParams parameters) {
        super(messageHandler);
        this.parameters = parameters;
        this.parameters.setCommandName("tapCoordinates");
    }

    public void Execute() {
        SendCommand(parameters);
        String data = recvall(parameters, String.class);
        validateResponse("Ok", data);
        if (parameters.getWait()) {
            data = recvall(parameters, String.class);
            validateResponse("Finished", data);
        }
    }
}
