# Frequently Asked Questions(FAQ)


<details>
<summary><strong> On what platforms can I run tests with AltUnity tester? </strong></summary>
<br>
<strong>Answer:</strong> PC, Android, iOS, consoles are work in progress 
</details>
<br>

<details>
<summary><strong>What programming languages does AltUnity Tester support?</strong></summary>
<br>
<strong>Answer:</strong> C#, Python and Java  
</details>
<br>

<details>
<summary><strong>Can AltUnity Tester be integrated with Appium?</strong></summary>
<br>
<strong>Answer:</strong> AltUnity Tester can be used alongside Appium. Appium allows you to access the native objects and AltUnity Tester can be used to access the Unity objects.  For more info regarding how to run tests together with appium check https://altom.gitlab.io/altunity/altunitytester/pages/tester-with-appium.html 
</details>
<br>

<details>
<summary><strong>Is there a specific version of Unity it works with?</strong></summary>
<br>
<strong>Answer:</strong> AltUnityTester does not require a specific Unity Version. If you encounter any issues, we’d like to hear about them. 
</details>
<br>

<details>
<summary><strong>Can I use AltUnity Tester to run tests using device cloud services?</strong> </summary>
<br>
<strong>Answer:</strong> It works with some of the cloud services. We tried it with Bitbar Cloud and AWS Device Farm.  
These give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. More info about this here: https://altom.gitlab.io/altunity/altunitytester/pages/tester-with-cloud.html 
</details>
<br>

<details>
<summary><strong>Do I need access to the source code to write tests?</strong></summary>
<br>
<strong>Answer:</strong> For AltUnity Tester you need access to the source code. However,  we’ve recently released another tool, AltUnity Inspector, which allows you to inspect the game objects outside the unity editor without access to the source code: https://altom.com/everything-you-need-to-know-about-altunity-inspector/ 
</details>
<br>

<details>
<summary><strong>How can I use the Input from AltUnity Tester if my project is using Assembly Definitions? </strong></summary>
<br>
<strong>Answer:</strong>In order to fix this issue you have to:

1. Create .asmdef files in these directories (3rdParty, AltUnityDriver, AltUnityServer)

2. Reference other asemblies in AltUnityServer assembly

3. Reference AltUnityServer assembly in Project-Main-Assembly
</details>
<br>


## Troubleshooting

<details>
<summary><strong>I get a “waiting for connection on port 13000” popup message, when I try to run the tests. How do I get rid of it? </strong></summary>
<br>
<strong>Answer:</strong> This message is a good thing, it tells you that the game is ready and you can start running your tests.  
</details>
<br>

<details>
<summary><strong>Why do I get an error when trying to call the port forwarding methods? </strong></summary>
<br>
<strong>Answer:</strong> You need to make sure the following third party tools are installed: ADB - Android  or iproxy - iOS
</details>
<br>

<details>
<summary><strong>I get a Newtonsoft error, due to duplicate Newtonsoft files. It looks like there are multiple versions in conflict. How can I solve this error?</strong> </summary>
<br>
<strong>Answer:</strong> You can remove the Newtonsoft version from AltUnity tester.
</details>
<br>