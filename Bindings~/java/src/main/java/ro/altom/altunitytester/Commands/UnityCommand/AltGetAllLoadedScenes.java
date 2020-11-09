package ro.altom.altunitytester.Commands.UnityCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltGetAllLoadedScenes extends AltBaseCommand {
    public AltGetAllLoadedScenes(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }

    public String[] Execute() {
        log.debug("Get all loaded scenese...");
        send(CreateCommand("getAllLoadedScenes"));
        String data = recvall();
        if (!data.contains("error:")) {
            return (new Gson().fromJson(data, String[].class));
        }
        handleErrors(data);
        return null;
    }
}
