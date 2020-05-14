package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltCommandReturningAltObjects;

public class AltSetText extends AltCommandReturningAltObjects {
    private AltUnityObject altUnityObject;
    private String newText;
    
    public AltSetText(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject, String text) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
        this.newText = text;
    }
    public AltUnityObject Execute() {
        String altObject = new Gson().toJson(altUnityObject);
        send(CreateCommand("setText", altObject, newText));
        return ReceiveAltUnityObject();
    }
}