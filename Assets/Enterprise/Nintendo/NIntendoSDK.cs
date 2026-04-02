/*
    Copyright(C) 2023 Altom Consulting
*/

// using System.Collections;
// using System.Collections.Generic;
// using AltTester.AltTesterUnitySDK.Notification;
// using UnityEngine;

// public class NIntendoSDK : MonoBehaviour
// {
//     private static bool isLoaded;

//     public static bool IsLoaded { get => isLoaded; }

//     public void Awake()
//     {
// #if UNITY_SWITCH
//         isLoaded = true;
// #else
//         isLoaded = false;
// #endif
//     }
//     void Start()
//     {
// #if UNITY_SWITCH
//         UnityEngine.Switch.Notification.SetOperationModeChangedNotificationEnabled(true);
//         UnityEngine.Switch.Notification.SetPerformanceModeChangedNotificationEnabled(true);
//         UnityEngine.Switch.Notification.SetResumeNotificationEnabled(true);
//         UnityEngine.Switch.Notification.SetFocusHandlingMode(UnityEngine.Switch.Notification.FocusHandlingMode.Notify);
//         UnityEngine.Switch.Notification.notificationMessageReceived += Notification_notificationMessageReceived;
// #endif
//     }


// #if UNITY_SWITCH
//     private void Notification_notificationMessageReceived(UnityEngine.Switch.Notification.Message obj)
//     {

//         switch (obj)
//         {
//             case UnityEngine.Switch.Notification.Message.FocusStateChanged:
//                 var a = UnityEngine.Switch.Notification.GetCurrentFocusState();
//                 UnityEngine.Debug.Log("Focused Changed: " + a.ToString());
//                 break;
//             case UnityEngine.Switch.Notification.Message.Resume:
//                 AltTesterApplicationPausedNotification.OnPause(false);
//                 break;
//             case UnityEngine.Switch.Notification.Message.OperationModeChanged:
//                 break;
//             case UnityEngine.Switch.Notification.Message.PerformanceModeChanged:
//                 break;
//             case UnityEngine.Switch.Notification.Message.ExitRequest:
//                 break;
//             default:
//                 break;
//         }
//     }
// #endif
// }
