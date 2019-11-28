public enum Platform
{
    Android,
#if UNITY_EDITOR_OSX
    iOS,
#endif
    Editor,
    Standalone
}
