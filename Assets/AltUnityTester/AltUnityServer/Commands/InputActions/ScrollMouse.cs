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
            UnityEngine.Debug.Log("scrollMouse with: " + scrollValue);
            Input.Scroll(scrollValue, duration);
            return "Ok";
#endif
            return null;
        }
    }
}
