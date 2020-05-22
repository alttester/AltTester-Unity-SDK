using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneButtonController : MonoBehaviour {

	public void NextSceneButtonPressed(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
