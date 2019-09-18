package ro.altom.altunitytester.Commands.ObjectCommand;

public class AltGetComponentPropertyParameters {
    public static class Builder{
        private String componentName;
        private String methodName;
        private String assembly;
        public Builder(String componentName,String methodName){
            this.componentName=componentName;
            this.methodName=methodName;
        }
        public Builder withAssembly(String assembly){
            this.assembly=assembly;
            return this;
        }
        public AltGetComponentPropertyParameters build(){
            AltGetComponentPropertyParameters altGetComponentPropertyParameters=new AltGetComponentPropertyParameters();
            altGetComponentPropertyParameters.assembly=this.assembly;
            altGetComponentPropertyParameters.methodName=this.methodName;
            altGetComponentPropertyParameters.componentName=this.componentName;
            return altGetComponentPropertyParameters;
        }
    }

    private AltGetComponentPropertyParameters() {
    }

    public String getComponentName() {
        return componentName;
    }

    public void setComponentName(String componentName) {
        this.componentName = componentName;
    }

    public String getMethodName() {
        return methodName;
    }

    public void setMethodName(String methodName) {
        this.methodName = methodName;
    }

    public String getAssembly() {
        return assembly;
    }

    public void setAssembly(String assembly) {
        this.assembly = assembly;
    }

    private String componentName;
    private String methodName;
    private String assembly;
}
