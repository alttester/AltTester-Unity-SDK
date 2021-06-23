package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltTapCoordinates extends AltBaseCommand {
    private AltTapClickCoordinatesParameters parameters;

    public AltTapCoordinates(AltBaseSettings altBaseSettings, AltTapClickCoordinatesParameters parameters) {
        super(altBaseSettings);
        this.parameters = parameters;
    }

    public void Execute() {
        String position = vectorToJsonString(this.parameters.getCoordinates().x, this.parameters.getCoordinates().y);
        SendCommand("tapCoordinates", position, String.valueOf(this.parameters.getCount()),
                String.valueOf(this.parameters.getInterval()), String.valueOf(this.parameters.getWait()));
        String data = recvall();
        validateResponse("Ok", data);
        if (parameters.getWait()) {
            data = recvall();
            validateResponse("Finished", data);
        }
    }
}
