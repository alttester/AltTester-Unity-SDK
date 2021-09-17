package ro.altom.altunitytester.Commands.InputActions;

import ro.altom.altunitytester.AltMessage;
import ro.altom.altunitytester.position.Vector2;

public class AltMoveMouseParameters extends AltMessage{
    public static class Builder{
        private Vector2 location;
        private float duration =1;
        public Builder(int x,int y){
            this.location = new Vector2(x, y);
        }
        public Builder withDuration(float duration){
            this.duration = duration;
            return this;
        }
        public AltMoveMouseParameters build(){
            AltMoveMouseParameters altMoveMouseParameter=new AltMoveMouseParameters();
            altMoveMouseParameter.location = this.location;
            altMoveMouseParameter.duration =this.duration;
            return altMoveMouseParameter;
        }
    }

    private AltMoveMouseParameters() {
        this.setCommandName("moveMouse");
    }

    private Vector2 location;
    private float duration;

    public int getX() {
        return (int)location.x;
    }

    public void setX(int x) {
        this.location.x = x;
    }

    public int getY() {
        return (int)location.y;
    }

    public void setY(int y) {
        this.location.y = y;
    }

    public float getDuration() {
        return duration;
    }

    public void setDuration(float duration) {
        this.duration = duration;
    }
}
