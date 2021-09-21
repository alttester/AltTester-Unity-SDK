package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSetComponentProperty extends AltBaseCommand {
    private AltSetComponentPropertyParameters altSetComponentPropertyParameters;

    public AltSetComponentProperty(IMessageHandler messageHandler,
            AltSetComponentPropertyParameters altSetComponentPropertyParameters) {
        super(messageHandler);
        this.altSetComponentPropertyParameters = altSetComponentPropertyParameters;
        this.altSetComponentPropertyParameters.setCommandName("setObjectComponentProperty");
    }

    public String Execute() {
        SendCommand(altSetComponentPropertyParameters);
        return recvall(altSetComponentPropertyParameters, String.class);
    }
}
