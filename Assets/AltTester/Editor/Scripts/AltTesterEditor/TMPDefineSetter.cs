/*
    Copyright(C) 2026 Altom Consulting
*/
#if UNITY_EDITOR

/// <summary>
/// TextMeshPro support no longer depends on a TMP_PRESENT scripting define: the commands find TextMeshPro at
/// runtime, so nothing is written to the Player Settings any more.
/// </summary>
[System.Obsolete("TextMeshPro is detected at runtime, the TMP_PRESENT define is no longer needed.")]
public class TMPDefineSetter
{
    public static void CheckAndSetTMPDefine()
    {
    }
}
#endif
