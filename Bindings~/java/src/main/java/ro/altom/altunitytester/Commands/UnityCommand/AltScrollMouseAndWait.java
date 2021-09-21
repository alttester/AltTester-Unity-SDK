package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.Commands.InputActions.AltScrollMouse;
import ro.altom.altunitytester.Commands.InputActions.AltScrollMouseParameters;

public class AltScrollMouseAndWait extends AltBaseCommand {
    private AltScrollMouseParameters altScrollMouseParameters;

    public AltScrollMouseAndWait(IMessageHandler messageHandler, AltScrollMouseParameters altScrollMouseParameters) {
        super(messageHandler);
        this.altScrollMouseParameters = altScrollMouseParameters;
    }

    public void Execute() {
        new AltScrollMouse(messageHandler, altScrollMouseParameters).Execute();
        sleepFor(altScrollMouseParameters.getDuration());
        String data;
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("actionFinished");
        do {
            SendCommand(altMessage);
            data = recvall(altScrollMouseParameters, String.class);
        } while (data.equals("No"));
        validateResponse("Yes", data);
    }
}
