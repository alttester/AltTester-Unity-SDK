
using Newtonsoft.Json;

namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    public class AltUnitySetMultipointSwipeChainCommand : AltUnityCommand
    {
        readonly UnityEngine.Vector2[] positions;
        readonly string duration;

        public AltUnitySetMultipointSwipeChainCommand(params string[] parameters) : base(parameters, parameters.Length)
        {
            var length = parameters.Length - 3;
            var positions = new UnityEngine.Vector2[length];
            for (var i = 0; i < length; i++)
                positions[i] = JsonConvert.DeserializeObject<UnityEngine.Vector2>(parameters[i + 3]);
            this.positions = positions;
            this.duration = parameters[2];
        }

        public override string Execute()
        {
#if ALTUNITYTESTER

            Input.SetMultipointSwipe(positions, float.Parse(duration));
            return "OK";
#else
            return null;
#endif
        }
    }
}
