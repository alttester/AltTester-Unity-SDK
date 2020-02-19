using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnitySetMultipointSwipeChainCommand : AltUnityCommand
    {
        UnityEngine.Vector2[] positions;
        string duration;

        public AltUnitySetMultipointSwipeChainCommand(UnityEngine.Vector2[] positions, string duration)
        {
            this.positions = positions;
            this.duration = duration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.LogMessage("Start moving touch chain for: " + string.Join(", ", positions.Select(p => p.ToString()).ToArray()));
            
            Input.SetMultipointSwipe(positions, float.Parse(duration));
            return "OK";
#endif
            return null;
        }
    }
}
