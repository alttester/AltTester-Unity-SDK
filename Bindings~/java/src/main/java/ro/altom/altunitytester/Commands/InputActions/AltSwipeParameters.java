package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltSwipeParameters extends AltMessage{

    private Vector2 start;
    private Vector2 end;

    /**
     * @param durationInSeconds The time measured in seconds to move the mouse from
     *                          current position to the set location.
     */
    private float durationInSeconds;

    AltSwipeParameters(int xStart, int yStart, int xEnd, int yEnd, float durationInSeconds){
        this.setCommandName("multipointSwipe");
        this.start = new Vector2(xStart, yStart);
        this.end = new Vector2(xEnd, yEnd);
        this.setDurationInSeconds(durationInSeconds);
    }

    public float getDurationInSeconds() {
        return durationInSeconds;
    }

    public void setDurationInSeconds(float durationInSeconds) {
        this.durationInSeconds = durationInSeconds;
    }

    public int getxStart(){
        return (int)start.x;
    }

    public int getyStart(){
        return (int)start.y;
    }

    public int getxEnd(){
        return (int)end.x;
    }

    public int getyEnd(){
        return (int)end.y;
    }
}
