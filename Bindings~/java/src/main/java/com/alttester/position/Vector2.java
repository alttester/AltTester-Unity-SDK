/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.position;

import com.google.gson.Gson;
import lombok.Data;
import lombok.Getter;

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
