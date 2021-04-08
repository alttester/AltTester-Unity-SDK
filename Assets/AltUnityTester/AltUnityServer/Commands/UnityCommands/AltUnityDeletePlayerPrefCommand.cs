
namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class AltUnityDeletePlayerPrefCommand : AltUnityCommand
    {
        public AltUnityDeletePlayerPrefCommand(params string[] parameters) : base(parameters, 2)
        { }

        public override string Execute()
        {
            UnityEngine.PlayerPrefs.DeleteAll();
            return "Ok";
        }
    }
}
