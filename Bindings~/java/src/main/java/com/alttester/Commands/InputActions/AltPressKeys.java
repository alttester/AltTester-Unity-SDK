package com.alttester.Commands.InputActions;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

/**
 * Similar command like swipe but instead of swipe from point A to point B you
 * are able to give list a points.
 */
public class AltPressKeys extends AltBaseCommand {
    /**
     * @param altPressKeysParameters the builder for the press key commands.
     */
    private AltPressKeysParams altPressKeysParameters;

    public AltPressKeys(IMessageHandler messageHandler, AltPressKeysParams altPressKeysParameters) {
        super(messageHandler);
        this.altPressKeysParameters = altPressKeysParameters;
        this.altPressKeysParameters.setCommandName("pressKeyboardKeys");
    }

    public void Execute() {
        SendCommand(altPressKeysParameters);
        String data = recvall(altPressKeysParameters, String.class);
        validateResponse("Ok", data);

        if (altPressKeysParameters.getWait()) {
            for (int i = 0; i < altPressKeysParameters.getKeyCodes().length; i++) {
                data = recvall(altPressKeysParameters, String.class);
                validateResponse("Finished", data);
            }
        }
    }
}
