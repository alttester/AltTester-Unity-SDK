using System.Linq;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class SetMovingTouchChainCommand : Command
    {
        UnityEngine.Vector2[] positions;
        string duration;

        public SetMovingTouchChainCommand(UnityEngine.Vector2[] positions, string duration)
        {
            this.positions = positions;
            this.duration = duration;
        }

        public override string Execute()
        {
#if ALTUNITYTESTER
            AltUnityRunner._altUnityRunner.LogMessage("Start moving touch chain for: " + string.Join(", ", positions.Select(p => p.ToString()).ToArray()));
            
            Input.SetMovingTouch(positions, float.Parse(duration));
            return "OK";
#endif
            return null;
        }
    }
}
