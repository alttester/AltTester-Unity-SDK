namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityActionFinishedCommand : AltUnityCommand
    {
        public AltUnityActionFinishedCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
            string response = AltUnityErrors.errorNotFoundMessage;
#if ALTUNITYTESTER
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
