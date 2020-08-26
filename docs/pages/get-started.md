# Get Started  

To run the first test for your Unity game you need to:
1. [Import AltUnity Tester package in Unity Editor](#import-altunity-tester-package)
2. [Instrument your game with AltUnity Server](#instrument-your-game-with-altunity-server)
3. [Run the build in Unity Editor or on desired platform](#run-your-game-in-unity-or-on-desired-platform)
4. [Write and execute first test](#write-and-execute-first-test-for-your-game)

```eval_rst note::
    If you don't have access to source code of the game you need to ask a person with access to give you an instrumented version of the game. 
```

## Import AltUnity Tester package

To instrument your game with AltUnity Server you first need to import the AltUnity Tester package into Unity.

```eval_rst
.. tabs::

    .. tab:: From Unity Asset Store

        1. Download from Unity `Asset Store - link <https://assetstore.unity.com/packages/tools/utilities/altunitytester-112101>`_.
        2. Go to your Asset Store Downloads Manager from Unity Editor.
        3. Import the package into your Unity project.


    .. tab:: UnityPackage from Gitlab pages

        1. Download from `Gitlab pages (deployed using CI) - link <https://altom.gitlab.io/altunity/altunitytester/master/AltUnityPackage/AltUnityTester.unitypackage>`_.
        2. Import it by drag and drop inside your Unity project.

```


``` important:: To make sure the import was correct, check if you can open AltUnity Tester Editor Window from Unity Editor -> Window -> AltUnityTester.
```
 
![window menu with altUnity Tester](../_static/images/DownloadingImportingAltUnityTesterWindow.png)


## Instrument your game with AltUnity Server

In order for the tests to have access to Unity objects via AltUnity Client you need to instrument the game with AltUnity Server.

Steps:

1. Open AltUnity Tester Window from Unity Editor -> Window -> AltUnityTester.
2. Select on what platform you want to build the game.
3. Press "Build Only" to instrument the game.
4. Check the console to see if the build was successful.

``` important::
        Make sure to set the "Api Compatibility Level" to ".NET 4.x" in Unity when building using the Standalone option.  

        This setting can be found under Edit menu -> Project Settings -> Player -> Other Settings -> Configuration.   
```

``` note::
        Your build files are available in the configured Output path. By default, the Output path is a folder with the same name as your game.
```

``` note::
    If you have a custom build, check how you can build from the command line using the instructions in the `Advanced Usage section <advanced-usage.html#Build-games-from-the-command-line>`_.
```

``` note::
    If changes are made inside a test, rebuilding the application is not necessary. 
    A rebuild is needed only if changes are made inside the Unity project.
```



## Run your game in Unity or on desired platform

Before running your tests you need to start the game instrumented with AltUnity Server. Upon startup, your game should display a popup with the message: "waiting for connection on port 13000". 

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
        

        .. note:: 
            For more details read about `port forwarding at this link <advanced-usage.html#what-is-port-forwarding-and-when-to-use-it>`_.

```


## Write and execute first test for your game

To write tests with AltUnity Tester you need to import the AltUnity Client in your tests project. 

AltUnity Tester package contains AltUnityDriver class used to connect to the instrumented game. In the setup method create an instance of the driver and in the tear-down method invoke the stop method of the driver. With the instance of the driver you can query the Unity objects and interact with the game.


``` note:: 
        Example tests below are setup to run on Scene 2 and Scene 3 from the Example folder under the AltUnity Tester package.  
        
        If you want to use these examples for other scenes / games, make sure to update the test accordingly.
```


```eval_rst
.. tabs::

    .. tab:: C#
        
        AltUnity C# Client is already included in AltUnity Tester package. If you are writing tests in C# then you can create your tests directly from Unity.
        
        1.  Create a folder named Editor in your Unity Project.
        2.  Right-click on Editor folder and select `Create -> AltUnityTest`. This will create a template file in which you could start to write your test.
        3.  Name the file MyFirstTest.
        4.  Open AltUnity Tester Window.
        5.  In the `Run Tests` section press "Run All Tests" button. You should see the output of the tests in Unity Editor Console 


        Example test file:

        .. tabs::

            .. tab:: Unity Editor & PC

                .. literalinclude:: other~/test-files/cSharp-test.cs
                    :language: c#

            .. tab:: Android

                .. literalinclude:: other~/test-files/cSharp-Android-test.cs
                    :language: c#
                    :emphasize-lines: 12,20

            .. tab:: iOS

                .. literalinclude:: other~/test-files/cSharp-iOS-test.cs
                    :language: c#
                    :emphasize-lines: 12,20

        Run your test file from the command line by using the following command:

                .. code-block:: bash

                    <UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltUnityTestRunner.RunTestFromCommandLine -tests MyFirstTest.TestStartGame -logFile logFile.log -batchmode -quit

    .. tab:: Java

        AltUnity Java Client is available as a maven package or as a standalone jar. Use one of the following methods to import the client in your tests project.

           Method 1: 

                * Add AltUnity Java Client as a dependency in your pom.xml file:

                .. code-block:: xml
                
                    <dependency>
                      <groupId>com.altom</groupId>
                      <artifactId>altunitytester-java-client</artifactId>
                      <version>1.5.7</version>
                    </dependency>


           Method 2: Use the .jar file from GIT (**without building it from source**)

                * Download `AltUnity Java Client <https://altom.gitlab.io/altunity/altunitytester/master/AltUnityJAR/altunitytester-java-client-jar.jar>`__.

                * Install the .jar file:

                .. code-block:: sh

                    mvn install:install-file -Dfile=./target/altunitytester-java-client-jar-with-dependencies.jar -DgroupId=ro.altom -DartifactId=altunitytester -Dversion=1.5.7 -Dpackaging=jar`` 

        Example test file:

        .. tabs::

            .. tab:: Unity Editor & PC

                .. literalinclude:: other~/test-files/java-test.java
                    :language: java

            .. tab:: Android

                .. literalinclude:: other~/test-files/java-Android-test.java
                    :language: java
                    :emphasize-lines: 18,25

            .. tab:: iOS

                .. literalinclude:: other~/test-files/java-iOS-test.java
                    :language: java
                    :emphasize-lines: 18,25

        Run your tests by using the following command (in the test project folder):

                ``mvn test``

    .. tab:: Python

        There are two methods of installing the AltUnity Python Client pip package:

            Method 1 - Installing using Pip:
        
                ``pip install altunityrunner``

            Method 2 - Install from the source code in the repository:
           
                ``cd <project-dir>/Bindings~/python``                

                ``python setup.py install``
        

        Example test file:  

        .. tabs::

            .. tab:: Unity Editor & PC

                .. literalinclude:: other~/test-files/python-test.py
                    :language: py

            .. tab:: Android

                .. literalinclude:: other~/test-files/python-Android-test.py
                    :language: py
                    :emphasize-lines: 16,17,25,26

            .. tab:: iOS

                .. literalinclude:: other~/test-files/python-iOS-test.py
                    :language: py
                    :emphasize-lines: 13,17,18,26,27

        Run your test file by using the following command:

                ``python <nameOfYourTestFile.py>``
```


Now your project can use all the [AltUnity Client Commands](./commands.md).


``` note:: 
        Before running your tests, start the instrumented game and wait for popup with the message: "waiting for connection on port 13000".
```