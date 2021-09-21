package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltTapCoordinates extends AltBaseCommand {
    private AltTapClickCoordinatesParameters parameters;

    public AltTapCoordinates(IMessageHandler messageHandler, AltTapClickCoordinatesParameters parameters) {
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
