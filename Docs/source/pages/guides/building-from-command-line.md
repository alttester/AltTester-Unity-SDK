# Building from the command line

## Build apps from the command line

To build your Unity application from command line you need a static method in
your project that handles the build logic. To instrument your Unity application
with AltTester® Unity SDK, your build method must define `ALTTESTER` scripting
symbol and must insert AltTester® Prefab in the first scene of the app.

Depending on your project's setup, there are two ways in which apps can be
built from the command line:


```eval_rst
.. note::

    AltTester® Unity SDK does not work by default in release mode. If you instrument
    your app in release mode, AltTester® Prefab self removes from the scenes and
    the socket server does not start. Best case practice is to customize your
    build script to insert AltTester® Prefab only in Debug mode.

    If you do want to use AltTester® Unity SDK in release mode see
    `Using AltTester® Unity SDK in Release mode section <#using-alttester-unity-sdk-in-release-mode>`_.

```


**1. If you already have a custom build method for your app**

If you already have a custom build method for your app, you can add the
following lines to your build method. Also, the BuildPlayerOptions should
check for *BuildOptions.Development* and *BuildOptions.IncludeTestAssemblies*.

```c#
var buildTargetGroup = BuildTargetGroup.Android;
AltBuilder.AddAltTesterInScriptingDefineSymbolsGroup(buildTargetGroup);
AltBuilder.CreateJsonFileForInputMappingOfAxis();
var instrumentationSettings = new AltInstrumentationSettings();
AltBuilder.InsertAltInScene(FirstSceneOfTheApp, instrumentationSettings);
```

```eval_rst
.. note::

    Change ``buildTargetGroup`` above to the target group for which you are
    building.

```
```eval_rst
.. important::

    If your custom build method sets scripting define symbols manually instead of using the list from the Unity Editor, and your project uses TextMeshPro, you must also include the **TMP_PRESENT** define. This is required for methods that interact with TextMeshPro elements to function correctly.

```

**2. If you create a new custom build method for your app**

The following example script can be used. It sets all the project settings
needed and uses the same two important lines from point 1 above.

This example method is configured for the Android platform, so make sure to
update it based on your target platform.

```eval_rst
.. literalinclude:: ../../_static/examples~/advanced-usage/build-from-command-line.txt
    :language: c#

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


## How to make an instrumented build using Unity Cloud Build

To instrument your Unity project with AltTester® Unity SDK using Unity Cloud Build, follow these steps:

1. **Create or select a build configuration**  
   In Unity Cloud Build, either use an existing configuration or create a new one for your instrumented build.

2. **Set the build to Development Mode**  
   In the configuration settings, ensure that the build is set to Development Mode.

3. **Configure Script Hooks**  
    In the Script Hooks section, add your method name to the **Pre-Export Method** field. This method should contain the code that inserts AltTester® into your build.

4. **Add Scripting Define Symbols**  
   In the Script Hooks section, add `ALTTESTER` to the **Scripting Define Symbols** field.  

```eval_rst
    .. image:: ../../_static/img/advanced-usage/unity-cloud-configuration.png
```

```c#
         public static void OnPreExportWindows()
        {
            Debug.Log("Unity Cloud Build - OnPreExportWindows called");

            var buildTargetGroup = BuildTargetGroup.Standalone;

         
            AltBuilder.CreateJsonFileForInputMappingOfAxis();
            var instrumentationSettings = new AltInstrumentationSettings();
            instrumentationSettings.AltServerHost = "127.0.0.1";
            instrumentationSettings.AltServerPort = 13000;
            instrumentationSettings.AppName = "__default__";
            instrumentationSettings.ResetConnectionData = true;
            AltBuilder.InsertAltInScene("Assets/Scenes/SampleScene.unity", instrumentationSettings);
        }

```

```eval_rst
.. note::
     An example with a working script can be found at `Unity-Project <https://github.com/alttester/UnityCloudTestBuild>`_
```


## How to make a production build

There is no need to remove the AltTester® package entirely from the project, only the `ALTTESTER` Scripting Define Symbol should be deleted from the Player Settings. Also, make sure that the `Keep ALTTESTER symbol defined` checkbox is unchecked. After that, you can build your app normally as you would do in Unity.
