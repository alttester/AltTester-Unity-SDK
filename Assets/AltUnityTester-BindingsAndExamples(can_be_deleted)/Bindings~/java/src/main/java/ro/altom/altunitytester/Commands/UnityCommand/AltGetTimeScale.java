package ro.altom.altunitytester.Commands.UnityCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltGetTimeScale extends AltBaseCommand {

    public AltGetTimeScale(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    public float Execute(){
        send(CreateCommand("getTimeScale"));
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, float.class));
        }
        handleErrors(data);
        return 0;
    }
}
