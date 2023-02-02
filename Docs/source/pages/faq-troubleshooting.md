# FAQ

<details>
<summary> On what platforms can I run tests with AltTester Unity SDK? </summary>
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
<summary> Can AltTester Unity SDK be integrated with Appium?</summary>
<br>
Yes, AltTester Unity SDK can be used alongside Appium. Appium allows you to access the native objects and AltTester Unity SDK can be used to access the Unity objects.  For more info regarding how to run tests together with appium check <em><a href="alttester-with-appium.html">Running tests together with Appium</a></em>.
</details>
<br>

<details>
<summary> What versions of Unity does AltTester Unity SDK work with? </summary>
<br>
AltTester Unity SDK works with Unity 2020.3.0 or higher. If you encounter any issues we'd like to hear about them. You can <a href="contributing.html#did-you-find-a-bug">raise an issue</a> or join our community on <a href="https://discord.gg/Ag9RSuS">Discord</a> or <a href="https://groups.google.com/a/altom.com/g/alttesterforum">Google Groups</a>.
</details>
<br>

<details>
<summary>Can I use AltTester Unity SDK to run tests using device cloud services? </summary>
<br>
It works with some of the cloud services. We tried it with Bitbar Cloud and AWS Device Farm.
These give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. More info about this here:<em><a href=" alttester-with-cloud.html"> Running tests using device cloud services.</a></em>
</details>
<br>

<details>
<summary> Do I need access to the source code of the Unity App to write tests?</summary>
<br>
In order to run tests using AltTester Unity SDK you require an <a href="get-started.html#instrument-your-game-with-alttester-unity-sdk">instrumented build</a> of the Unity App. To create an instrumented build of the Unity App you need to <a href="get-started.html#import-alttester-package-in-unity-editor">import</a> the AltTester package in Unity Editor.
</details>
<br>

<details>
<summary> I don’t have access to source code, but I do have access to an instrumented build. How can I begin to write tests?</summary>
<br>
 We’ve published AltTester Desktop, which allows you to inspect the game objects outside the unity editor without access to the source code. More information about AltTester Desktop can be found in this <a href="https://altom.com/alttester/docs/desktop/">documentation</a>.
</details>
<br>

## Troubleshooting

<details>
<summary> I get <strong>`waiting for connection on port 13000`</strong> popup message when i start my Unity App </summary>
<br>
The popup message shows up when you start your instrumented Unity App. It tells you that the AltTester Unity SDK is ready and you can start running your tests.
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
You get this error due to multiple imports of Newtonsoft.Json.dll library. You can remove the Newtonsoft.Json version from AltTester Unity SDK by deleting the <em>Newtonsonft</em> folder <em>Assets/AltTester/3rdParty/Newtonsonft</em>.
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
<summary>How can I <strong>use the Input from AltTester Unity SDK</strong> if my project is using <strong>Assembly Definitions </strong>?</summary>
<br>
To use the Input from AltTester Unity SDK you have to:

1. Create .asmdef files in these directories (3rdParty, AltDriver, AltServer)

2. Reference other assemblies in AltServer assembly

3. Reference AltServer assembly in Project-Main-Assembly
 </details>
 <br>

<details>
<summary>I get the error: <strong>Error while running command: iproxy 13000 13000 </strong></summary>
<br>

If the inner exception is:
<br>

<em>System.ComponentModel.Win32Exception : ApplicationName='iproxy', CommandLine='13000 13000', CurrentDirectory='', Native error= Cannot find the specified file</em>
<br>

Pass the full path of iproxy to <em>AltPortForwarding.ForwardIos</em>

</details>
<br>

<details>
<summary> I downloaded the AltTester package v1.7.2 from the documentation on MacOS. I got a warning pop-up about the input system where I chose 'Yes' because I am using the New Input System. After reopening Unity Editor, <strong>AltTester Unity SDK is missing.</strong></summary>
<br>


After reopening Unity Editor, add again the AltTester package in your project.
<br>

</details>
<br>