============
Known Issues
============

This section lists the known bugs and issues with the AltUnity Tester. If
available, we list a workaround to help troubleshoot the issue.

To report a bug that isn’t listed here, see our :doc:`contributing` section
to learn how to best report the issue.



1. Calling ``GetPNGScreenshot`` throws ``StackOverflow`` error (.NET Driver)

**Problem**: For high resolutions calling ``GetPNGScreenshot`` might throw a
``StackOverflow`` error.

**Workaround**: The issue only happens with .NET 6 as a workaround you can use
.NET 5, or if you can't downgrade to .NET 5, try to run your tests with a lower
resolution until this issue is fixed.

**Affects**: AltUnity Tester v1.7.0 with .NET 6


2. New Input System

**Problem**: Inputs are not executed in the UnityEditor

**Workaround**: Focus the game view after starting the tests.

**Affects**: AltUnityTester v1.7.1 in UnityEditor with Input System v1.0.2 or earlier


3. BlueStacks

**Problem**: Instrumented game with AltUnity Tester stops working in BlueStacks

**Workaround**: The issue seems to happen because of the communication protocol 
used inside the AltUnity Tester. There are other alternatives that work with instrumented builds. 
For Android you can use the Android Emulator inside the Android Studio. 
For iOS you can use a simulator inside Xcode.

**Affects**: AltUnity Tester v1.7.0 and v1.7.1
