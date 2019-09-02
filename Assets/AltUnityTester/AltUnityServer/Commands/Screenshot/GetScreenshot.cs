using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetScreenshot: Command
    {
        UnityEngine.Vector2 size;
        AltClientSocketHandler handler;

        public GetScreenshot(Vector2 size, AltClientSocketHandler handler)
        {
            this.size = size;
            this.handler = handler;
        }

        public override string Execute()
        {
            UnityEngine.Debug.Log("getScreenshot" + size);
            AltUnityRunner._altUnityRunner.StartCoroutine(AltUnityRunner._altUnityRunner.TakeScreenshot(size, handler));
            return "Ok";
        }
    }
}
