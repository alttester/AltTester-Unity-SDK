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
            UnityEngine.Debug.Log("Start moving touch chain for: " + string.Join(", ", positions.Select(p => p.ToString()).ToArray()));
            var response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
            
            Input.SetMovingTouch(positions, float.Parse(duration));
            response = "Ok";
            return response;
#endif
            return null;
        }
    }
}
