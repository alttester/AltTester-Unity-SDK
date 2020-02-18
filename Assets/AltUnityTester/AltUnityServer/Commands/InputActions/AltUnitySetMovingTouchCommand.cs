namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnitySetMultipointSwipeCommand :AltUnityCommand
    {
        UnityEngine.Vector2 start;
        UnityEngine.Vector2 destination;
        string duration;

        public AltUnitySetMultipointSwipeCommand (UnityEngine.Vector2 start, UnityEngine.Vector2 destination, string duration)
        {
            this.start = start;
            this.destination = destination;
            this.duration = duration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.LogMessage("Touch at: " + start);
            string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            
            UnityEngine.Vector2[] positions = {start, destination};
            Input.SetMultipointSwipe(positions, float.Parse(duration));
            response = "Ok";
            
            return response;
#endif
            return null;
        }
    }
}
