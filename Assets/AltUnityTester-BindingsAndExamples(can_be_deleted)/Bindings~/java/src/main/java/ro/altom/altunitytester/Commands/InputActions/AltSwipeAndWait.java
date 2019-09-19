package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSwipeAndWait extends AltBaseCommand {
    private int xStart;
    private int yStart;
    private int xEnd;
    private int yEnd;
    private float durationInSeconds;

    public AltSwipeAndWait(AltBaseSettings altBaseSettings, int xStart, int yStart, int xEnd, int yEnd, float durationInSeconds) {
        super(altBaseSettings);
        this.xStart = xStart;
        this.yStart = yStart;
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        this.durationInSeconds = durationInSeconds;
    }
    public void Execute(){
        new AltSwipe(altBaseSettings,xStart, yStart, xEnd, yEnd, durationInSeconds).Execute();
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
