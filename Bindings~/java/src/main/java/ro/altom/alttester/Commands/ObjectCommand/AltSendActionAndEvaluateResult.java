package ro.altom.alttester.Commands.ObjectCommand;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.AltObject;
import ro.altom.alttester.Commands.AltBaseCommand;

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
