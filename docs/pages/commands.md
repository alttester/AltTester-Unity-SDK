# Commands
## FindObjects

###  FindObject

Find the first object in the scene that respects the given criteria. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes   | Set what criteria to use in order to find the object|
| value         | string       |   Yes   | The value to which object will be compared to see if they respect the criteria or not|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

***Returns***
- AltUnityObject

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindElement()
        {
            const string name = "Capsule";
            var altElement = altUnityDriver.FindObject(By.NAME,name);
            Assert.NotNull(altElement);
            Assert.AreEqual(name, altElement.name);
        }

    .. code-tab:: java
    
        @Test
        public void testfindElement() throws Exception
        {
            String name = "Capsule";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                    name).isEnabled(true).withCamera(AltUnityDriver.By.NAME,"Main Camera").build();
            AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
            assertNotNull(altElement);
            assertEquals(name, altElement.name);
        }

    .. code-tab:: py

       def test_find_objects_by_tag(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        altElements = self.altdriver.find_objects(By.TAG,"plane")
        self.assertEquals(2, len(altElements))
        for altElement in altElements: 
        self.assertEquals("Plane", altElement.name)
```

###  FindObjects

Find all objects in the scene that respects the given criteria. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes  | Set what criteria to use in order to find the object|
| value         | string       |   Yes   | The value to which object will be compared to see if they respect the criteria or not|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

***Returns***
- List of AltUnityObjects/ empty list if no objects were found

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindObjectsByTag()
        {
            var altElements = altUnityDriver.FindObjects(By.TAG,"plane");
            Assert.AreEqual(2, altElements.Count);
            foreach(var altElement in altElements)
            {
                Assert.AreEqual("Plane", altElement.name);
            }
        }

    .. code-tab:: java

           @Test
            public void testfindElements() throws Exception
            {
                String name = "Plane";
                AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                    name).isEnabled(true).withCamera(AltUnityDriver.By.NAME,"Main Camera").build();
                AltUnityObject[] altElements = altUnityDriver.findObjects(altFindObjectsParameters);
                assertNotNull(altElements);
                assertEquals(altElements[0].name, name);
            }

    .. code-tab:: py

        def test_find_objects_by_layer(self):
                self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
                altElements = self.altdriver.find_objects(By.LAYER,"Default")
                self.assertEquals(8, len(altElements))

```

###  FindObjectWhichContains

Find the first object in the scene that respects the given criteria. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes   | Set what criteria to use in order to find the object|
| value         | string       |   Yes  | The value to which object will be compared to see if they respect the criteria or not|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

***Returns***
- List of AltUnityObjects/ empty list if no objects were found

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

       [Test]
        public void TestFindObjectWhichContains()
        {
            var altElement = altUnityDriver.FindObjectWhichContains(By.NAME, "Event");
            Assert.AreEqual("EventSystem", altElement.name);
        }


    .. code-tab:: java

        @Test
        public void TestFindObjectWhichContains()
        {
            String name = "Event";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                   name).isEnabled(true).withCamera(AltUnityDriver.By.NAME,"Main Camera").build();
            AltUnityObject altElement = altUnityDriver.findObjectWhichContains(altFindObjectsParameters);
            assertEquals("EventSystem", altElement.name);
        }

    .. code-tab:: py

       def test_find_object_which_contains(self):
        altElement = self.altdriver.find_object_which_contains(By.NAME, "Event");
        self.assertEqual("EventSystem", altElement.name)

```


###  FindObjectsWhichContains

Find all objects in the scene that respects the given criteria. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes   | Set what criteria to use in order to find the object|
| value         | string       |   Yes   | The value to which object will be compared to see if they respect the criteria or not|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

***Returns***
- List of AltUnityObjects/ empty list if no objects were found

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        {
           AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
    
           var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star","Player2");
           var player = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Player", "Player2");
            Assert.AreEqual(1, stars.Count);
    
           AltUnityDriver.MoveMouse(new UnityEngine.Vector2(player[0].x, player[0].y+500), 1);
           UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
           Thread.Sleep(1500);
    
           AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0, 0);
           AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(player[0].x, player[0].y-500), 1);
           Thread.Sleep(1500);
           AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0, 1);
    
           stars = AltUnityDriver.FindObjectsWhichContain(By.NAME,"Star");
           Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java

        @Test
        public void testFindElementsWhereNameContains() throws Exception
        {
            String name = "Pla";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject[] altElements = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertNotNull(altElements);
            assertTrue(altElements[0].name.contains(name));
        }

    .. code-tab:: py

        def test_creating_stars(self):
                self.altdriver.load_scene("Scene 5 Keyboard Input")
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star","Player2")
                self.assertEqual(1, len(stars))
                player = self.altdriver.find_objects_which_contains(By.NAME,"Player","Player2")
        
                self.altdriver.move_mouse(int(stars[0].x),int(player[0].y)+500, 1)
                time.sleep(1.5)
        
                self.altdriver.press_key('Mouse0', 1,0)
                self.altdriver.move_mouse_and_wait(int(stars[0].x),int(player[0].y)-500, 1)
                self.altdriver.press_key('Mouse0', 1,0)
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star")
                self.assertEqual(3, len(stars))
```

###  GetAllElements

Returns information about every objects loaded in the currently loaded scenes. This also means objects that are set as DontDestroyOnLoad.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No  | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

***Returns***
- List of AltUnityObjects/ empty list if no objects were found

***Examples***
```eval_rst
.. tabs::    

    .. code-tab:: c#
    
        [Test]
        public void TestGetAllEnabledElements()
        {
        
            var altElements = altUnityDriver.GetAllElements(enabled: true);
            Assert.IsNotEmpty(altElements);
            string listOfElements="";
                foreach(var element in altElements){
                listOfElements=element.name+"; ";
            }
            Debug.Log(listOfElements);
            Assert.AreEqual(19, altElements.Count);
            Assert.IsNotNull(altElements.Where(p => p.name == "Capsule"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Main Camera"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Directional Light"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Plane"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Canvas"));
            Assert.IsNotNull(altElements.Where(p => p.name == "EventSystem"));
            Assert.IsNotNull(altElements.Where(p => p.name == "AltUnityRunner"));
            Assert.IsNotNull(altElements.Where(p => p.name == "CapsuleInfo"));
            Assert.IsNotNull(altElements.Where(p => p.name == "UIButton"));
            Assert.IsNotNull(altElements.Where(p => p.name == "Text"));
        }

    .. code-tab:: java 

        @Test
        public void testGetAllElements() throws Exception {
            AltGetAllElementsParameters altGetAllElementsParameters = new AltGetAllElementsParameters.Builder().withCamera(AltUnityDriver.By.NAME,"Main Camera").isEnabled(true).build();
            AltUnityObject[] altElements = altUnityDriver.getAllElements(altGetAllElementsParameters);
            assertNotNull(altElements);
            String altElementsString = new Gson().toJson(altElements);
            assertTrue(altElementsString.contains("Capsule"));
            assertTrue(altElementsString.contains("Main Camera"));
            assertTrue(altElementsString.contains("Directional Light"));
            assertTrue(altElementsString.contains("Plane"));
            assertTrue(altElementsString.contains("Canvas"));
            assertTrue(altElementsString.contains("EventSystem"));
            assertTrue(altElementsString.contains("AltUnityRunnerPrefab"));
            assertTrue(altElementsString.contains("CapsuleInfo"));
            assertTrue(altElementsString.contains("UIButton"));
            assertTrue(altElementsString.contains("Text"));
        }

    .. code-tab:: py
    
        def test_get_all_elements(self):
            alt_elements = self.altdriver.get_all_elements(enabled= False)
            self.assertIsNotNone(alt_elements)
            
            list_of_elements=[]
            for element in alt_elements:
                list_of_elements.append(element.name)
            
            self.assertEqual(28, len(list_of_elements))
            self.assertTrue("Capsule" in list_of_elements)
            self.assertTrue("Main Camera" in list_of_elements)
            self.assertTrue("Directional Light" in list_of_elements)
            self.assertTrue("Plane" in list_of_elements)
            self.assertTrue("Canvas" in list_of_elements)
            self.assertTrue("EventSystem" in list_of_elements)
            self.assertTrue("AltUnityRunnerPrefab" in list_of_elements)
            self.assertTrue("CapsuleInfo" in list_of_elements)
            self.assertTrue("UIButton" in list_of_elements)
            self.assertTrue("Cube" in list_of_elements)
            self.assertTrue("Camera" in list_of_elements)
            self.assertTrue("InputField" in list_of_elements)


```

###  WaitForObject

Wait until there is no longer any objects that respect the given criteria or times run out and will throw an error. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes   | Set what criteria to use in order to find the object|
| value         | string       |   Yes  | The value to which object will be compared to see if they respect the criteria or not|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   No   | number of seconds that it will wait for object|
| interval        | double        |   No   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

