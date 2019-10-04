package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.Commands.InputActions.AltScrollMouse;
import ro.altom.altunitytester.Commands.InputActions.AltScrollMouseParameters;

public class AltScrollMouseAndWait extends AltBaseCommand {
    private AltScrollMouseParameters altScrollMouseParameters;
    public AltScrollMouseAndWait(AltBaseSettings altBaseSettings, AltScrollMouseParameters altScrollMouseParameters) {
        super(altBaseSettings);
        this.altScrollMouseParameters = altScrollMouseParameters;
    }
    public void Execute(){
        new AltScrollMouse(altBaseSettings,altScrollMouseParameters).Execute();
        sleepFor(altScrollMouseParameters.getDuration());
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
