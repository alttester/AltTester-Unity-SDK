# Overview

AltTester® Unity SDK is part of the AltTester® test automation framework for games.  Combined with the AltTester® Desktop, AltTester® Unity SDK helps you find objects in your Unity application and interact with them using tests written in C#, Python, Java or Robot Framework.

You can run your tests on real devices (mobile, PCs, etc.) or inside the Unity Editor.

## How it works

AltTester® works by assembling three pieces — the SDK in your build, the
AltTester® Server, and the driver in your tests. See
[How it works](how-it-works.md) for what each one is, a diagram of how they fit together, and
why their versions have to agree.

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
