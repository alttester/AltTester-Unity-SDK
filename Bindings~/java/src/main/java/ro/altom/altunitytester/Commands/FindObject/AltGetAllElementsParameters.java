package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.AltUnityDriver.By;

public class AltGetAllElementsParameters {

    public static class Builder {
        private By cameraBy = By.NAME;
        private String cameraPath = "";
        private boolean enabled = true;

        public Builder() {
        }

        public AltGetAllElementsParameters.Builder isEnabled(boolean enabled) {
            this.enabled = enabled;
            return this;
        }

        public AltGetAllElementsParameters.Builder withCamera(By cameraBy, String cameraPath) {
            this.cameraPath = cameraPath;
            this.cameraBy = cameraBy;
            return this;
        }

        public AltGetAllElementsParameters build() {
            AltGetAllElementsParameters altGetAllElementsParameters = new AltGetAllElementsParameters();
            altGetAllElementsParameters.cameraBy = this.cameraBy;
            altGetAllElementsParameters.cameraPath = this.cameraPath;
            altGetAllElementsParameters.enabled = this.enabled;
            return altGetAllElementsParameters;
        }
    }

    private AltGetAllElementsParameters() {
    }

    private By cameraBy;
    private String cameraPath;
    private boolean enabled;

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
