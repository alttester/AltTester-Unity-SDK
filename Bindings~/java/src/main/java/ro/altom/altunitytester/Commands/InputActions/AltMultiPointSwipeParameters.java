package ro.altom.altunitytester.Commands.InputActions;

import java.util.ArrayList;
import java.util.List;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltMultiPointSwipeParameters extends AltMessage{
    
    /**
     * @param positions collection of positions on the screen where the swipe be
     *                  made
     */
    private List<Vector2> positions;
    /**
     * @param durationInSeconds how many seconds the swipe will need to complete
     */
    private float durationInSeconds;

    AltMultiPointSwipeParameters(List<Vector2> positions, float durationInSeconds){
        this.setCommandName("multipointSwipeChain");
        this.setPositions(new ArrayList<>(positions));
        this.setDurationInSeconds(durationInSeconds);
    }

    public float getDurationInSeconds() {
        return durationInSeconds;
    }

    public void setDurationInSeconds(float durationInSeconds) {
        this.durationInSeconds = durationInSeconds;
    }

    public List<Vector2> getPositions() {
        return positions;
    }

    public void setPositions(List<Vector2> positions) {
        this.positions = positions;
    }
}
