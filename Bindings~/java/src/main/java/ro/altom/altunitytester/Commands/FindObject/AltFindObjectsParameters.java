package ro.altom.altunitytester.Commands.FindObject;

import ro.altom.altunitytester.AltUnityDriver;
import ro.altom.altunitytester.Commands.InputActions.AltScrollMouseParameters;

public class AltFindObjectsParameters {

    public static class Builder{
        private AltUnityDriver.By by;
        private String value;
        private String cameraName="";
        private boolean enabled=true;
        public Builder(AltUnityDriver.By by,String value){
            this.by=by;
            this.value=value;
        }
        public AltFindObjectsParameters.Builder isEnabled(boolean enabled){
            this.enabled= enabled;
            return this;
        }
        public AltFindObjectsParameters.Builder withCamera(String cameraName){
            this.cameraName= cameraName;
            return this;
        }
        public AltFindObjectsParameters build(){
            AltFindObjectsParameters altFindObjectsParameters =new AltFindObjectsParameters();
            altFindObjectsParameters.by=this.by;
            altFindObjectsParameters.value =this.value;
            altFindObjectsParameters.cameraName=this.cameraName;
            altFindObjectsParameters.enabled=this.enabled;
            return altFindObjectsParameters;
        }
    }

    private AltFindObjectsParameters() {
    }

    private AltUnityDriver.By by;
    private String value;
    private String cameraName;
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
