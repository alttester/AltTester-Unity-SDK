/*
    Copyright(C) 2026 Altom Consulting
*/

package com.alttester.position;

import com.google.gson.Gson;
import lombok.Data;
import lombok.Getter;

@Getter
public @Data class Vector3 extends Vector2 {
  public float z;

  public Vector3(float x, float y, float z) {
    super(x, y);
    this.z = z;
  }

  public String toJson() {
    return new Gson().toJson(this);
  }
}
