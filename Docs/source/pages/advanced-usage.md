# Advanced Usage

This guide covers some of the more advanced features, patterns and
configuration options of AltUnity Tester.

## Build games from the command line

To build your Unity application from command line you need a static method in
your project that handles the build logic. To instrument your Unity application
with AltUnity Tester, your build method must define `ALTUNITYTESTER` scripting
symbol and must insert AltUnity Prefab in the first scene of the game.

Depending on your project's setup, there are two ways in which games can be
built from the command line:


```eval_rst
.. note::

    AltUnity Tester does not work by default in release mode. If you instrument
    your game in release mode, AltUnity Prefab self removes from the scenes and
    the socket server does not start. Best case practice is to customize your
    build script to insert AltUnity Prefab only in Debug mode.

    If you do want to use AltUnity Tester in release mode see
    `Using AltUnity Tester in Release mode section <#using-altunity-tester-in-release-mode>`_.

```


**1. If you already have a custom build method for your game**

If you already have a custom build method for your game, you can add the
following lines to your build method. Also, the BuildPlayerOptions should
check for *BuildOptions.Development* and *BuildOptions.IncludeTestAssemblies*.

```c#
var buildTargetGroup = BuildTargetGroup.Android;
AltUnityBuilder.AddAltUnityTesterInScriptingDefineSymbolsGroup(buildTargetGroup);
if (buildTargetGroup == UnityEditor.BuildTargetGroup.Standalone) {
    AltUnityBuilder.CreateJsonFileForInputMappingOfAxis();
}
var instrumentationSettings = new AltUnityInstrumentationSettings();
AltUnityBuilder.InsertAltUnityInScene(FirstSceneOfTheGame, instrumentationSettings);
```

```eval_rst
.. note::

    Change ``buildTargetGroup`` above to the target group for which you are
    building.

```


**2. If you create a new custom build method for your game**

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

## Run tests from the command line

In order to run tests from the command line you can use the following example
commands:

```eval_rst
.. tabs::

    .. tab:: C#

        .. code-block:: bash

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltUnityTestRunner.RunTestFromCommandLine -tests MyFirstTest.TestStartGame -logFile logFile.log -batchmode -quit

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

1. Instrument your game build with AltUnity Tester from Unity or by [building from the command line](#build-games-from-the-command-line).
2. Start the game build on a device.
3. Run your tests - see commands in the ["Run tests from the command line" section](#run-tests-from-the-command-line).

An example CI configuration file can be viewed in the [AltUnity Tester GitLab repository](https://gitlab.com/altom/altunity/altunitytester/-/blob/master/.gitlab-ci.yml).

## Using AltUnity Tester in Release mode

By default AltUnity Tester does not run in release mode. We recommended that you do not instrument your Unity application in release mode with AltUnity Tester. That being said, if you do want to instrument your application in release mode, you need to uncheck `RunOnlyInDebugMode` flag on AltUnityRunnerPrefab inside AltUnity Tester asset folder `AltUnityTester/Prefab/AltUnityRunnerPrefab.prefab`

## Logging

There are two types of logging that can be configured in AltUnityTester. The logs from AltUnity Driver (from the tests) and the logs from the AltUnity Tester (from the instrumented Unity application)

```eval_rst
.. note::

    From version 1.7.0 on logs from `Server` are referred to as logs from `Tester`.

```

### AltUnity Tester logging

Logging inside the instrumented Unity application is handled using a custom NLog LogFactory. The Server LogFactory can be accessed here: `Altom.AltUnityTester.Logging.ServerLogManager.Instance`

There are two logger targets that you can configure on the server:

-   FileLogger
-   UnityLogger

Logging inside the instrumented app can be configured from the driver using the SetServerLogging command:

```eval_rst
.. tabs::

    .. code-tab:: c#

        altUnityDriver.SetServerLogging(AltUnityLogger.File, AltUnityLogLevel.Off);
        altUnityDriver.SetServerLogging(AltUnityLogger.Unity, AltUnityLogLevel.Info);

    .. code-tab:: java

        altUnityDriver.setServerLogging(AltUnityLogger.File, AltUnityLogLevel.Off);
        altUnityDriver.setServerLogging(AltUnityLogger.Unity, AltUnityLogLevel.Info);

    .. code-tab:: py

        altUnityDriver.set_server_logging(AltUnityLogger.File, AltUnityLogLevel.Off);
        altUnityDriver.set_server_logging(AltUnityLogger.Unity, AltUnityLogLevel.Info);

```

### AltUnity Driver logging

Logging on the driver is handled using `NLog` in C#, `loguru` in python and `log4j` in Java. By default logging is disabled in the driver (tests). If you want to enable it you can set the `enableLogging` in `AltUnityDriver` constructor.

```eval_rst
.. tabs::

    .. tab:: C#

        Logging is handled using a custom NLog LogFactory.  The Driver LogFactory can be accessed here: `Altom.AltUnityDriver.Logging.DriverLogManager.Instance`

        There are three logger targets that you can configure on the driver:

        * FileLogger
        * UnityLogger //available only when runnning tests from Unity
        * ConsoleLogger //available only when runnning tests using the Nuget package

        If you want to configure different level of logging for different targets you can use `Altom.AltUnityDriver.Logging.DriverLogManager.SetMinLogLevel(AltUnityLogger.File, AltUnityLogLevel.Info)`

        .. code-block:: c#

            /* start altunity driver with logging disabled */
            var altUnityDriver = new AltUnityDriver (enableLogging=false);

            /* start altunity driver with logging enabled for Debug.Level; this is the default behaviour*/
            var altUnityDriver = new AltUnityDriver (enableLogging=true);

            /* set logging level to Info for File target */
            Altom.AltUnityDriver.Logging.DriverLogManager.SetMinLogLevel(AltUnityLogger.File, AltUnityLogLevel.Info);



    .. tab:: Java

        Logging is handled via log4j. You can use log4j configuration files to customize your logging.

        Setting the `enableLogging` in `AltUnityDriver` initializes logger named `ro.altom.altunitytester` configured with two appenders, a file appender `AltUnityFileAppender` and a console appender `AltUnityConsoleAppender`

        .. code-block:: java

            /* start altunity driver with logging enabled */
            altUnityDriver = new AltUnityDriver("127.0.0.1", 13000, true);

            /* disable logging for ro.altom.altunitytester logger */
            final LoggerContext ctx = (LoggerContext) LogManager.getContext(false);
            final Configuration config = ctx.getConfiguration();
            config.getLoggerConfig("ro.altom.altunitytester").setLevel(Level.OFF);

            ctx.updateLoggers();


    .. tab:: Python

        Logging is handled via loguru.

        Setting the `enable_logging` to `False` in AltUnityDriver, all logs from `altunityrunner` package are disabled.

        .. code-block:: python

            # enable logging in driver:
            loguru.logger.enable("altunityrunner")

            # disable logging in driver:
            loguru.logger.disable("altunityrunner")

```

## Code Stripping

AltUnity Tester is using reflection in some of the commands to get information from the instrumented application. If you application is using IL2CPP scripting backend then it might strip code that you would use in your tests. If this is the case we recommend creating an `link.xml` file. More information on how to manage code stripping and create an `link.xml` file is found in [Unity documentation](https://docs.unity3d.com/Manual/ManagedCodeStripping.html)
