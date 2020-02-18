package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;

public class AltSwipe extends AltBaseCommand {
    private int xStart;
    private int yStart;
    private int xEnd;
    private int yEnd;
    private float durationInSeconds;
    public AltSwipe(AltBaseSettings altBaseSettings, int xStart, int yStart, int xEnd, int yEnd, float durationInSeconds) {
        super(altBaseSettings);
        this.xStart = xStart;
        this.yStart = yStart;
        this.xEnd = xEnd;
        this.yEnd = yEnd;
        this.durationInSeconds = durationInSeconds;
    }
    public void Execute(){
        String vectorStartJson = vectorToJsonString(xStart, yStart);
        String vectorEndJson = vectorToJsonString(xEnd, yEnd);
        send(CreateCommand("MultipointSwipe", vectorStartJson, vectorEndJson, String.valueOf(durationInSeconds)));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }
}
