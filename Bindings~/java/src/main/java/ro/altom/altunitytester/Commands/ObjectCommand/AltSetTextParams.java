package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.AltUnityObject;

public class AltSetTextParams extends AltUnityObjectParams {

    private String newText;

    public AltSetTextParams(String newText, AltUnityObject altUnityObject) {
        this.setNewText(newText);
        super.altUnityObject = altUnityObject;
    }

    public String getNewText() {
        return newText;
    }

    public void setNewText(String newText) {
        this.newText = newText;
    }
}
