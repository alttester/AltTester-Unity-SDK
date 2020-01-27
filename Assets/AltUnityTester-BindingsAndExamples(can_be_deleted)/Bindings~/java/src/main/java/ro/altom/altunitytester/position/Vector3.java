package ro.altom.altunitytester.position;

import lombok.Data;
import lombok.Getter;

@Getter
public @Data class Vector3 {
    public int x;
    public int y;
    public int z;

    public Vector3(int x, int y, int z)
    {
        this(x, y);
        this.z = z;
    }

    public Vector3(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public String toVector2Json() {
        return "{\"x\":" + x + ", \"y\":" + y + "}";
    }

    public String toVector3Json() {
        return "{\"x\":" + x + ", \"y\":" + y + ", \"z\":" + z + "}";
    }
}
