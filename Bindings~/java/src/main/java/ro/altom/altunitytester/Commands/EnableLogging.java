package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;

@Deprecated
public class EnableLogging extends AltBaseCommand {
    public EnableLogging(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }

    public void Execute() {
        SendCommand("enableLogging", altBaseSettings.logEnabled.toString());
        String data = recvall();
        validateResponse("OK", data);
    }
}
