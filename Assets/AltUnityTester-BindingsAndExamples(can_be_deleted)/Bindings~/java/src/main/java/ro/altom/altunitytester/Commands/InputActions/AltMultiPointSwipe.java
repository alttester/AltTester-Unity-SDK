package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltBaseSettings;
import ro.altom.altunitytester.Commands.AltBaseCommand;
import ro.altom.altunitytester.position.Vector2;

import java.util.ArrayList;
import java.util.List;

public class AltMultiPointSwipe extends AltBaseCommand {
    private List<Vector2> positions;
    private float durationInSeconds;
    public AltMultiPointSwipe(AltBaseSettings altBaseSettings, List<Vector2> positions, float durationInSeconds) {
        super(altBaseSettings);
        this.positions = positions;
        this.durationInSeconds = durationInSeconds;
    }
    public void Execute(){
        ArrayList<String> args = new ArrayList<String>();
        args.add("MultipointSwipeChain");
        args.add(String.valueOf(durationInSeconds));
        for (Vector2 v : positions) {
            args.add(v.toVector2Json());
        }
        String[] results = new String[args.size()];
        results = args.toArray(results);
        send(CreateCommand(results));
        String data = recvall();
        if (data.equals("Ok")) {
            return;
        }
        handleErrors(data);
    }
}