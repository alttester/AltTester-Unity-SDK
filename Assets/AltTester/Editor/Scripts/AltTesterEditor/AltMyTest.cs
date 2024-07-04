/*
    Copyright(C) 2024 Altom Consulting

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

using System;
using UnityEngine;

namespace AltTester.AltTesterUnitySDK.Editor
{
    [System.Serializable]
    public class AltMyTest
    {
        [SerializeField]
        private bool _selected;
        [SerializeField]
        private string _testName;
        [SerializeField]
        private string _testAssembly;
        [SerializeField]
        private int _status;
        [SerializeField]
        private bool _isSuite;
        [SerializeField]
        private string _type;
        [SerializeField]
        private string _parentName;
        [SerializeField]
        private int _testCaseCount;
        [SerializeField]
        private bool _foldOut;
        [SerializeField]
        private string _testResultMessage;
        [SerializeField]
        private string _testStackTrace;
        [SerializeField]
        private Double _testDuration;
        [SerializeField]
        private string path;
        [SerializeField]
        private int _testSelectedCount;

        public AltMyTest(bool selected, string testName, string testAssembly, int status, bool isSuite, string type, string parentName, int testCaseCount, bool foldOut, string testResultMessage, string testStackTrace, Double testDuration, string path, int testSelectedCount)
        {
            _selected = selected;
            _testName = testName;
            _testAssembly = testAssembly;
            _status = status;
            _isSuite = isSuite;
            _type = type;
            _parentName = parentName;
            _testCaseCount = testCaseCount;
            _foldOut = foldOut;
            _testResultMessage = testResultMessage;
            _testStackTrace = testStackTrace;
            _testDuration = testDuration;
            _testSelectedCount = testSelectedCount;
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

        public string TestAssembly
        {
            get
            {
                return _testAssembly;
            }

            set
            {
                _testAssembly = value;
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

        public string Type
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
        public int TestSelectedCount
        {
            get
            {
                return _testSelectedCount;
            }
            set
            {
                _testSelectedCount = value;
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
}
