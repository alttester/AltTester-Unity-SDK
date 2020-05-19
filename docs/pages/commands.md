# Commands
## FindObjects

###  FindObject

#### Description:

Find the first object in the scene that respects the given criteria. Check [by](other/by.md) for more information about criterias.

###### Observation: No longer possible to search for object by name giving a path in hierarchy, if you want to search that way please use by path.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltFindObjectParameters](other/java-builders.html#altfindobjectparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples
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
        public void testfindElement() throws Exception {
            String name = "Capsule";
            AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,name);
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

#### Description:

Find all objects in the scene that respects the given criteria. Check [By](other/by.html) for more information about criterias.

###### Observation: No longer possible to search for object by name giving a path in hierarchy, if you want to search that way please use by path.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltFindObjectParameters](other/java-builders.html#altfindobjectparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Returns

- List of AltUnityObjects/ empty list if no objects were found


#### Examples

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
        }
    .. code-tab:: java

           @Test
            public void testfindElements() throws Exception {
                String name = "Plane";
                AltUnityObject[] altElements = altUnityDriver.findObjects(AltUnityDriver.By.NAME,name);
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

#### Description:

Find the first object in the scene that respects the given criteria. Check [by](other/by.md) for more information about criterias.

###### Observation: Every criteria except of path works for this command

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltFindObjectParameters](other/java-builders.html#altfindobjectparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Returns

- List of AltUnityObjects/ empty list if no objects were found

#### Examples


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
            AltUnityObject altElement = altUnityDriver.findObjectWhichContains(AltUnityDriver.By.NAME, "Event");
            assertEquals("EventSystem", altElement.name);
        }


    .. code-tab:: py
       def test_find_object_which_contains(self):
        altElement = self.altdriver.find_object_which_contains(By.NAME, "Event");
        self.assertEqual("EventSystem", altElement.name)

```


###  FindObjectsWhichContains

#### Description:

Find all objects in the scene that respects the given criteria. Check [By](other/by.html) for more information about criterias.

###### Observation: Every criteria except of path works for this command

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltFindObjectParameters](other/java-builders.html#altfindobjectparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Returns

- List of AltUnityObjects/ empty list if no objects were found

#### Examples


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
          public void testFindElementsWhereNameContains() throws Exception {
              String name = "Pla";
              AltUnityObject[] altElements = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,name);
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

#### Description:

Returns information about every objects loaded in the currently loaded scenes. This also means objects that are set as DontDestroyOnLoad.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|


###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **AltGetAllElementsParameters** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Return

- List of AltUnityObjects/ empty list if no objects were found

#### Examples

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
                AltUnityObject[] altElements = altUnityDriver.getAllElements();
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

#### Description:

Wait until there is no longer any objects that respect the given criteria or times run out and will throw an error. Check [By](other/by.html) for more information about criterias.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   true   | number of seconds that it will wait for object|
| interval        | double        |   true   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltWaitForObjectsParameters](other/java-builders.html#altwaitforobjectsparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Return
- Nothing
#### Examples


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

        //TODO


    .. code-tab:: py
        def test_wait_for_object(self):
            altElement=self.altdriver.wait_for_object(By.NAME,"Capsule")
            self.assertEqual(altElement.name,"Capsule")

```
###  WaitForObjectWhichContains

#### Description:

Wait until it finds an object that respect the given criteria or times run out and will throw an error. Check [By](other/by.html) for more information about criterias.

