# Overview

AltTester® Unity SDK is part of the AltTester® test automation framework for games.  Combined with the AltTester® Desktop, AltTester® Unity SDK helps you find objects in your Unity application and interact with them using tests written in C#, Python, Java or Robot Framework.

You can run your tests on real devices (mobile, PCs, etc.) or inside the Unity Editor.

## How it works

AltTester® framework contains the following components:

* AltTester® Unity SDK (illustrated inside the game / app on the left below)
* AltTester® Desktop (illustrated in the middle)
* AltTester® Bindings / Clients (for C#, Python, Java, Robot Framework, illustrated on the right)

```eval_rst
        .. figure:: ../_static/img/overview/architecture1.svg
            :scale: 150 %

```

* **AltTester® Unity SDK**

    This is a Unity plugin used to instrument your Unity game / app to expose access to all the objects in the Unity hierarchy. The AltTester® Unity SDK starts a websocket client connection inside the game / app that communicates with the AltTester® Server running within AltTester® Desktop app. 

* **AltTester® Desktop** 
    This is a desktop application for Mac, Windows and Linux that contains the following:

    * **AltTester® Server** - a WebSocket server that facilitates the communication between AltTester® Unity SDK within the game / app and the automated scripts controlling the game / app. 

    * **AltTester® Inspector and Recorder** - tools that help you create automated tests by recording your actions within the game / app and having them automatically transformed into test automation scripts.

* **AltTester® Bindings / Clients (for C#, Python, Java, Robot Framework)**
    These are packages used to write automated tests in your preferred scripting language. They give you access to the API described in this documentation that enables you to control the instrumented Unity game / app programmatically. The bindings / clients open a websocket client connection that communicates with the AltTester® Server running within the AltTester® Desktop app. 

    The AltDriver module inside each of the clients / bindings, similar to Appium Driver for mobile apps or Selenium WebDriver for web apps, is used to connect to the instrumented Unity game / app, access all the game objects and interact with them through tests written in C#, Python, Java and Robot Framework.

![Architecture](../_static/img/overview/architecture2.png)

* **AltTester® Server in the Cloud** - (COMING SOON) is a cloud implementation of the AltTester® Server that will allow you to write and execute tests without needing an instance of the AltTester® Desktop running locally, thus simplifying both local development environments and CI setups. 

## Key features

- find elements and get all their (public) properties: coordinates, text, values, Unity components, etc.
- use and modify any of the (public) methods and properties of a Unity element
- simulate any kind of device input (support for Input Manager and Input System)
- manipulate and generate test data
- get screenshots from your Unity App
- instrument your app and run C# tests from within the Unity Editor using the AltTester® Editor window
- run C#, Python, Java or Robot Framework tests using your favorite IDE and against the app running on a device or inside the Unity Editor
- integrate with Appium tests for the ability to interact with native elements
- support for Browserstack on Android and iOS
- see test results and reports inside the Unity Editor
- generate XML test report from the Editor Window
- run tests concurrently on different devices
