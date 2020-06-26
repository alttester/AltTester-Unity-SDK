using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicController : MonoBehaviour {
    public GameObject starPrefab;
    public Camera cameraControlled;
    public StarCounter starCounter;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cameraControlled.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out hit))
            {
                if (hit.transform.gameObject.name == "Floor")
                {
                    starCounter.UpdateStarCounter(true);
                    var position = hit.point;
                    position.y += 1;
                    var star=Instantiate(starPrefab, position, Quaternion.identity);
                    star.GetComponent<StarController>().starCounter = starCounter;

                }
            }
        }
	}
}
