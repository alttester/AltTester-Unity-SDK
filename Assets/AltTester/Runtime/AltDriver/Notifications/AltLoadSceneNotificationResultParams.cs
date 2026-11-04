/*
    Copyright(C) 2026 Altom Consulting
*/


namespace AltTester.AltTesterSDK.Driver.Notifications
{
    public class AltLoadSceneNotificationResultParams
    {
        public string sceneName;
        public AltLoadSceneMode loadSceneMode;

        public AltLoadSceneNotificationResultParams(string sceneName, AltLoadSceneMode loadSceneMode)
        {
            this.sceneName = sceneName;
            this.loadSceneMode = loadSceneMode;
        }
    }
}
