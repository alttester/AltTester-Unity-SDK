package ro.altom.alttester.Commands.InputActions;

import ro.altom.alttester.IMessageHandler;
import ro.altom.alttester.Commands.AltBaseCommand;

public class AltClickCoordinates extends AltBaseCommand {
    private AltTapClickCoordinatesParams parameters;

    public AltClickCoordinates(IMessageHandler messageHandler, AltTapClickCoordinatesParams parameters) {
        super(messageHandler);
        this.parameters = parameters;
        this.parameters.setCommandName("clickCoordinates");
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
