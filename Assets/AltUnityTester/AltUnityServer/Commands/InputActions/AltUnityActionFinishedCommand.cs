namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityActionFinishedCommand : AltUnityCommand
    {
        public override string Execute()
        {
                string response = AltUnityRunner._altUnityRunner.errorNotFoundMessage;
#if ALTUNITYTESTER
                AltUnityRunner._altUnityRunner.LogMessage("actionFinished");
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
