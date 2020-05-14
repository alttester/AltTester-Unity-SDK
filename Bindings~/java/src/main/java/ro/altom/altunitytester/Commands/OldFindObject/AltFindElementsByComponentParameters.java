package ro.altom.altunitytester.Commands.OldFindObject;

public class AltFindElementsByComponentParameters {
    public static class Builder{

        private String componentName;
        private String assemblyName;
        private String cameraName="";
        private boolean enabled=true;
        public Builder(String componentName){
            this.componentName=componentName;
        }
        public AltFindElementsByComponentParameters.Builder isEnabled(boolean enabled){
            this.enabled= enabled;
            return this;
        }
        public AltFindElementsByComponentParameters.Builder withCamera(String cameraName){
            this.cameraName= cameraName;
            return this;
        }
        public AltFindElementsByComponentParameters.Builder inAssembly(String assemblyName){
            this.assemblyName=assemblyName;
            return this;
        }
        public AltFindElementsByComponentParameters build(){
            AltFindElementsByComponentParameters altFindElementsByComponentParameters =new AltFindElementsByComponentParameters();
            altFindElementsByComponentParameters.componentName =this.componentName;
            altFindElementsByComponentParameters.assemblyName=this.assemblyName;
            altFindElementsByComponentParameters.cameraName=this.cameraName;
            altFindElementsByComponentParameters.enabled=this.enabled;
            return altFindElementsByComponentParameters;
        }
    }

    private AltFindElementsByComponentParameters() {
    }

    private String componentName;
    private String assemblyName;

    public String getComponentName() {
        return componentName;
    }

    public void setComponentName(String componentName) {
        this.componentName = componentName;
    }

    public String getAssemblyName() {
        return assemblyName;
    }

    public void setAssemblyName(String assemblyName) {
        this.assemblyName = assemblyName;
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
