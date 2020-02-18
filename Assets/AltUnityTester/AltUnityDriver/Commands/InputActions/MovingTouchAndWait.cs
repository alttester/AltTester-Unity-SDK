using Assets.AltUnityTester.AltUnityDriver.UnityStruct;

namespace Assets.AltUnityTester.AltUnityDriver.Commands.InputActions
{
    public class MultipointSwipeAndWait : AltBaseCommand
    {
        Vector2[] positions;
        float duration;
        
        public MultipointSwipeAndWait(SocketSettings socketSettings, Vector2[] positions, float duration) : base(socketSettings)
        {
            this.positions = positions;
            this.duration = duration;
        }

        public void Execute()
        {
            new MultipointSwipe(SocketSettings, positions, duration).Execute();
            System.Threading.Thread.Sleep((int)duration * 1000);
            string data;
            do
            {
                Socket.Client.Send(toBytes(CreateCommand("actionFinished")));
                data = Recvall();
            } while (data == "No");
            if (data.Equals("Yes"))
                return;
            HandleErrors(data);
        }
    }
}