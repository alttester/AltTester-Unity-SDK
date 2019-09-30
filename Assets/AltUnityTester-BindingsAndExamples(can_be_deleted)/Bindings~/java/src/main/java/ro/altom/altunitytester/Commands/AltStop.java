package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.altUnityTesterExceptions.ConnectionException;

import java.io.IOException;

public class AltStop extends AltBaseCommand {
    public AltStop(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    public void Execute(){
        log.info("Closing connection with server.");
        send(CreateCommand("closeConnection"));
        try {
            altBaseSettings.in.close();
            altBaseSettings.out.close();
            altBaseSettings.socket.close();
        } catch (IOException e) {
            throw new ConnectionException("Could not close the socket.", e);
        }
    }
}
