using System;

namespace Altom.AltUnityDriver.Commands
{

    public class AltUnityMultipointSwipe : AltBaseCommand
    {
        AltUnityVector2[] positions;
        float duration;

        public AltUnityMultipointSwipe(SocketSettings socketSettings, AltUnityVector2[] positions, float duration) : base(socketSettings)
        {
            this.positions = positions;
            this.duration = duration;
        }

        public void Execute()
        {
            var args = new System.Collections.Generic.List<string> { "multipointSwipeChain", duration.ToString() };
            foreach (var pos in positions)
            {
                var posJson = PositionToJson(pos);
                args.Add(posJson);
            }

            SendCommand(args.ToArray());
            var data = Recvall();
            ValidateResponse("Ok", data, StringComparison.OrdinalIgnoreCase);
        }
    }
}
