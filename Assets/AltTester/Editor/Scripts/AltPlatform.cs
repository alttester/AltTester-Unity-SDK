namespace AltTester.AltTesterUnitySDK.Editor
{
    public enum AltPlatform
    {
        Android,
#if UNITY_EDITOR_OSX
        iOS,
#endif
        Editor,
        Standalone,
        WebGL
    }
}
