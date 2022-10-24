package com.alttester.Commands.ObjectCommand;

import com.alttester.IMessageHandler;
import com.alttester.AltObject;
import com.alttester.Commands.AltBaseCommand;

public class AltSendActionAndEvaluateResult extends AltBaseCommand {

    private AltSendActionAndEvaluateResultParams params;

    public AltSendActionAndEvaluateResult(IMessageHandler messageHandler, AltObject altObject,
            String command) {
        super(messageHandler);
        params = new AltSendActionAndEvaluateResultParams(altObject);
        params.setCommandName(command);
    }

    public AltObject Execute() {
        SendCommand(params);
        return recvall(params, AltObject.class);
    }
}
