# Get Started  
## Download and import the AltUnity Tester package

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


``` important:: To make sure the import was correct, check if in the ”Window” from the Unity menu bar you can see and open AltunityTester Window. 
```
 
![window menu with altUnity Tester](../_static/images/DownloadingImportingAltUnityTesterWindow.png)

## Run your first test for a game running in Unity Editor 

It is very simple to write tests with AltUnityTester. You can use any testing framework and all you need to do is to import the driver for the specific language to your test file.

After that in your setup method you will need to make an instance of the driver and in you tear-down method you have to invoke the stop method of the driver.

If you do this in your test method you could access all the commands that AltUnity Tester offers to test your game. 

``` note:: 
        Example tests below are setup to run on Scene 2 and Scene 3 from the Example folder under the AltUnity Tester package.  
        
        If you want to use these examples for other scenes / games, make sure to update the test accordingly.
```

```eval_rst
.. tabs::

    .. tab:: C#

        1. If you are writing tests in C# then you can create your tests directly from Unity.
                
                1.  Create a folder named Editor in your Unity Project.
                2.  Right-click and select to create a new AltUnityTest file. This will create a template file in which you could start to write your test. 
                3.  Name the file however you want.
                4.  Open AltUnityTester window.
                5.  You should be able to see your test in the left column.
                6.  Run it by pressing one of the 3 options on the right column (under "Run tests" section).               

        2. Using the driver:
        
                ``altUnityDriver = new AltUnityDriver ("deviceIp", 13000);``

        3. Example test file:

                .. include:: other/test-files/cSharp-test.cs
                        :code: c#

    .. tab:: Java

       To be able to use altUnity Tester from your Java code, you need to import it in your project.         

       1. There are two methods of importing the Java library:

           Method 1: 

                * Import it into your project by adding it as a dependency in your pom.xml file:
                        
                        | <dependency>
                        | <groupId>com.altom</groupId>
                        | <artifactId>altunitytester-java-client</artifactId>
                        | <version>1.5.4</version>
                        | <dependency>

           Method 2: Using the .jar file from GIT (**without building it from source**)

                * Download from Git -- `JAR Dependency link <https://altom.gitlab.io/altunity/altunitytester/master/AltUnityJAR/altunitytester-java-client-jar.jar>`__.

                * Install the .jar file:

                        ``mvn install:install-file -Dfile=./target/altunitytester-java-client-jar-with-dependencies.jar -DgroupId=ro.altom -DartifactId=altunitytester -Dversion=1.5.5 -Dpackaging=jar`` 

       2. Using the driver:
        
                ``altUnityDriver = new AltUnityDriver ("deviceIp", 13000, ";", "&", true);``  

       3. Example test file:

                .. include:: other/test-files/java-test.java
                        :code: java

       4. Run your test file by using the following command (in the test project folder):

                ``mvn test``

    .. tab:: Python

        1. There are two methods of installing the pip package:

            Method 1 - Installing using Pip:
        
                ``pip install altunityrunner``

            Method 2 - Install from the source code in the repository:
           
                ``cd <project-dir>/Bindings~/python``                

                ``python setup.py install``
        
        2. Using the driver:

                ``cls.altdriver = AltrunUnityDriver (None, 'platform', 'deviceIp', 13000)``

        3. Example test file:

                .. include:: other/test-files/python-test.py
                        :code: py

        4. Run your test file by using the following command:

                ``python <nameOfYourTestFile.py>``
```

Now your project can use all the AltUnityDriver methods. 
 
``` note:: 
        It is necessary for the game to "Play in Editor" (option under Run in "AltUnityTesterEditor" window) since only having it run in the Game tab in Unity will not add the altUnity Tester library to the game.
```

## Build a game with AltUnity Tester

If changes are made inside a test, rebuilding the application is not necessary. 
A rebuild is needed only if changes are made inside the Unity project.

Steps:

        1. Open AltUnityTester from UnityEditor -> Window -> AltUnityTester.
        2. Select on what platform do you want to build the game.
        3. Press "Build Only" or "Build & Run" button.

If the build was made successfully, your game should display a popup with the following message: "waiting for connection on port 13000".

``` note:: 
        Check the following link to see how to build a development build for iOS (.ipa file) -- `link <https://altom.com/testing-ios-applications-using-java-and-altunity-tester/>`_.

```

``` note:: 
        If you have a custom build, check how you can build from the command line using the instructions in the `Advanced Usage section <advanced-usage.html#Build-games-from-the-command-line>`__.
```


## Run your first test on the target platform

``` note:: 
        Example tests below are setup to run on Scene 2 and Scene 3 from the Example folder under the AltUnity Tester package. 
        
        If you want to use these examples for other scenes / games, make sure to update the test accordingly.
