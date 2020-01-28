# AltUnityTester - Java Bindings

The AltUnityTester package contains a alt_driver module that will open a socket connection on the device running the Unity application and will give access to all the objects in the Unity hierarchy. 

Using this socket connection and the actions available in the AltUnity driver, we can run python tests tests against the Unity app running on iOS or Android. 

Links:

* [Setup AltUnityTester and configure your app for testing](https://gitlab.com/altom/altunitytester/blob/master/README.md#setup)
* [Downloads](#downloads-altunitytester-package)
* [Java Docs - AltUnityRunner - Getting started](#python-altunityrunner-module)
    * [Installation](#installation)
    * [Getting Started](#getting-started)
    * [AltElements](#altelements)
    * [Available Actions](#available-actions)
      * [Finding elements](#finding-elements)
      * [Waiting for elements](#waiting-for-elements)
      * [Managing Unity Scenes](#managing-unity-scenes)
      * [Managing Unity PlayerPrefs](#managing-unity-playerprefs)
      * [Call static method](#call-static-methods)
      * [Actions on Screen](#actions-on-screen)
      * [Actions on elements](#actions-on-elements)
      * [Keyboard and Mouse](#keyboard-and-mouse)


## Downloads - AltUnityTester Package

* From repository: 
   * https://gitlab.com/altom/altunitytester/blob/master/AltUnityTester.unitypackage
  
  
* From Unity Asset Store - import inside your project directly:
   * links soon


## Java AltUnityTester

The project contains a Java project called ``altunitytester`` that gives access to the alt_driver commands so that objects can be accessed from  Java code. 

The code for this is available under ``AltUnityTester/Bindings/java`` in the repository. 

### Internal tests
In order to run unit tests for the bindings implementation run
`mvn clean test`

For integration tests which actually use the game artifact please use 
`mvn clean test -Pscene-tests`

### Installation

To be able to use altunitytester from your Java code, you need to first build a jar file, then import it in your project. 

Here's how to do that with maven:

   * Build the .jar file:
    ` Assets/AltUnityTester/Bindings/java`
    ` mvn clean compile assembly:single`

   * Install the jar file:
    `mvn install:install-file -Dfile=./target/altunitytester-java-client-1.5.2-SNAPSHOT-jar-with-dependencies.jar -DgroupId=ro.altom -DartifactId=altunitytester -Dversion=1.5.2 -Dpackaging=jar` 

Now your project can use all the AltUnityDriver methods. 


### AltUnityElements

All elements in AltUnityTester have the following structure, as seen in the AltUnityObject class:

  * `name` - the name of the object as it is in the Unity Scene Hierarchy
  * `cameraName` - the name of the camera from which the screen coordinate are calculated
  * `id` - the Unity Instance ID, this is unique for each element in the scene
  * `x` - the x coordinate of the middle of the element on screen
  * `y` - the y coordinate of the middle of the element on screen
  * `z` - the z coordinate of the middle of the element on screen(this is mostly used to see if the object is in front of camera or behind)
  * `mobileY` - the y coordinate of the middle of the element on a mobile screen
  * `type` - "scene" for Unity scenes and "" for all other elements
  * `worldX` - the x coordinate in world space of the element
  * `worldY` - the y coordinate in world space of the element
  * `worldZ` - the z coordinate in world space of the element


### Available Actions

#### Finding elements

  * `getAllElements`
    * params: cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
    * returns: all elements that are currently Active in the scene
    
    ```java
      List<AltUnityObject> objects = altUnityDriver.getAllElements();
    ```

  * `findElement`
    * params:
        * name - the name of the object to be found, as it's shown in the Unity Scene hierarchy
        * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
        * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    * you can search for elements also by specifying a hierarchy path to them. For example, you can look for `Player1/Hand` or `Player2/Hand`, to make sure you find the correct `Hand` object you are interested in. When doing so, make sure you specify all the objects in between the `parent` and the `object` you are interested in. For example, if `Hand` is under a `Body` element for each `Player`, when you search for it make sure you specify it as `Player1/Body/Hand` 

    ```java
    altUnityDriver.findElement("Capsule"); // find object by name
    altUnityDriver.findElement("Ship/Main/Capsule", "Main Camera"); //specify also the name of the parents, and the camera
    ```


  * `findElementWhereNameContains`
    * params: 
        * partOfTheName - part of the name of the object to be found, as it's shown in the Unity Scene hierarchy
        * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
        * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
    * returns: the element with a name that contains partOfTheName (or the last one found in the hierarchy if more than one element with the same name is present)

    ```java
    altDriver.findElementWhereNameContains("Capsul"); // should find Capsule     
    ```

  * `findElementByComponent`
    * params: 
        * componentName - the name of a Unity Component, for example a java script that is attached to an element, like Collider2D etc. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
        * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
    * returns: the element with a componentName component (or the last one found in the hierarchy if more than one element with the same component is present)
   
    ```java
    altUnityDriver.findElementByComponent("AltUnityRunnerPrefab"); 
    ```

  * `findElements`
    * params: 
        * name - the name of the objects to be found, as they are shown in the Unity Scene hierarchy
        * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
        * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
    * returns: a list of elements with the correct name

    ```java
    altUnityDriver.findElements("Capsule");     
    ```

  * `findElementsWhereNameContains`
    * params: 
        * partOfTheName - part of the name of the objects to be found, as they are shown in the Unity Scene hierarchy
        * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene. 
        * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
    * returns: a list of elements with a name that contains partOfTheName 

    ```java
    altUnityDriver.findElementsWhereNameContains("Capsul"); # should find Capsule, Capsules, Capsule1, Capsule 2 etc.     
    ```

  * `findElementsByComponent`
    * params: 
        * componentName - the name of a Unity Component, for example a java script that is attached to an element, like Collider2D etc. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
        * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
    * returns: a list of elements with a componentName component

    ```java
    altUnityDriver.findElementsByComponent("Plane"); 
    ```

#### Waiting for elements

* `waitForElement`
    * params: 
      * name - the name of the object to be found, as it's shown in the Unity Scene hierarchy
      * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
      * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```java
    altUnityDriver.waitForElement("Capsule"); //specify also the name of the parents
    ```

* `waitForElementWhereNameContains`
    * params: 
      * partOfTheName - part of the name of the object to be found, as it's shown in the Unity Scene hierarchy
      * cameraName="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
      * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with a name that contains partOfTheName (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```java
    altUnityDriver.waitForElementWhereNameContains("Capsul", timeout=30); // should find Capsule     
    ```

  * `waitForElementToNotBePresent`
   * params: 
      * name - the name of the object, as it's shown in the Unity Scene hierarchy
      * cameraName=""="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
      * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)

    ```java
    altUnityDriver.waitForElementToNotBePresent("Capsule") ;
    ``` 

  * `waitForElementWithText`
    * params: 
      * text - the text that we want to wait for (we are looking for an element with a Text component that has the correct value)
      * cameraName=""="" - the name of the camera for wich the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
      * enabled - true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
   
    ```java
        altUnityDriver.waitForElementWithText("CapsuleInfo", "Capsule was clicked to jump!")  ;
    ``` 
  

#### Managing Unity Scenes
  * `getCurrentScene`
    * params: none
    * returns: the name of the current scene

    ```java
    altUnityDriver.getCurrentScene();
    ```
  * `waitForCurrentSceneToBe`
   * params: 
      * sceneName - the scene that we want to wait for 
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the name of the scene that we waited for

  ```java
    altUnityDriver.waitForCurrentSceneToBe("AltUnityDriverTestScene");
  ``` 
* `loadScene`
    * params: scene - name of the scene to be loved

     ```java
    altUnityDriver.loadScene("AltUnityDriverTestScene");
  ``` 

#### Managing Unity PlayerPrefs
* `deletePlayerPref`
    * params: none
    * returns: none
    * Delete all keys and values stored in PlayerPref

    ```java
    altUnityDriver.deletePlayerPref();
    ``` 
   
* `deleteKeyPlayer`
    * params: keyName - name of the key that will be deleted
    * returns: none

     ```java
    altUnityDriver.deleteKeyPlayer("PlayerHp");
    ``` 
    
* `setKeyPlayerPref`
    * params:
        * keyName - name of the key for wich a value will be set
        * value - value that will be associated with keyName. This can be an integer, float or a string
    * returns: none

    ```java
    altUnityDriver.setKeyPlayerPref("PlayerHp",100);
    ``` 
   
  * `getIntPlayerPref`
    * params: keyName -name of the key 
    * returns: an int that is associated with the key

    ```java
    altUnityDriver.getIntPlayerPref("PlayerHp");
     ``` 
   
* `getFloatPlayerPref`
    * params: keyName -name of the key 
    * returns: an float that is associated with the key

    ```java
    altUnityDriver.getFloatPlayerPref("PlayerMana");
    ``` 

  * `GetStringPlayerPref`
    * params: keyName -name of the key 
    * returns: an string that is associated with the key

    ```java
    altUnityDriver.GetStringPlayerPref("PlayerName");
    ``` 


#### Call static methods

* `CallStaticMethods`
    * params:
        * componentName: name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * method - the name of the public method that we want to call
        * parameters - a string containing the serialized parameters to be sent to the component method. This uses '?' to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]
        * typeOfParameters -  a string containing the serialized type of parameters to be sent to the component method. This uses '?' to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints 
    * return: string with the output from the method
    
        ```java
            altUnityDriver.callStaticMethods("UnityEngine.PlayerPrefs", "SetInt","Test?1");
        ``` 
       
       
#### Actions on screen

* `swipe`
	* params: 
		* start - position on the screen where the swipe will start
		* end - postion on the screen where the swipe will end
		* duration - how many seconds the swipe will need to complete
	* return: none
	* Use this method if more than one input is needed because this method will not wait until the swipe is completed to execute the next command. If you want to wait until the swipe is completed use `swipeAndWait`

    ```java
        AltUnityObject altElement1 = altUnityDriver.FindElement("Drag Image1");
        AltUnityObject altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);
    ``` 
	
* `swipeAndWait`
    * params: 
    	* start - position on the screen where the swipe will start
    	* end - postion on the screen where the swipe will end
    	* duration - how many seconds the swipe will need to complete
    * return: none
    * Use this method if you don't need more inputs to run until th swipe is completed because this method will wait until the swipe is completed to execute the next command. If you want to use more inputs or check something mid-swipe use `swipe`
    
    ```java
        AltUnityObject altElement1 = altUnityDriver.findElement("Drag Image1");
        AltUnityObject altElement2 = altUnityDriver.findElement("Drop Box1");
        altUnityDriver.swipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);
    ``` 
 
* `moveTouch`
    * params: 
        * positions - collection of positions on the screen where the swipe be made
        * duration - how many seconds the swipe will need to complete
    * return: none
    * Use this method if more than one input is needed because this method will not wait until the swipe is completed to execute the next command. If you want to wait until the swipe is completed use `moveTouchAndWait`

     ```java
         AltUnityObject altElement1 = altUnityDriver.FindElement("Drag Image1");
         AltUnityObject altElement2 = altUnityDriver.findElement("Drop Box1");
         List<Vector2> positions = Arrays.asList(
              altElement1.getScreenPosition(), 
              new Vector2(altElement2.x, altElement2.y));
          
         altUnityDriver.moveTouch(positions, 3);
     ``` 

* `moveTouchAndWait`
     * params: 
        * positions - collection of positions on the screen where the swipe be made
        * duration - how many seconds the swipe will need to complete
     * return: none
     * Use this method if you don't need more inputs to run until th swipe is completed because this method will wait until the swipe is completed to execute the next command. If you want to use more inputs or check something mid-swipe use `moveTouch`
     
     ```java
         AltUnityObject altElement1 = altUnityDriver.findElement("Drag Image1");
         AltUnityObject altElement2 = altUnityDriver.findElement("Drop Box1");
         List<Vector2> positions = Arrays.asList(
              altElement1.getScreenPosition(), 
              new Vector2(altElement2.x, altElement2.y));
          
         altUnityDriver.moveTouchAndWait(positions, 3);
     ``` 
   
* `holdButton`
	* params: 
		* position - (x,y) coordinates on the screen where a touch will be simulated
		* duration - how many seconds the touch will exist/be pressing 
	* return: none
	*Use this method if more than one input is needed because this method will not wait until the swipe is completed to execute the next command.If you want to wait until the hold is completed use `holdButtonAndWait`

 ```java
        AltUnityObject altElement1 = altUnityDriver.findElement("Button");
        altUnityDriver.holdButton(new Vector2(altElement1.x, altElement1.y), 2);
  ``` 
 
  * `holdButtonAndWait`
	* params: 
		* position - (x,y) coordinates on the screen where a touch will be simulated
		* duration - how many seconds the touch will exist/be pressing 
	* return: none
	* Use this method if you don't need more inputs to run until the action is completed because this method will wait until the action is completed to execute the next command. If you want to use more inputs or check something while holding use `holdButton`

 ```java
      AltUnityObject altElement1 = altUnityDriver.findElement("Button");
      altUnityDriver.holdButtonAndWait(new Vector2(altElement1.x, altElement1.y), 2);
  ``` 
 
  * `tapScreen`
	* params: (x,y) - coordinates on the screen where it will be simulated a tap
	* return: the element that received the tap

 ```java
     altUnityDriver.tapScreen(100,200);
  ``` 
 
  * `tilt`
	* params: acceleration - (x,y,z) values to simulate the device rotation
	*return: none

 ```java
     altUnityDriver.tilt(new Vector3(2, 2, 2));
  ``` 
 
#### Actions on elements
  * `clickEvent`
	* params: none
	* Execute pointerClick event on the object
 
 ```java
     altUnityDriver.findElement("Capsule").clickEvent();
  ``` 
  
 * `dragObject`
	* params: position - (x,y) coordinates of the screen where the object will be dragged
	* Execute drag event on the object
 
 ```java
      altUnityDriver.findElement("Capsule").dragObject(new Vector2(200, 200));
  ``` 
  
  * `dropObject`
	* params: (x,y) coordinates of the screen where the object will be dropped
	* Execute drop event on the object
 
 ```java
     altUnityDriver.findElement("Capsule").dropObject(new Vector2(200, 200));
  ``` 
  
  * `pointerUpFromObject`
	* params: none
	* Execute pointerUp event on the object

 ```java
      altUnityDriver.findElement("Capsule").pointerUpFromObject();
  ``` 
  
  * `pointerDownFromObject`
	* params: none
	* Execute pointerDown event on the object

 ```java
     altUnityDriver.findElement("Capsule").pointerDownFromObject();
  ``` 
  
  * `pointerEnterObject`
	* params: none
	* Execute pointerEnter event on the object

    ```java
      altUnityDriver.findElement("Capsule").pointerEnterObject();
    ```
  
  * `pointerExitObject`
	* params: none
	* Execute pointerExit event on the object
	
    ```java
      altUnityDriver.findElement("Capsule").pointerExitObject();
    ``` 

* `tap`
    * params: none
    * simulates a tap on the object that trigger multiple events similar to a real tap 
    
    ```java
    altUnityDriver.findElement("UIButton").tap();
    ``` 

  * `getText`
    * params: none
    * returns: the value of the Text component if the element has one 
  
   ```java
   String text=altUnityDriver.findElement("CapsuleInfo").getText();'
  ``` 
  
* `getComponentProperty`
    * params: 
        * componentName: name of the Unity component that has the public property we want to get the value for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * propertyName - the name of the public property (or field) that we want the value for
        * assembly - name of the assembly where the component is.(This is optional parameter, most of the time should work without this) 
   For example, since Capsule.cs has a public "arrayOfInts", we can get the value of that:

   ```java
    String result = altUnityDriver.findElement("Capsule").getComponentProperty("Capsule", "arrayOfInts");
   ```
   
* `setComponentProperty`
    * params: 
        * componentName: name of the Unity component that has the public property we want to set the value for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * propertyName - the name of the public property (or field) that we want to set the value for
        * value - the value that we want to set. This will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.
        * assembly - name of the assembly where the component is.(This is optional parameter, most of the time should work without this) 
   For example, since Capsule.cs has a public "arrayOfInts", we can set the value of that:

   ```java
   altUnityDriver.findElement("Capsule").setComponentProperty("Capsule", "arrayOfInts", "[2,3,4]");
   result = altUnityDriver.findElement("Capsule").getComponentProperty("Capsule", "arrayOfInts");
   ```

* `callComponentMethod`
    * params: 
        * componentName: name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * method - the name of the public method that we want to call
        * parameters - a string containing the serialized parameters to be sent to the component method. This uses '?' to separate between parameters, like this:
        'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]
        Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.
        * typeOfParameters -  a string containing the serialized type of parameters to be sent to the component method. This uses '?' to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints 
        * assembly - name of the assembly where the component is.(This is optional parameter, most of the time should work without this) 

    
   For example, since Capsule.cs has a public "Jump" method that takes a string as a parameter, we can call it like this:

   ```java
   altUnityDriver.findElement("Capsule").callComponentMethod("Capsule", "Jump", "setFromMethod");
   ```

   This calls `jump("setFromMethod)` in java

   If the Capsule.cs also has the following method:
 
   ```c#
   public void TestMethodWithManyParameters(int param1, string param2, float param, int[] arrayOfInts) {
   ```
  
   we can call that by:
 
   ```java
   altUnityDriver.findElement("Capsule").callComponentMethod("Capsule", "TestMethodWithManyParameters", "1?this is a text?0.5?[1,2,3]");
   ```

   This will call `TestMethodWithManyParameters(1, "this is a text", 0,5, new int[]{1, 2, 3})` 

    #### Keyboard and Mouse

  * `pressKey`
    * params: 
      * keyName: Name of the buttons. Check https://docs.unity3d.com/ScriptReference/KeyCode.html to see what how the button are named. For exemple if you want to press left arrow key then you will need to write "LeftArrow". It is case sensitive.
      * duration: the time in seconds while the key is pressed
    * This method will not wait for the button to be released. To wait for the button to be release please use 'PressKeyAndRelease'
    
   
   ```java
    altUnityDriver.pressKey("K", 2);
   ```
  * `pressKeyAndWait`
      * params: 
        * keyName: Unity Enum that maps what button is pressed. Check https://docs.unity3d.com/ScriptReference/KeyCode.html to see what buttons are available
        * duration: the time in seconds while the key is pressed
      * This method will wait for the button to be released. If you don't want to wait for the button to be release please use 'PressKey'
      
    
    ```java
      altUnityDriver.pressKeyAndWait("Keypad0", 2);
    ```
  * `moveMouse`
      * params: 
        * x: x coordinate of the screen where to move the mouse
        * y: y coordinate of the screen where to move the mouse
        * duration: the time in seconds for the mouse to move from the current position to the one given by location
      * This method will not wait for the mouse to get to the location. To wait for the mouse to get to the location please use 'PressKeyAndRelease'
      
    ```java
        altUnityDriver.moveMouse(800, 200, 1);
    ```
  * `moveMouseAndWait`
    * params: 
        * x: x coordinate of the screen where to move the mouse
        * y: y coordinate of the screen where to move the mouse
        * duration: the time in seconds for the mouse to move from the current position to the one given by location
    * This method will wait for the mouse to get to the location. If you don't want to wait please use 'MoveMouse'
    
   
   ```java
        altUnityDriver.moveMouseAndWait(800, 200, 1);
   ```
  * `scrollMouse`
      * params: 
        * speed: Positive values simulate scrolling up and negative values simulate scrolling down
        * duration: the time in seconds while the scroll is applied
      * This method will not wait for the scroll to be completed. If you want to wait please use 'ScrollMouseAndWait'
      
    
    ```java
       altUnityDriver.scrollMouse(3, 2);
    ```
  * `scrollMouseAndWait`
      * params: 
        * speed: Positive values simulate scrolling up and negative values simulate scrolling down
        * duration: the time in seconds while the scroll is applied
      * This method will wait for the scroll to be completed. If you don't want to wait please use 'ScrollMouse'
      
    
    ```java
       altUnityDriver.scrollMouseAndWait(3, 2);
    ```
