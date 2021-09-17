package ro.altom.altunitytester.Commands.ObjectCommand;

public class AltSetComponentPropertyParameters extends AltUnityObjectParameters{
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
            altSetComponentPropertyParameters.property=this.propertyName;
            altSetComponentPropertyParameters.component=this.componentName;
            altSetComponentPropertyParameters.value=this.value;

            return altSetComponentPropertyParameters;
        }
    }

    private AltSetComponentPropertyParameters() {
    }

    public String getComponentName() {
        return component;
    }

    public void setComponentName(String componentName) {
        this.component = componentName;
    }

    public String getPropertyName() {
        return property;
    }

    public void setPropertyName(String propertyName) {
        this.property = propertyName;
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    private String component;
    private String property;
    private String assembly;

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    private String value;
}
