/*
    Copyright(C) 2025 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using UnityEngine;

public class AltSampleClass
{

    private string testString;
    private int testInt;
    private static bool testBool = false;
    [SerializeField]
    private List<float> listFloatTest;
    [SerializeField]
    private Dictionary<string, double> testDictionaryStringDouble;
    public int[] arrayIntTest = new int[1] { 1 };

    public AltSampleClass(string testString, int testInt, List<float> listFloatTest, Dictionary<string, double> testDictionaryStringDouble)
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
