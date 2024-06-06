# FAQ

<details>
<summary> On what platforms can I run tests with AltTester® Unity SDK? </summary>
<br>
PC, Mac, Android, iOS and Unity Editor; support for WebGL and Consoles is work in progress.
</details>
<br>

<details>
<summary> What programming language can I use to write tests?</summary>
<br>
 C#, Python, Java or Robot Framework.
</details>
<br>

<details>
<summary> Can AltTester® Unity SDK be integrated with Appium?</summary>
<br>
Yes, AltTester® Unity SDK can be used alongside Appium. Appium allows you to access the native objects and AltTester® Unity SDK can be used to access the Unity objects.  For more info regarding how to run tests together with appium check <em><a href="alttester-with-appium.html">Running tests together with Appium</a></em>.
</details>
<br>

<details>
<summary> What versions of Unity does AltTester® Unity SDK work with? </summary>
<br>
AltTester® Unity SDK works with Unity 2020.3.0 or higher. If you encounter any issues we'd like to hear about them. You can <a href="contributing.html#did-you-find-a-bug">raise an issue</a> or join our community on <a href="https://discord.gg/Ag9RSuS">Discord</a>.
</details>
<br>

<details>
<summary>Can I use AltTester® Unity SDK to run tests using device cloud services? </summary>
<br>
It works with some of the cloud services. We tried it with Bitbar, AWS Device Farm, BrowserStack and SauceLabs.
These give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. Some of these cloud services allow running Appium automated tests by giving you access to an Appium server running in the cloud that has access to all their mobile devices. More info about this here:<em><a href=" alttester-with-cloud.html"> Running tests using device cloud services.</a></em>
</details>
<br>

<details>
<summary> Do I need access to the source code of the Unity App to write tests?</summary>
<br>
In order to run tests using AltTester® Unity SDK you require an <a href="get-started.html#instrument-your-app-with-alttester-unity-sdk">instrumented build</a> of the Unity App. To create an instrumented build of the Unity App you need to <a href="get-started.html#import-alttester-package-in-unity-editor">import</a> the AltTester® package in Unity Editor.
</details>
<br>

<details>
<summary> I don’t have access to source code, but I do have access to an instrumented build. How can I begin to write tests?</summary>
<br>

```eval_rst
We’ve published AltTester® Desktop, which allows you to inspect the app objects outside the unity editor without access to the source code. More information about AltTester® Desktop can be found in :altTesterdesktopdocumentation:`this documentation <home.html>`.
```
</details>
<br>

## Troubleshooting

<details>
<summary> I get <strong>`waiting for connection on port 13000`</strong> popup message when i start my Unity App </summary>
<br>
The popup message shows up when you start your instrumented Unity App. It tells you that the AltTester® Unity SDK is ready and you can start running your tests.
</details>
<br>

<details>
<summary>I get the error: <strong>Multiple precompiled assemblies with the same name Newtonsoft.Json.dll included or the current platform.</strong> </summary>
<br>
You get this error due to multiple imports of Newtonsoft.Json.dll library. You can remove the Newtonsoft.Json version from AltTester® Unity SDK by deleting the <em>Newtonsonft</em> folder <em>Assets/AltTester/3rdParty/Newtonsonft</em>.
</details>
<br>

<details>
<summary> I get the error: <strong>The type or namespace name 'Newtonsoft' could not be found (are you missing a using directive or an assembly reference?)</strong>,  </summary>
<br>
You get this error because you don't have a reference to Newtonsoft.Json package.
<br>
Add `"com.unity.nuget.newtonsoft-json": "3.1.0"` to your project `manifest.json`, inside `dependencies`.

```
{
    "dependencies": {
        "com.unity.nuget.newtonsoft-json": "3.1.0"
    }
}
```

</details>
<br>

<details>
<summary> I get the error: <strong>The type or namespace name 'InputTestFixture' could not be found (are you missing a using directive or an assembly reference?)</strong>, </summary>
<br>
You get this error because you don't have `com.unity.inputsystem` added as a testables dependency.
<br>
Add `"com.unity.inputsystem"` to your `manifest.json`, inside `testables.`

```
{
    "testables": [
        "com.unity.inputsystem"
  ]
}
```

</details>
<br>

<details>
<summary><strong>[Addressable]</strong>Building the instrumented app, I get the error: <strong>The type or namespace name 'InputTestFixture' could not be found (are you missing a using directive or an assembly reference?)</strong>, </summary>
<br>
If you have Addressables package included in your project, set the Addressables settings to not build addressable when building the instrumented app. This can be done in <strong>Windows->Asset Management->Addressables->Settings</strong> and you will have an option <strong>Build Addressables on Player Build</strong>. Select <strong>Do not build Addressables content on Player build</strong>
<br><br>
When building Addressable from Asset Management make sure that the option for <strong>Keep ALTTESTER symbol defined</strong> is not checked.
<br><br>
Make sure you built your latest addressable before instrumenting your app with AltTester
</details>
<br>

