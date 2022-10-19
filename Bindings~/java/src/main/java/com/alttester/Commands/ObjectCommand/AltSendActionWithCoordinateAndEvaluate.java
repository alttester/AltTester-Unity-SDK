package com.alttester.Commands.ObjectCommand;

import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.Commands.AltBaseCommand;

public class AltSendActionWithCoordinateAndEvaluate extends AltBaseCommand {

    private AltSendActionWithCoordinateAndEvaluateParams params;

    public AltSendActionWithCoordinateAndEvaluate(IMessageHandler messageHandler, AltObject altObject, int x,
            int y, String command) {
        super(messageHandler);
        params = new AltSendActionWithCoordinateAndEvaluateParams(altObject, x, y);
        params.setCommandName(command);
    }

    public AltObject Execute() {
        SendCommand(params);
        return recvall(params, AltObject.class);
    }
}
