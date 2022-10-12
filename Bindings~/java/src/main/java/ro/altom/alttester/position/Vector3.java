package ro.altom.alttester.position;

import lombok.Data;
import lombok.Getter;

import com.google.gson.Gson;

@Getter
public @Data class Vector3 extends Vector2 {
    public float z;

    public Vector3(float x, float y, float z)
    {
        super(x, y);
        this.z = z;
    }

    public String toJson() {
        return new Gson().toJson(this);
    }
}
