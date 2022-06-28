# FAQ

<details>
<summary> On what platforms can I run tests with AltUnity Tester? </summary>
<br>
PC, Mac, Android, iOS and Unity Editor; support for WebGL and Consoles is work in progress.
</details>
<br>

<details>
<summary> What programming language can I use to write tests?</summary>
<br>
 C#, Python and Java.
</details>
<br>

<details>
<summary> Can AltUnity Tester be integrated with Appium?</summary>
<br>
Yes, AltUnity Tester can be used alongside Appium. Appium allows you to access the native objects and AltUnity Tester can be used to access the Unity objects.  For more info regarding how to run tests together with appium check <em><a href="tester-with-appium.html">Running tests together with Appium</a></em>.
</details>
<br>

<details>
<summary> What versions of Unity does AltUnity Tester work with? </summary>
<br>
AltUnity Tester works with Unity 2018.1 or higher. If you encounter any issues we'd like to hear about them. You can <a href="contributing.html#did-you-find-a-bug">raise an issue</a> or join our community on <a href="https://discord.gg/Ag9RSuS">Discord</a> or <a href="https://groups.google.com/a/altom.com/forum/#!forum/altunityforum">Google Groups</a>.
</details>
<br>

<details>
<summary>Can I use AltUnity Tester to run tests using device cloud services? </summary>
<br>
It works with some of the cloud services. We tried it with Bitbar Cloud and AWS Device Farm.
These give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. More info about this here:<em><a href=" tester-with-cloud.html"> Running tests using device cloud services.</a></em>
</details>
<br>

<details>
<summary> Do I need access to the source code of the Unity App to write tests?</summary>
<br>
In order to run tests using AltUnity Tester you require an <a href="get-started.html#instrument-your-game-with-altunity-server">instrumented build</a> of the Unity App. To create an instrumented build of the Unity App you need to <a href="get-started.html#import-altunity-tester-package-in-unity-editor">import</a> the AltUnity Tester package in Unity Editor.
</details>
<br>

<details>
<summary> I don’t have access to source code, but I do have access to an instrumented build. How can I begin to write tests?</summary>
<br>
 We’ve published AltUnity Inspector, which allows you to inspect the game objects outside the unity editor without access to the source code. More information about AltUnity Inspector can be found in this <a href="https://altom.com/everything-you-need-to-know-about-altunity-inspector/">article</a>.
</details>
<br>

## Troubleshooting

<details>
<summary> I get <strong>`waiting for connection on port 13000`</strong> popup message when i start my Unity App </summary>
<br>
The popup message shows up when you start your instrumented Unity App. It tells you that the AltUnity Tester is ready and you can start running your tests.
</details>
<br>

<details>
<summary> Why do I get an <strong>error when trying to call the port forwarding </strong>methods? </summary>
<br>
You need to make sure the following third party tools are installed: ADB - Android  or iproxy - iOS. For more information you can check our <a href="advanced-usage.html#how-to-setup-port-forwarding">setup port forwarding guide</a>.
</details>
<br>

<details>
<summary>I get the error: <strong>Multiple precompiled assemblies with the same name Newtonsoft.Json.dll included or the current platform.</strong> </summary>
<br>
You get this error due to multiple imports of Newtonsoft.Json.dll library. You can remove the Newtonsoft.Json version from AltUnity Tester by deleting the <em>Newtonsonft</em> folder <em>Assets/AltUnityTester/ThirdParty/Newtonsonft</em>.
</details>
<br>

<details>
<summary> I get the error: <strong>The type or namespace name 'Newtonsoft' could not be found (are you missing a using directive or an assembly reference?)</strong>,  </summary>
<br>
You get this error because you don't have a reference to Newtonsoft.Json package.
<br>
Add `"com.unity.nuget.newtonsoft-json": "3.0.1"` to your project `manifest.json`, inside `dependencies`.

```
{
    "dependencies": {
        "com.unity.nuget.newtonsoft-json": "3.0.1"
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
<summary>How can I <strong>use the Input from AltUnity Tester</strong> if my project is using <strong>Assembly Definitions </strong>?</summary>
<br>
To use the Input from AltUnity Tester you have to:

1. Create .asmdef files in these directories (3rdParty, AltUnityDriver, AltUnityServer)

2. Reference other assemblies in AltUnityServer assembly

3. Reference AltUnityServer assembly in Project-Main-Assembly
 </details>
 <br>

<details>
<summary>I get the error: <strong>Error while running command: iproxy 13000 13000 </strong></summary>
<br>

If the inner exception is:
<br>

<em>System.ComponentModel.Win32Exception : ApplicationName='iproxy', CommandLine='13000 13000', CurrentDirectory='', Native error= Cannot find the specified file</em>
<br>

Pass the full path of iproxy to <em>AltUnityPortForwarding.ForwardIos</em>

</details>
<br>

<details>
<summary> I downloaded the AltUnity Tester package v1.7.1 from the documentation on MacOS. I got a warning pop-up about the input system where I chose 'Yes' because I am using the New Input System. After reopening Unity Editor, <strong>AltUnity Tester is missing.</strong></summary>
<br>


After reopening Unity Editor, add again the AltUnity Tester Asset in your project.
<br>

</details>
<br>