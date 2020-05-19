package ro.altom.altunitytester.Commands;

import ro.altom.altunitytester.AltBaseSettings;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.nio.file.Files;
import java.util.Base64;

/**
 * Create a screenshot of the current scene in png format.
 */
public class GetPNGScreenshotCommand extends AltBaseCommand{
    /**
     * @param path location where the image is created
     */
    private String path;
    public GetPNGScreenshotCommand(AltBaseSettings altBaseSettings,String path) {
        super(altBaseSettings);
        this.path=path;
    }
    public String Execute(){
        send(CreateCommand("getPNGScreenshot"));
        String data = recvall();
        if (data.equals("Ok")) {
            String screenshotData=recvall();
            byte[] screenshotDataBytes= Base64.getDecoder().decode(screenshotData);
            try (FileOutputStream stream = new FileOutputStream(path)) {
                stream.write(screenshotDataBytes);
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        handleErrors(data);
        return "";
    }
}
