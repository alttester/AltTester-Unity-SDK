package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityDriver.By;

public class AltFindObjectsParameters {

    public static class Builder {
        private By by;
        private String value;
        private By cameraBy = By.NAME;
        private String cameraPath = "";
        private boolean enabled = true;

        public Builder(AltUnityDriver.By by, String value) {
            this.by = by;
            this.value = value;
        }

        public AltFindObjectsParameters.Builder isEnabled(boolean enabled) {
            this.enabled = enabled;
            return this;
        }

        public AltFindObjectsParameters.Builder withCamera(By cameraBy, String cameraPath) {
            this.cameraBy = cameraBy;
            this.cameraPath = cameraPath;
            return this;
        }

        public AltFindObjectsParameters build() {
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters();
            altFindObjectsParameters.by = this.by;
            altFindObjectsParameters.value = this.value;
            altFindObjectsParameters.cameraBy = this.cameraBy;
            altFindObjectsParameters.cameraPath = this.cameraPath;
            altFindObjectsParameters.enabled = this.enabled;
            return altFindObjectsParameters;
        }
    }

    private AltFindObjectsParameters() {
    }

    private AltUnityDriver.By by;
    private String value;
    private By cameraBy;
    private String cameraPath;
    private boolean enabled;

    public AltUnityDriver.By getBy() {
        return by;
    }

    public void setBy(AltUnityDriver.By by) {
        this.by = by;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
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
