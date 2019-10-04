package ro.altom.altunitytester.Commands;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;

import java.io.DataInputStream;
import java.io.PrintWriter;
import java.net.Socket;

public class AltCommandReturningAltObjects extends AltBaseCommand {

    public AltCommandReturningAltObjects(AltBaseSettings altBaseSettings) {
        super(altBaseSettings);
    }

    protected AltUnityObject ReceiveAltUnityObject()
    {
        String data = recvall();
        if (!data.contains("error:")) {
            AltUnityObject altUnityObject=new Gson().fromJson(data, AltUnityObject.class);
            altUnityObject.setAltBaseSettings(altBaseSettings);
            return altUnityObject;
        }
        handleErrors(data);
        return null;
    }
    protected AltUnityObject[] ReceiveListOfAltUnityObjects()
    {
        String data = recvall();
        if (!data.contains("error:")) {
            AltUnityObject[] altUnityObjects=new Gson().fromJson(data, AltUnityObject[].class);
            for (AltUnityObject altUnityObject:altUnityObjects
                 ) {
                altUnityObject.setAltBaseSettings(altBaseSettings);
            }
            return altUnityObjects;
        }
        handleErrors(data);
        return new AltUnityObject[]{};
    }

}
