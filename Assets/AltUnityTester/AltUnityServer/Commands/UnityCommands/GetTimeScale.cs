using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetTimeScale :  Command
    {
        public override string Execute()
        {
            UnityEngine.Debug.Log("GetTimeScale");
            string response = AltUnityRunner._altUnityRunner.errorCouldNotPerformOperationMessage;
            response = Newtonsoft.Json.JsonConvert.SerializeObject(UnityEngine.Time.timeScale);
            return response;
        }
    }
}
