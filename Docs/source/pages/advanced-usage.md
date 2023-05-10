# Advanced Usage

This guide covers some of the more advanced features, patterns and
configuration options of AltTester Unity SDK.

## AltTester input

AtTester Unity SDK has an `Input` class which overrides the Input class implemented by Unity. This way AltTester intercepts the input actions to be performed in the instrumented app and simulates them through this class.
In case you are using assembly definitions inside your project, you will have to reference the `AltTesterUnitySDK.asmdef` in all your .asmdef files which use input actions.

## AltTester input vs. regular input

AltTester's custom input is active, by default, in any instrumented build. This means that certain input related actions (the ones that are part of Unity's `Input` class) will be inactive for regular input (the device's input). Because of this, pressing a key from the keyboard for example will not have any effect on the app. However, the simulated input from the tests, like the `PressKey` command, will be able to manipulate the object within the scene. While the AltTester input is active, the icon from the right bottom corner is green. You can change this behaviour by clicking on the AltTester's icon and unchecking the box with the `AltTester Input` message. Now the icon will turn darker, signaling that the regular input is active. In this state, you can interfere with the object from the app using the keyboard or other input. Keep in mind that, input actions from the AltTester Desktop won't have any effect while regular input is active. At the same time, if you want to run some automated tests, the AltTester input will be activated automatically for you.

## Build apps from the command line

To build your Unity application from command line you need a static method in
your project that handles the build logic. To instrument your Unity application
with AltTester Unity SDK, your build method must define `ALTTESTER` scripting
symbol and must insert AltTester Prefab in the first scene of the app.

Depending on your project's setup, there are two ways in which apps can be
built from the command line:


```eval_rst
.. note::

    AltTester Unity SDK does not work by default in release mode. If you instrument
    your app in release mode, AltTester Prefab self removes from the scenes and
    the socket server does not start. Best case practice is to customize your
    build script to insert AltTester Prefab only in Debug mode.

    If you do want to use AltTester Unity SDK in release mode see
    `Using AltTester Unity SDK in Release mode section <#using-alttester-unity-sdk-in-release-mode>`_.

```


**1. If you already have a custom build method for your app**

If you already have a custom build method for your app, you can add the
following lines to your build method. Also, the BuildPlayerOptions should
check for *BuildOptions.Development* and *BuildOptions.IncludeTestAssemblies*.

```c#
var buildTargetGroup = BuildTargetGroup.Android;
AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(buildTargetGroup);
if (buildTargetGroup == UnityEditor.BuildTargetGroup.Standalone) {
    AltBuilder.CreateJsonFileForInputMappingOfAxis();
}
var instrumentationSettings = new AltInstrumentationSettings();
AltBuilder.InsertAltInScene(FirstSceneOfTheApp, instrumentationSettings);
```

```eval_rst
.. note::

    Change ``buildTargetGroup`` above to the target group for which you are
    building.

```


**2. If you create a new custom build method for your app**

The following example script can be used. It sets all the project settings
needed and uses the same two important lines from point 1 above.

This example method is configured for the Android platform, so make sure to
update it based on your target platform.

```eval_rst
.. include:: ../_static/examples~/advanced-usage/build-from-command-line.txt
    :code: c#

```

The following command is used to call the build method:

```eval_rst
.. code-block:: bash

    <UnityPath>/Unity -projectPath $CI_PROJECT_DIR -executeMethod BuilderClass.BuildFromCommandLine -logFile logFile.log -quit

```

You can find more information about the build command and arguments
[here](https://docs.unity3d.com/Manual/CommandLineArguments.html).

```eval_rst
.. note::

    After building from the command line you can run the tests by using the
    commands from the `next section <#run-tests-from-the-command-line>`_.

```

## How to make a production build

There is no need to remove the AltTester package entirely from the project, only the `ALTTESTER` Scripting Define Symbol should be deleted from the Player Settings. Also, make sure that the `Keep ALTTESTER symbol defined` checkbox is unchecked. After that, you can build your app normally as you would do in Unity.

## Run tests from the command line

In order to run tests from the command line you can use the following example
commands:

```eval_rst
.. tabs::

    .. tab:: C#

        Available AltTester SDK command line arguments:

        ``-testsClass`` - runs tests from given class/classes

        Example command running tests from a single test class:

        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltTester.AltTesterUnitySDK.Editor.AltTestRunner.RunTestFromCommandLine -testsClass MyTestClass -logFile logFile.log -batchmode -quit

        Example command running tests from two test classes:

        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltTester.AltTesterUnitySDK.Editor.AltTestRunner.RunTestFromCommandLine -testsClass MyTestClass1 MyTestClass2 -logFile logFile.log -batchmode -quit

        ``-tests`` - runs given test/tests

        Example command running a single test:

        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltTester.AltTesterUnitySDK.Editor.AltTestRunner.RunTestFromCommandLine -tests MyTestClass.MyTestName -logFile logFile.log -batchmode -quit

        Example command running two tests:

        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltTester.AltTesterUnitySDK.Editor.AltTestRunner.RunTestFromCommandLine -tests MyTestClass1.MyTestName1 MyTestClass2.MyTestName2 -logFile logFile.log -batchmode -quit

        ``-testsAssembly`` - runs tests from given assembly/assemblies

        Example command running all tests from given assembly:

        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltTester.AltTesterUnitySDK.Editor.AltTestRunner.RunTestFromCommandLine -testsAssembly MyAssembly -logFile logFile.log -batchmode -quit

        Example command running tests from two assemblies:

        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltTester.AltTesterUnitySDK.Editor.AltTestRunner.RunTestFromCommandLine -testsAssembly MyAssembly1 MyAssembly2 -logFile logFile.log -batchmode -quit

        ``-reportPath`` - the xml test report will be generated here
        
        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltTester.AltTesterUnitySDK.Editor.AltTestRunner.RunTestFromCommandLine -tests MyFirstTest.TestStartGame -reportPath $PROJECT_DIR/testReport.xml -logFile logFile.log -batchmode -quit

    .. tab:: Java

        .. code-block:: bash

            mvn test

    .. tab:: Python

        Using the ``unittest`` module:

        .. code-block:: bash

            python -m unittest <name_of_your_test_file.py>

        Using the ``pytest`` package:

        .. code-block:: bash

            pytest  <name_of_your_test_file.py>

```

## Run tests on a Continuous Integration Server

1. Instrument your app build with AltTester Unity SDK from Unity or by [building from the command line](#build-apps-from-the-command-line).
2. Start the app build on a device.
3. Run your tests - see commands in the ["Run tests from the command line" section](#run-tests-from-the-command-line).

An example CI configuration file can be viewed in the [GitLab repository](https://gitlab.com/altom/altunity/altunitytester/-/blob/master/.gitlab-ci.yml).


## What is reverse port forwarding and when to use it

Reverse port forwarding, is the behind-the-scenes process of intercepting
data traffic and redirecting it from a device's IP and/or port to the computer's IP and/or port.

When you run your app instrumented with AltTester Unity SDK on a device, you need
to tell your build how to connect to the AltServer.

Reverse port forwarding can be set up either through the command line or in the
test code by using the methods available in the AltTester SDK classes.

The following are some cases when reverse port forwarded is needed:

1. [Connect to the app running on a USB connected device](#connect-to-the-app-running-on-a-usb-connected-device)
2. [Connect to multiple devices running the app](#connect-to-multiple-devices-running-the-app)

### How to setup reverse port forwarding

#### In case of Android

Reverse port forwarding can be set up in two ways:

- through the command line using ADB
- in the test code by using the methods available in the AltTester SDK classes

All methods listed above require that you have ADB installed.

For further information including how to install ADB, check [this article](https://developer.android.com/studio/command-line/adb).

#### In case of iOS

Unfortunately, IProxy does not have a way of setting up reverse port forwarding. As a workaround, you should follow the steps below:
- set the iPhone as a personal hotspot
- add the IP of the local machine to the first input field in the green popup from the instrumented app

In the routing table, the personal hotspot network would be secondary, therefore the traffic shouldn't be redirected through the hotspot:

```eval_rst
    .. image:: ../_static/img/advanced-usage/workaround_iOS.png
```

```eval_rst
.. tabs::

    .. tab:: Command Line

        .. tabs::

            .. tab:: Android

                - Reverse port forwarding using the following command::

                    adb [-s UDID] reverse tcp:device_port tcp:local_port.

            .. tab:: iOS

                - Not available. A workaround is described above.

    .. tab:: C#

        .. tabs::

            .. tab:: Android

                Use the following static methods from the **AltReversePortForwarding** class in your test file:

                    - **ReversePortForwardingAndroid** (int remotePort = 13000, int localPort = 13000, string deviceId = "", string adbPath = "")
                    - **RemoveReversePortForwardingAndroid** (int remotePort = 13000, string deviceId = "", string adbPath = "")

                Example test file:

                    .. include:: ../_static/examples~/common/csharp-android-test.cs
                        :code: c#

            .. tab:: iOS

                Not available. A workaround is described above.

    .. tab:: Java

        .. tabs::

            .. tab:: Android

                Use the following static methods from the **AltReversePortForwarding** class in your test file:

                    - **reversePortForwardingAndroid** (int remotePort = 13000, int localPort = 13000, string deviceId = "", string adbPath = "")
                    - **removeReverseForwardingAndroid** (int remotePort = 13000, string deviceId = "", string adbPath = "")

                Example test file:

                    .. include:: ../_static/examples~/common/java-android-test.java
                        :code: java

            .. tab:: iOS

                Not available. A workaround is described above.

    .. tab:: Python

        .. tabs::

            .. tab:: Android

                Use the following static methods from the **AltPortForwarding** class in your test file:

                    - **reverse_port_forwarding_android** (device_port = 13000, local_port = 13000, device_id = "")
                    - **remove_reverse_port_forwarding_android** (device_port = 13000, device_id = "")

                Example test file:

                    .. include:: ../_static/examples~/common/python-android-test.py
                        :code: py

            .. tab:: iOS

                Not available. A workaround is described above.

```

```eval_rst
.. note::
    The default port on which the AltTester Unity SDK is running is 13000.
    The port can be changed from the green popup. Make sure to press `Restart` after modifying its value.
```

## Connect the AltTester Unity SDK running inside the app to the AltServer

There are multiple scenarios on how to connect to the AltTester Unity SDK running inside a app:

  - [Establish connection when the instrumented app and the test code are running on the same machine](#establish-connection-when-the-instrumented-app-and-the-test-code-are-running-on-the-same-machine)
  - [Establish connection when the app is running on a device connected via USB](#establish-connection-when-the-app-is-running-on-a-device-connected-via-usb)
  - [Establish connection via IP when the app is running on a device](#establish-connection-via-ip-when-the-app-is-running-on-a-device)
  - [Establish connection when different instances of the same app are running on multiple devices](#establish-connection-when-different-instances-of-the-same-app-are-running-on-multiple-devices)
  - [Establish connection when multiple instances of the same application are running on the same device](#establish-connection-when-multiple-instances-of-the-same-application-are-running-on-the-same-device)

### Establish connection when the instrumented app and the test code are running on the same machine

![reverse port forwarding case 1](../_static/img/advanced-usage/case1.png)

In this case **reverse port forwarding** is not needed as both the app and tests are using localhost (127.0.0.1) connection and the default 13000 port.

### Establish connection when the app is running on a device connected via USB

In this case you need to use **reverse port forwarding** to direct the data traffic from the device's port to the computer's port. Data transmission happens via localhost.

![reverse port forwarding case 2](../_static/img/advanced-usage/case2.png)

Check [Reverse Port Forwarding](#what-is-reverse-port-forwarding-and-when-to-use-it) for more details about **reverse port forwarding** and [Setup Reverse Port Forwarding](#how-to-setup-reverse-port-forwarding) section on how to make the setup.

### Establish connection via IP when the app is running on a device

![reverse port forwarding case 3](../_static/img/advanced-usage/case3.png)

You can connect directly through an IP address if the port the instrumented Unity App is listening on is available and the IP address is reachable.
It is recommended to use [Reverse Port Forwarding](#what-is-reverse-port-forwarding-and-when-to-use-it) since IP addresses could change and would need to be updated more frequently.

The following command can be used to connect the running instrumented Unity App to the AltServer:

```eval_rst
.. tabs::
    .. code-tab:: c#

            altDriver = new AltDriver ("deviceIp", 13000);

    .. code-tab:: java

            altDriver = new AltDriver ("deviceIp", 13000, true);

    .. code-tab:: py

            cls.altDriver = AltDriver(host='deviceIp', port=13000)
```

### Establish connection when different instances of the same app are running on multiple devices

![reverse port forwarding case 4](../_static/img/advanced-usage/case4.png)

For two devices you have to do the same steps as above, by [connecting through reverse port forwarding](#how-to-setup-reverse-port-forwarding) twice.

So, in the end, you will have:

-   2 devices, each with one instrumented Unity App
-   1 computer with two AltDrivers

Then, in your tests, you will send commands from each of the two AltDrivers.

The same happens with n devices, repeat the steps n times.

### Establish connection when multiple instances of the same application are running on the same device

If you want to run two builds on the same device you will need to use a different port for each instance.

For example, you will instrument an app with AltTester Unity SDK to listen on port 13001 and another one to listen on port 13002.

![reverse port forwarding case 5](../_static/img/advanced-usage/case5.png)

Then in your tests you will need to create two AltDriver instances, one for each of the configured ports.

```eval_rst
.. important::

    On mobile devices, AltDriver can only interact with a single app at a time and the app needs to be in focus.

    On Android/iOS only one application is in focus at a time so you need to switch (in code) between the applications if using two drivers at the same time.
    This applies even when using split screen mode.

```

You can set the port for your app when building from the AltTester Editor window or later from the second input field inside the green popup.

![Alt Editor Server Settings Screenshot](../_static/img/advanced-usage/server-settings.png)

```eval_rst

.. note::
    After you have done the AltTester Unity SDK reverse port forwarding or connected to the AltDriver directly, you can use it in your tests to send commands to the server and receive information from the app.

```

## Using AltTester Unity SDK in Release mode

By default AltTester Unity SDK does not run in release mode. We recommended that you do not instrument your Unity application in release mode with AltTester Unity SDK. That being said, if you do want to instrument your application in release mode, you need to uncheck `RunOnlyInDebugMode` flag on AltRunnerPrefab inside AltTester Unity SDK asset folder `AltTester/Prefab/AltRunnerPrefab.prefab`

## Logging

There are two types of logging that can be configured in AltTester Unity SDK. The logs from AltDriver (from the tests) and the logs from the AltTester Unity SDK (from the instrumented Unity application)

```eval_rst
.. note::

    From version 1.7.0 on logs from `Server` are referred to as logs from `Tester`.

```

### AltTester Unity SDK logging

Logging inside the instrumented Unity application is handled using a custom NLog LogFactory. The Server LogFactory can be accessed here: `AltTester.AltTesterUnitySDK.Logging.ServerLogManager.Instance`

There are two logger targets that you can configure on the server:

-   FileLogger
-   UnityLogger

Logging inside the instrumented app can be configured from the driver using the SetServerLogging command:

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Off);
        altDriver.SetServerLogging(AltLogger.Unity, AltLogLevel.Info);

    .. code-tab:: java

        altDriver.setServerLogging(AltLogger.File, AltLogLevel.Off);
        altDriver.setServerLogging(AltLogger.Unity, AltLogLevel.Info);

    .. code-tab:: py

        altDriver.set_server_logging(AltLogger.File, AltLogLevel.Off);
        altDriver.set_server_logging(AltLogger.Unity, AltLogLevel.Info);

```

### AltDriver logging

Logging on the driver is handled using `NLog` in C#, `loguru` in python and `log4j` in Java. By default logging is disabled in the driver (tests). If you want to enable it you can set the `enableLogging` in `AltDriver` constructor.

```eval_rst
.. tabs::

    .. tab:: C#

        Logging is handled using a custom NLog LogFactory.  The Driver LogFactory can be accessed here: `AltTester.AltTesterUnitySDK.Driver.Logging.DriverLogManager.Instance`

        There are three logger targets that you can configure on the driver:

        * FileLogger
        * UnityLogger //available only when runnning tests from Unity
        * ConsoleLogger //available only when runnning tests using the Nuget package

        If you want to configure different level of logging for different targets you can use `AltTester.AltTesterUnitySDK.Driver.Logging.DriverLogManager.SetMinLogLevel(AltLogger.File, AltLogLevel.Info)`

        .. code-block:: c#

            /* start AltDriver with logging disabled */
            var altDriver = new AltDriver (enableLogging: false);

            /* start AltDriver with logging enabled for Debug.Level; this is the default behaviour*/
            var altDriver = new AltDriver (enableLogging: true);

            /* disable AltDriver logging */
            altDriver.SetLogging(enableLogging: false);

            /* enable AltDriver logging */
            altDriver.SetLogging(enableLogging: true);

            /* set logging level to Info for File target */
            AltTester.AltTesterUnitySDK.Driver.Logging.DriverLogManager.SetMinLogLevel(AltLogger.File, AltLogLevel.Info);



    .. tab:: Java

        Logging is handled via log4j. You can use log4j configuration files to customize your logging.

        Setting the `enableLogging` in `AltDriver` initializes logger named `ro.AltTester` configured with two appenders, a file appender `AltFileAppender` and a console appender `AltConsoleAppender`

        .. code-block:: java

            /* start AltDriver with logging enabled */
            altDriver = new AltDriver("127.0.0.1", 13000, true);

            /* disable logging for ro.AltTester logger */
            final LoggerContext ctx = (LoggerContext) LogManager.getContext(false);
            final Configuration config = ctx.getConfiguration();
            config.getLoggerConfig("ro.AltTester").setLevel(Level.OFF);

            ctx.updateLoggers();


    .. tab:: Python

        Logging is handled via loguru.

        Setting the `enable_logging` to `False` in AltDriver, all logs from `alttester` package are disabled.

        .. code-block:: python

            # enable logging in driver:
            loguru.logger.enable("alttester")

            # disable logging in driver:
            loguru.logger.disable("alttesterr")

```

## Logging in WebGL

The logs for a WebGL instrumented build are displaied in the browser's console. You can open the `Console` tab by pressing `F12`. To download the logs right click inside the `Console` and choose `Save as...`.

![Save as...](../_static/img/advanced-usage/save.png)

## Code Stripping

AltTester Unity SDK is using reflection in some of the commands to get information from the instrumented application. If you application is using IL2CPP scripting backend then it might strip code that you would use in your tests. If this is the case we recommend creating an `link.xml` file. More information on how to manage code stripping and create an `link.xml` file is found in [Unity documentation](https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
