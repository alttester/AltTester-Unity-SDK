package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

/**
 * Simulate scroll mouse action in your game. This command does not wait for the
 * action to finish.
 */
public class AltSwipe extends AltBaseCommand {
    /**
     * @param xStart x coordinate of the screen where the swipe begins.
     */
    private int xStart;
    /**
     * @param yStart y coordinate of the screen where the swipe begins.
     */
    private int yStart;
    /**
     * @param xEnd x coordinate of the screen where the swipe ends.
     */
    private int xEnd;
    /**
     * @param yEnd y coordinate of the screen where the swipe ends.
     */
    private int yEnd;
    /**
     * @param durationInSeconds The time measured in seconds to move the mouse from
     *                          current position to the set location.
     */
    private float durationInSeconds;

    public AltSwipe(AltBaseSettings altBaseSettings, int xStart, int yStart, int xEnd, int yEnd,
            float durationInSeconds) {
        super(altBaseSettings);
        this.xStart = xStart;
        this.yStart = yStart;
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        this.durationInSeconds = durationInSeconds;
    }

    public void Execute() {
        String vectorStartJson = vectorToJsonString(xStart, yStart);
        String vectorEndJson = vectorToJsonString(xEnd, yEnd);
        send(CreateCommand("multipointSwipe", vectorStartJson, vectorEndJson, String.valueOf(durationInSeconds)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }
}
