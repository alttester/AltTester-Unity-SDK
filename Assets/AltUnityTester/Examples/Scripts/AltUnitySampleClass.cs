using System.Collections.Generic;
using UnityEngine;

public class AltUnitySampleClass
{

    private string testString;
    private int testInt;
    private static bool testBool = false;
    [SerializeField]
    private List<float> listFloatTest;
    [SerializeField]
    private Dictionary<string, double> testDictionaryStringDouble;
    public int[] arrayIntTest = new int[1] { 1 };

    public AltUnitySampleClass(string testString, int testInt, List<float> listFloatTest, Dictionary<string, double> testDictionaryStringDouble)
    {
        TestInt = testInt;
        ListFloatTest = listFloatTest;
        TestDictionaryStringDouble = testDictionaryStringDouble;
        TestString = testString;
    }

    public string TestString
    {
        get
        {
            return testString;
        }

        set
        {
            testString = value;
        }
    }

    public int TestInt
    {
        get
        {
            return testInt;
        }

        set
        {
            testInt = value;
        }
    }

    public static bool TestBool
    {
        get
        {
            return testBool;
        }

        set
        {
            testBool = value;
        }
    }

    internal List<float> ListFloatTest
    {
        get
        {
            return listFloatTest;
        }

        set
        {
            listFloatTest = value;
        }
    }

    protected Dictionary<string, double> TestDictionaryStringDouble
    {
        get
        {
            return testDictionaryStringDouble;
        }

        set
        {
            testDictionaryStringDouble = value;
        }
    }
    public string TestMethod()
    {
        return "Test";
    }
}
