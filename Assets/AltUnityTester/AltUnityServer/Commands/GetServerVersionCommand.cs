namespace Assets.AltUnityTester.AltUnityServer.Commands
{
    class GetServerVersionCommand : Command
    {
        public override string Execute()
        {
            var filePath = "AltUnityTester/ServerVersion";
            UnityEngine.TextAsset targetFile = UnityEngine.Resources.Load<UnityEngine.TextAsset>(filePath);
            string text = targetFile.text;
            return text;
        }
    }
}
