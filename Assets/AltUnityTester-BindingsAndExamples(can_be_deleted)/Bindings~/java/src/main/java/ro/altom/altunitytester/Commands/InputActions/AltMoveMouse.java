package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltMoveMouse extends AltBaseCommand {
    private AltMoveMouseParameters altMoveMouseParameters;
    public AltMoveMouse(AltBaseSettings altBaseSettings, AltMoveMouseParameters altMoveMouseParameters) {
        super(altBaseSettings);
        this.altMoveMouseParameters = altMoveMouseParameters;
    }
    public void Execute(){
        send(CreateCommand("moveMouse", vectorToJsonString(altMoveMouseParameters.getX(),altMoveMouseParameters.getY()),String.valueOf(altMoveMouseParameters.getDuration())));
        String data = recvall();
        if (!data.contains("error:")) {
            return;
        }
        handleErrors(data);
    }
}

