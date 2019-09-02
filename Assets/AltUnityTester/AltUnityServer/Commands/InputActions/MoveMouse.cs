using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class MoveMouse:Command
    {
        UnityEngine.Vector2 location;
        float duration;

        public MoveMouse(Vector2 location, float duration)
        {
            this.location = location;
            this.duration = duration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
                UnityEngine.Debug.Log("moveMouse to: " + location);
                Input.MoveMouse(location, duration);
                return "Ok";
#endif
            return null; ;
        }
    }
}
