package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.UnityStruct.AltUnityKeyCode;

public class AltKeyUpParameters extends AltMessage{
    
    private AltUnityKeyCode keyCode;

    public AltKeyUpParameters(AltUnityKeyCode keyCode){
        this.setCommandName("keyUp");
        this.setKeyCode(keyCode);
    }

    public AltUnityKeyCode getKeyCode() {
        return keyCode;
    }

    public void setKeyCode(AltUnityKeyCode keyCode) {
        this.keyCode = keyCode;
    }
}
