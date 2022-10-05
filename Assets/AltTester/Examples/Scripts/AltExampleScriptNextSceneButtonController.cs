using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AltExampleScriptNextSceneButtonController : MonoBehaviour
{

    public void NextSceneButtonPressed(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
