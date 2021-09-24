package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;

public class AltEndTouchParameters extends AltMessage{
    
    private int fingerId;

    public AltEndTouchParameters(int fingerId){
        this.setCommandName("endTouch");
        this.setFingerId(fingerId);
    }

    public int getFingerId() {
        return fingerId;
    }

    public void setFingerId(int fingerId) {
        this.fingerId = fingerId;
    }
}
