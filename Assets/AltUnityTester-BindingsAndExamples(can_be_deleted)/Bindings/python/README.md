# AltUnityTester Python Bindings

The AltUnityTester package contains an alt_driver module that will open a socket connection on the device running the Unity application and will give access to all the objects in the Unity hierarchy. 

Using this socket connection and the actions available in the AltUnity driver, we can run python tests tests against the Unity app running on iOS or Android. 

Links:

* [Setup AltUnityTester and configure your app for testing](https://gitlab.com/altom/altunitytester/)
* [Downloads](#downloads-altunitytester-package)
* [Python Docs - AltUnityRunner - Getting started](#python-altunityrunner-module)
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


## Downloads - AltUnityTester Package

* From repository: 
   * https://gitlab.com/altom/altunitytester/blob/master/AltUnityTester.unitypackage
  
  
* From Unity Asset Store - import inside your project directly:
   * links soon


## Python AltUnityRunner module

The project contains a Python module called ``altunityrunner`` that gives access to the alt_driver commands so that objects can be accessed from  Python scripts. 

The code for this is available under ``AltUnityTester/Bindings/python`` in the repository. 


### Installation

   * using pip

      ``pip install altunityrunner``

   * from the source code in the repo

      ``cd <project-dir>/Assets/AltUnityTester/Bindings/python``
     
      ``python setup.py install``


### Getting Started

To start using AltUnityRunner from your Appium scripts you need to:

1. import the alt_driver 

   ```python
      from altunityrunner import AltrunUnityDriver
   ```

2. instatiate it in your test/suite setup phase:

   ```python
      self.altdriver = AltrunUnityDriver()
   ```

   or, if you want to also use Appium:

   ```python
      self.altdriver = AltrunUnityDriver(self.appiumDriver)
   ```

3. start finding objects and testing your app:

   ```python
      self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule Info')
   ```

See the Examples section for more detailed examples of alt_driver in use. 

The list below contains all the actions that are currently supported by alt_driver. 

### AltElements

All elements in AltUnityTester have the following structure, as seen in the AltElement class:

  * `alt_unity_driver` - a reference to the current alt_driver
  * `appium_driver` - a reference to the current Appium driver used for actions like tap and drag (can be null)
  * `name` - the name of the object as it is in the Unity Scene Hierarchy
  * `idCamera` - the name of the camera from which the screen coordinate are calculated
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

  * `get_all_elements`
       * params: camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
       * returns: all elements that are currently Active in the scene
    
    ```python
    assert len(self.altdriver.get_all_elements())  10
    ```

  * `find_element`
    * params:
        * name - the name of the object to be found, as it's shown in the Unity Scene hierarchy
        * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    * you can search for elements also by specifying a hierarchy path to them. For example, you can look for `Player1/Hand` or `Player2/Hand`, to make sure you find the correct `Hand` object you are interested in. When doing so, make sure you specify all the objects in between the `parent` and the `object` you are interested in. For example, if `Hand` is under a `Body` element for each `Player`, when you search for it make sure you specify it as `Player1/Body/Hand` 
    
    ```python
    self.altdriver.find_element('Capsule') # find object by name
    self.altdriver.find_element('Ship/Main/Capsule') #specify also the name of the parents
    ```


  * `find_element_where_name_contains`
    * params: 
        * name - part of the name of the object to be found, as it's shown in the Unity Scene hierarchy
        * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
    * returns: the element with a name that contains partOfTheName (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.find_element_where_name_contains('Capsul') # should find Capsule     
    ```

  * `find_element_by_component`
    * params: 
        * component_name - the name of a Unity Component, for example a C# script that is attached to an element, like Collider2D etc. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
    * returns: the element with a componentName component (or the last one found in the hierarchy if more than one element with the same component is present)
    
    ```python
    assert self.altdriver.find_element_by_component("Capsule").name == "Capsule" 
    ```

  * `find_elements`
    * params: 
        * name - the name of the objects to be found, as they are shown in the Unity Scene hierarchy
        * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
    * returns: a list of elements with the correct name
    
    ```python
    self.altdriver.find_elements('Capsule')     
    ```

  * `find_elements_where_name_contains`
    * params: 
       * name - part of the name of the objects to be found, as they are shown in the Unity Scene hierarchy
       * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene. 
    * returns: a list of elements with a name that contains partOfTheName 
    
    ```python
    self.altdriver.find_elements_where_name_contains('Capsul') # should find Capsule, Capsules, Capsule1, Capsule 2 etc.     
    ```

  * `find_elements_by_component`
    * params: 
        * component_name - the name of a Unity Component, for example a C# script that is attached to an element, like Collider2D etc. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
    * returns: a list of elements with a componentName component

    ```python
    assert len(self.altdriver.find_elements_by_component("Plane")) == 2 # we have exactly two objects with a Plane comonent in the scene 
    ```

#### Waiting for elements
* `wait_for_element`
    * params: 
        * name - the name of the object to be found, as it's shown in the Unity Scene hierarchy
        * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
        * timeout=20 - time in seconds before we timeout (default 20)
        * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.wait_for_element('Capsule') #specify also the name of the parents
    ```
    
* `wait_for_element_where_name_contains`
    * params: 
      * name - part of the name of the object to be found, as it's shown in the Unity Scene hierarchy
      * camera_name="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with a name that contains partOfTheName (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.wait_for_element_where_name_contains('Capsul', timeout=30) # should find Capsule     
    ```
    
  * `wait_for_element_to_not_be_present`
   * params: 
      * name - the name of the object, as it's shown in the Unity Scene hierarchy
      * camera_name=""="" - the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.wait_for_element_to_not_be_present('Capsule') 
    ``` 

  * `wait_for_element_with_text`
    * params: 
      * text - the text that we want to wait for (we are looking for an element with a Text component that has the correct value)
      * camera_name=""="" - the name of the camera for wich the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!')  
    ``` 
  

#### Managing Unity Scenes
  * `get_current_scene`
    * params: none
    * returns: the name of the current scene

  ```python
    self.altdriver.get_current_scene()
  ``` 

  * `wait_for_current_scene_to_be`
   * params: 
      * scene_name - the scene that we want to wait for 
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the name of the scene that we waited for
  
  ```python
    self.altdriver.wait_for_current_scene_to_be('alt_driverTestScene')
  ``` 

  * `load_scene`
    * params: scene - name of the scene to be loved
  
  ```python
    self.altdriver.load_scene('alt_driverTestScene')
  ``` 
#### Managing Unity PlayerPrefs
  * `delete_player_prefs`
    * params: none
    * returns: none
    * Delete all keys and values stored in PlayerPref

     ```python
    alt_driver.delete_player_prefs()
    ``` 
   
  * `delete_player_pref_key`
    * params: key_name - name of the key that will be deleted
    * returns: none

    ```python
    alt_driver.delete_player_pref_key('PlayerHp')
    ``` 
    
  * `set_player_pref_key`
    * params:
        * key_name - name of the key for wich a value will be set
        * value - value that will be associated with keyName. This can be an integer, float or a string
        * type - keytype (PlayerPrefKeyType.Int, PlayerPrefKeyType.Float, PlayerPrefKeyType.String)

    * returns: none

    ```python
    alt_driver.set_player_pref_key('PlayerHp',100)
    ``` 
   
  * `get_player_pref_key`
    * params: 
      * key_name -name of the key 
      * type - keytype (PlayerPrefKeyType.Int, PlayerPrefKeyType.Float, PlayerPrefKeyType.String)
    * returns: an int that is associated with the key

     ```python
    alt_driver.get_player_pref_key('PlayerHp',PlayerPrefKeyType.Int)
    ``` 

#### Actions on screen

  * `swipe`
	* params: 
		* x_start - x position on the screen where the swipe will start
		* y_start - y position on the screen where the swipe will start
		* x_end - x postion on the screen where the swipe will end
		* y_end - y postion on the screen where the swipe will end
		* duration - how many seconds the swipe will need to complete
	* return: none
	* Use this method if more than one input is needed because this method will not wait until the swipe is completed to execute the next command. If you want to wait until the swipe is completed use `swipe_and_wait`

    ```python
        var altElement1 = alt_driver.find_element('Drag Image1')
        var altElement2 = alt_driver.find_element('Drop Box1')
        alt_driver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2)
    ```

  * `swipe_and_wait`
	* params: 
		* x_start - x position on the screen where the swipe will start
		* y_start - y position on the screen where the swipe will start
		* x_end - x postion on the screen where the swipe will end
		* y_end - y postion on the screen where the swipe will end
		* duration - how many seconds the swipe will need to complete
	* return: none
	* Use this method if you don't need more inputs to run until th swipe is completed because this method will wait until the swipe is completed to execute the next command. If you want to use more inputs or check something mid-swipe use `Swipe`

	```python
        var altElement1 = alt_driver.find_element("Drag Image1")
        var altElement2 = alt_driver.find_element("Drop Box1")
        alt_driver.swipe_and_wait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2)
    ``` 
 
  * `tap_at_coordinates`
	* params: x,y - coordinates on the screen where it will be simulated a tap
	* return: the element that received the tap

 ```python
     alt_driver.tap_at_coordinates(100,200)
  ``` 
 
  * `tilt`
	* params: acceleration - x,y,z values to simulate the device rotation
	* return: none

 ```python
     alt_driver.tilt(2, 2, 2)
  ``` 
 
#### Call static methods

* `CallStaticMethods`
    * params:
        * componentName: name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * method - the name of the public method that we want to call
        * parameters - a string containing the serialized parameters to be sent to the component method. This uses '?' to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]
        * typeOfParameters -  a string containing the serialized type of parameters to be sent to the component method. This uses '?' to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints 
        * assembly - name of the assembly where the component is.(This is optional parameter, most of the time should work without this) 
    * return: string with the output from the method
    
        ```python
            self.altdriver.call_static_methods("UnityEngine.PlayerPrefs", "SetInt","Test?1")
        ``` 

#### Actions on elements

  * `mobile_tap` 
   * params: durationInSeconds=0.5 - the duration of the tap
   * uses Appium to tap on the screen at the x and mobileY coordinates of the element 
  
  ```python
  self.altdriver.find_element('UIButton').mobile_tap()
  ``` 

  * `mobile_dragTo`
   * params: 
      * x, y - coordinates to drag to 
      * durationInSeconds=0.5 - duration for the drag action
   * uses Appium to swipe on the screen from the x and mobileY coordinates of the element to the x and y given
  
  ```python
  self.altdriver.find_element('UIButton').mobile_dragTo(100, 100)
  ``` 
  
  * `mobile_dragToElement`
    * params: 
      * another element
      * durationInSeconds=0.5 - duration for the drag action
   * uses Appium to swipe on the screen from the x and mobileY coordinates of the element to the x and y of the other element
  
  ```python
  other_button = self.altdriver.find_element('Button1')
  self.altdriver.find_element('Button2').mobile_dragToElement(other_button)
  ``` 

* `click_Event`
	* params: none
	* Execute pointerClick event on the object
 
 ```python
     self.alt_driver.find_element("Capsule").click_Event()
  ``` 
  
 * `drag`
	* params: x,y coordinates of the screen where the object will be dragged
	* Execute drag event on the object
 
 ```python
      alt_driver.find_element("Capsule").drag(200, 200)
  ``` 
  
  * `drop`
	* params: x,y coordinates of the screen where the object will be dropped
	* Execute drop event on the object
 
 ```python
     alt_driver.find_element("Capsule").drop(200, 200)
  ``` 
  
  * `pointer_up`
	* params: none
	* Execute pointerUp event on the object

 ```python
      alt_driver.find_element("Capsule").pointer_up()
  ``` 
  
  * `pointer_down`
	* params: none
	* Execute pointerDown event on the object

 ```python
     alt_driver.find_element("Capsule").pointer_down()
  ``` 
  
  * `pointer_enter`
	* params: none
	* Execute pointerEnter event on the object

	 ```python
      alt_driver.find_element("Capsule").pointer_enter()
    ```
  
  * `pointer_exit`
	* params: none
	* Execute pointerExit event on the object
	
    ```python
    alt_driver.find_element("Capsule").pointer_exit()
    ``` 

* `tap`
    * params: none
    * simulates a tap on the object that trigger multiple events similar to a real tap 
    
    ```python
    alt_driver.find_element("UIButton").tap()
    ``` 


* `get_text`
    * params: none
    * returns: the value of the Text component if the element has one (or "" if it doesn't)
    
    ```python
    assert self.altdriver.find_element('CapsuleInfo').get_text() == 'Capsule was clicked to jump!'
    ``` 
  
  * `get_component_property`
    * params: 
      * component_name: name of the Unity component that has the public property we want to get the value for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
      * property - the name of the public property (or field) that we want the value for
      * assembly - name of the assembly where the component is.(This is optional parameter, most of the time should work without this) 

   For example, since Capsule.cs has a public "arrayOfInts", we can get the value of that:

   ```python
   result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
   assert result == "[1,2,3]"
   ```
   
  * `set_component_property`
  * params: 
      * component_name: name of the Unity component that has the public property we want to set the value for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
      * property - the name of the public property (or field) that we want to set the value for
      * value - the value that we want to set. This will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.
      * assembly - name of the assembly where the component is.(This is optional parameter, most of the time should work without this) 

   For example, since Capsule.cs has a public "arrayOfInts", we can set the value of that:

   ```python
   self.altdriver.find_element("Capsule").set_component_property("Capsule", "arrayOfInts", "[2,3,4]")
   result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
   assert result == "[2,3,4]", "result was: " + result
   ```

  * `call_component_method`
   * params: 
        * component_name: name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. For more info: https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx
        * method - the name of the public method that we want to call
        * parameters - a string containing the serialized parameters to be sent to the component method. This uses '?' to separate between parameters, like this:
        'some string ? [1,2,3]' - this repesents two parameters "some string" and "[1,2,3]
        Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.
        * typeOfParameters -  a string containing the serialized type of parameters to be sent to the component method. This uses '?' to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints 
        * assembly - name of the assembly where the component is.(This is optional parameter, most of the time should work without this) 

   For example, since Capsule.cs has a public "Jump" method that takes a string as a parameter, we can call it like this:

   ```python
   self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
   ```
 
   This calls `Jump("setFromMethod)` in C#

   If the Capsule.cs also has the following method:
   ```python
   public void TestMethodWithManyParameters(int param1, string param2, float param, int[] arrayOfInts) {
   ```
   
   we can call that by:
   
   ```python
   self.altdriver.find_element("Capsule").call_component_method("Capsule", "TestMethodWithManyParameters", "1?this is a text?0.5?[1,2,3]")
   ```

   This will call `TestMethodWithManyParameters(1, "this is a text", 0,5, new int[]{1, 2, 3})` in C#

  
  