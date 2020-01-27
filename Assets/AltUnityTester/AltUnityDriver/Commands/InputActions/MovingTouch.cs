using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

namespace Assets.AltUnityTester.AltUnityDriver.Commands.InputActions
{
    public class MovingTouch : AltBaseCommand
    {
        Vector2[] positions;
        float duration;
        
        public MovingTouch(SocketSettings socketSettings, float duration, Vector2[] positions) : base(socketSettings)
        {
            this.positions = positions;
            this.duration = duration;
        }

        public void Execute()
        {
            var args = new System.Collections.Generic.List<string>{"movingTouchChain", duration.ToString()};
            foreach (var pos in positions)
            {
                var posJson = Newtonsoft.Json.JsonConvert.SerializeObject(pos, Newtonsoft.Json.Formatting.Indented, new Newtonsoft.Json.JsonSerializerSettings
                {
                    ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                });
                args.Add(posJson);
            }

            Socket.Client.Send(toBytes(CreateCommand(args.ToArray())));
            var data = Recvall();
            if (data.Equals("Ok"))
                return;
            HandleErrors(data);
        }
    }
}