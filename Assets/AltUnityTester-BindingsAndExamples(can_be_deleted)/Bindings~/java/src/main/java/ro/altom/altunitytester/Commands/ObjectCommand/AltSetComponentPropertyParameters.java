package ro.altom.altunitytester.Commands.ObjectCommand;

public class AltSetComponentPropertyParameters {
    public static class Builder{
        private String componentName;
        private String propertyName;
        private String assembly;
        private String value;
        public Builder(String componentName,String propertyName,String value){
            this.componentName=componentName;
            this.propertyName=propertyName;
            this.value=value;
        }
        public Builder withAssembly(String assembly){
            this.assembly=assembly;
            return this;
        }
        public AltSetComponentPropertyParameters build(){
            AltSetComponentPropertyParameters altSetComponentPropertyParameters=new AltSetComponentPropertyParameters();
            altSetComponentPropertyParameters.assembly=this.assembly;
            altSetComponentPropertyParameters.propertyName=this.propertyName;
            altSetComponentPropertyParameters.componentName=this.componentName;
            altSetComponentPropertyParameters.value=this.value;

            return altSetComponentPropertyParameters;
        }
    }

    private AltSetComponentPropertyParameters() {
    }

    public String getComponentName() {
        return componentName;
    }

    public void setComponentName(String componentName) {
        this.componentName = componentName;
    }

    public String getPropertyName() {
        return propertyName;
    }

    public void setPropertyName(String propertyName) {
        this.propertyName = propertyName;
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    private String componentName;
    private String propertyName;
    private String assembly;

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    private String value;
}
