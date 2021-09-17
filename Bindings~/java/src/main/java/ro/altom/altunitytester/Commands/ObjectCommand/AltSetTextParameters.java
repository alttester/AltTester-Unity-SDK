package ro.altom.altunitytester.Commands.ObjectCommand;

import ro.altom.altunitytester.AltUnityObject;

public class AltSetTextParameters extends AltUnityObjectParameters{
    
    private String newText;

    public AltSetTextParameters(String newText, AltUnityObject altUnityObject){
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
