package com.alttester;

import java.lang.reflect.Type;

import com.alttester.UnityStruct.AltKeyCode;
import com.google.gson.JsonElement;
import com.google.gson.JsonPrimitive;
import com.google.gson.JsonSerializationContext;
import com.google.gson.JsonSerializer;

public class StatusSerializer implements JsonSerializer<AltKeyCode> {
    @Override
    public JsonElement serialize(AltKeyCode src, Type typeOfSrc, JsonSerializationContext context) {
        return new JsonPrimitive(src.getValue());
    }
}