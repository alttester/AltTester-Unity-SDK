package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltTiltAndWait extends AltBaseCommand {
    private AltTiltParameters altTiltParameters;

    public AltTiltAndWait(AltBaseSettings altBaseSettings, AltTiltParameters altTiltParameters) {
        super(altBaseSettings);
        this.altTiltParameters = altTiltParameters;
    }

    public void Execute() {
        new AltTilt(altBaseSettings, altTiltParameters).Execute();
        sleepFor(altTiltParameters.getDuration());
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
