============
Known Issues
============

This section lists the known bugs and issues with the AltTester® Unity SDK. If
available, we list a workaround to help troubleshoot the issue.

To report a bug that isn't listed here, see our :doc:`contributing` section
to learn how to best report the issue.


.. contents:: Table of Contents
    :local:
    :depth: 2
    :backlinks: none


Driver
------

Calling ``GetPNGScreenshot`` throws ``StackOverflow`` error (.NET Driver)
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Problem**: For high resolutions calling ``GetPNGScreenshot`` might throw a
``StackOverflow`` error.

**Workaround**: The issue only happens with .NET 6. As a workaround you can use
.NET 5, or if you can't downgrade to .NET 5, try to run your tests with a lower
resolution until this issue is fixed.

**Affects**: AltUnity Tester v1.7.* and AltTester® Unity SDK v1.8.* with .NET 6

New Input System
----------------

Touch or Mouse actions do not work in tests
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Workaround**: In Analysis -> Input Debugger -> Options make sure the setting
Simulate Touch Input From Mouse or Pen is not checked and Lock Input to
Game View is checked.

**Affects**: AltUnity Tester v1.7.1, v1.7.2, AltTester® Unity SDK v1.8.*, v2.0.0 and Input System with a version below 1.3.0

The PressKey command does not work
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Workaround**: This issue might happen if your code uses the methods
``wasPressedThisFrame`` and ``wasReleasedThisFrame*``. Use ``isPressed``
instead.

**Affects**: AltUnity Tester v1.7.1, v1.7.2, AltTester® Unity SDK v1.8.*, v2.0.x, v2.x

Player Input is not working when connected to AltTester® Unity SDK/Desktop
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Problem**: Player Input is not working in the instrumented build when
connected to the tests or to AltTester® Desktop.

**Affects**: All input actions created with the New Input System for app
objects. The New Input System actions for UI objects are not affected, as well
as the Old Input System actions.

AltTester® Editor
-----------------

Playing in Editor throws ``EntryPointNotFoundException: WebSocketSetOnOpen assembly:<unknown assembly> type:<unknown type> member:(null)``
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Problem**: The WebGL platform was previously selected (check Project Settings) and when playing in Editor the WebSocket is trying to compile the WebGL script

**Workaround**: Select a platform other than WebGL before Editor and if the error is still thrown while playing in Editor re-start de Unity project

**Affects**: AltTester® Unity SDK v2.0.x, v2.x

Play in Editor does not start the instrumented app in the Unity Editor
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Problem**: If ``Keep ALTTESTER symbol defined`` is not checked, Play in Editor will not start the instrumented app in the Unity Editor.

**Workaround**: Check the ``Keep ALTTESTER symbol defined`` box. Note that this problem does not occur when building an instrumented app for different platforms, only in the Editor.

**Affects**: AltTester® Unity SDK v1.8.*, AltTester® Unity SDK v2.0.0

Opening AltTester® Editor throws ``IndexOutOfRangeException: Index was outside the bounds of the array``
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Problem**: Sometimes when opening the AltTester® Editor window ``IndexOutOfRangeException: Index was outside the bounds of the array`` is thrown.

**Workaround**: Close and reopen the AltTester® Editor window.

**Affects**: AltTester® Unity SDK v2.0.0, v2.0.1

BlueStacks
----------

Instrumented app with AltTester® Unity SDK stops working in BlueStacks
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Workaround**: The issue seems to happen because of the communication protocol
used inside the AltTester® Unity SDK. There are other alternatives that work with
instrumented builds. For Android you can use the Android Emulator inside the
Android Studio. For iOS you can use a simulator inside Xcode.

**Affects**: AltUnity Tester v1.7.*.

Note: In version 2.0.2 this issue has been fixed.

Networking & Connectivity
-------------------------

AltTester® does not support proxy configurations
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

**Problem**: At the moment, AltTester® does not have built-in support for proxy configurations. When using AltTester®, the tool does not handle proxy settings, which can cause connectivity issues

**Affects**: AltUnity Tester v1.7.1, v1.7.2, AltTester® Unity SDK v1.8.*, v2.0.x, v2.x

WegGL app disconnects
~~~~~~~~~~~~~~~~~~~~~

**Problem** The WebGL application disconnects due to WebSocket timeouts occurring over time when no data is transmitted.

**Affects**: AltTester® Unity SDK v2.x

Impossibility to connect to AltTester® Desktop an ``IL2CPP`` instrumented app built with Managed Stripping Level higher than ``Minimal`` which throws ``InvalidCommandException: Unable to find a constructor to use for type AltTester.AltTesterUnitySDK.Driver.Commands.AltGetServerVersionParams. A class should either have a default constructor, one constructor with arguments or a constructor marked with the JsonConstructor attribute``
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
**Workaround**: Set the Managed Stripping Level setting to ``Minimal`` from Player Settings -> Other Settings -> Optimization 

**Affects**: AltTester® Unity SDK v2.x