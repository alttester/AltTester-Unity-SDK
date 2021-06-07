namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityActionFinishedCommand : AltUnityCommand
    {
        public AltUnityActionFinishedCommand(params string[] parameters) : base(parameters, 2) { }
        public override string Execute()
        {
#if ALTUNITYTESTER
            return Input.Finished ? "Yes" : "No";
#else
            return AltUnityErrors.errorInputModule;
#endif
        }
    }
}
