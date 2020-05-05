package ro.altom.altunitytester.position;

import lombok.Data;
import lombok.Getter;

@Getter
public @Data class Vector2 {
    public float x;
    public float y;

    public Vector2(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public String toVector2Json() {
        return "{\"x\":" + x + ", \"y\":" + y + "}";
    }
}
