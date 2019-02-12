using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
[Serializable]
public class MyTest
{

    public bool _selected;
    public string _testName;
    public int _status;
    public bool _isSuite;
    public Type _type;
    public string _parentName;
    public int _testCaseCount;
    public bool _foldOut;
    public string _testResultMessage;

    public MyTest(bool selected, string testName, int status, bool isSuite, Type type, string parentName, int testCaseCount, bool foldOut, string testResultMessage)
    {
        _selected = selected;
        _testName = testName;
        _status = status;
        _isSuite = isSuite;
        _type = type;
        _parentName = parentName;
        _testCaseCount = testCaseCount;
        _foldOut = foldOut;
        _testResultMessage = testResultMessage;
    }

    public string TestResultMessage
    {
        get { return _testResultMessage; }
        set { _testResultMessage = value; }
    }

    public bool FoldOut
    {
        get { return _foldOut; }
        set { _foldOut = value; }
    }

    public int TestCaseCount
    {
        get { return _testCaseCount; }
        set { _testCaseCount = value; }
    }

    public Type Type
    {
        get { return _type; }
        set { _type = value; }
    }

    public string ParentName
    {
        get { return _parentName; }
        set { _parentName = value; }
    }

    public bool IsSuite
    {
        get { return _isSuite; }
        set { _isSuite = value; }
    }
    public bool Selected
    {
        get { return _selected; }
        set { _selected = value; }
    }

    public string TestName
    {
        get { return _testName; }
        set { _testName = value; }
    }

    public int Status
    {
        get { return _status; }
        set { _status = value; }
    }


}