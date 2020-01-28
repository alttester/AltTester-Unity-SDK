package ro.altom.altunitytester.position;

import lombok.Data;
import lombok.Getter;

@Getter
public @Data class Vector3 extends Vector2 {
    public float z;

    public Vector3(float x, float y, float z)
    {
        super(x, y);
        this.z = z;
    }

    public String toVector3Json() {
        return "{\"x\":" + x + ", \"y\":" + y + ", \"z\":" + z + "}";
    }
}
