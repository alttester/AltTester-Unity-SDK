package ro.altom.altunitytester.Commands.ObjectCommand;

public class AltGetComponentPropertyParameters {
    public static class Builder{
        private String componentName;
        private String propertyName;
        private String assembly="";
        private int maxDepth=2;
        public Builder(String componentName,String propertyName){
            this.componentName=componentName;
            this.propertyName=propertyName;
        }
        public Builder withAssembly(String assembly){
            this.assembly=assembly;
            return this;
        }
        public Builder withMaxDepth(int maxDepth){
            this.maxDepth=maxDepth;
            return this;
        }
        public AltGetComponentPropertyParameters build(){
            AltGetComponentPropertyParameters altGetComponentPropertyParameters=new AltGetComponentPropertyParameters();
            altGetComponentPropertyParameters.assembly=this.assembly;
            altGetComponentPropertyParameters.propertyName=this.propertyName;
            altGetComponentPropertyParameters.componentName=this.componentName;
            altGetComponentPropertyParameters.maxDepth=this.maxDepth;
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
    public int getMaxDepth() {
        return maxDepth;
    }

    public void setMaxDepth(int maxDepth) {
        this.maxDepth = maxDepth;
    }


    private String componentName;
    private String propertyName;
    private String assembly;
    private int maxDepth;
}
