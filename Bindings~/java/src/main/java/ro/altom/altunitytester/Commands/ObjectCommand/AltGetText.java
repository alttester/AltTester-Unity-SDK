package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.IMessageHandler;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Get text value from a Button, Text, InputField. This also works with
 * TextMeshPro elements.
 */
public class AltGetText extends AltBaseCommand {

    private AltGetTextParams params;

    public AltGetText(IMessageHandler messageHandler, AltGetTextParams params) {
        super(messageHandler);
        this.params = params;
        params.setCommandName("getText");
        ;
    }

    public String Execute() {
        SendCommand(params);
        return recvall(params, String.class);
    }
}
