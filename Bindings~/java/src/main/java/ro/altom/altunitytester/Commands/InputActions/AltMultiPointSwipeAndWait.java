package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

import java.util.List;

/**
 * Similar command like swipe but instead of swipe from point A to point B you are able to give list a points. 
 */
public class AltMultiPointSwipeAndWait extends AltBaseCommand {

    /**
     * @param positions collection of positions on the screen where the swipe be made
     */
    private List<Vector2> positions;
    /**
     * @param durationInSeconds how many seconds the swipe will need to complete
     */
    private float durationInSeconds;
    public AltMultiPointSwipeAndWait(AltBaseSettings altBaseSettings, List<Vector2> positions, float durationInSeconds) {
        super(altBaseSettings);
        this.positions = positions;
        this.durationInSeconds = durationInSeconds;
    }
    public void Execute(){
        new AltMultiPointSwipe(altBaseSettings, positions, durationInSeconds).Execute();
        sleepFor(durationInSeconds );
        String data;
        do {
            send(CreateCommand("actionFinished"));
            data = recvall();
        } while (data.equals("No"));

        if (data.equals("Yes")) {
            return;
        }
        handleErrors(data);
    }
}