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

    public static void DeleteAltTesterPrefabIfExists()
    {
        var AltTesterPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(AltBuilder.GetAltTesterPrefabLocation());
        if (AltTesterPrefab != null)
        {
            Component[] components = AltTesterPrefab.GetComponents<Component>();
            bool hasMissingReferences = Array.Exists(components, component => component == null);
            if (!hasMissingReferences)
            {
                EditorApplication.delayCall -= DeleteAltTesterPrefabIfExists;
                return;
            }
            UnityEditor.AssetDatabase.DeleteAsset(AltBuilder.GetAltTesterPrefabLocation());
        }
        EditorApplication.delayCall -= DeleteAltTesterPrefabIfExists;
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