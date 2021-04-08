using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AltUnityExampleScriptCapsule : AltUnityInheritedFields
{
    public Text capsuleInfo;
    public int[] arrayOfInts = { 1, 2, 3 };
    public bool TestBool = true;
    public string stringToSetFromTests = "intialValue";
    public AltUnitySampleClass AltUnitySampleClass;
    public List<AltUnitySampleClass> listOfSampleClass = new List<AltUnitySampleClass>() { new AltUnitySampleClass("test", 1, new List<float> { 2.3f, 4.4f }, new Dictionary<string, double>() { { "first", 1.1 }, { "second", 2.2 }, { "third", 3.3 } }),
        new AltUnitySampleClass("test2", 1, new List<float> { 2.3f, 4.4f }, new Dictionary<string, double>() { { "first", 1.1 }, { "second", 2.2 }, { "third", 3.3 } }) };
    private bool testProperty;
    public static AltUnitySampleClass StaticSampleClass = new AltUnitySampleClass("test", 1, new List<float> { 2.3f, 4.4f }, new Dictionary<string, double>() { { "first", 1.1 }, { "second", 2.2 }, { "third", 3.3 } });
    protected void Awake()
    {
        AltUnitySampleClass1 = new AltUnitySampleClass("test", 1, new List<float> { 2.3f, 4.4f }, new Dictionary<string, double>() { { "first", 1.1 }, { "second", 2.2 }, { "third", 3.3 } });
    }

    public bool TestProperty
    {
        get
        {
            return testProperty;
        }

        set
        {
            testProperty = value;
        }
    }

    public AltUnitySampleClass AltUnitySampleClass1
    {
        get
        {
            return AltUnitySampleClass;
        }

        set
        {
            AltUnitySampleClass = value;
        }
    }
    public static int PublicStaticVariable = 0;

    private int privateVariable = 0;
    private static int privateStaticVariable = 0;

    protected void Update()
    {
        transform.Rotate(Input.acceleration);
    }
    protected void OnMouseDown()
    {
        Jump("Capsule was clicked to jump!");
    }


    public void Jump(string capsuleInfoText)
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
        capsuleInfo.text = capsuleInfoText;
    }

    public void UIButtonClicked()
    {
        Jump("UIButton clicked to jump capsule!");

    }

    public void TestMethodWithManyParameters(int param1, string param2, float param, int[] arrayOfInts)
    {
        Debug.Log("test method with many parameters called");
    }
    public int TestMethodWithOptionalParameters(int param1, int param2 = 0)
    {
        return param1 + param2;
    }
    public string TestMethodWithOptionalParameters(string param1, string param2 = "Test")
    {
        return param1 + param2;
    }

    public void Test(string a)
    {
        Jump(a);
    }
    public void Test(int a)
    {
        Jump((a + 4).ToString());
    }

}
