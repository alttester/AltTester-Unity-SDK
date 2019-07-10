using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicController : MonoBehaviour {
    public GameObject starPrefab;
    public Camera camera;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray,out hit))
            {
                if (hit.transform.gameObject.name == "Floor")
                {
                    var position = hit.point;
                    position.y += 1;
                    Instantiate(starPrefab, position, Quaternion.identity);

                }
            }
        }
	}
}
