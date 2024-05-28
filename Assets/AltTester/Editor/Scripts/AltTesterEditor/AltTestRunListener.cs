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

using AltTester.AltTesterUnitySDK.Editor.Logging;

namespace AltTester.AltTesterUnitySDK.Editor
{
    public class AltTestRunListener : NUnit.Framework.Interfaces.ITestListener
    {
        private static readonly NLog.Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        private readonly TestRunDelegate callRunDelegate;

        public AltTestRunListener(TestRunDelegate callRunDelegate)
        {
            this.callRunDelegate = callRunDelegate;
        }

        public void TestStarted(NUnit.Framework.Interfaces.ITest test)
        {
            if (!test.IsSuite)
            {
                if (callRunDelegate != null)
                    callRunDelegate(test.Name);
            }
        }

        public void TestFinished(NUnit.Framework.Interfaces.ITestResult result)
        {
            if (!result.Test.IsSuite)
            {
                logger.Info("==============> TEST " + result.Test.FullName + ": " + result.ResultState.ToString().ToUpper());
                if (result.ResultState != NUnit.Framework.Interfaces.ResultState.Success)
                {
                    logger.Error(result.Message + System.Environment.NewLine + result.StackTrace);
                }
                logger.Info("======================================================");
            }
        }

        public void TestOutput(NUnit.Framework.Interfaces.TestOutput output)
        {
        }
    }
}
