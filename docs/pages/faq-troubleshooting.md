# FAQ

**Question**: On what platforms can I run tests with AltUnity tester? 

**A**: PC, Android, iOS, consoles are work in progress 


**Question**: What programming languages does AltUnity Tester support?

**A**: C#, Python and Java 


**Question**: Can AltUnity Tester be integrated with Appium?

**A**: AltUnity Tester can be used alongside Appium. Appium allows you to access the native objects and AltUnity Tester can be used to access the Unity objects.  For more info regarding how to run tests together with appium check https://altom.gitlab.io/altunity/altunitytester/pages/tester-with-appium.html 


**Question**: Is there a specific version of Unity it works with? 

**A**: AltUnityTester does not require a specific Unity Version. If you encounter any issues, we’d like to hear about them.

**Question**: Can I use AltUnity Tester to run tests using device cloud services? 

**A**: It works with some of the cloud services. We tried it with Bitbar Cloud and AWS Device Farm.  
These give you access to a virtual machine or a Docker container that has a cloud device attached, where you upload your tests, configure your environment and run your tests. More info about this here: https://altom.gitlab.io/altunity/altunitytester/pages/tester-with-cloud.html 


**Question**: Do I need access to the source code to write tests? 

**A**: For AltUnity Tester you need access to the source code. However,  we’ve recently released another tool, AltUnity Inspector, which allows you to inspect the game objects outside the unity editor without access to the source code: https://altom.com/everything-you-need-to-know-about-altunity-inspector/ 


## Troubleshooting

**Problem**: I get a “waiting for connection on port 13000” popup message, when I try to run the tests. How do I get rid of it? 

**Answer**: This message is a good thing, it tells you that the game is ready and you can start running your tests. 


**Problem**: Why do I get an error when trying to call the port forwarding methods? 

**Answer**: You need to make sure the following third party tools are installed: ADB - Android  or iproxy - iOS


**Problem**: I get a Newtonsoft error, due to duplicate Newtonsoft files. It looks like there are multiple versions in conflict. How can I solve this error? 

**Answer**:You can remove the Newtonsoft version from AltUnity tester. 
