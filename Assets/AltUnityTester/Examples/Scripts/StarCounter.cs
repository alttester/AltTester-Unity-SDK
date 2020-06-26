using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarCounter : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    int stars = 1;
    int starsCollected = 0;
    private void Start()
    {
        text.text = starsCollected + "/" + stars;
    }
    public void UpdateStarCounter(bool created)
    {
        if (created)
        {
            stars++;
        }
        else
        {
            starsCollected++;
        }
        text.text = starsCollected + "/" + stars;
    }
}
