package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

import java.util.List;

public class AltMovingTouchAndWait extends AltBaseCommand {
    private List<Vector2> positions;
    private float durationInSeconds;
    public AltMovingTouchAndWait(AltBaseSettings altBaseSettings, List<Vector2> positions, float durationInSeconds) {
        super(altBaseSettings);
        this.positions = positions;
        this.durationInSeconds = durationInSeconds;
    }
    public void Execute(){
        new AltMovingTouch(altBaseSettings, positions, durationInSeconds).Execute();
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