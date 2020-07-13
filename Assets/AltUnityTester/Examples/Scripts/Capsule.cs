using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Capsule : MonoBehaviour {

    public Text capsuleInfo;
    public int[] arrayOfInts = {1, 2, 3};
    public bool TestBool = true;
    public string stringToSetFromTests = "intialValue";

    void OnMouseDown() {
        Jump("Capsule was clicked to jump!");
    }
   

    public void Jump(string capsuleInfoText) {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        capsuleInfo.text = capsuleInfoText;
    }

    public void UIButtonClicked() {
        Jump("UIButton clicked to jump capsule!");
        
    }

    public void TestMethodWithManyParameters(int param1, string param2, float param, int[] arrayOfInts) {
        Debug.Log("test method with many parameters called");
    }

    public void Test(string a)
    {
        Jump(a);
    }
    public void Test(int a)
    {
        Jump((a+4).ToString());
    }

}
