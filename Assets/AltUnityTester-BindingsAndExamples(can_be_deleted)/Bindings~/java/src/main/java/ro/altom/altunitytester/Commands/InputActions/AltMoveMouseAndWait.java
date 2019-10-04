package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltMoveMouseAndWait extends AltBaseCommand {
    private  AltMoveMouseParameters altMoveMouseParameters;
    public AltMoveMouseAndWait(AltBaseSettings altBaseSettings, AltMoveMouseParameters altMoveMouseParameters) {
        super(altBaseSettings);
        this.altMoveMouseParameters = altMoveMouseParameters;
    }
    public void Execute(){
        new AltMoveMouse(altBaseSettings,altMoveMouseParameters).Execute();
        sleepFor(altMoveMouseParameters.getDuration());
        String data;
        do {
            send(CreateCommand("actionFinished"));
            data = recvall();
        } while (data.equals("No"));

        if (data.equals("Yes")) {
            return;
        }
        handleErrors(data);
    }
}
