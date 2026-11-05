using System;
using System.Reflection;
using AltTester.AltTesterUnitySDK.Editor;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AltTesterImportErrorChecker : EditorWindow
{
    static AltTesterImportErrorChecker()
    {
        EditorApplication.delayCall += CheckLogsForImportErrors;
        EditorApplication.delayCall += DeleteAltTesterPrefabIfExists;
    }

    /// <summary>
    /// Repairs the generated AltTester prefab when it has missing scripts, e.g. after Active Input Handling changed.
    /// It is generated again in place instead of deleted, so it keeps its GUID and the scenes using it keep working.
    /// </summary>
    public static void DeleteAltTesterPrefabIfExists()
    {
        EditorApplication.delayCall -= DeleteAltTesterPrefabIfExists;
        var AltTesterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(AltBuilder.GetAltTesterPrefabLocation());
        // GetAltTesterPrefab regenerates a prefab with missing scripts.
        if (AltTesterPrefab != null && AltBuilder.HasMissingScripts(AltTesterPrefab))
            AltBuilder.GetAltTesterPrefab();
    }

    public static void CheckLogsForImportErrors()
    {
#if UNITY_6000_0_OR_NEWER
        Type logEntriesType = Type.GetType("UnityEditor.LogEntries, UnityEditor");
        if (logEntriesType == null)
            return;

        MethodInfo getCountMethod = logEntriesType.GetMethod("GetCount");
        if (getCountMethod == null)
            return;
        MethodInfo getEntryMethod = logEntriesType.GetMethod("GetEntryInternal", BindingFlags.Static | BindingFlags.Public);
        if (getEntryMethod == null)
            return;
        Type logEntryType = Type.GetType("UnityEditor.LogEntry, UnityEditor");
        if (logEntryType == null)
            return;
        object logEntry = Activator.CreateInstance(logEntryType);
        if (logEntry == null)
            return;

        int count = (int)getCountMethod.Invoke(null, null);
        FieldInfo messageField = logEntryType.GetField("condition", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                          ?? logEntryType.GetField("message", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);


        for (int i = 0; i < count; i++)
        {
            try
            {

                getEntryMethod.Invoke(null, new object[] { i, logEntry });
                string message = (string)messageField.GetValue(logEntry);
                if (message.Contains("[AssemblyUpdater] Failed to resolve assembly UnityEngine.UI, Version=1.0.0.0, Culture=neutral,") && message.Contains("AltTester"))
                {
                    UnityEngine.Debug.LogError("AltTester Import Error Detected. Please clear the Unity Console and handle just the remaining errors.");
                    return;
                }
            }
            catch (Exception)
            {
                // Ignore exceptions and continue checking other log entries
            }
        }
        EditorApplication.delayCall -= CheckLogsForImportErrors;
#endif
    }
}