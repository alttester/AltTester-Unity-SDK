using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour {
    public StarCounter starCounter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 2, 1);
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Player1")
        {
            starCounter.UpdateStarCounter(false);
            Destroy(gameObject);
        }
    }
}
