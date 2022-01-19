package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityDriver.By;

public class AltFindObjectsParams extends AltMessage {

    public static class Builder {
        private By by;
        private String value;
        private By cameraBy = By.NAME;
        private String cameraValue = "";
        private boolean enabled = true;

        public Builder(AltUnityDriver.By by, String value) {
            this.by = by;
            this.value = value;
        }

        public AltFindObjectsParams.Builder isEnabled(boolean enabled) {
            this.enabled = enabled;
            return this;
        }

        public AltFindObjectsParams.Builder withCamera(By cameraBy, String cameraValue) {
            this.cameraBy = cameraBy;
            this.cameraValue = cameraValue;
            return this;
        }

        public AltFindObjectsParams build() {
            AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams();
            altFindObjectsParameters.by = this.by;
            altFindObjectsParameters.value = this.value;
            altFindObjectsParameters.cameraBy = this.cameraBy;
            altFindObjectsParameters.cameraValue = this.cameraValue;
            altFindObjectsParameters.enabled = this.enabled;
            return altFindObjectsParameters;
        }
    }

    private AltFindObjectsParams() {
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    public String getCameraValue() {
        return cameraValue;
    }

    public void setCameraValue(String cameraValue) {
        this.cameraValue = cameraValue;
    }

    private AltUnityDriver.By by;
    private String path;
    private By cameraBy;
    private String cameraPath;
    private boolean enabled;
    private String value;
    private String cameraValue;

    public AltUnityDriver.By getBy() {
        return by;
    }

    public void setBy(AltUnityDriver.By by) {
        this.by = by;
    }

    public String getPath() {
        return path;
    }

    public void setPath(String path) {
        this.path = path;
    }

    public By getCameraBy() {
        return cameraBy;
    }

    public void setCameraBy(By cameraBy) {
        this.cameraBy = cameraBy;
    }

    public String getCameraPath() {
        return cameraPath;
    }

    public void setCameraPath(String cameraPath) {
        this.cameraPath = cameraPath;
    }

    public boolean isEnabled() {
        return enabled;
    }

    public void setEnabled(boolean enabled) {
        this.enabled = enabled;
    }
}
