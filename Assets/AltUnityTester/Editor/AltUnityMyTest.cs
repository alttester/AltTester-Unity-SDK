
using System;

[System.Serializable]
public class AltUnityMyTest
{

    public bool _selected;
    public string _testName;
    public int _status;
    public bool _isSuite;
    public System.Type _type;
    public string _parentName;
    public int _testCaseCount;
    public bool _foldOut;
    public string _testResultMessage;
    public string _testStackTrace;
    public Double _testDuration;
    public string path;

    public AltUnityMyTest(bool selected, string testName, int status, bool isSuite, Type type, string parentName, int testCaseCount, bool foldOut, string testResultMessage, string testStackTrace, Double testDuration, string path)
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
        _testStackTrace = testStackTrace;
        _testDuration = testDuration;
        this.path = path;
    }

    public bool Selected
    {
        get
        {
            return _selected;
        }

        set
        {
            _selected = value;
        }
    }

    public string TestName
    {
        get
        {
            return _testName;
        }

        set
        {
            _testName = value;
        }
    }

    public int Status
    {
        get
        {
            return _status;
        }

        set
        {
            _status = value;
        }
    }

    public bool IsSuite
    {
        get
        {
            return _isSuite;
        }

        set
        {
            _isSuite = value;
        }
    }

    public Type Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    public string ParentName
    {
        get
        {
            return _parentName;
        }

        set
        {
            _parentName = value;
        }
    }

    public int TestCaseCount
    {
        get
        {
            return _testCaseCount;
        }

        set
        {
            _testCaseCount = value;
        }
    }

    public bool FoldOut
    {
        get
        {
            return _foldOut;
        }

        set
        {
            _foldOut = value;
        }
    }

    public string TestResultMessage
    {
        get
        {
            return _testResultMessage;
        }

        set
        {
            _testResultMessage = value;
        }
    }

    public string TestStackTrace
    {
        get
        {
            return _testStackTrace;
        }

        set
        {
            _testStackTrace = value;
        }
    }

    public Double TestDuration
    {
        get
        {
            return _testDuration;
        }

        set
        {
            _testDuration = value;
        }
    }

    public string Path
    {
        get
        {
            return path;
        }

        set
        {
            path = value;
        }
    }
}