***Returns***
- AltUnityObject

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
    
       [Test]
        public void TestWaitForObjectToNotExistFail()
        {
            try
            {
                altUnityDriver.WaitForObjectNotBePresent(By.NAME,"Capsule", timeout: 1, interval: 0.5f);
                Assert.Fail();
            }
            catch (WaitTimeOutException exception)
            {
                Assert.AreEqual("Element //Capsule still found after 1 seconds", exception.Message);
            }
        }

    .. code-tab:: java

        @Test
        public void TestWaitForObjectWithCameraId() {
            AltFindObjectsParameters altFindObjectsParametersButton = new AltFindObjectsParameters.Builder(
                    AltUnityDriver.By.PATH, "//Button").build();
            AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParametersButton);
            altButton.clickEvent();
            altButton.clickEvent();
            AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                    "//Camera").build();
            AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);
            AltFindObjectsParameters altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.COMPONENT,
                    "CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
            AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                    altFindObjectsParametersCapsule).build();
            AltUnityObject altElement = altUnityDriver.waitForObject(altWaitForObjectsParameters);

            assertTrue("True", altElement.name.equals("Capsule"));

            altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH, "//Main Camera").build();
            AltUnityObject camera2 = altUnityDriver.findObject(altFindObjectsParametersCamera);
            altFindObjectsParametersCapsule = new AltFindObjectsParameters.Builder(By.COMPONENT, "CapsuleCollider")
                    .withCamera(By.ID, String.valueOf(camera2.id)).build();
            altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(altFindObjectsParametersCapsule).build();
            AltUnityObject altElement2 = altUnityDriver.waitForObject(altWaitForObjectsParameters);

            assertNotEquals(altElement.getScreenPosition(), altElement2.getScreenPosition());
        }

    .. code-tab:: py

        def test_wait_for_object(self):
            altElement=self.altdriver.wait_for_object(By.NAME,"Capsule")
            self.assertEqual(altElement.name,"Capsule")

```
###  WaitForObjectWhichContains

Wait until it finds an object that respect the given criteria or times run out and will throw an error. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes   | Set what criteria to use in order to find the object|
| value         | string       |   Yes   | The value to which object will be compared to see if they respect the criteria or not|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   No  | number of seconds that it will wait for object|
| interval        | double        |   No   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

***Returns***
- AltUnityObject

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForObjectWhichContains()
        {
            var altElement = altUnityDriver.WaitForObjectWhichContains(By.NAME, "Canva");
            Assert.AreEqual("Canvas", altElement.name);
        }
  
    .. code-tab:: java
    
         @Test

        public void TestWaitForObjectWhichContainsWithCameraId() {
            AltFindObjectsParameters altFindObjectsParametersCamera = new AltFindObjectsParameters.Builder(By.PATH,
                    "//Main Camera").build();
            AltUnityObject camera = altUnityDriver.findObject(altFindObjectsParametersCamera);

            AltFindObjectsParameters altFindObjectsParametersObject = new AltFindObjectsParameters.Builder(By.NAME, "Canva")
                    .withCamera(By.ID, String.valueOf(camera.id)).build();
            AltWaitForObjectsParameters altWaitForObjectsParameters = new AltWaitForObjectsParameters.Builder(
                    altFindObjectsParametersObject).build();
            AltUnityObject altElement = altUnityDriver.waitForObjectWhichContains(altWaitForObjectsParameters);
            assertEquals("Canvas", altElement.name);

        }

    .. code-tab:: py

        def test_wait_for_object_which_contains(self):
            altElement=self.altdriver.wait_for_object_which_contains(By.NAME,"Main")
            self.assertEqual(altElement.name,"Main Camera")
```
###  WaitForObjectWithText

Wait until it finds an object that respect the given criteria and it has the text you are looking for or times run out and will throw an error. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes   | Set what criteria to use in order to find the object|
| value         | string       |   Yes   | The value to which object will be compared to see if they respect the criteria or not|
| text    |   string  | Yes  | Text that the intented object should have|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   No   | number of seconds that it will wait for object|
| interval        | double        |   No   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

***Returns***
- AltUnityObject

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
    
        [Test]
        public void TestWaitForElementWithText()
        {
            const string name = "CapsuleInfo";
            string text = altUnityDriver.FindObject(By.NAME,name).GetText();
            var timeStart = DateTime.Now;
            var altElement = altUnityDriver.WaitForObjectWithText(By.NAME, name, text);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.GetText(), text);
        }

    .. code-tab:: java
    
        @Test
        public void testWaitForElementWithText() throws Exception {
                String name = "CapsuleInfo";
                AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera(AltUnityDriver.By.NAME,"Main Camera").build();
                String text = altUnityDriver.findObject(altFindObjectsParameters).getText();
                long timeStart = System.currentTimeMillis();
                AltWaitForObjectWithTextParameters altWaitForElementWithTextParameters = new AltWaitForObjectWithTextParameters.Builder(altFindObjectsParameters,text).withInterval(0).withTimeout(0).build();
                AltUnityObject altElement = altUnityDriver.waitForObjectWithText(altWaitForElementWithTextParameters);
                long timeEnd = System.currentTimeMillis();
                long time = timeEnd - timeStart;
                assertTrue(time / 1000 < 20);
                assertNotNull(altElement);
                assertEquals(altElement.getText(), text);      
            }

    .. code-tab:: py
    
        def test_wait_for_object_with_text(self):
            altElement=self.altdriver.wait_for_object_with_text(By.NAME,"CapsuleInfo","Capsule Info")
            self.assertEqual(altElement.name,"CapsuleInfo")

```
###  WaitForObjectNotBePresent

Wait until the object in the scene that respect the given criteria is no longer in the scene or times run out and will throw an error. Check [By](#by) for more information about criterias.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](#by)    |   Yes   | Set what criteria to use in order to find the object|
| value         | string       |   Yes  | The value to which object will be compared to see if they respect the criteria or not|
| cameraBy      |   [By](#by)     | No    |  Set what criteria to use in order to find the camera|
| cameraName      |     string    |   No   | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   No  | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   No   | number of seconds that it will wait for object|
| interval        | double        |   No   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
    
        [Test]
        public void TestWaitForObjectToNotExist()
        {
            altUnityDriver.WaitForObjectNotBePresent(By.NAME, "Capsulee", timeout: 1, interval: 0.5f);
        }

    .. code-tab:: java
        
        @Test
        public void TestWaitForObjectToNotBePresent(){
            AltFindObjectsParameters altFindObjectsParameters=new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,"Capsulee").build();
            AltWaitForObjectsParameters altWaitForObjectsParameters=new AltWaitForObjectsParameters.Builder(altFindObjectsParameters).build();
            altUnityDriver.waitForObjectToNotBePresent(altWaitForObjectsParameters);
        }


    .. code-tab:: py
    
        def test_wait_for_object_to_not_be_present(self):
            self.altdriver.wait_for_object_to_not_be_present(By.NAME,"Capsuule")


```
### By

It is used in find objects methods to set the criteria of which the objects are searched.  
Currenty there are 6 type implemented:
  * *Tag* - search for objects that have a specific tag
  * *Layer* - search for objects that are set on a specific layer
  * *Name* - search for objects that are named in a certain way
  * *Component* - search for objects that have certain component
  * *Id* - search for objects that has assigned certain id (every object has an unique id so this criteria always will return 1 or 0 objects)
  * *Path* - search for objects that respect a certain path


**Searching object by path**

