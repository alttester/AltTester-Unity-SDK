namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityScrollMouseCommand :AltUnityCommand
    {
        float scrollValue;
        float duration;

        public AltUnityScrollMouseCommand (float scrollValue, float duration)
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
