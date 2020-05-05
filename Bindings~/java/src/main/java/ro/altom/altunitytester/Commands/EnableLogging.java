package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;

public class EnableLogging extends AltBaseCommand {
    public EnableLogging(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    public String Execute(){
        send(CreateCommand("enableLogging", altBaseSettings.logEnabled.toString()));
        String data = recvall();
        if (data.equals("OK")) {
            return data;
        }
        handleErrors(data);
        return "";
    }
}

