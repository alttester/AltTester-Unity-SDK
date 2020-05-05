package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityDriver;


public class GetServerVersionCommand extends AltBaseCommand {
    public GetServerVersionCommand(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    public void Execute() throws Exception {
        send(CreateCommand("getServerVersion", altBaseSettings.logEnabled.toString()));
        String serverVersion = recvall();
        String driverVersion= AltUnityDriver.VERSION;
        if(serverVersion.equals("error:unknownError")){
            String message="Version mismatch. You are using different versions of server and driver. Server version is earlier then 1.5.3 and Driver version: " + driverVersion; 
            super.WriteInLogFile(message);
            throw new Exception(message);
        }
        if (serverVersion.contains("error:")) {
            handleErrors(serverVersion);
        }
        if(!driverVersion.equals(serverVersion))
        {
            String message="Version mismatch. You are using different versions of server and driver. Server version: " + serverVersion + " and Driver version: " + driverVersion;
            super.WriteInLogFile(message);
            throw new Exception(message);
        }
    }
}

