# Overview 

AltUnity Tester is an open-source UI driven test automation tool that helps you find objects in your game and interacts with them using tests written in C#, Python or Java. You can run your tests on real devices (mobile, PCs, etc.) or inside the Unity Editor. 

## Key features 

- find elements and get all their (public) properties: coordinates, text, values, Unity components, etc. 
- use and modify any of the (public) methods and properties that a Unity element has
- simulate any kind of device input  
- manipulate and generate test data 
- get screenshots from your Unity Game 
- instrument your game and run C# tests from within the Unity Editor using the AltUnityTester window
- run Python or Java tests using your favorite IDE and run them against - the game running on a device or inside the Unity Editor
- integrate with Appium tests for the ability to interact with native elements
- visualize input actions during test execution 
- see test results and reports inside the Unity Editor


## How it works 

The AltUnityTester package contains the AltUnityDriver module that opens a TCP socket connection on the device running the Unity application and gives access to all the objects in the Unity hierarchy.

Using this TCP socket connection and the actions available in the AltUnity driver, we can run tests against the Unity app. 
