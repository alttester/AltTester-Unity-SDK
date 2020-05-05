package ro.altom.altunitytester.Commands.UnityCommand;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSetTimeScale extends AltBaseCommand {
    private float timeScale;
    public AltSetTimeScale(AltBaseSettings altBaseSettings, float timeScale) {
        super(altBaseSettings);
        this.timeScale = timeScale;
    }
    public void Execute(){
        send(CreateCommand("setTimeScale", String.valueOf(timeScale)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }
}
