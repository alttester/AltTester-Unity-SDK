# Overview 

AltUnity Tester is an open-source UI driven test automation tool that helps you find objects in your Unity game and interact with them using tests written in C#, Python or Java.  
You can run your tests on real devices (mobile, PCs, etc.) or inside the Unity Editor. 

## Key features 

- find elements and get all their (public) properties: coordinates, text, values, Unity components, etc. 
- use and modify any of the (public) methods and properties of a Unity element
- simulate any kind of device input  
- manipulate and generate test data 
- get screenshots from your Unity Game 
- instrument your game and run C# tests from within the Unity Editor using the AltUnity Tester window
- run C#, Python or Java tests using your favourite IDE and against the game running on a device or inside the Unity Editor
- integrate with Appium tests for the ability to interact with native elements
- visualize input actions during test execution 
- see test results and reports inside the Unity Editor


## How it works 

AltUnity Tester package provides build capabilities for your unity project. A Unity project instrumented with AltUnity Tester exposes access to all the objects in the Unity hierarchy. Tests can access the Unity objects via the AltUnityDriver module with bindings available in C#, Java and Python. 

The AltUnity Tester build opens up a TCP socket connection on the device running the Unity application and waits for a client to connect before starting the application. In the tests, AltUnityDriver connects to the TCP socket and sends commands to interact with the Unity App.
