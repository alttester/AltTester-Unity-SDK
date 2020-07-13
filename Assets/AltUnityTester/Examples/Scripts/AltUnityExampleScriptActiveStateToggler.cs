using UnityEngine;
using System.Collections;

public class AltUnityExampleScriptActiveStateToggler : MonoBehaviour {

	public void ToggleActive () {
		gameObject.SetActive (!gameObject.activeSelf);
	}
}
