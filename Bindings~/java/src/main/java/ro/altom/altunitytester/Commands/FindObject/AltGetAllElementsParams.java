package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.AltUnityDriver.By;

public class AltGetAllElementsParams extends AltMessage {

    public static class Builder {
        private By cameraBy = By.NAME;
        private String cameraValue = "";
        private boolean enabled = true;

        public Builder() {
        }

        public AltGetAllElementsParams.Builder isEnabled(boolean enabled) {
            this.enabled = enabled;
            return this;
        }

        public AltGetAllElementsParams.Builder withCamera(By cameraBy, String cameraValue) {
            this.cameraValue = cameraValue;
            this.cameraBy = cameraBy;
            return this;
        }

        public AltGetAllElementsParams build() {
            AltGetAllElementsParams altGetAllElementsParameters = new AltGetAllElementsParams();
            altGetAllElementsParameters.cameraBy = this.cameraBy;
            altGetAllElementsParameters.cameraValue = this.cameraValue;
            altGetAllElementsParameters.enabled = this.enabled;
            return altGetAllElementsParameters;
        }
    }

    private AltGetAllElementsParams() {
    }

    public String getCameraValue() {
        return cameraValue;
    }

    public void setCameraValue(String cameraValue) {
        this.cameraValue = cameraValue;
    }

    private By cameraBy;
    private boolean enabled;
    private String cameraValue;

    public By getCameraBy() {
        return cameraBy;
    }

    public void setCameraBy(By cameraBy) {
        this.cameraBy = cameraBy;
    }

    public boolean isEnabled() {
        return enabled;
    }

    public void setEnabled(boolean enabled) {
        this.enabled = enabled;
    }
}