The following selecting nodes, attributes and attributes are implemented:
  * *object* -	Selects all object with the name "object"
  * */* - 	Selects from the root node
  * *//* - Selects nodes in the document from the current node that match the selection no matter where they are
  * *..* - Selects the parent of the current node
  * *\** - 	Matches any element node
  * *@tag* - 
  * *@layer* -
  * *@name* -
  * *@component* -
  * *@id* -
  * *contains* -
  


How a correct path should look like:  
  ```//Canvas/Panel/*[@tag="UI"]```
  
**Examples**
 ```
//Button - Returns every object named button in the scene 
//*[@tag=UI] -Returns every object that is tagged as UI
/Canvas//Button[@component=ButtonLogic] - Return every button who are in an canvas that is a root object and has a component name ButtonLogic
//*[contains(@name,Ca)] - Returns every object in the scene that contains in the name "Ca"
```

 
## InputActions 

###  MoveMouseAndWait

Simulate mouse movement in your game. This command will wait for the movement to finish. If you don't want to wait until the mouse movement stops use [MoveMouse](#movemouse)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| location      |     Vector2    |   Yes   | The destination coordinates for mouse to go from the current mouse position|
| duration      |     float    |   Yes  | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestCreatingStars()
        {
            AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        
            var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star","Player2");
            var player = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Player", "Player2");
            Assert.AreEqual(1, stars.Count);
        
            AltUnityDriver.MoveMouse(new UnityEngine.Vector2(player[0].x, player[0].y+500), 1);
            UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
            Thread.Sleep(1500);
        
            AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0, 0);
            AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(player[0].x, player[0].y-500), 1);
            Thread.Sleep(1500);
            AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0, 1);
        
            stars = AltUnityDriver.FindObjectsWhichContain(By.NAME,"Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java
        @Test
        public void TestCreatingStars() throws InterruptedException 
        {
            String name = "Star";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(1, stars.length);

            AltUnityObject player=altUnityDriver.findElement("Player1","Player2");AltMoveMouseParameters altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y+500).withDuration(1).build();
            altUnityDriver.moveMouse(altMoveMouseParameters);
                
            Thread.sleep(1500);

            AltPressKeyParameters altPressKeyParameters=new AltPressKeyParameters.Builder("Mouse0").withPower(1).withDuration(1).build();
            altUnityDriver.pressKey(altPressKeyParameters);

            altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y-500).withDuration(1).build();
            altUnityDriver.moveMouseAndWait(altMoveMouseParameters);
            altUnityDriver.pressKeyAndWait(altPressKeyParameters);

            stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(3, stars.length);
        }

    .. code-tab:: py
        def test_creating_stars(self):
                self.altdriver.load_scene("Scene 5 Keyboard Input")
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star","Player2")
                self.assertEqual(1, len(stars))
                player = self.altdriver.find_objects_which_contains(By.NAME,"Player","Player2")
        
                self.altdriver.move_mouse(int(stars[0].x),int(player[0].y)+500, 1)
                time.sleep(1.5)
        
                self.altdriver.press_key('Mouse0', 1,0)
                self.altdriver.move_mouse_and_wait(int(stars[0].x),int(player[0].y)-500, 1)
                self.altdriver.press_key('Mouse0', 1,0)
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star")
                self.assertEqual(3, len(stars))

```
###  MoveMouse

Simulate mouse movement in your game. This command does not wait for the movement to finish. To also wait for the movement to finish use [MoveMouseAndWait](#movemouseandwait)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| location      |     Vector2    |   Yes  | The destination coordinates for mouse to go from the current mouse position|
| duration      |     float    |   Yes  | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestCreatingStars()
        {
            AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        
            var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star","Player2");
            var player = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Player", "Player2");
            Assert.AreEqual(1, stars.Count);
        
            AltUnityDriver.MoveMouse(new UnityEngine.Vector2(player[0].x, player[0].y+500), 1);
            UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
            Thread.Sleep(1500);
        
            AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0, 0);
            AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(player[0].x, player[0].y-500), 1);
            Thread.Sleep(1500);
            AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0, 1);
        
            stars = AltUnityDriver.FindObjectsWhichContain(By.NAME,"Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java
        @Test
        public void TestCreatingStars() throws InterruptedException 
        {
            String name = "Star";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(1, stars.length);

            AltUnityObject player=altUnityDriver.findElement("Player1","Player2");AltMoveMouseParameters altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y+500).withDuration(1).build();
            altUnityDriver.moveMouse(altMoveMouseParameters);
                
            Thread.sleep(1500);

            AltPressKeyParameters altPressKeyParameters=new AltPressKeyParameters.Builder("Mouse0").withPower(1).withDuration(1).build();
            altUnityDriver.pressKey(altPressKeyParameters);

            altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y-500).withDuration(1).build();
            altUnityDriver.moveMouseAndWait(altMoveMouseParameters);
            altUnityDriver.pressKeyAndWait(altPressKeyParameters);

            stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(3, stars.length);
        }

    .. code-tab:: py
        def test_creating_stars(self):
                self.altdriver.load_scene("Scene 5 Keyboard Input")
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star","Player2")
                self.assertEqual(1, len(stars))
                player = self.altdriver.find_objects_which_contains(By.NAME,"Player","Player2")
        
                self.altdriver.move_mouse(int(stars[0].x),int(player[0].y)+500, 1)
                time.sleep(1.5)
        
                self.altdriver.press_key('Mouse0', 1,0)
                self.altdriver.move_mouse_and_wait(int(stars[0].x),int(player[0].y)-500, 1)
                self.altdriver.press_key('Mouse0', 1,0)
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star")
                self.assertEqual(3, len(stars))

```
###  PressKeyAndWait

Simulate key press action in your game. This command waist for the action to finish. If you don't want to wait until the action to finish use [PressKey](#presskey)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| keycode      |     KeyCode(C#)/string(python/java)    |   Yes  | Name of the button. Please check [KeyCode for C#](https://docs.unity3d.com/ScriptReference/KeyCode.html) or [key section for python/java](https://docs.unity3d.com/Manual/ConventionalGameInput.html) for more information about key names|
| power      |     float    |   Yes   | A value from \[-1,1\] that defines how strong the key was pressed. This is mostly used for joystick button since the keyboard button will always be 1 or -1|
| duration      |     float    |   Yes   | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestCreatingStars()
        {
            AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        
            var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star","Player2");
            var player = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Player", "Player2");
            Assert.AreEqual(1, stars.Count);
        
            AltUnityDriver.MoveMouse(new UnityEngine.Vector2(player[0].x, player[0].y+500), 1);
            UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
            Thread.Sleep(1500);
        
            AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0, 0);
            AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(player[0].x, player[0].y-500), 1);
            Thread.Sleep(1500);
            AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0, 1);
        
            stars = AltUnityDriver.FindObjectsWhichContain(By.NAME,"Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java
        @Test
        public void TestCreatingStars() throws InterruptedException 
        {
            String name = "Star";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(1, stars.length);

            AltUnityObject player=altUnityDriver.findElement("Player1","Player2");AltMoveMouseParameters altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y+500).withDuration(1).build();
            altUnityDriver.moveMouse(altMoveMouseParameters);
                
            Thread.sleep(1500);

            AltPressKeyParameters altPressKeyParameters=new AltPressKeyParameters.Builder("Mouse0").withPower(1).withDuration(1).build();
            altUnityDriver.pressKey(altPressKeyParameters);

            altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y-500).withDuration(1).build();
            altUnityDriver.moveMouseAndWait(altMoveMouseParameters);
            altUnityDriver.pressKeyAndWait(altPressKeyParameters);

            stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(3, stars.length);
        }

    .. code-tab:: py
        def test_creating_stars(self):
                self.altdriver.load_scene("Scene 5 Keyboard Input")
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star","Player2")
                self.assertEqual(1, len(stars))
                player = self.altdriver.find_objects_which_contains(By.NAME,"Player","Player2")
        
                self.altdriver.move_mouse(int(stars[0].x),int(player[0].y)+500, 1)
                time.sleep(1.5)
        
                self.altdriver.press_key('Mouse0', 1,0)
                self.altdriver.move_mouse_and_wait(int(stars[0].x),int(player[0].y)-500, 1)
                self.altdriver.press_key('Mouse0', 1,0)
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star")
                self.assertEqual(3, len(stars))

```
###  PressKey

Simulate key press action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [PressKeyAndWait](#presskeyandwait)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| keycode      |     KeyCode(C#)/string(python/java)    |   Yes   | Name of the button. Please check [KeyCode for C#](https://docs.unity3d.com/ScriptReference/KeyCode.html) or [key section for python/java](https://docs.unity3d.com/Manual/ConventionalGameInput.html) for more information about key names|
| power      |     float    |   Yes   | A value from \[-1,1\] that defines how strong the key was pressed. This is mostly used for joystick button since the keyboard button will always be 1 or -1|
| duration      |     float    |   Yes   | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestCreatingStars()
        {
            AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
        
            var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star","Player2");
            var player = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Player", "Player2");
            Assert.AreEqual(1, stars.Count);
        
            AltUnityDriver.MoveMouse(new UnityEngine.Vector2(player[0].x, player[0].y+500), 1);
            UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
            Thread.Sleep(1500);
        
            AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0, 0);
            AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(player[0].x, player[0].y-500), 1);
            Thread.Sleep(1500);
            AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0, 1);
        
            stars = AltUnityDriver.FindObjectsWhichContain(By.NAME,"Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java
        @Test
        public void TestCreatingStars() throws InterruptedException 
        {
            String name = "Star";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(1, stars.length);

            AltUnityObject player=altUnityDriver.findElement("Player1","Player2");AltMoveMouseParameters altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y+500).withDuration(1).build();
            altUnityDriver.moveMouse(altMoveMouseParameters);
                
            Thread.sleep(1500);

            AltPressKeyParameters altPressKeyParameters=new AltPressKeyParameters.Builder("Mouse0").withPower(1).withDuration(1).build();
            altUnityDriver.pressKey(altPressKeyParameters);

            altMoveMouseParameters = new AltMoveMouseParameters.Builder(player.x, player.y-500).withDuration(1).build();
            altUnityDriver.moveMouseAndWait(altMoveMouseParameters);
            altUnityDriver.pressKeyAndWait(altPressKeyParameters);

            stars = altUnityDriver.findObjectsWhichContains(altFindObjectsParameters);
            assertEquals(3, stars.length);
        }

    .. code-tab:: py
        def test_creating_stars(self):
                self.altdriver.load_scene("Scene 5 Keyboard Input")
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star","Player2")
                self.assertEqual(1, len(stars))
                player = self.altdriver.find_objects_which_contains(By.NAME,"Player","Player2")
        
                self.altdriver.move_mouse(int(stars[0].x),int(player[0].y)+500, 1)
                time.sleep(1.5)
        
                self.altdriver.press_key('Mouse0', 1,0)
                self.altdriver.move_mouse_and_wait(int(stars[0].x),int(player[0].y)-500, 1)
                self.altdriver.press_key('Mouse0', 1,0)
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star")
                self.assertEqual(3, len(stars))

```
###  ScrollMouseAndWait

Simulate scroll mouse action in your game. This command waist for the action to finish. If you don't want to wait until the action to finish use [ScrollMouse](#scrollmouse)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| speed      |     float    |   Yes   | Set how fast to scroll. Positive values will scroll up and negative values will scroll down.|
| duration      |     float    |   Yes   | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        
        [Test]
        public void TestScrollAndWait()
        {

            AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
            var player2 = AltUnityDriver.FindObject(By.NAME, "Player2");
            UnityEngine.Vector3 cubeInitialPostion = new UnityEngine.Vector3(player2.worldX, player2.worldY, player2.worldY);
            AltUnityDriver.ScrollMouseAndWait(4, 2);
            player2 = AltUnityDriver.FindObject(By.NAME, "Player2");
            UnityEngine.Vector3 cubeFinalPosition = new UnityEngine.Vector3(player2.worldX, player2.worldY, player2.worldY);

            Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
        }


    .. code-tab:: java
        
        @Test
        public void TestScrollAndWait() throws InterruptedException {

            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                    "Player2").build();
            AltUnityObject player2 = altUnityDriver.findObject(altFindObjectsParameters);
            Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            altUnityDriver.scrollMouse(4, 2);
            Thread.sleep(2000);
            player2 = altUnityDriver.findObject(altFindObjectsParameters);
            Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            assertNotEquals(cubeInitialPostion, cubeFinalPosition);
        }



    .. code-tab:: py
        
        def test_scroll_and_wait(self):
            self.altdriver.load_scene("Scene 5 Keyboard Input")
            player2 = self.altdriver.find_object(By.NAME, "Player2")
            cubeInitialPostion = [player2.worldX, player2.worldY, player2.worldY]
            self.altdriver.scroll_mouse_and_wait(4, 2)
            player2 = self.altdriver.find_object(By.NAME, "Player2")
            cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]
            self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)

```
###  ScrollMouse

Simulate scroll mouse action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [ScrollMouseAndWait](#scrollmouseandwait)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| speed      |     float    |   Yes   | Set how fast to scroll. Positive values will scroll up and negative values will scroll down.|
| duration      |     float    |   Yes  | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        
        [Test]
        public void TestScroll()
        {
        
            AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
            var player2 = AltUnityDriver.FindObject(By.NAME, "Player2");
            UnityEngine.Vector3 cubeInitialPostion = new UnityEngine.Vector3(player2.worldX, player2.worldY, player2.worldY);
            AltUnityDriver.ScrollMouse(4,2);
            Thread.Sleep(2000);
            player2 = AltUnityDriver.FindObject(By.NAME, "Player2");
            UnityEngine.Vector3 cubeFinalPosition = new UnityEngine.Vector3(player2.worldX, player2.worldY, player2.worldY);
            Assert.AreNotEqual(cubeInitialPostion, cubeFinalPosition);
        }

    .. code-tab:: java
        
        @Test
        public void TestScroll() throws InterruptedException {
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME,
                    "Player2").build();
            AltUnityObject player2 = altUnityDriver.findObject(altFindObjectsParameters);
            Vector3 cubeInitialPostion = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            altUnityDriver.scrollMouse(4, 2);
            Thread.sleep(2000);
            player2 = altUnityDriver.findObject(altFindObjectsParameters);
            Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            assertNotEquals(cubeInitialPostion, cubeFinalPosition);
        }


    .. code-tab:: py
        
        def test_scroll(self):
            self.altdriver.load_scene("Scene 5 Keyboard Input")
            player2 = self.altdriver.find_object(By.NAME, "Player2")
            cubeInitialPostion = [player2.worldX, player2.worldY, player2.worldY]
            self.altdriver.scroll_mouse(4, 2)
            time.sleep(2)
            player2 = self.altdriver.find_object(By.NAME, "Player2")
            cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]
            self.assertNotEqual(cubeInitialPostion, cubeFinalPosition)


```

###  SwipeAndWait

Simulate a swipe action in your game. This command waist for the action to finish. If you don't want to wait until the action to finish use [Swipe](#swipe)


***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| start      |     Vector2(C#)    |   Yes   | Starting location of the swipe|
| end      |     Vector2(C#)    |   Yes   | Ending location of the swipe|
| xStart      |     float(python/java)    |   Yes   | x coordinate of the screen where the swipe begins.|
| yStart      |     float(python/java)    |   Yes   | y coordinate of the screen where the swipe begins|
| xEnd      |     float(python/java)    |   Yes  | x coordinate of the screen where the swipe ends|
| yEnd      |     float(python/java)    |   Yes   | x coordinate of the screen where the swipe ends|
| duration      |     float    |   Yes   | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
            [Test]
            public void MultipleDragAndDropWait()
            {
                var altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                var altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image2");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box2");
                altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image3");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);
        
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.SwipeAndWait(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);
                var imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                var imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);
        
                imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);
        
            }

    .. code-tab:: java
            @Test
            public void testMultipleDragAndDropWait() throws Exception
            {
                String altElement1Name = "Drag Image1";
                String altElement2Name = "Drop Box1";
                AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                AltUnityObject altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);

                altElement1Name = "Drag Image2";
                altElement2Name = "Drop Box2";
                altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                altElement2 = altUnityDriver.findObject(altFindObjectsParameters2,);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);

                altElement1Name = "Drag Image3";
                altElement2Name = "Drop Box1";
                altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);

                altElement1Name = "Drag Image1";
                altElement2Name = "Drop Box1";
                altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 1);

                altFindObjectsParameters = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image",  "sprite").build();
                String imageSource = altUnityDriver.findObject(altFindObjectsParameters1).getComponentProperty(altGetComponentPropertyParameters);

                String altDropElementImageName = "Drop Image";
                altFindObjectsParameters = new AltFindObjectsParameters.Builder(altDropElementImageName).isEnabled(true).withCamera("Main Camera").build();
                String imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters).getComponentProperty(altGetComponentPropertyParameters);
                assertNotSame(imageSource, imageSourceDropZone);

                altElement1Name = "Drag Image2";
                altFindObjectsParameters = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                imageSource = altUnityDriver.findObject(altFindObjectsParameters).getComponentProperty(altGetComponentPropertyParameters);
                imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters).getComponentProperty(altGetComponentPropertyParameters);
                assertNotSame(imageSource, imageSourceDropZone);
            }

    .. code-tab:: py
            def test_multiple_swipe_and_waits(self):
                self.altdriver.load_scene('Scene 3 Drag And Drop')
        
                image2 = self.altdriver.find_element('Drag Image2')
                box2 = self.altdriver.find_element('Drop Box2')
        
                self.altdriver.swipe_and_wait(image2.x, image2.y, box2.x, box2.y, 2)
        
        
                image3 = self.altdriver.find_element('Drag Image3')
                box1 = self.altdriver.find_element('Drop Box1')
        
                self.altdriver.swipe_and_wait(image3.x, image3.y, box1.x, box1.y, 1)
        
                image1 = self.altdriver.find_element('Drag Image1')
                box1 = self.altdriver.find_element('Drop Box1')
        
                self.altdriver.swipe_and_wait(image1.x, image1.y, box1.x, box1.y, 3)
        
                image_source = image1.get_component_property('UnityEngine.UI.Image', 'sprite')
                image_source_drop_zone = self.altdriver.find_element('Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
                self.assertNotEqual(image_source, image_source_drop_zone)
        
                image_source = image2.get_component_property('UnityEngine.UI.Image', 'sprite')
                image_source_drop_zone = self.altdriver.find_element('Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
                self.assertNotEqual(image_source, image_source_drop_zone)

```
###  Swipe

Simulate a swipe action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [SwipeAndWait](#swipeandwait)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| start      |     Vector2(C#)    |   Yes   | Starting location of the swipe|
| end      |     Vector2(C#)    |   Yes   | Ending location of the swipe|
| xStart      |     float(python/java)    |   Yes   | x coordinate of the screen where the swipe begins.|
| yStart      |     float(python/java)    |   Yes   | y coordinate of the screen where the swipe begins|
| xEnd      |     float(python/java)    |   Yes   | x coordinate of the screen where the swipe ends|
| yEnd      |     float(python/java)    |   Yes   | x coordinate of the screen where the swipe ends|
| duration      |     float    |   Yes  | The time measured in seconds to move the mouse from current position to the set location.|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
            [Test]
            public void MultipleDragAndDrop()
            {
                var altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                var altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 1);
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image2");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box2");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image3");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 2);
        
        
                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.Swipe(new Vector2(altElement1.x, altElement1.y), new Vector2(altElement2.x, altElement2.y), 3);
        
                Thread.Sleep(4000);
                
                var imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                var imageSourceDropZone= altUnityDriver.FindObject(By.NAME,"Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);
        
                 imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                 imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);
               
            }

    .. code-tab:: java
            @Test
            public void testMultipleDragAndDropWait() throws Exception
            {
                String altElement1Name = "Drag Image1";
                String altElement2Name = "Drop Box1";
                AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                AltUnityObject altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);

                altElement1Name = "Drag Image2";
                altElement2Name = "Drop Box2";
                altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                altElement2 = altUnityDriver.findObject(altFindObjectsParameters2,);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);

                altElement1Name = "Drag Image3";
                altElement2Name = "Drop Box1";
                altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);

                altElement1Name = "Drag Image1";
                altElement2Name = "Drop Box1";
                altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(altElement2Name).isEnabled(true).withCamera("Main Camera").build();

                altElement1 = altUnityDriver.findObject(altFindObjectsParameters1);
                altElement2 = altUnityDriver.findObject(altFindObjectsParameters2);
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 1);

                altFindObjectsParameters = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                AltGetComponentPropertyParameters altGetComponentPropertyParameters = new AltGetComponentPropertyParameters.Builder("UnityEngine.UI.Image",  "sprite").build();
                String imageSource = altUnityDriver.findObject(altFindObjectsParameters1).getComponentProperty(altGetComponentPropertyParameters);

                String altDropElementImageName = "Drop Image";
                altFindObjectsParameters = new AltFindObjectsParameters.Builder(altDropElementImageName).isEnabled(true).withCamera("Main Camera").build();
                String imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters).getComponentProperty(altGetComponentPropertyParameters);
                assertNotSame(imageSource, imageSourceDropZone);

                altElement1Name = "Drag Image2";
                altFindObjectsParameters = new AltFindObjectsParameters.Builder(altElement1Name).isEnabled(true).withCamera("Main Camera").build();
                imageSource = altUnityDriver.findObject(altFindObjectsParameters).getComponentProperty(altGetComponentPropertyParameters);
                imageSourceDropZone = altUnityDriver.findObject(altFindObjectsParameters).getComponentProperty(altGetComponentPropertyParameters);
                assertNotSame(imageSource, imageSourceDropZone);
            }

    .. code-tab:: py
            def test_multiple_swipes(self):
                self.altdriver.load_scene('Scene 3 Drag And Drop')
         
                image1 = self.altdriver.find_element('Drag Image1')
                box1 = self.altdriver.find_element('Drop Box1')
        
                self.altdriver.swipe(image1.x, image1.y, box1.x, box1.y, 5)
        
        
                image2 = self.altdriver.find_element('Drag Image2')
                box2 = self.altdriver.find_element('Drop Box2')
        
                self.altdriver.swipe(image2.x, image2.y, box2.x, box2.y, 2)
        
        
                image3 = self.altdriver.find_element('Drag Image3')
                box1 = self.altdriver.find_element('Drop Box1')
        
                self.altdriver.swipe(image3.x, image3.y, box1.x, box1.y, 3)
        
                time.sleep(6)
        
                image_source = image1.get_component_property('UnityEngine.UI.Image', 'sprite')
                image_source_drop_zone = self.altdriver.find_element('Drop Image').get_component_property('UnityEngine.UI.Image', 'sprite')
                self.assertNotEqual(image_source, image_source_drop_zone)
        
                image_source = image2.get_component_property('UnityEngine.UI.Image', 'sprite')
                image_source_drop_zone = self.altdriver.find_element('Drop').get_component_property('UnityEngine.UI.Image', 'sprite')
                self.assertNotEqual(image_source, image_source_drop_zone)

```
###  MultiPointSwipe

Similar command like swipe but instead of swipe from point A to point B you are able to give list a points. 

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| positions      |   List/Array of Vector2    |   Yes   | collection of positions on the screen where the swipe be made|
| duration      |     float    |   Yes   | how many seconds the swipe will need to complete|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
            [Test]
            public void ResizePanelWithMultipointSwipe()
            {
                var altElement = altUnityDriver.FindObject(By.NAME,"Resize Zone");
                var position = new AltUnityVector2(altElement.x, altElement.y);
                var pos = new []
                {
                    altElement.getScreenPosition(),
                    new AltUnityVector2(altElement.x - 200, altElement.y - 200),
                    new AltUnityVector2(altElement.x - 300, altElement.y - 100),
                    new AltUnityVector2(altElement.x - 50, altElement.y - 100),
                    new AltUnityVector2(altElement.x - 100, altElement.y - 100)
                };
                altUnityDriver.MultipointSwipe(pos, 4);

                Thread.Sleep(4000);

                altElement = altUnityDriver.FindObject(By.NAME,"Resize Zone");
                var position2 = new AltUnityVector2(altElement.x, altElement.y);
                Assert.AreNotEqual(position, position2);
            }

    .. code-tab:: java
        @Test
        public void testResizePanelWithMultipointSwipe() throws Exception
        {
            String name = "Resize Zone";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);

            List<Vector2> positions = Arrays.asList(
                altElement.getScreenPosition(), 
                new Vector2(altElement.x + 100, altElement.y + 100),
                new Vector2(altElement.x + 100, altElement.y + 200));

            altUnityDriver.multipointSwipe(positions, 3);
            Thread.sleep(3000);

            AltUnityObject altElementAfterResize = altUnityDriver.findObject(altFindObjectsParameters);
            assertNotSame(altElement.x, altElementAfterResize.x);
            assertNotSame(altElement.y, altElementAfterResize.y);
        }   

    .. code-tab:: py
        def test_resize_panel_with_multipoinit_swipe(self):
            self.altdriver.load_scene('Scene 2 Draggable Panel')
            altElement = self.altdriver.find_element('Resize Zone')
            positionInitX = altElement.x
            positionInitY = altElement.y 
            positions = [
            altElement.get_screen_position(),
            [int(altElement.x) - 200, int(altElement.y) - 200],
            [int(altElement.x) - 300, int(altElement.y) - 100],
            [int(altElement.x) - 50, int(altElement.y) - 100],
            [int(altElement.x) - 100, int(altElement.y) - 100]
            ]
            self.altdriver.multipoint_swipe(positions, 4)

            time.sleep(4)

            altElement = self.altdriver.find_element('Resize Zone')
            positionFinalX = altElement.x
            positionFinalY = altElement.y 
            self.assertNotEqual(positionInitX, positionFinalX)
            self.assertNotEqual(positionInitY, positionFinalY)
         
                
```

###  MultiPointSwipeAndWait

Similar command like [SwipeAndWait](#swipeandwait) but instead of swipe from point A to point B you are able to give list a points. 

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| positions      |   List/Array of Vector2    |   Yes   | collection of positions on the screen where the swipe be made|
| duration      |     float    |   Yes   | how many seconds the swipe will need to complete|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
            [Test]
            public void MultipleDragAndDropWaitWithMultipointSwipe()
            {
                var altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                var altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                altUnityDriver.MultipointSwipe(new []{new AltUnityVector2(altElement1.x, altElement1.y), new AltUnityVector2(altElement2.x, altElement2.y)}, 2);
                Thread.Sleep(2000);

                altElement1 = altUnityDriver.FindObject(By.NAME,"Drag Image1");
                altElement2 = altUnityDriver.FindObject(By.NAME,"Drop Box1");
                var altElement3 = altUnityDriver.FindObject(By.NAME,"Drop Box2");
                var positions = new[]
                {
                    new AltUnityVector2(altElement1.x, altElement1.y), 
                    new AltUnityVector2(altElement2.x, altElement2.y), 
                    new AltUnityVector2(altElement3.x, altElement3.y)
                };
                
                altUnityDriver.MultipointSwipeAndWait(positions, 3);
                var imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                var imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);

                imageSource = altUnityDriver.FindObject(By.NAME,"Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                imageSourceDropZone = altUnityDriver.FindObject(By.NAME,"Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite");
                Assert.AreNotEqual(imageSource, imageSourceDropZone);
            }

    .. code-tab:: java
        @Test
        public void testResizePanelWithMultipointSwipe() throws Exception
        {
            String name = "Resize Zone";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);

            List<Vector2> positions = Arrays.asList(
                altElement.getScreenPosition(), 
                new Vector2(altElement.x + 100, altElement.y + 100),
                new Vector2(altElement.x + 100, altElement.y + 200));

            altUnityDriver.multipointSwipe(positions, 3);

            AltUnityObject altElementAfterResize = altUnityDriver.findObject(altFindObjectsParameters);
            assertNotSame(altElement.x, altElementAfterResize.x);
            assertNotSame(altElement.y, altElementAfterResize.y);
        }



    .. code-tab:: py
        def test_multiple_swipe_and_waits_with_multipoint_swipe(self):
            altElement1 = self.altdriver.find_element('Drag Image1')
            altElement2 = self.altdriver.find_element('Drop Box1')

            multipointPositions = [altElement1.get_screen_position(), [altElement2.x, altElement2.y]]

            self.altdriver.multipoint_swipe_and_wait(multipointPositions, 2)
            time.sleep(2)

            altElement1 = self.altdriver.find_element('Drag Image1')
            altElement2 = self.altdriver.find_element('Drop Box1')
            altElement3 = self.altdriver.find_element('Drop Box2')

            positions = [
            [altElement1.x, altElement1.y], 
            [altElement2.x, altElement2.y], 
            [altElement3.x, altElement3.y]
            ]

            self.altdriver.multipoint_swipe_and_wait(positions, 3)
            imageSource = self.altdriver.find_element('Drag Image1').get_component_property("UnityEngine.UI.Image", "sprite")
            imageSourceDropZone = self.altdriver.find_element('Drop Image').get_component_property("UnityEngine.UI.Image", "sprite")
            self.assertNotEqual(imageSource, imageSourceDropZone)

            imageSource = self.altdriver.find_element('Drag Image2').get_component_property("UnityEngine.UI.Image", "sprite")
            imageSourceDropZone = self.altdriver.find_element('Drop').get_component_property("UnityEngine.UI.Image", "sprite")
            self.assertNotEqual(imageSource, imageSourceDropZone)
         
                
```

###  TapScreen(c#) / TapAtCoordinates(python/java)

Simulate a tap action on the screen at the given coordinates.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| x      |     float    |   Yes  |  x coordinate of the screen|
| y      |     float    |   Yes  |  y coordinate of the screen|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestClickScreen()
        {
            const string name = "UIButton";
            var altElement2 = altUnityDriver.FindObject(By.NAME,name);
            var altElement = altUnityDriver.TapScreen(altElement2.x, altElement2.y);
            Assert.AreEqual(name, altElement.name);
            altUnityDriver.WaitForObjectWithText(By.NAME,"CapsuleInfo", "UIButton clicked to jump capsule!");
        }

    .. code-tab:: java
        @Test
            public void testTapScreen() throws Exception {
                String capsuleName = "Capsule";
                String capsuleInfo = "CapsuleInfo";
                AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, capsuleName).isEnabled(true).withCamera("Main Camera").build();
                AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters);
                altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, capsuleInfo).isEnabled(true).withCamera("Main Camera").build();
                AltUnityObject capsuleInfo = altUnityDriver.findObject(altMoveMouseParameters);
                altUnityDriver.tapScreen(capsule.x, capsule.y);
                Thread.sleep(2);
                String text = capsuleInfo.getText();
                assertEquals(text, "Capsule was clicked to jump!");
            }

    .. code-tab:: py
        def test_tap_at_coordinates(self):
                self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
                capsule_element = self.altdriver.find_element('Capsule')
                self.altdriver.tap_at_coordinates(capsule_element.x, capsule_element.y)
                self.altdriver.wait_for_element_with_text('CapsuleInfo', 'Capsule was clicked to jump!','',1)


```

###  TapCustom

Simulate n number of tap actions on the screen at the given coordinates .

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| x      |     float    |   Yes   |  x coordinate of the screen|
| y      |     float    |   Yes   |  y coordinate of the screen|
| count  |     int      |   Yes   | number of taps|
| interval |   float    |   No    | how many seconds will be between touches |

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
            public void TestCustomTap()
            {
                var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
                var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
                altUnityDriver.TapCustom(counterButton.x, counterButton.y, 4);
                Thread.Sleep(1000);
                Assert.AreEqual("4", counterButtonText.GetText());
            }


    .. code-tab:: java
        @Test
        public void TestCustomTap() throws InterruptedException
        {
            AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, "ButtonCounter").build();
            AltUnityObject counterButton = altUnityDriver.findObject(altFindObjectsParameters1);
            AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, "ButtonCounter/Text").build();
            AltUnityObject counterButtonText = altUnityDriver.findObject(altFindObjectsParameters2);
            altUnityDriver.tapCustom(counterButton.x, counterButton.y, 4);
            Thread.sleep(1000);
            assertEquals("4", counterButtonText.getText());    
        }

    .. code-tab:: py
        def test_custom_tap(self):
            counterButton = self.altdriver.find_object(By.NAME, "ButtonCounter");
            counterButtonText = self.altdriver.find_object(By.NAME, "ButtonCounter/Text");
            self.altdriver.tap_custom(counterButton.x, counterButton.y, 4);
            time.sleep(1);
            self.assertEqual("4", counterButtonText.get_text());


```

###  Tilt

Simulates device rotation action in your game.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| acceleration      |     Vector3(C#)    |   Yes  | Linear acceleration of a device in three-dimensional space |
| x      |     float(python/java)    |   Yes  |  Linear acceleration of a device on x |
| y      |     float(python/java)    |   Yes  |  Linear acceleration of a device on y |
| z      |     float(python/java)    |   Yes  |  Linear acceleration of a device on z |
| duration |   float                 |   Yes  |  How long the rotation will take in seconds |

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        
        [Test]
        public void TestAcceleration()
        {
            var capsule= altUnityDriver.FindObject(By.NAME, "Capsule");
            var initialWorldCoordinates = capsule.getWorldPosition();
            altUnityDriver.Tilt(new AltUnityVector3(1, 1, 1),1);
            Thread.Sleep(1000);
            capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
            var afterTiltCoordinates = capsule.getWorldPosition();
            Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);
        }

    .. code-tab:: java
        
        
        @Test
        public void TestAcceleration() throws InterruptedException {
            AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                    AltUnityDriver.By.NAME, "Capsule").build();
            AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
            Vector3 initialWorldCoordinates = capsule.getWorldPosition();
            AltTiltParameters altTiltParameters = new AltTiltParameters.Builder(1, 1, 1).withDuration(1).build();
            altUnityDriver.tilt(altTiltParameters);
            Thread.sleep(1000);
            capsule = altUnityDriver.findObject(altFindObjectsParameters1);
            Vector3 afterTiltCoordinates = capsule.getWorldPosition();
            assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
        }


    .. code-tab:: py
        
        def test_acceleration(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            capsule = self.altdriver.find_object(By.NAME, "Capsule")
            initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            self.altdriver.tilt(1, 1, 1, 1)
            time.sleep(1)
            capsule = self.altdriver.find_object(By.NAME, "Capsule")
            final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            self.assertNotEqual(initial_position, final_position)

```


###  TiltAndWait

Simulates device rotation action in your game. This command waits for the action to finish. If you don’t want to wait until the action to finish use [Tilt](#tilt)

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| acceleration      |     Vector3(C#)    |   Yes  | Linear acceleration of a device in three-dimensional space |
| x      |     float(python/java)    |   Yes  |  Linear acceleration of a device on x |
| y      |     float(python/java)    |   Yes  |  Linear acceleration of a device on y |
| z      |     float(python/java)    |   Yes  |  Linear acceleration of a device on z |
| duration |   float                 |   Yes  |  How long the rotation will take in seconds |

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        
        [Test]
        public void TestAccelerationAndWait()
        {
            var capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
            var initialWorldCoordinates = capsule.getWorldPosition();
            altUnityDriver.TiltAndWait(new AltUnityVector3(1, 1, 1), 1);
            Thread.Sleep(1000);
            capsule = altUnityDriver.FindObject(By.NAME, "Capsule");
            var afterTiltCoordinates = capsule.getWorldPosition();
            Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);
        }

    .. code-tab:: java
        
        
        @Test
        public void TestAccelerationAndWait() throws InterruptedException {
            AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                    AltUnityDriver.By.NAME, "Capsule").build();
            AltUnityObject capsule = altUnityDriver.findObject(altFindObjectsParameters1);
            Vector3 initialWorldCoordinates = capsule.getWorldPosition();
            AltTiltParameters altTiltParameters = new AltTiltParameters.Builder(1, 1, 1).withDuration(1).build();
            altUnityDriver.tiltAndWait(altTiltParameters);
            capsule = altUnityDriver.findObject(altFindObjectsParameters1);
            Vector3 afterTiltCoordinates = capsule.getWorldPosition();
            assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
        }


    .. code-tab:: py
       
       def test_acceleration_and_wait(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            capsule = self.altdriver.find_object(By.NAME, "Capsule")
            initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            self.altdriver.tilt_and_wait(1, 1, 1, 1)
            capsule = self.altdriver.find_object(By.NAME, "Capsule")
            final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            self.assertNotEqual(initial_position, final_position)

```
## ObjectCommands

###  CallComponentMethod

Invoke a method from an existing component of the object.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| componentName      |     string    |   Yes   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| methodName      |     string    |   Yes  |   The name of the public method that we want to call |
| parameters      |     string    |   Yes  |   a string containing the serialized parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]" Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.|
| typeOfParamaters      |     string    |   Yes   |  a string containing the serialized type of parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints |
| assemblyName  | string | No | name of the assembly where the component is |

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestCallMethodWithAssembly(){
            AltUnityObject capsule = altUnityDriver.FindObject(By.NAME,"Capsule");
            var initialRotation = capsule.GetComponentProperty("UnityEngine.Transform", "rotation");
            capsule.CallComponentMethod("UnityEngine.Transform", "Rotate", "10?10?10", "System.Single?System.Single?System.Single", "UnityEngine.CoreModule");
            AltUnityObject capsuleAfterRotation = altUnityDriver.FindObject(By.NAME,"Capsule");
            var finalRotation = capsuleAfterRotation.GetComponentProperty("UnityEngine.Transform", "rotation");
            Assert.AreNotEqual(initialRotation, finalRotation);
        }

    .. code-tab:: java
        @Test
        public void TestCallMethodWithMultipleDefinitions() throws Exception
        {

            String capsuleName = "Capsule";
            String capsuleInfo = "CapsuleInfo";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, capsuleName).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject capsule=altUnityDriver.findObject(altFindObjectsParameters);

            AltCallComponentMethodParameters altCallComponentMethodParameters=new AltCallComponentMethodParameters.Builder("Capsule","Test","2").withTypeOfParameters("System.Int32").withAssembly("").build();
            capsule.callComponentMethod(altCallComponentMethodParameters);

            altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, capsuleInfo).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject capsuleInfo=altUnityDriver.findObject(altFindObjectsParameters);

            assertEquals("6",capsuleInfo.getText());
        }

    .. code-tab:: py
        def test_call_component_method(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            result = self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
            self.assertEqual(result,"null")
            self.altdriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
            self.assertEqual('setFromMethod', self.altdriver.find_element('CapsuleInfo').get_text())

```

###  CallStaticMethod

Invoke static methods from your game.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| typeName      |     string    |   Yes  | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| methodName      |     string    |   Yes   |   The name of the public method that we want to call |
| parameters      |     string    |   Yes   |   a string containing the serialized parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]" Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.|
| typeOfParamaters      |     string    |   Yes |  a string containing the serialized type of parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints |
| assemblyName  | string | No | name of the assembly where the component is |

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestCallStaticMethod()
        {

            altUnityDriver.CallStaticMethods("UnityEngine.PlayerPrefs", "SetInt", "Test?1");
            int a = Int32.Parse(altUnityDriver.CallStaticMethods("UnityEngine.PlayerPrefs", "GetInt", "Test?2"));
            Assert.AreEqual(1, a);

        }

    .. code-tab:: java
        @Test
        public void TestCallStaticMethod() throws Exception
        {

            AltCallStaticMethodsParameters altCallStaticMethodsParameters = new AltCallStaticMethodsParameters.Builder("UnityEngine.PlayerPrefs","SetInt","Test?1").withAssembly("").withTypeOfParameters("").build();
            altUnityDriver.callStaticMethods(altCallStaticMethodsParameters);
            altCallStaticMethodsParameters = new AltCallStaticMethodsParameters.Builder("UnityEngine.PlayerPrefs","GetInt","Test?2").withAssembly("").withTypeOfParameters("").build();
            int a=Integer.parseInt(altUnityDriver.callStaticMethods(altCallStaticMethodsParameters);
            assertEquals(1,a);
        }

    .. code-tab:: py
        def test_call_static_method(self):
            self.altdriver.call_static_methods("UnityEngine.PlayerPrefs", "SetInt","Test?1",assembly="UnityEngine.CoreModule")
            a=int(self.altdriver.call_static_methods("UnityEngine.PlayerPrefs", "GetInt", "Test?2",assembly="UnityEngine.CoreModule"))
            self.assertEquals(1,a)

```

###  ClickEvent

Simulate a click on the object. It will click the object even if the object is not visible something that you could not do on a real device.

***Parameters***

None

***Returns***
- Nothing

***Examples***

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestDifferentCamera()
        {
            var altButton = altUnityDriver.FindObject(By.NAME,"Button", "Main Camera");
            altButton.ClickEvent();
            altButton.ClickEvent();
            var altElement = altUnityDriver.FindObject(By.NAME,"Capsule", "Main Camera");
            var altElement2 = altUnityDriver.FindObject(By.NAME,"Capsule", "Camera");
            Vector2 pozOnScreenFromMainCamera = new Vector2(altElement.x, altElement.y);
            Vector2 pozOnScreenFromSecondaryCamera = new Vector2(altElement2.x, altElement2.y);

            Assert.AreNotEqual(pozOnScreenFromSecondaryCamera, pozOnScreenFromMainCamera);

        }

    .. code-tab:: java

        @Test
        public void testDifferentCamera() throws Exception
        {
            String name = "Button";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject altButton = altUnityDriver.findObject(altFindObjectsParameters);
            altButton.clickEvent();
            altButton.clickEvent();
            String capsuleName = "Capsule";
            altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, capsuleName).isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
            altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, capsuleName).isEnabled(true).withCamera("Camera").build();
            AltUnityObject altElement2 = altUnityDriver.findObject(altFindObjectsParameters);
            assertNotSame(altElement.x, altElement2.x);
            assertNotSame(altElement.y, altElement2.y);
        }

    .. code-tab:: py
     //TODO

```

###  DragObject

Drag an object to a certain position on the screen

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| position      |     Vecto2(C#)    |   Yes   | coordinates of the screen where the object will be dragged|
| x      |     int(python/java)    |   Yes  |   x coordinate of the screen where the object will be dragged |
| y      |     int(python/java)    |   Yes   |   y coordinate of the screen where the object will be dragged|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        public void TestDragObject()
        {
            var panel = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            UnityEngine.Vector3 panelInitialPostion = new UnityEngine.Vector3(panel.worldX, panel.worldY, panel.worldY);
            panel.DragObject( new AltUnityVector2(200, 200));
            Thread.Sleep(2000); 
            panel = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            UnityEngine.Vector3 panelFinalPostion = new UnityEngine.Vector3(panel.worldX, panel.worldY, panel.worldY);
            Assert.AreNotEqual(panelInitialPostion, panelFinalPostion);
        }

    .. code-tab:: java
        @Test
        public void testDragObject() throws InterruptedException {
            AltUnityObject dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
            Vector3 initPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
            dragPanel.drag(200,200);
            Thread.sleep(1000);
            dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
            Vector3 finalPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
            assertTrue(initPosition != finalPosition);
        }

    .. code-tab:: py
        def test_drag_object(self):
          self.altdriver.load_scene('Scene 2 Draggable Panel')
          time.sleep(1)
          d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
          p_initial = (d_panel.x, d_panel.y, d_panel.z)
          d_panel.drag(200,200)
          time.sleep(1)
          d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
          p_final = (d_panel.x, d_panel.y, d_panel.z)
          self.assertNotEqual(p_initial, p_final)

```
###  DropObject

Drop an object to a certain position on the screen

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| position      |     Vecto2(C#)    |   Yes   | coordinates of the screen where the object will be dragged|
| x      |     int(python/java)    |   Yes  |   x coordinate of the screen where the object will be dragged |
| y      |     int(python/java)    |   Yes   |   y coordinate of the screen where the object will be dragged|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        public void TestDropObject()
        {
            var panel = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            UnityEngine.Vector3 panelInitialPostion = new UnityEngine.Vector3(panel.worldX, panel.worldY, panel.worldY);
            panel.DropObject(new AltUnityVector2(100, 200));
            Thread.Sleep(2000); 
            panel = altUnityDriver.FindObject(By.NAME, "Drag Zone");
            UnityEngine.Vector3 panelFinalPostion = new UnityEngine.Vector3(panel.worldX, panel.worldY, panel.worldY);
            Assert.AreNotEqual(panelInitialPostion, panelFinalPostion);
        }

    .. code-tab:: java
        @Test
        public void testDropObject() throws InterruptedException {
            AltUnityObject dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
            Vector3 initPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
            dragPanel.drop(200,200);
            Thread.sleep(1000);
            dragPanel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Drag Zone");
            Vector3 finalPosition = new Vector3(dragPanel.x, dragPanel.y, dragPanel.z);
            assertTrue(initPosition != finalPosition);
        }

    .. code-tab:: py
        def test_drop_object(self):
            self.altdriver.load_scene('Scene 2 Draggable Panel')
            time.sleep(1)
            d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
            p_initial = (d_panel.x, d_panel.y, d_panel.z)
            d_panel.drag(100,200)
            time.sleep(1)
            d_panel.drop(100,200)
            time.sleep(1)
            d_panel = self.altdriver.find_object(By.NAME, 'Drag Zone')
            p_final = (d_panel.x, d_panel.y, d_panel.z)
            self.assertNotEqual(p_initial, p_final)

```

###  GetAllComponents(C#)

Get all components attached to an object

***Parameters***

None

***Returns***
- Nothing

***Examples***

```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestGetAllComponents()
        {
            List<AltUnityComponent> components = altUnityDriver.FindObject(By.NAME,"Canvas").GetAllComponents();
            Assert.AreEqual(4, components.Count);
            Assert.AreEqual("UnityEngine.RectTransform", components[0].componentName);
            Assert.AreEqual("UnityEngine.CoreModule", components[0].assemblyName);
        }
```

###  GetAllMethods(C#)

Get all methods from a component attached to an object

***Parameters***

None

***Returns***
- Nothing

***Examples***

```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestGetAllMethods()
        {
            var altElement = altUnityDriver.FindObject(By.NAME,"Capsule");
            List<String> methods = altElement.GetAllMethods(altElement.GetAllComponents().First(component => component.componentName.Equals("Capsule")));
            Assert.IsTrue(methods.Contains("Void UIButtonClicked()"));
        }
```

###  GetAllProperties(C#)

Get all properties from a component attached to an object. This method is implement only in C#.

***Parameters***

None

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestGetAllFields()
        {
            var altElement = altUnityDriver.FindObject(By.NAME,"Capsule");
            var componentList = altElement.GetAllComponents();
            var component = componentList.First(componenta =>
                componenta.componentName.Equals("Capsule") && componenta.assemblyName.Equals("Assembly-CSharp"));
            List<AltUnityProperty> properties = altElement.GetAllProperties(component);
            AltUnityProperty field = properties.First(prop => prop.name.Equals("stringToSetFromTests"));
            Assert.NotNull(field);
            Assert.AreEqual(field.value, "intialValue");
        }
```

###  GetComponentProperty

Get the value of a property from one of the component of the object.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| componentName      |     string    |   Yes   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| propertyName      |     string    |   Yes   |  name of the property of which value you want |
| assemblyName  | string | No | name of the assembly where the component is |

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestGetComponentProperty()
        {
            const string componentName = "AltUnityRunner";
            const string propertyName = "SocketPortNumber";
            var altElement = altUnityDriver.FindObject(By.NAME,"AltUnityRunnerPrefab");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty(componentName, propertyName);
            Assert.AreEqual(propertyValue, "13000");
        }

    .. code-tab:: java

        @Test
        public void testGetComponentProperty() throws Exception
        {
            String componentName = "AltUnityRunner";
            String propertyName = "SocketPortNumber";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, "AltUnityRunnerPrefab").isEnabled(true).withCamera("Main Camera").build();
            AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParameters);
            assertNotNull(altElement);
            AltGetComponentPropertyParameters altGetComponentPropertyParameters=new AltGetComponentPropertyParameters.Builder(componentName,propertyName).withAssembly("").build();
            String propertyValue = altElement.getComponentProperty(altGetComponentPropertyParameters);
            assertEquals(propertyValue, "13000");
        }

    .. code-tab:: py
        def test_get_component_property(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
            self.assertEqual(result,"[1,2,3]")

```

###  GetText

Get text value from a Button, Text, InputField. This also works with TextMeshPro elements.

***Parameters***

None

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
        public void TestWaitForElementWithText()
        {
            const string name = "CapsuleInfo";
            string text = altUnityDriver.FindObject(By.NAME,name).GetText();
            var timeStart = DateTime.Now;
            var altElement = altUnityDriver.WaitForObjectWithText(By.NAME, name, text);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.GetText(), text);

        }

    .. code-tab:: java
        @Test
        public void testWaitForElementWithText() throws Exception
        {
            String name = "CapsuleInfo";
            AltFindObjectsParameters altFindObjectsParameters = new AltFindObjectsParameters.Builder(AltUnityDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            String text = altUnityDriver.findObject(altFindObjectsParameters).getText();
            long timeStart = System.currentTimeMillis();
            AltWaitForObjectWithTextParameters altWaitForElementWithTextParameters = new AltWaitForObjectWithTextParameters.Builder(altFindObjectsParameters,text).withInterval(0).withTimeout(0).build();
            AltUnityObject altElement = altUnityDriver.waitForObjectWithText(altWaitForElementWithTextParameters);
            long timeEnd = System.currentTimeMillis();
            long time = timeEnd - timeStart;
            assertTrue(time / 1000 < 20);
            assertNotNull(altElement);
            assertEquals(altElement.getText(), text);
        }

    .. code-tab:: py
        def test_call_component_method(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            result = self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
            self.assertEqual(result,"null")
            self.altdriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
            self.assertEqual('setFromMethod', self.altdriver.find_element('CapsuleInfo').get_text())

```

###  DoubleTap

Simulates a double tap on the object that trigger multiple events similar to a real double tap but they happens in one frame.

***Parameters***

None

***Returns***
- Nothing

***Examples***

```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
            public void TestDoubleTap()
            {
                var counterButton = altUnityDriver.FindObject(By.NAME, "ButtonCounter");
                var counterButtonText = altUnityDriver.FindObject(By.NAME, "ButtonCounter/Text");
                counterButton.DoubleTap();
                Thread.Sleep(500);
                Assert.AreEqual("2", counterButtonText.GetText());
            }


    .. code-tab:: java
        @Test
        public void TestDoubleTap() throws InterruptedException
        {
            AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ButtonCounter").build();
            AltUnityObject counterButton = altUnityDriver.findObject(altFindObjectsParameters1);
            AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                AltUnityDriver.By.NAME, "ButtonCounter/Text").build();
            AltUnityObject counterButtonText = altUnityDriver.findObject(altFindObjectsParameters2);
            counterButton.doubleTap();
            Thread.sleep(500);
            assertEquals("2", counterButtonText.getText());
        }


    .. code-tab:: py
        def test_double_tap(self):  
            counterButton = self.altdriver.find_object(By.NAME, "ButtonCounter");
            counterButtonText = self.altdriver.find_object(By.NAME, "ButtonCounter/Text");
            counterButton.double_tap();
            time.sleep(0.5);
            self.assertEqual("2", counterButtonText.get_text());


```

###  PointerDownFromObject

Simulates pointer down action on the object.

***Parameters***

None

***Returns***
- AltUnityObject

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
            public void TestPointerDownFromObject(){
                var panel = altUnityDriver.FindObject(By.NAME, "Panel");
                var color1 = panel.GetComponentProperty("PanelScript","normalColor");
                panel.PointerDownFromObject();
                Thread.Sleep(1000);
                var color2 = panel.GetComponentProperty("PanelScript","highlightColor");
                Assert.AreNotEqual(color1, color2);
            }

    .. code-tab:: java
        @Test
        public void testPointerDownFromObject() throws InterruptedException {
            AltUnityObject panel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Panel");
            String color1 = panel.getComponentProperty("PanelScript", "normalColor");
            panel.pointerDown();
            Thread.sleep(1000);
            String color2 = panel.getComponentProperty("PanelScript", "highlightColor");
            assertTrue(color1 != color2);
        }


    .. code-tab:: py
        def test_pointer_down_from_object(self):
            self.altdriver.load_scene('Scene 2 Draggable Panel')
            time.sleep(1)
            p_panel = self.altdriver.find_object(By.NAME, 'Panel')
            color1 = p_panel.get_component_property('PanelScript', 'normalColor')
            p_panel.pointer_down()
            time.sleep(1)
            color2 = p_panel.get_component_property('PanelScript', 'highlightColor')
            self.assertNotEquals(color1, color2)

```

###  PointerUpFromObject

Simulates pointer up action on the object.

***Parameters***

None

***Returns***
- AltUnityObject

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test] 
            public void TestPointerUpFromObject(){
                var panel = altUnityDriver.FindObject(By.NAME, "Panel");
                var color1 = panel.GetComponentProperty("PanelScript","normalColor");
                panel.PointerDownFromObject();
                Thread.Sleep(1000);
                panel.PointerUpFromObject();
                var color2 = panel.GetComponentProperty("PanelScript","highlightColor");
                Assert.AreEqual(color1, color2);
            }

    .. code-tab:: java
        @Test
        public void testPointerUpFromObject() throws InterruptedException {
            AltUnityObject panel = altUnityDriver.findObject(AltUnityDriver.By.NAME, "Panel");
            String color1 = panel.getComponentProperty("PanelScript", "normalColor");
            panel.pointerDown();
            Thread.sleep(1000);
            panel.pointerUp();
            String color2 = panel.getComponentProperty("PanelScript", "highlightColor");
            assertEquals(color1, color2);
        }

    .. code-tab:: py
        def test_pointer_up_from_object(self):
            self.altdriver.load_scene('Scene 2 Draggable Panel')
            time.sleep(1)
            p_panel = self.altdriver.find_object(By.NAME, 'Panel')
            color1 = p_panel.get_component_property('PanelScript', 'normalColor')
            p_panel.pointer_down()
            time.sleep(1)
            p_panel.pointer_up()
            color2 = p_panel.get_component_property('PanelScript', 'highlightColor')
            self.assertEquals(color1, color2)

```

## UnityCommands 

###  DeleteKeyPlayerPref  

Delete from games player pref a key

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
| keyname      |     sting    |   Yes   | Key to be deleted|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestDeleteKey()
        {
            altUnityDriver.DeletePlayerPref();
            altUnityDriver.SetKeyPlayerPref("test", 1);
            var val = altUnityDriver.GetIntKeyPlayerPref("test");
            Assert.AreEqual(1, val);
            altUnityDriver.DeleteKeyPlayerPref("test");
            try
            {
                altUnityDriver.GetIntKeyPlayerPref("test");
                Assert.Fail();
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual("error:notFound", exception.Message);
            }
    
        }
    .. code-tab:: java

        @Test
            public void testDeleteKey() throws Exception
            {
                altUnityDriver.deletePlayerPref();
                altUnityDriver.setKeyPlayerPref("test", 1);
                int val = altUnityDriver.getIntKeyPlayerPref("test");
                assertEquals(1, val);
                altUnityDriver.deleteKeyPlayerPref("test");
                try {
                    altUnityDriver.getIntKeyPlayerPref("test");
                    fail();
                } catch (NotFoundException e) {
                    assertEquals(e.getMessage(), "error:notFound");
                }
            }


    .. code-tab:: py

       //TODO
```

###  DeletePlayerPref

Delete entire player pref of the game

***Parameters***

None

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
            public void TestSetKeyInt()
            {
                altUnityDriver.DeletePlayerPref();
                altUnityDriver.SetKeyPlayerPref("test", 1);
                var val = altUnityDriver.GetIntKeyPlayerPref("test");
                Assert.AreEqual(1, val);
            }
    .. code-tab:: java

         @Test
        public void testSetKeyFloat() throws Exception
        {
            altUnityDriver.deletePlayerPref();
            altUnityDriver.setKeyPlayerPref("test", 1f);
            float val = altUnityDriver.getFloatKeyPlayerPref("test");
            assertEquals(1f, val, 0.01);
        }


    .. code-tab:: py

       def test_set_player_pref_keys_int(self):
              self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
              self.altdriver.delete_player_prefs()
              self.altdriver.set_player_pref_key('test', 1, PlayerPrefKeyType.Int)
              value = self.altdriver.get_player_pref_key('test', PlayerPrefKeyType.Int)
              self.assertEqual(int(value), 1)
```

###  GetAllCameras(C#)

Return all cameras that are in the scene. This method is only implemented in C#

***Parameters***

None

***Returns***
- List of AltUnityObjects

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetAllCameras()
        {
            var cameras = altUnityDriver.GetAllCameras();
            Assert.AreEqual(2,cameras.Count);
        }
    
```
###  GetAllScenes

Return list of scene in the game

***Parameters***

None

***Returns***

- List of string

***Examples***

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetAllScenes()
        {
            var scenes = altUnityDriver.GetAllScenes();
            Assert.AreEqual(5, scenes.Count);
            Assert.AreEqual("Scene 1 AltUnityDriverTestScene", scenes[0]);
        }
```

###  GetCurrentScene

Get the current active scene.


***Parameters***

None

***Returns***
- String

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetCurrentScene()
        {
            Assert.AreEqual("Scene 1 AltUnityDriverTestScene", altUnityDriver.GetCurrentScene());
        }
    .. code-tab:: java

        @Test
        public void testGetCurrentScene() throws Exception
        {
            assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());
        }

    .. code-tab:: py

       def test_get_current_scene(self):
        self.assertEqual("Scene 1 AltUnityDriverTestScene",self.altdriver.get_current_scene())
```

## Screenshot

###  GetPNGScreenshot

Create a screenshot of the current scene in png format.

***Parameters***

|      Name       |     Type      | Required | Description |
| --------------- | ------------- | -------- | ----------- |
|path| string | Yes| location where the image is created|

***Returns***
- Nothing

***Examples***
```eval_rst
.. tabs::

    .. code-tab:: c#

        public void TestGetScreenshot(){
        var path="testC.png";
        altUnityDriver.GetPNGScreenshot(path);
        FileAssert.Exists(path);
        }
    .. code-tab:: java

        @Test
        public void testScreenshot()
        {
            String path="testJava2.png";
            altUnityDriver.getPNGScreeshot(path);
            assertTrue(new File(path).isFile());
        }


    .. code-tab:: py

       def test_screenshot(self):
        png_path="testPython.png"
        self.altdriver.get_png_screenshot(png_path)
        self.assertTrue(path.exists(png_path))
```
