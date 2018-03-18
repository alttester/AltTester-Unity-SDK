# AltUnityTester

The AltUnityTester package contains a AltUnityDriver module that will open a socket connection on the device running the Unity application and will give access to all the objects in the Unity hierarchy. 

Using this socket connection and the actions available in the AltUnity driver, we can run Appium tests against the Unity app running on iOS or Android. 

Links:

* [Setup](#setup)
* [Downloads](#downloads-altunitytester-package)
* [Python Docs - AltUnityRunner - Getting started](#python-altunityrunner-module)
* [Python Docs - Available Actions and Exmaples](#available-actions)

## Codacy info

* [![Codacy Badge](https://api.codacy.com/project/badge/Grade/cfefb05c67484170b69def598ef2989a)](https://www.codacy.com/app/Altom/altunitytester?utm_source=gitlab.com&amp;utm_medium=referral&amp;utm_content=altom/altunitytester&amp;utm_campaign=Badge_Grade)

## Setup

Using the AltUnityDriver requires a bit of setup in your Unity app and a few prerequisites on the machine that will be running the Appium tests. 

### A Getting Started Video tutorial

Follow the link below for a short tutorial on how to get started with AltUnityTester:

https://www.youtube.com/watch?v=uTO-Uzt3AEQ

### Setting up your Unity Game/App

To use the AltUnityDriver, you need to:

1. Import the AltUnityTester asset/package into your Unity project:
  * if you use a downloaded Unity package, go to Assets->Import Package -> Custom Package in Unity Editor and select the ```AltUnityTester.unitypackage``` file
  * if you dowloaded it from the Unity Asset store, just go to your Asset Store Downloads Manager from Unity Editor and import the package. 

2. Add the AltUnityDriver prefab (from AltUnityTester/Prefab folder) to your main scene in the game/app, the one that will be first shown to the users. By default, this will use ```13000``` as the port for the socket connection. 
   If you want to change this, you can update the port value number in Unity Editor in the Inspector.


### Setting up your local machine

To run your tests on iOS and Android, you need to have Appium installed. 
Info on that is available here: http://appium.io/docs/en/about-appium/getting-started

For iOS, to run the tests on real iOS device, please make sure you also go through this:
http://appium.io/docs/en/drivers/ios-xcuitest/ 

To connect to the AltUnityDriver socket connection, with the device connected to your computer, you will need to forward the tcp port from the device to your computer:

To do this on Android, use:

```adb forward tcp:13001 tcp:13000```
where first port (13001 in the example above) is the port on your computer and the second port (13000 in the example above) is the AltUnityDriver port. This is 13000 by default.

To do this on iOS, use:

```iproxy 13002 13000```
where first port (13002 in the example above) is the port on your computer and the second port (13000 in the example above) is the AltUnityDriver port. This is 13000 by default.

The ```iproxy``` command is installed as part of the libimobiledevice package that you should have already installed when setting up your iOS environment (http://appium.io/docs/en/drivers/ios-xcuitest-real-devices/)

## Downloads - AltUnityTester Package

* From repository: 
   * https://git.altom.ro/altrun/altunity-tester/blob/master/AltUnityTester.unitypackage
  
  
* From Unity Asset Store - import inside your project directly:
   * links soon


## Python 2.7 AltUnityRunner module

The project contains a Python 2.7 module called ``altunityrunner`` that gives access to the AltUnityDriver commands so that objects can be accessed from Appium Python scripts. 

The code for this is available under ``AltUnityTester/Bindings/python`` in the repository. 

Python 3 support will be added soon. 

### Installation

   * using pip

      ``pip install altunityrunner``

   * from the source code in the repo

      ``cd <project-dir>/Assets/AltUnityTester/Bindings/python``
     
      ``python setup.py install``


### Getting Started

To start using AltUnityRunner from your Appium scripts you need to:

1. import the AltUnityDriver 

   ```python
      from altunityrunner import AltrunUnityDriver
   ```

2. instatiate it in your test/suite setup phase:

   ```python
      self.altdriver = AltrunUnityDriver(self.driver)
   ```

3. start finding objects and testing your app:

   ```python
      self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule Info')
   ```

See the Examples section for more detailed examples of AltUnityDriver in use. 

The list below contains all the actions that are currently supported by AltUnityDriver. 

### AltUnityElements

All elements in AltUnityTester have the following structure, as seen in the AltUnityObject class:

  * `alt_unity_driver` - a reference to the current AltUnityDriver
  * `appium_driver` - a reference to the current Appium driver, used for actions like tap and drag
  * `name` - the name of the object as it is in the Unity Scene Hierarchy
  * `id` - the Unity Instance ID, this is unique for each element in the scene
  * `x` - the x coordinate of the middle of the element on screen
  * `y` - the y coordinate of the middle of the element on screen
  * `mobileY` - the y coordinate of the middle of the element on a mobile screen
  * `text` - the value of the Unity Text component if the element has one (or "" if it doesn't)
  * `type` - "scene" for Unity scenes and "" for all other elements


### Available Actions

#### Finding elements

  * `get_all_elements`
    * params: none
    * returns: all elements that are currently Active in the scene
    
    ```python
    assert len(self.altdriver.get_all_elements()) > 10
    ```

  * `find_element`
    * params: name - the name of the object to be found, as it's shown in the Unity Scene hierarchy
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.find_element('Capsule') # find object by name
    self.altdriver.find_element('Ship/Main/Capsule') #specify also the name of the parents
    ```

  * `find_element_where_name_contains`
    * params: part_of_the_name - part of the name of the object to be found, as it's shown in the Unity Scene hierarchy
    * returns: the element with a name that contains part_of_the_name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.find_element_where_name_contains('Capsul') # should find Capsule     
    ```

  * `find_element_by_component`
    * params: component_name - the name of a Unity Component, for example a C# script that is attached to an element, like Collider2D etc. 
    * returns: the element with a component_name component (or the last one found in the hierarchy if more than one element with the same component is present)
    
    ```python
    assert self.altdriver.find_element_by_component("Capsule").name == "Capsule" 
    ```

  * `find_elements`
    * params: name - the name of the objects to be found, as they are shown in the Unity Scene hierarchy
    * returns: an array of elements with the correct name
    
    ```python
    self.altdriver.find_elements('Capsule')     
    ```

  * `find_elements_where_name_contains`
   * params: part_of_the_name - part of the name of the objects to be found, as they are shown in the Unity Scene hierarchy
    * returns: an array of elements with a name that contains part_of_the_name 
    
    ```python
    self.altdriver.find_elements_where_name_contains('Capsul') # should find Capsule, Capsules, Capsule1, Capsule 2 etc.     
    ```

  * `find_elements_by_component`
    * params: component_name - the name of a Unity Component, for example a C# script that is attached to an element, like Collider2D etc. 
    * returns: an array of elements with a component_name component
    
    ```python
    assert len(self.altdriver.find_elements_by_component("Plane")) == 2 # we have exactly two objects with a Plane comonent in the scene 
    ```

#### Waiting for elements
  * `wait_for_element`
   * params: 
      * name - the name of the object to be found, as it's shown in the Unity Scene hierarchy
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.wait_for_element('Capsule') #specify also the name of the parents
    ```
    
  * `wait_for_element_where_name_contains`
    * params: 
      * part_of_the_name - part of the name of the object to be found, as it's shown in the Unity Scene hierarchy
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with a name that contains part_of_the_name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.wait_for_element_where_name_contains('Capsul', timeout=30) # should find Capsule     
    ```
    
  * `wait_for_element_to_not_be_present`
   * params: 
      * name - the name of the object, as it's shown in the Unity Scene hierarchy
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
    self.altdriver.wait_for_element_to_not_be_present('Capsule') 
    ``` 

  * `wait_for_element_with_text`
    * params: 
      * text - the text that we want to wait for (we are looking for an element with a Text component that has the correct value)
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the element with the correct name (or the last one found in the hierarchy if more than one element with the same name is present)
    
    ```python
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!')  
    ``` 
  

#### Unity Scenes
  * `get_current_scene`
    * params: none
    * returns: the name of the current scene
  * `wait_for_current_scene_to_be`
   * params: 
      * scene_name - the scene that we want to wait for 
      * timeout=20 - time in seconds before we timeout (default 20)
      * interval=0.5 - how often to check again to see if the element is there (default 0.5)
    * returns: the name of the scene that we waited for
  
  ```python
    self.altdriver.wait_for_current_scene_to_be('AltUnityDriverTestScene')
  ``` 

  
#### Actions on elements

  * `tap` 
   * params: none
   * uses Appium to tap on the screen at the x and mobileY coordinates of the element 
  
  ```python
  self.altdriver.find_element('UIButton').tap()
  ``` 

  * `dragTo`
   * params: x, y (coordinate)
   * uses Appium to swipe on the screen from the x and mobileY coordinates of the element to the x and y given
  
  ```python
  self.altdriver.find_element('UIButton').dragTo(100, 100)
  ``` 
  
  * `dragToElement`
    * params: another element
   * uses Appium to swipe on the screen from the x and mobileY coordinates of the element to the x and y of the other element
  
  ```python
  other_button = self.altdriver.find_element('Button1')
  self.altdriver.find_element('Button2').dragToElement(other_button)
  ``` 

  * `get_text`
    * params: none
    * returns: the value of the Text component if the element has one (or "" if it doesn't)
  
   ```python
  assert self.altdriver.find_element('CapsuleInfo').get_text() == 'Capsule was clicked to jump!'
  ``` 
  
  * `get_component_property`
    * params: 
      * component_name: name of the Unity component that has the public property we want to get the value for
      * property - the name of the public property (or field) that we want the value for

   For example, since Capsule.cs has a public "arrayOfInts", we can get the value of that:

   ```python
   result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
   assert result == "[1,2,3]"
   ```
   
  * `set_component_property`
  * params: 
      * component_name: name of the Unity component that has the public property we want to set the value for
      * property - the name of the public property (or field) that we want to set the value for
      * value - the value that we want to set. This will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.

   For example, since Capsule.cs has a public "arrayOfInts", we can set the value of that:

   ```python
   self.altdriver.find_element("Capsule").set_component_property("Capsule", "arrayOfInts", "[2,3,4]")
   result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
   assert result == "[2,3,4]", "result was: " + result
   ```

  * `call_component_method`
   * params: 
      * component_name: name of the Unity component that has the public property we want to call a method for
      * method - the name of the public method that we want to call
      * parameters - a string containing the serialized parameters to be sent to the component method. This uses '?' to separate between parameters, like this:
      'some string ? [1,2,3]' - this repesents two parameters "some string" and "[1,2,3]
       Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.

   For example, since Capsule.cs has a public "Jump" method that takes a string as a parameter, we can call it like this:

   ```python
   self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
   ```
 
   This calls `Jump("setFromMethod)` in C#

   If the Capsule.cs also has the following method:
   ```C#
   public void TestMethodWithManyParameters(int param1, string param2, float param, int[] arrayOfInts) {
   ```
   
   we can call that by:
   
   ```python
   self.altdriver.find_element("Capsule").call_component_method("Capsule", "TestMethodWithManyParameters", "1?this is a text?0.5?[1,2,3]")
   ```

   This will call `TestMethodWithManyParameters(1, "this is a text", 0,5, new int[]{1, 2, 3})` in C#

  
  