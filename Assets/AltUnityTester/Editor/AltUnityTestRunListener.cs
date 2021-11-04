﻿using Altom.AltUnityTesterEditor.Logging;
using NLog;

namespace Altom.AltUnityTesterEditor
{
    public class AltUnityTestRunListener : NUnit.Framework.Interfaces.ITestListener
    {
        private static readonly Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        private readonly TestRunDelegate callRunDelegate;

        public AltUnityTestRunListener(TestRunDelegate callRunDelegate)
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