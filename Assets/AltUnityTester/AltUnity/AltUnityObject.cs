using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct AltUnityObject {
    public string name;
	public int id;
    public int x;
    public int y;
    public int mobileY;
    public string text;
    public string type;
    public bool enabled;

	public AltUnityObject(string name, int id=0, int x=0, int y=0, int mobileY=0, string text="", string type="", bool enabled = true) {
		this.name = name;
		this.id = id;
		this.x = x;
		this.y = y;
		this.mobileY = mobileY;
		this.text = text;
		this.type = type;
		this.enabled = enabled;
	}
}
