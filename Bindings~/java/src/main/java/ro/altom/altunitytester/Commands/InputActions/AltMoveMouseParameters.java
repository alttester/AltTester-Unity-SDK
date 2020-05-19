package ro.altom.altunitytester.Commands.InputActions;

public class AltMoveMouseParameters {
    public static class Builder{
        private int x;
        private int y;
        private float duration =1;
        public Builder(int x,int y){
            this.x=x;
            this.y=y;
        }
        public Builder withDuration(float duration){
            this.duration = duration;
            return this;
        }
        public AltMoveMouseParameters build(){
            AltMoveMouseParameters altMoveMouseParameter=new AltMoveMouseParameters();
            altMoveMouseParameter.x=this.x;
            altMoveMouseParameter.y=this.y;
            altMoveMouseParameter.duration =this.duration;
            return altMoveMouseParameter;
        }
    }

    private AltMoveMouseParameters() {
    }

    private int x;
    private int y;
    private float duration;

    public int getX() {
        return x;
    }

    public void setX(int x) {
        this.x = x;
    }

    public int getY() {
        return y;
    }

    public void setY(int y) {
        this.y = y;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }
}
