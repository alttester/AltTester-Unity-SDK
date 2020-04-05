# Download&Import
## Downloads - AltUnityTester Package

* From pages (deployed using CI): 
	* [UnityPackage](https://altom.gitlab.io/altunity/altunitytester/master/AltUnityPackage/AltUnityTester.unitypackage)
  * [JAR Dependency](https://altom.gitlab.io/altunity/altunitytester/master/AltUnityJAR/altunitytester-java-client-jar-with-dependencies.jar)       
* From Unity Asset Store - import inside your project directly:
	* [Asset Store Link](https://assetstore.unity.com/packages/tools/utilities/altunitytester-112101) 


## Import AltUnityTester asset/package into your Unity project:
* If you use a downloaded Unity package, go to Assets-Import Package - Custom Package in Unity Editor and select the AltUnityTester.unitypackage file
* If you download it from the Unity Asset store, just go to your Asset Store Downloads Manager from Unity Editor and import the package

After import check if in the menu bar->"Window" you can see and open AltunityTester Window. If you cannot see it then you didn't import it correctly.


## Bindings


### Python


#### Installation

   * using pip

      ``pip install altunityrunner``

   * from the source code in the repo

      ``cd <project-dir>/Assets/AltUnityTester/Bindings/python``
     
      ``python setup.py install``


### Java

#### Installation

To be able to use altunitytester from your Java code, you need to first build a jar file, then import it in your project. 

Here's how to do that with maven:

   * Build the .jar file:
    ` Assets/AltUnityTester/Bindings/java`
    ` mvn clean compile assembly:single`

   * Install the jar file:
    `mvn install:install-file -Dfile=./target/altunitytester-java-client-1.5.4-SNAPSHOT-jar-with-dependencies.jar -DgroupId=ro.altom -DartifactId=altunitytester -Dversion=1.5.4 -Dpackaging=jar` 

Now your project can use all the AltUnityDriver methods. 
