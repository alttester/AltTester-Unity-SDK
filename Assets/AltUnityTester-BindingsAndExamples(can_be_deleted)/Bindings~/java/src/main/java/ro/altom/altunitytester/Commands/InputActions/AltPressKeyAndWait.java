package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltPressKeyAndWait extends AltBaseCommand {
    private AltPressKeyParameters altPressKeyParameters;
    public AltPressKeyAndWait(AltBaseSettings altBaseSettings, AltPressKeyParameters altPressKeyParameters) {
        super(altBaseSettings);
        this.altPressKeyParameters = altPressKeyParameters;
    }
    public void Execute(){
        new AltPressKey(altBaseSettings,altPressKeyParameters).Execute();
        sleepFor(altPressKeyParameters.getDuration());
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