<details>
<summary>How can I <strong>use the Input from AltTester® Unity SDK</strong> if my project is using <strong>Assembly Definitions </strong>?</summary>
<br>
To use the Input from AltTester® Unity SDK you have to reference <strong>AltTesterUnitySDK.asmdef</strong> in your .asmdef. In case you are using multiple assembly definitions you will have to reference our .asmdef in all of your .asmdef files which contain a reference to any kind of input (Unity's input or your custom built input).
</details>
<br>

<details>
<summary> I downloaded the AltTester® package v1.7.2 from the documentation on MacOS. I got a warning pop-up about the input system where I chose 'Yes' because I am using the New Input System. After reopening Unity Editor, <strong>AltTester® Unity SDK is missing.</strong></summary>
<br>

After reopening Unity Editor, add again the AltTester® package in your project.
<br>

</details>
<br>

<details>
<summary>I get the error: <strong>The type or namespace name 'InputSystem' does not exist in the namespace 'UnityEngine' (are you missing an assembly reference?)</strong></summary>
<br>

You get this error because you don't have the Input System (New) package. If you only want to use the Input Manager (Old) in your project, follow this steps:
<br>

-   <strong>delete</strong>:
    -   `Assets\AltTester\AltServer\NewInputSystem.cs`
    -   `Assets\AltTester\AltServer\AltKeyMapping.cs`
-   <strong>comment</strong> in `Assets\AltTester\AltServer\AltPrefabDrag.cs` the entire `#else` statement

    ```
    #if ENABLE_LEGACY_INPUT_MANAGER
                eventData.pointerDrag.transform.position = Input.mousePosition;
    // #else
            // eventData.pointerDrag.gameObject.transform.position = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
    #endif
    ```

-   <strong>comment</strong> in `Assets\AltTester\AltServer\Input.cs`:

    -   all imports for using `UnityEngine.InputSystem.UI`

        ```
        #if ALTTESTER && ENABLE_LEGACY_INPUT_MANAGER

        using System;
        using System.Collections;
        using System.Collections.Generic;
        using System.Linq;
        using AltTester.AltTesterUnitySDK.Driver;
        using AltTester.AltTesterUnitySDK;
        using AltTester.AltTesterUnitySDK.InputModule;
        using UnityEngine;
        using UnityEngine.EventSystems;
        // using UnityEngine.InputSystem.UI;
        using UnityEngine.Scripting;
        ```

    -   all `if` lines that contain `InputSystemUIInputModule` and the curly brackets inside these `if` statements making sure to leave the code inside the brackets uncommented
        ```
        // if (EventSystem.current.currentInputModule != null && EventSystem.current.currentInputModule.GetType().Name != typeof(InputSystemUIInputModule).Name)
                // {
                    if (eventSystemTarget != previousEventSystemTarget)
                    {
                        if (previousEventSystemTarget != null) UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(previousEventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerExitHandler);
                        if (eventSystemTarget != null && previousMousePosition != mousePosition) UnityEngine.EventSystems.ExecuteEvents.ExecuteHierarchy(eventSystemTarget, pointerEventData, UnityEngine.EventSystems.ExecuteEvents.pointerEnterHandler);
                        previousEventSystemTarget = eventSystemTarget;
                    }
                // }
        ```

-   <strong>comment</strong> in `Assets\AltTester\AltServer\AltMockUpPointerInputModule.cs` the same as the above

</details>
<br>

<details>
<summary> <strong>Lean Touch:</strong> AltTester® is not working in my application that uses Old Input System and Lean Touch </strong></summary>
<br>
There are two steps to make AltTester work with Lean Touch:

1. Add `AltTesterUnitySDK` as an assembly definition reference in `CW.Common` asmdef that can be found usually at `Plugin->CW->Shared->Common`. 
2. In the `CwInput.cs` file replace every occurrence of `UnityEngine.Input.` with `Input.`

</details>
<br>

<details>
<summary>When I try to run tests in C#/ Python I get the error: <strong>System.PlatformNotSupportedException : Operation is not supported on this platform.</strong>(C#) / <strong>Error: the JSON object must be str, bytes or bytearray, not NoneType</strong> and <strong>Connection to AltServer closed with status code: None and message: 'None'.</strong> (Python)</summary>
<br>

You get this error because you are using an older binding. (Eg: You use the AltTester Unity SDK v 2.1.x and a binding with v 2.0.3). You should update it to the latest version.
</details>
<br>

