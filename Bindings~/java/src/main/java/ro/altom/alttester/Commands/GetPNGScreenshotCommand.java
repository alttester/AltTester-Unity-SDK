package ro.altom.alttester.Commands;

import ro.altom.alttester.AltMessage;
import ro.altom.alttester.IMessageHandler;

import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Base64;

/**
 * Create a screenshot of the current scene in png format.
 */
public class GetPNGScreenshotCommand extends AltBaseCommand {
    /**
     * @param path location where the image is created
     */
    private String path;

    public GetPNGScreenshotCommand(IMessageHandler messageHandler, String path) {
        super(messageHandler);
        this.path = path;
    }

    public void Execute() {
        AltMessage altMessage = new AltMessage();
        altMessage.setCommandName("getPNGScreenshot");
        SendCommand(altMessage);
        String data = recvall(altMessage, String.class);
        validateResponse("Ok", data);

        String screenshotData = recvall(altMessage, String.class);
        byte[] screenshotDataBytes = Base64.getDecoder().decode(screenshotData);
        try (FileOutputStream stream = new FileOutputStream(path)) {
            stream.write(screenshotDataBytes);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
