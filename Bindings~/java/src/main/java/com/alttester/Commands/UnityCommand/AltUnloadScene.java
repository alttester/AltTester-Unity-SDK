package com.alttester.Commands.UnityCommand;

import com.alttester.IMessageHandler;
import com.alttester.Commands.AltBaseCommand;

public class AltUnloadScene extends AltBaseCommand {

    private AltUnloadSceneParams params;

    public AltUnloadScene(IMessageHandler messageHandler, AltUnloadSceneParams params) {
        super(messageHandler);
        this.params = params;
        params.setCommandName("unloadScene");
    }

    public void Execute() {
        SendCommand(params);
        String data = recvall(params, String.class);
        validateResponse("Ok", data);

        data = recvall(params, String.class);
        validateResponse("Scene Unloaded", data);
    }
}
