package ro.altom.altunitytester.Commands.ObjectCommand;

import com.google.gson.Gson;
import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.AltUnityObject;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Get text value from a Button, Text, InputField. This also works with TextMeshPro elements.
 */
public class AltGetText extends AltBaseCommand {
    /**
     * @param altUnityObject The game object
     */
    private AltUnityObject altUnityObject;
    public AltGetText(AltBaseSettings altBaseSettings, AltUnityObject altUnityObject) {
        super(altBaseSettings);
        this.altUnityObject = altUnityObject;
    }
    public String Execute() {
        String altObject = new Gson().toJson(altUnityObject);
        send(CreateCommand("getText", altObject));
        String data = recvall();
        if (!data.contains("error:")) {
            return data;
        }
        handleErrors(data);
        return "";
    }
}
