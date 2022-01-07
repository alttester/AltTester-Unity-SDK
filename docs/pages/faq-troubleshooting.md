# FAQ

<details>
<summary><strong> On what platforms can I run tests with AltUnity Tester? </strong></summary>
<br>
<strong>Answer:</strong> PC, Mac, Android, iOS and Unity Editor; support for WebGL and Consoles is work in progress. 
</details>
<br>

<details>
<summary><strong> What programming language can I use to write tests?</strong></summary>
<br>
<strong>Answer:</strong> C#, Python and Java.
</details>
<br>

<details>
<summary><strong> Can AltUnity Tester be integrated with Appium?</strong></summary>
<br>
<strong>Answer:</strong> Yes, AltUnity Tester can be used alongside Appium. Appium allows you to access the native objects and AltUnity Tester can be used to access the Unity objects.  For more info regarding how to run tests together with appium check <em><a href="tester-with-appium.html">Running tests together with Appium</a></em>.
</details>
<br>

<details>
<summary><strong> What versions of Unity does AltUnity Tester work with? </strong></summary>
<br>
<strong>Answer:</strong> AltUnity Tester works with Unity 2018.1 or higher. If you encounter any issues we'd like to hear about them. You can <a href="contributing.html#did-you-find-a-bug">raise an issue</a> or join our community on <a href="https://discord.gg/Ag9RSuS">Discord</a> or <a href="https://groups.google.com/a/altom.com/forum/#!forum/altunityforum">Google Groups</a>.
</details>
<br>

<details>
<summary><strong> Can I use AltUnity Tester to run tests using device cloud services?</strong> </summary>
<br>
<strong>Answer:</strong> It works with some of the cloud services. We tried it with Bitbar Cloud and AWS Device Farm.  
These give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. More info about this here:<em><a href=" tester-with-cloud.html"> Running tests using device cloud services.</a></em>
</details>
<br>

<details>
<summary><strong> Do I need access to the source code of the Unity App to write tests?</strong></summary>
<br>
<strong>Answer:</strong> In order to run tests using AltUnity Tester you require an <a href="get-started.html#instrument-your-game-with-altunity-server">instrumented build</a> of the Unity App. To create an instrumented build of the Unity App you need to <a href="get-started.html#import-altunity-tester-package-in-unity-editor">import</a> the AltUnity Tester package in Unity Editor. 
</details>
<br>

<details>
<summary><strong> I don’t have access to source code, but I do have access to an instrumented build. How can I begin to write tests?</strong></summary>
<br>
<strong>Answer:</strong> We’ve published AltUnity Inspector, which allows you to inspect the game objects outside the unity editor without access to the source code. More information about AltUnity Inspector can be found in this <a href="https://altom.com/everything-you-need-to-know-about-altunity-inspector/">article</a>.
</details>
<br>

## Troubleshooting

<details>
<summary><strong> I get a “waiting for connection on port 13000” popup message, when I try to run the tests. How do I get rid of it? </strong></summary>
<br>
<strong>Answer:</strong> This message is a good thing, it tells you that the game is ready and you can start running your tests.  
</details>
<br>

<details>
<summary><strong> Why do I get an error when trying to call the port forwarding methods? </strong></summary>
<br>
<strong>Answer:</strong> You need to make sure the following third party tools are installed: ADB - Android  or iproxy - iOS. For more information you can check our <a href="advanced-usage.html#how-to-setup-port-forwarding">setup port forwarding guide</a>.
</details>
<br>

<details>
<summary><strong> I get the error: <em>Multiple precompiled assemblies with the same name Newtonsoft.Json.dll included or the current platform.</em>, </strong> </summary>
<br>
<strong>Answer:</strong> You get this error due to multiple imports of Newtonsoft.Json.dll library. You can remove the Newtonsoft.Json version from AltUnity Tester by deleting the <em>JsonDotNet</em> folder <em>Assets/AltUnityTester/ThirdParty/JsonDotNet</em>. 
</details>
<br>

<details>
<summary><strong> How can I use the Input from AltUnity Tester if my project is using Assembly Definitions? </strong></summary>
<br>
<strong>Answer:</strong> In order to fix this issue you have to:

1. Create .asmdef files in these directories (3rdParty, AltUnityDriver, AltUnityServer)

2. Reference other asemblies in AltUnityServer assembly

3. Reference AltUnityServer assembly in Project-Main-Assembly
 </details>
 <br>

<details>
<summary><strong> I get the error: <em>Error while running command: iproxy 13000 13000</em> </strong></summary>
<br>
<strong>Answer:</strong> 
If the inner exception is:
<br>

<em>System.ComponentModel.Win32Exception : ApplicationName='iproxy', CommandLine='13000 13000', CurrentDirectory='', Native error= Cannot find the specified file</em>
<br>

Pass the full path of iproxy to <em>AltUnityPortForwarding.ForwardIos</em>

</details>
<br>
