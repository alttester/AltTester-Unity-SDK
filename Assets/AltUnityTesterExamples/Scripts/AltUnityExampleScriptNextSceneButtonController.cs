using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AltUnityExampleScriptNextSceneButtonController : MonoBehaviour {

	public void NextSceneButtonPressed(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
