package com.alttester.Position;

import lombok.Data;
import lombok.Getter;

import com.google.gson.Gson;

@Getter
public @Data class Vector2 {
    public float x;
    public float y;

    public Vector2(float x, float y) {
        this.x = x;
        this.y = y;
    }

    public String toJson() {
        return new Gson().toJson(this);
    }
}
