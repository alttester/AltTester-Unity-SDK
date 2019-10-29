package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;

public class EnableDebugging extends AltBaseCommand {
    public EnableDebugging(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    public String Execute(){
        send(CreateCommand("enableDebug", altBaseSettings.debugEnabled.toString()));
        String data = recvall();
        if (data.equals("OK")) {
            return data;
        }
        handleErrors(data);
        return "";
    }
}

