using System;
using System.Collections.Generic;
using System.Reflection;
using Altom.AltUnityDriver;
using Assets.AltUnityTester.AltUnityServer;
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
    private int mouseOverCounter = 0;
    public static AltUnitySampleClass StaticSampleClass = new AltUnitySampleClass("test", 1, new List<float> { 2.3f, 4.4f }, new Dictionary<string, double>() { { "first", 1.1 }, { "second", 2.2 }, { "third", 3.3 } });

    public TouchPhase TouchPhase = TouchPhase.Canceled;
    private TestStructure testStructure = new TestStructure("test", "test2", new List<int>() { 0, 1 });
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

#pragma warning disable 0414
    private int privateVariable = 0;
    private static int privateStaticVariable = 0;
#pragma warning restore 0414

    protected void Update()
    {
        transform.Rotate(Input.acceleration);
        if (Input.touchCount > 0)
        {
            TouchPhase = Input.GetTouch(0).phase;
        }
    }
    protected void OnMouseDown()
    {
        Jump("Capsule was clicked to jump!");
    }
    public void OnMouseOver()
    {
        // Jump("Capsule was hovered to jump!");
        mouseOverCounter++;
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

    public string TestCallComponentMethod(int param1, string param2, float param, int[] arrayOfInts)
    {
        return string.Format("{0},{1},{2},[{3}]", param1, param2, param, string.Join(",", arrayOfInts));
    }

    public void Test(string a)
    {
        Jump(a);
    }
    public void Test(int a)
    {
        Jump((a + 4).ToString());
    }

    public struct TestStructure
    {
        string text;
        string text2;
        List<int> list;

        public TestStructure(string text, string text2, List<int> list)
        {
            this.text = text;
            this.text2 = text2;
            this.list = list;
        }

        public string Text { get => text; set => text = value; }
        public List<int> List { get => list; set => list = value; }
    }

}