###### Observation: Every criteria except of path works for this command

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   true   | number of seconds that it will wait for object|
| interval        | double        |   true   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltWaitForObjectsParameters](other/java-builders.html#altwaitforobjectsparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Return

- AltUnityObject

#### Examples


```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO
    .. code-tab:: java
    
        //TODO



    .. code-tab:: py
        def test_wait_for_object_which_contains(self):
            altElement=self.altdriver.wait_for_object_which_contains(By.NAME,"Main")
            self.assertEqual(altElement.name,"Main Camera")
```
###  WaitForObjectWithText

#### Description:

Wait until it finds an object that respect the given criteria and it has the text you are looking for or times run out and will throw an error. Check [By](other/by.html) for more information about criterias.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| text    |   string  | false  | Text that the intented object should have|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   true   | number of seconds that it will wait for object|
| interval        | double        |   true   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltWaitForObjectWithTextParameters](other/java-builders.html#altwaitforobjectwithtextparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Return
- AltUnityObject
#### Examples

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

    .. code-tab:: java
        @Test
            public void testWaitForElementWithText() throws Exception {
                String name = "CapsuleInfo";
                String text = altUnityDriver.findObject(AltUnityDriver.By.NAME,name).getText();
                long timeStart = System.currentTimeMillis();
                AltUnityObject altElement = altUnityDriver.waitForObjectWithText(AltUnityDriver.By.NAME,name, text);
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

#### Description:

Wait until the object in the scene that respect the given criteria is no longer in the scene or times run out and will throw an error. Check [By](other/by.html) for more information about criterias.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   true   | number of seconds that it will wait for object|
| interval        | double        |   true   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltWaitForObjectsParameters](other/java-builders.html#altwaitforobjectsparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Return

- Nothing

#### Examples

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

## InputActions 

###  MoveMouseAndWait

#### Description:

Simulate mouse movement in your game. This command will wait for the movement to finish. If you don't want to wait until the mouse movement stops use [MoveMouse]({{ site.baseurl }}/pages/commands/input-actions/move-mouse)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| location      |     Vector2    |   false   | The destination coordinates for mouse to go from the current mouse position|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltMoveMouseParameters](other/java-builders.html#altmovemouseparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples

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
            public void TestCreatingStars() throws InterruptedException {
        
                AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
                assertEquals(1, stars.length);
                AltUnityObject player=altUnityDriver.findElement("Player1","Player2");
                altUnityDriver.moveMouse(player.x, player.y+500, 1);
                Thread.sleep(1500);
        
                altUnityDriver.pressKey("Mouse0", 1,1);
                altUnityDriver.moveMouseAndWait(player.x, player.y-500, 1);
                altUnityDriver.pressKeyAndWait("Mouse0", 1,1);
        
        
                stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
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

#### Description:

Simulate mouse movement in your game. This command does not wait for the movement to finish. To also wait for the movement to finish use [MoveMouseAndWait]({{ site.baseurl }}/pages/commands/input-actions/move-mouse-and-wait)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| location      |     Vector2    |   false   | The destination coordinates for mouse to go from the current mouse position|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltMoveMouseParameters](other/java-builders.html#altmovemouseparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples

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
            public void TestCreatingStars() throws InterruptedException {
        
                AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
                assertEquals(1, stars.length);
                AltUnityObject player=altUnityDriver.findElement("Player1","Player2");
                altUnityDriver.moveMouse(player.x, player.y+500, 1);
                Thread.sleep(1500);
        
                altUnityDriver.pressKey("Mouse0", 1,1);
                altUnityDriver.moveMouseAndWait(player.x, player.y-500, 1);
                altUnityDriver.pressKeyAndWait("Mouse0", 1,1);
        
        
                stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
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

#### Description:

Simulate key press action in your game. This command waist for the action to finish. If you don't want to wait until the action to finish use [PressKey]({{ site.baseurl }}/pages/commands/input-actions/press-key)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| keycode      |     KeyCode(C#)/string(python/java)    |   false   | Name of the button. Please check [KeyCode for C#](https://docs.unity3d.com/ScriptReference/KeyCode.html) or [key section for python/java](https://docs.unity3d.com/Manual/ConventionalGameInput.html) for more information about key names|
| power      |     float    |   false   | A value from \[-1,1\] that defines how strong the key was pressed. This is mostly used for joystick button since the keyboard button will always be 1 or -1|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltPressKeyParameters](other/java-builders.html#altpresskeyparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples

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
            public void TestCreatingStars() throws InterruptedException {
        
                AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
                assertEquals(1, stars.length);
                AltUnityObject player=altUnityDriver.findElement("Player1","Player2");
                altUnityDriver.moveMouse(player.x, player.y+500, 1);
                Thread.sleep(1500);
        
                altUnityDriver.pressKey("Mouse0", 1,1);
                altUnityDriver.moveMouseAndWait(player.x, player.y-500, 1);
                altUnityDriver.pressKeyAndWait("Mouse0", 1,1);
        
        
                stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
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

#### Description:

Simulate key press action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [PressKeyAndWait]({{ site.baseurl }}/pages/commands/input-actions/press-key-and-wait)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| keycode      |     KeyCode(C#)/string(python/java)    |   false   | Name of the button. Please check [KeyCode for C#](https://docs.unity3d.com/ScriptReference/KeyCode.html) or [key section for python/java](https://docs.unity3d.com/Manual/ConventionalGameInput.html) for more information about key names|
| power      |     float    |   false   | A value from \[-1,1\] that defines how strong the key was pressed. This is mostly used for joystick button since the keyboard button will always be 1 or -1|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltPressKeyParameters](other/java-builders.html#altpresskeyparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples

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
            public void TestCreatingStars() throws InterruptedException {
        
                AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
                assertEquals(1, stars.length);
                AltUnityObject player=altUnityDriver.findElement("Player1","Player2");
                altUnityDriver.moveMouse(player.x, player.y+500, 1);
                Thread.sleep(1500);
        
                altUnityDriver.pressKey("Mouse0", 1,1);
                altUnityDriver.moveMouseAndWait(player.x, player.y-500, 1);
                altUnityDriver.pressKeyAndWait("Mouse0", 1,1);
        
        
                stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
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

#### Description:

Simulate scroll mouse action in your game. This command waist for the action to finish. If you don't want to wait until the action to finish use [ScrollMouse]({{ site.baseurl }}/pages/commands/input-actions/scroll-mouse)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| speed      |     float    |   false   | Set how fast to scroll. Positive values will scroll up and negative values will scroll down.|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltScrollMouseParameters](other/java-builders.html#altscrollmouseparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples
```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO
    .. code-tab:: java
        //TODO


    .. code-tab:: py
        //TODO
```
###  ScrollMouse

#### Description:

Simulate scroll mouse action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [ScrollMouseAndWait]({{ site.baseurl }}/pages/commands/input-actions/scroll-mouse-and-wait)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| speed      |     float    |   false   | Set how fast to scroll. Positive values will scroll up and negative values will scroll down.|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltScrollMouseParameters](other/java-builders.html#altscrollmouseparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples
```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO
    .. code-tab:: java
        //TODO


    .. code-tab:: py
        //TODO
```

###  SwipeAndWait

#### Description:

Simulate a swipe action in your game. This command waist for the action to finish. If you don't want to wait until the action to finish use [Swipe]({{ site.baseurl }}/pages/commands/input-actions/swipe)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| start      |     Vector2(C#)    |   false   | Starting location of the swipe|
| end      |     Vector2(C#)    |   false   | Ending location of the swipe|
| xStart      |     float(python/java)    |   false   | x coordinate of the screen where the swipe begins.|
| yStart      |     float(python/java)    |   false   | y coordinate of the screen where the swipe begins|
| xEnd      |     float(python/java)    |   false   | x coordinate of the screen where the swipe ends|
| yEnd      |     float(python/java)    |   false   | x coordinate of the screen where the swipe ends|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|


#### Examples

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
            public void testMultipleDragAndDropWait() throws Exception {
                AltUnityObject altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
                AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box2");
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image3");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);
        
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipeAndWait(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 1);
                String imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
                String imageSourceDropZone = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
                assertNotSame(imageSource, imageSourceDropZone);
        
                imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
                imageSourceDropZone = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
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

#### Description:

Simulate a swipe action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [SwipeAndWait]({{ site.baseurl }}/pages/commands/input-actions/swipe-and-wait)

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| start      |     Vector2(C#)    |   false   | Starting location of the swipe|
| end      |     Vector2(C#)    |   false   | Ending location of the swipe|
| xStart      |     float(python/java)    |   false   | x coordinate of the screen where the swipe begins.|
| yStart      |     float(python/java)    |   false   | y coordinate of the screen where the swipe begins|
| xEnd      |     float(python/java)    |   false   | x coordinate of the screen where the swipe ends|
| yEnd      |     float(python/java)    |   false   | x coordinate of the screen where the swipe ends|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|


#### Examples

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
            public void testMultipleDragAndDrop() throws Exception {
                AltUnityObject altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
                AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipe(altElement1.x, altElement1.y,altElement2.x, altElement2.y, 2);
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box2");
                altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 2);
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image3");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 3);
        
        
                altElement1 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1");
                altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Box1");
                altUnityDriver.swipe(altElement1.x, altElement1.y, altElement2.x, altElement2.y, 5);
        
                Thread.sleep(6000);
        
                String imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image1").getComponentProperty("UnityEngine.UI.Image", "sprite");
                String imageSourceDropZone= altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop Image").getComponentProperty("UnityEngine.UI.Image", "sprite");
                assertNotSame(imageSource, imageSourceDropZone);
        
                imageSource = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drag Image2").getComponentProperty("UnityEngine.UI.Image", "sprite");
                imageSourceDropZone = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Drop").getComponentProperty("UnityEngine.UI.Image", "sprite");
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

#### Description:

Similar command like swipe but instead of swipe from point A to point B you are able to give list a points. 

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| positions      |   List/Array of Vector2    |   false   | collection of positions on the screen where the swipe be made|
| duration      |     float    |   false   | how many seconds the swipe will need to complete|


#### Examples

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
        public void testResizePanelWithMultipointSwipe() throws Exception {
            AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");

            List<Vector2> positions = Arrays.asList(
                altElement.getScreenPosition(), 
                new Vector2(altElement.x + 100, altElement.y + 100),
                new Vector2(altElement.x + 100, altElement.y + 200));
            
            altUnityDriver.multipointSwipe(positions, 3);
            Thread.sleep(3000);

            AltUnityObject altElementAfterResize = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");
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

#### Description:

Similar command like `SwipeAndWait` but instead of swipe from point A to point B you are able to give list a points. 

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| positions      |   List/Array of Vector2    |   false   | collection of positions on the screen where the swipe be made|
| duration      |     float    |   false   | how many seconds the swipe will need to complete|


#### Examples

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
        public void testResizePanelWithMultipointSwipe() throws Exception {
            AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");

            List<Vector2> positions = Arrays.asList(
                altElement.getScreenPosition(), 
                new Vector2(altElement.x + 100, altElement.y + 100),
                new Vector2(altElement.x + 100, altElement.y + 200));
            
            altUnityDriver.multipointSwipeAndWait(positions, 3);

            AltUnityObject altElementAfterResize = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Resize Zone");
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

#### Description:

Simulate a tap action on the screen at the given coordinates.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| x      |     float    |   false   |  x coordinate of the screen|
| y      |     float    |   false   |  y coordinate of the screen|



#### Examples

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
                AltUnityObject capsule = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule");
                AltUnityObject capsuleInfo = altUnityDriver.findObject(AltUnityDriver.By.NAME,"CapsuleInfo");
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

#### Description:

Simulate n number of tap actions on the screen at the given coordinates .

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| x      |     float    |   false   |  x coordinate of the screen|
| y      |     float    |   false   |  y coordinate of the screen|
| count  |     int      |   false   | number of taps|
| interval |   float    |   true    | how many seconds will be between touches |



#### Examples

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
            public void TestCustomTap() throws InterruptedException {
                AltFindObjectsParameters altFindObjectsParameters1 = new AltFindObjectsParameters.Builder(
                    AltUnityDriver.By.NAME, "ButtonCounter").build();
                AltUnityObject counterButton = altUnityDriver.findObject(altFindObjectsParameters1);
                AltFindObjectsParameters altFindObjectsParameters2 = new AltFindObjectsParameters.Builder(
                    AltUnityDriver.By.NAME, "ButtonCounter/Text").build();
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

#### Description:

Simulates device rotation action in your game.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| acceleration      |     Vector3(C#)    |   false   | Linear acceleration of a device in three-dimensional space|
| x      |     float(python/java)    |   false   |  Linear acceleration of a device on x|
| y      |     float(python/java)    |   false   |  Linear acceleration of a device on y|
| z      |     float(python/java)    |   false   |  Linear acceleration of a device on z|



#### Examples
```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO

    .. code-tab:: java
        //TODO


    .. code-tab:: py
        //TODO

```
## ObjectCommands
###  CallComponentMethod

#### Description:

Invoke a method from an existing component of the object.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| componentName      |     string    |   false   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| methodName      |     string    |   false   |   The name of the public method that we want to call |
| parameters      |     string    |   false   |   a string containing the serialized parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]" Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.|
| typeOfParamaters      |     string    |   false   |  a string containing the serialized type of parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints |
| assemblyName  | string | true | name of the assembly where the component is |

###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltComponentMethodParameters](other/java-builders.html#ltcomponentmethodparameters) which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples

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
        public void TestCallMethodWithMultipleDefinitions() throws Exception {

            AltUnityObject capsule=altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule");
            capsule.callComponentMethod("","Capsule", "Test","2","System.Int32");
            AltUnityObject capsuleInfo=altUnityDriver.findObject(AltUnityDriver.By.NAME,"CapsuleInfo");
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

#### Description:

Invoke static methods from your game.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| typeName      |     string    |   false   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| methodName      |     string    |   false   |   The name of the public method that we want to call |
| parameters      |     string    |   false   |   a string containing the serialized parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]" Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.|
| typeOfParamaters      |     string    |   false   |  a string containing the serialized type of parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints |
| assemblyName  | string | true | name of the assembly where the component is |

###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltCallStaticMethodsParameters](other/java-builders.html#altcallstaticmethodsparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples

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
        public void TestCallStaticMethod() throws Exception {

            altUnityDriver.callStaticMethods("UnityEngine.PlayerPrefs", "SetInt","Test?1");
            int a=Integer.parseInt(altUnityDriver.callStaticMethods("UnityEngine.PlayerPrefs", "GetInt", "Test?2"));
            assertEquals(1,a);
        }


    .. code-tab:: py
        def test_call_static_method(self):
            self.altdriver.call_static_methods("UnityEngine.PlayerPrefs", "SetInt","Test?1",assembly="UnityEngine.CoreModule")
            a=int(self.altdriver.call_static_methods("UnityEngine.PlayerPrefs", "GetInt", "Test?2",assembly="UnityEngine.CoreModule"))
            self.assertEquals(1,a)

```

###  ClickEvent

#### Description:

Simulate a click on the object. It will click the object even if the object is not visible something that you could not do on a real device.


#### Examples

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
        public void testDifferentCamera() throws Exception {
            AltUnityObject altButton = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Button","Main Camera");
            altButton.clickEvent();
            altButton.clickEvent();
            AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule", "Main Camera");
            AltUnityObject altElement2 = altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule", "Camera");
            assertNotSame(altElement.x, altElement2.x);
            assertNotSame(altElement.y, altElement2.y);
        }



    .. code-tab:: py
     //TODO

```


###  DragObject

#### Description:

Drag an object to a certain position on the screen

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| position      |     Vecto2(C#)    |   false   | coordinates of the screen where the object will be dragged|
| x      |     int(python/java)    |   false   |   x coordinate of the screen where the object will be dragged |
| y      |     int(python/java)    |   false   |   y coordinate of the screen where the object will be dragged|



#### Examples

```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO

    .. code-tab:: java
        //TODO

    .. code-tab:: py
        //TODO

```

###  DropObject

#### Description:

Drop an object to a certain position on the screen

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| position      |     Vecto2(C#)    |   false   | coordinates of the screen where the object will be dragged|
| x      |     int(python/java)    |   false   |   x coordinate of the screen where the object will be dragged |
| y      |     int(python/java)    |   false   |   y coordinate of the screen where the object will be dragged|



#### Examples

```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO

    .. code-tab:: java
        //TODO

    .. code-tab:: py
        //TODO

```


###  GetAllComponents(C#)

#### Description:

Get all components attached to an object

#### Examples

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

#### Description:

Get all methods from a component attached to an object


#### Examples

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

#### Description:

Get all properties from a component attached to an object. This method is implement only in C#.

#### Examples


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

#### Description:

Get the value of a property from one of the component of the object.

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| componentName      |     string    |   false   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| propertyName      |     string    |   false   |  name of the property of which value you want |
| assemblyName  | string | true | name of the assembly where the component is |

###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltGetComponentPropertyParameters](other/java-builders.html#altgetcomponentpropertyparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

#### Examples

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
        public void testGetComponentProperty() throws Exception {
            String componentName = "AltUnityRunner";
            String propertyName = "SocketPortNumber";
            AltUnityObject altElement = altUnityDriver.findObject(AltUnityDriver.By.NAME,"AltUnityRunnerPrefab");
            assertNotNull(altElement);
            String propertyValue = altElement.getComponentProperty(componentName, propertyName);
            assertEquals(propertyValue, "13000");
        }



    .. code-tab:: py
        def test_get_component_property(self):
            self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
            result = self.altdriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
            self.assertEqual(result,"[1,2,3]")

```


###  GetText

#### Description:

Get text value from a Button, Text, InputField. This also works with TextMeshPro elements.


#### Examples

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
            String text = altUnityDriver.findObject(AltUnityDriver.By.NAME,name).getText();
            long timeStart = System.currentTimeMillis();
            AltUnityObject altElement = altUnityDriver.waitForObjectWithText(AltUnityDriver.By.NAME,name, text);
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

#### Description:

 Simulates a double tap on the object that trigger multiple events similar to a real double tap but they happens in one frame.


#### Examples

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
        public void TestDoubleTap() throws InterruptedException {
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

#### Description:

Get text value from a Button, Text, InputField. This also works with TextMeshPro elements.


#### Examples
```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO

    .. code-tab:: java
        //TODO

    .. code-tab:: py
        //TODO

```


###  PointerUpFromObject

#### Description:

Simulates pointer up action on the object


#### Examples
```eval_rst
.. tabs::

    .. code-tab:: c#
        //TODO

    .. code-tab:: java
        //TODO

    .. code-tab:: py
        //TODO

```

## UnityCommands 
###  DeleteKeyPlayerPref  

#### Description:

Delete from games player pref a key


#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| keyname      |     sting    |   false   | Key to be deleted|

#### Return 
- Nothing

#### Examples
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
            public void testDeleteKey() throws Exception {
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

#### Description:

Delete entire player pref of the game

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|None|
#### Return:
- Nothing

#### Examples
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
            public void testSetKeyFloat() throws Exception {
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

#### Description:

Return all cameras that are in the scene. This method is only implemented in C#


#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|None|

#### Return
- List of AltUnityObjects

#### Examples
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

#### Description:

Return list of scene in the game

#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|None|

#### Return

- List of string

#### Examples

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

#### Description:

Get the current active scene.


#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|None|

#### Return
- String

#### Examples
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
            public void testGetCurrentScene() throws Exception {
                assertEquals("Scene 1 AltUnityDriverTestScene", altUnityDriver.getCurrentScene());
            }


    .. code-tab:: py

       //TODO
```

## Screenshot

###  GetPNGScreenshot

#### Description:

Create a screenshot of the current scene in png format.


#### Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
|path| string | false | location where the image is created|

#### Return
- nothing

#### Examples
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
        public void testScreenshot(){
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
