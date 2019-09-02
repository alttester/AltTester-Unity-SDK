using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class HoldButton: Command
    {
        UnityEngine.KeyCode keyCode;
        float power;
        float duration;

        public HoldButton(KeyCode keyCode, float power, float duration)
        {
            this.keyCode = keyCode;
            this.power = power;
            this.duration = duration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            UnityEngine.Debug.Log("pressKeyboardKey: " + keyCode);
            var powerClamped = UnityEngine.Mathf.Clamp01(power);
            Input.SetKeyDown(keyCode, power, duration);
#endif      
            return "Ok";
        }
    }
}
