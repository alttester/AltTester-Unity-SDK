package ro.altom.altunitytester.Commands.OldFindObject;

public class AltFindElementsParameters {
        public static class Builder{

            private String name;
            private String cameraName="";
            private boolean enabled=true;
            public Builder(String name){
                this.name=name;
            }
            public AltFindElementsParameters.Builder isEnabled(boolean enabled){
                this.enabled= enabled;
                return this;
            }
            public AltFindElementsParameters.Builder withCamera(String cameraName){
                this.cameraName= cameraName;
                return this;
            }
            public AltFindElementsParameters build(){
                AltFindElementsParameters altFindObjectsParameters =new AltFindElementsParameters();
                altFindObjectsParameters.name =this.name;
                altFindObjectsParameters.cameraName=this.cameraName;
                altFindObjectsParameters.enabled=this.enabled;
                return altFindObjectsParameters;
            }
        }

        private AltFindElementsParameters() {
        }

        private String name;
        private String cameraName;
        private boolean enabled;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
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

