# Get Started

To run the first test for your Unity game you need to:

```eval_rst

.. contents::
    :local:
    :depth: 1
    :backlinks: none
    :class: custom-table-of-contents


.. note::

    If you don't have access to source code of the game you need to ask a person with access to give you an instrumented version of the game.

```

## Import AltUnity Tester package in Unity Editor

To instrument your Unity application with AltUnity Tester you first need to import the AltUnity Tester package into Unity.

```eval_rst

    1. Download `AltUnity Tester Alpha <https://altom.com/app/uploads/altUnityProAlpha/AltUnityTester.unitypackage>`_.

    2. Import it by drag and drop inside your Unity project.
    
```

```important:: To make sure the import was correct, check if you can open AltUnity Tester Editor Window from Unity Editor -> AltUnity Tools -> AltUnityTester.

```

![window menu with altUnity Tester](../_static/images/DownloadingImportingAltUnityTesterWindow.png)

## Instrument your game with AltUnity Tester Alpha

Steps:
1. Open AltUnity Tester Editor window from Unity Editor -> AltUnity Tools -> AltUnityTester
2. In the Build Settings section set the **Proxy host** to the IP/hostname of the device where the Proxy is running. Set the **Proxy port** to the port configured in the Proxy.
3. In the Scene Manager section select the scenes you want to include in your build
4. In the Platform section select desired platform and set the path to where you want to save the build
5. Press "Build Only" to instrument the game or "Build & Run" to start your instrumented game
after the build succeeded
6. Check the console to see if the build was successful.

![webgl](../_static/images/webgl.png)

```eval_rst
.. note::

    To be able to run your instrumented game in the background, go to File -> Build Settings -> Player Settings -> Project Settings -> Player -> Resolution and presentation and check the box next to Run in background*.
```
```eval_rst
.. note::

    When running the WebGL build of your game in browser, even with the Run in background* setting enabled, you still might experience slow performance if the tab with your content is not on focus. Make sure that the tab with your app is visible, otherwise your content will only update once per second in most browsers.
```

## Start the Proxy Module

The Proxy Module is incorporated in AltUnity Pro Alpha. In order to start it, all you have to do is to start AltUnity Pro Alpha.

## Run your game in Unity or on desired platform

Before running your tests you need to start the instrumented Unity application. Upon startup, your instrumented Unity app should display a popup with the message: "Connecting to AltUnity Proxy on {ProxyHost}:{ProxyPort}". The popup disappears when your app has successfully connected to the proxy.

```eval_rst

.. tabs::

    .. tab:: Unity Editor

        1. Open AltUnity Tester Window
        2. In platform section select Editor
        3. Click Play in Editor

    .. tab:: PC

        1. Open AltUnity Tester Window
        2. In platform section select Standalone
        3. Choose your build target
        4. Click Build & Run

        .. important::

            Make sure to set the "Api Compatibility Level" to ".NET 4.x" in Unity when building using the Standalone option.

            This setting can be found under Edit menu -> Project Settings -> Player -> Other Settings -> Configuration.

    .. tab:: Android

        Prerequisites:

        * Use the Unity Hub to install Android Build Support and the required dependencies: Android SDK & NDK tools, and OpenJDK

        Steps:

        1. Open AltUnity Tester Window
        2. In platform section select Android
        3. Click Build & Run


    .. tab:: iOS

        Prerequisites:

        * Have IProxy installed: ``brew install libimobiledevice``

        Steps:

        1. Open AltUnity Tester Window
        2. In platform section select iOS
        3. Click Build & Run

        .. note::
            Check the following link to see how to build and run your game for iOS (.ipa file) -- `link <https://altom.com/testing-ios-applications-using-java-and-altunity-tester/>`_.


    .. tab:: WebGL

        Prerequisites:

        * Use the Unity Hub to install WebGL Build Support 

        Steps:

        1. Open AltUnity Tester Window
        2. In platform section select WebGL
        3. Click Build & Run

```

## Write and execute first test for your game

To write tests with AltUnity Tester you need to import the AltUnity Driver in your tests project.

AltUnity Tester package contains AltUnityDriver class used to connect to the instrumented game. In the setup method create an instance of the driver and in the tear-down method invoke the stop method of the driver. With the instance of the driver you can query the Unity objects and interact with the game.

```eval_rst

.. tabs::

    .. tab:: C#-Unity

        AltUnity C# Driver is already included in AltUnity Tester package. If you are writing tests in C# then you can create your tests directly from Unity.

        1.  Create a folder named Editor in your Unity Project.
        2.  Right-click on Editor folder and select `Create -> AltUnityTest`. This will create a template file in which you could start to write your test.
        3.  Name the file MyFirstTest.
        4.  Open AltUnity Tester Window.
        5.  In the `Run Tests` section press "Run All Tests" button. You should see the output of the tests in Unity Editor Console


        Example test file:

        .. literalinclude:: other~/test-files/cSharp-test.cs
            :language: c#


        Run your test file from the command line by using the following command:

        .. code-block:: console

            <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltUnityTestRunner.RunTestFromCommandLine -tests MyFirstTest.TestStartGame -logFile logFile.log -batchmode -quit

    .. tab:: C#

        AltUnityDriver is available also as a nuget package. You can use the nuget package to write your tests in a separate tests project, independent of the Unity application.

        Create a new test project 

        .. code-block:: console

            dotnet new nunit

        Install AltUnityDriver nuget package

        .. code-block:: console

            dotnet add package AltUnityDriver --version 1.7.0-alpha


        Example test file:

        .. literalinclude:: other~/test-files/cSharp-test.cs
            :language: c#


        Run your tests 

        .. code-block:: console
        
            dotnet test

    .. tab:: Java

        AltUnity Java Driver is available as a maven package or as a standalone jar. Use one of the following methods to import the driver in your tests project.

            **Method 1**: Add AltUnity Java Driver as a dependency in your **pom.xml** file:

            .. code-block:: xml

                <dependency>
                    <groupId>com.altom</groupId>
                    <artifactId>altunitytester-java-client</artifactId>
                    <version>1.6.6</version>
                </dependency>


            **Method 2**: Use the **.jar** file from GIT (**without building it from source**)

                * Download `AltUnity Java Driver <https://altom.gitlab.io/altunity/altunitytester/master/AltUnityJAR/altunitytester-java-client-jar.jar>`__.

                * Install the **.jar** file:

                .. code-block:: console

                    mvn install:install-file -Dfile=./target/altunitytester-java-client-jar-with-dependencies.jar -DgroupId=ro.altom -DartifactId=altunitytester -Dversion=1.6.6 -Dpackaging=jar``


        Example test file:

        .. literalinclude:: other~/test-files/java-test.java
            :language: java


        Run your tests by using the following command (in the test project folder):

        .. code-block:: console

            mvn test

    .. tab:: Python

        There are two methods of installing the AltUnity Python Driver pip package:

            **Method 1**: Installing using Pip:

            .. code-block:: console

                pip install altunityrunner

            **Method 2**: Install from the source code in the repository:

            .. code-block:: console

                cd <project-dir>/Bindings~/python
                python setup.py install


        Example test file:

        .. literalinclude:: other~/test-files/python-test.py
            :language: py


        Run your test file by using the following command:

        .. code-block:: console

            python <nameOfYourTestFile.py>

```

Now your project can use all the [AltUnity Driver Commands](./commands.md).

```note::
        Before running your tests, start the Proxy and the Instrumented Unity app.
```
