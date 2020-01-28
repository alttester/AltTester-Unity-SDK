package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.Scanner;

public class GetServerVersionCommand extends AltBaseCommand {
    public GetServerVersionCommand(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }
    public void Execute() throws Exception {
        send(CreateCommand("getServerVersion", altBaseSettings.logEnabled.toString()));
        String serverVersion = recvall();
        if (serverVersion.contains("error:")) {
            handleErrors(serverVersion);
        }
        String driverVersion="";
        try {
            File myObj = new File("JavaDriverVersion.txt");
            Scanner myReader = new Scanner(myObj);
            driverVersion=myReader.nextLine();
            myReader.close();
        } catch (FileNotFoundException e) {
            System.out.println("An error occurred.");
            e.printStackTrace();
        }
        if(!driverVersion.equals(serverVersion))
            throw new Exception("Mismatch version. You are using different version of server and driver. Server version: " + serverVersion + " and Driver version: " + driverVersion);
    }
}

