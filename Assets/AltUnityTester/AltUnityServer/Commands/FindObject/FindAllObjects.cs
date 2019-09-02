using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class FindAllObjects :Command
    {
        string methodParameter;

        public FindAllObjects(string methodParameter)
        {
            this.methodParameter = methodParameter;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("all objects requested");
            var parameters = ";" + methodParameter;
            return new FindObjectsByName(parameters).Execute();
        }
    }
}
