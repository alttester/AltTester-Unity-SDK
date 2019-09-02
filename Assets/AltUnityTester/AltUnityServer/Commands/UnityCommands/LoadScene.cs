using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class LoadScene:Command
    {
        string scene;

        public LoadScene(string scene)
        {
            this.scene = scene;
        }

        public override string Execute()
        {
            AltUnityRunner._altUnityRunner.LogMessage("LoadScene " + scene);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
            response = "Ok";
            return response;
        }
    }
}