```

```eval_rst
.. tabs::

   .. tab:: PC

      .. tabs::

         .. tab:: C#

                1. If you are writing tests in C# then you can create your tests directly from Unity.
                
                        1.  Create a folder named Editor in your Unity Project.
                        2.  Right-click and select to create a new AltUnityTest file. This will create a template file in which you could start to write your test. 
                        3.  Name the file however you want.
                        4.  Open AltUnityTester window.
                        5.  You should be able to see your test in the left column.
                        6.  Run it by pressing one of the 3 options on the right column (under "Run tests" section).               
                2. Using the driver:

                        ``altUnityDriver = new AltUnityDriver ("deviceIp", 13000);``

                3. Example test file:

                        .. include:: other/test-files/cSharp-test.cs
                                :code: c#

         .. tab:: Java

                To be able to use altUnity Tester from your Java code, you need to import it in your project.         
                
                1. There are two methods of importing the Java library:

                   Method 1: 

                     * Import it into your project by adding it as a dependency in your pom.xml file:
                        
                            | <dependency>
                            | <groupId>com.altom</groupId>
                            | <artifactId>altunitytester-java-client</artifactId>
                            | <version>1.5.4</version>
                            | <dependency>

                   Method 2: Using the .jar file from GIT (**without building it from source**)

                     * Download from Git -- `JAR Dependency link <https://altom.gitlab.io/altunity/altunitytester/master/AltUnityJAR/altunitytester-java-client-jar.jar>`__.

                     * Install the .jar file:

                                ``mvn install:install-file -Dfile=./target/altunitytester-java-client-jar-with-dependencies.jar -DgroupId=ro.altom -DartifactId=altunitytester -Dversion=1.5.5 -Dpackaging=jar`` 

                2. Using the driver:
        
                        ``altUnityDriver = new AltUnityDriver ("deviceIp", 13000, ";", "&", true);``  

                3. Example test file:

                        .. include:: other/test-files/java-test.java
                                :code: java

                4. Run your test file by using the following command (in the test project folder):

                        ``mvn test``

         .. tab:: Python

                1. There are two methods of installing the pip package:

                    Method 1 - Installing using Pip:
        
                        ``pip install altunityrunner``

                    Method 2 - Install from the source code in the repository:
           
                        ``cd <project-dir>/Bindings~/python``                

                        ``python setup.py install``
        
                2. Using the driver:

                        ``cls.altdriver = AltrunUnityDriver (None, 'platform', 'deviceIp', 13000)``

                3. Example test file:

                        .. include:: other/test-files/python-test.py
                                :code: py

                4. Run your test file by using the following command:

                        ``python <nameOfYourTestFile.py>``

   .. tab:: Android

      Prerequisites:

        1. Have ADB installed -- for more information check this `article <https://www.xda-developers.com/install-adb-windows-macos-linux/>`_.
        2. Create a folder named Editor in your Unity Project.
        3. Right-click and select to create a new AltUnityTest file. This will create a template file in which you could start to write your test.
        4. Write you test (see example test below).

      .. tabs::

         .. tab:: C#

            1. To run your test file (see example below) you can use the following command:

                ``<UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltUnityTestRunner.RunTestFromCommandLine -tests MyFirstTest.TestStartGame -logFile logFile.log -batchmode -quit``

            2. Example test file: 

            .. include:: other/test-files/cSharp-Android-test.cs
                :code: c#

         .. tab:: Java

            1. To run your test file (see example below) you can use the following command:

                ``mvn test``

            2. Example test file:

            .. include:: other/test-files/java-Android-test.java
                :code: c#

         .. tab:: Python

            1. To run your test file (see example below) you can use the following command:

                ``python <nameOfYourTestFile.py>``

            2. Example test file:

            .. include:: other/test-files/python-Android-test.py
                :code: py
            
   .. tab:: iOS

     Prerequisites:

     * Have IProxy installed: ``brew install libimobiledevice``
     * For more details read about `port forwarding at this link <advanced-usage.html#Setup-Port-Forwarding>`__.

      .. tabs::

         .. tab:: C#

            1. To run your test file (see example below) you can use the following command:

                ``<UnityPath>/Unity -projectPath $PROJECT_DIR -executeMethod AltUnityTestRunner.RunTestFromCommandLine -tests MyFirstTest.TestStartGame -logFile logFile.log -batchmode -quit``

            2. Example test file: 

            .. include:: other/test-files/csharp-iOS-test.cs
                :code: c#

         .. tab:: Java

            1. To run your test file (see example below) you can use the following command:

                ``mvn test``

            2. Example test file:

            .. include:: other/test-files/java-iOS-test.java
                :code: java

         .. tab:: Python

            1. To run your test file (see example below) you can use the following command:

                ``python <nameOfYourTestFile.py>``

            2. Example test file:

            .. include:: other/test-files/python-iOS-test.py
                :code: py
