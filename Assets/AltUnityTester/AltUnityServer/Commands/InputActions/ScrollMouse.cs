using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class ScrollMouse:Command
    {
        float scrollValue;
        float duration;

        public ScrollMouse(float scrollValue, float duration)
        {
            this.scrollValue = scrollValue;
            this.duration = duration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.LogMessage("scrollMouse with: " + scrollValue);
            Input.Scroll(scrollValue, duration);
            return "Ok";
#endif
            return null;
        }
    }
}
