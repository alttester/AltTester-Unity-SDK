namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class ActionFinished : Command
    {
        public override string Execute()
        {
                string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
#if ALTUNITYTESTER
                UnityEngine.Debug.Log("actionFinished");
                if (Input.Finished)
                    response = "Yes";
                else
                {
                    response = "No";
                }
#endif
            return response;
        }
    }
}
