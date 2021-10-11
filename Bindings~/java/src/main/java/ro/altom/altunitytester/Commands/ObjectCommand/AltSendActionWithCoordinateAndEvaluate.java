package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSendActionWithCoordinateAndEvaluate extends AltBaseCommand {

    private AltSendActionWithCoordinateAndEvaluateParams params;

    public AltSendActionWithCoordinateAndEvaluate(IMessageHandler messageHandler, AltUnityObject altUnityObject, int x,
            int y, String command) {
        super(messageHandler);
        params = new AltSendActionWithCoordinateAndEvaluateParams(altUnityObject, x, y);
        params.setCommandName(command);
    }

    public AltUnityObject Execute() {
        SendCommand(params);
        return recvall(params, AltUnityObject.class);
    }
}
