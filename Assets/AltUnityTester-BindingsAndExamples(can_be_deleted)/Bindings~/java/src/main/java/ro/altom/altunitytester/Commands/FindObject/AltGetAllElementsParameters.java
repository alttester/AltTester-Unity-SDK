package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltUnityDriver;

public class AltGetAllElementsParameters {

    public static class Builder{
        private String cameraName="";
        private boolean enabled=true;
        public Builder(){
        }
        public AltGetAllElementsParameters.Builder isEnabled(boolean enabled){
            this.enabled= enabled;
            return this;
        }
        public AltGetAllElementsParameters.Builder withCamera(String cameraName){
            this.cameraName= cameraName;
            return this;
        }
        public AltGetAllElementsParameters build(){
            AltGetAllElementsParameters altGetAllElementsParameters =new AltGetAllElementsParameters();
            altGetAllElementsParameters.cameraName=this.cameraName;
            altGetAllElementsParameters.enabled=this.enabled;
            return altGetAllElementsParameters;
        }
    }

    private AltGetAllElementsParameters() {
    }

    private String cameraName;
    private boolean enabled;

    public String getCameraName() {
        return cameraName;
    }

    public void setCameraName(String cameraName) {
        this.cameraName = cameraName;
    }

    public boolean isEnabled() {
        return enabled;
    }

    public void setEnabled(boolean enabled) {
        this.enabled = enabled;
    }
}
