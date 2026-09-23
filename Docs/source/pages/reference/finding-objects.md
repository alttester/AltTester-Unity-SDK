# Finding objects

## Find Objects

### FindObject

Finds the first object in the scene that respects the given criteria. Check [By](by-selector.html#by-selector) for more information about criteria.

```eval_rst

.. important::
     Indexer functionality was changed in 2.0.2 to match that of XPath and it no longer returns the n-th child of the object. Now it returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0 so the first object has the index 0 then the second object has the index 1 and so on. For example //Button/Text[1] will return the second object named `Text` that is the child of the `Button`
    
``````

**_Parameters_**

```eval_rst
.. list-table:: FindObject Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_strategy``
     - `By <by-selector.html#by-selector>`_
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to see if it respects the criteria or not.
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinates of the object will be calculated. If no camera is given, it will search through all cameras that are in the scene until some camera sees the object or return the screen coordinates of the object calculated to the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, will match only objects that are active in hierarchy. If `false`, will match all objects.
```


**_Returns_**

- [AltObject](altobject.html#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindAltObject()
        {
            const string name = "Capsule";
            var altObject = altDriver.FindObject(By.NAME,name);
            Assert.NotNull(altObject);
            Assert.AreEqual(name, altObject.name);
        }

    .. code-tab:: java

        @Test
        public void testfindObject() throws Exception
        {
            String name = "Capsule";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    name).isEnabled(true).withCamera(AltDriver.By.NAME, "Main Camera").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            assertNotNull(altObject);
            assertEquals(name, altObject.name);
        }

    .. code-tab:: py

        def test_find_object(self):
            altObject = self.alt_driver.find_object(By.NAME, "Capsule")
            self.assertEqual(altObject.name, "Capsule")

    .. code-tab:: robot

        Test Find Object By Name
            ${capsule}=         Find Object         NAME        Capsule
            ${capsule_name}=    Get Object Name     ${capsule}
            Should Be Equal     ${capsule_name}     Capsule

```

### FindObjects

Finds all objects in the scene that respects the given criteria. Check [By](by-selector.html#by-selector) for more information about criteria.
```eval_rst

.. important::
     Indexer functionality was changed in 2.0.2 to match that of XPath and it no longer returns the n-th child of the object. Now it returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0 so the first object has the index 0 then the second object has the index 1 and so on. For example //Button/Text[1] will return the second object named `Text` that is the child of the `Button`
    
``````

**_Parameters_**

```eval_rst
.. list-table:: FindObjects Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_strategy``
     - `By <by-selector.html#by-selector>`_
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to see if it respects the criteria or not.
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinates of the object will be calculated. If no camera is given, it will search through all cameras that are in the scene until some camera sees the object or return the screen coordinates of the object calculated to the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, will match only objects that are active in hierarchy. If `false`, will match all objects.
```

**_Returns_**

- List of [AltObjects](altobject.html#altobject) or an empty list if no objects were found.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindObjectsByTag()
        {
            var altObjects = altDriver.FindObjects(By.TAG,"plane");
            Assert.AreEqual(2, altObjects.Count);
            foreach(var altObject in altObjects)
            {
                Assert.AreEqual("Plane", altObject.name);
            }
        }

    .. code-tab:: java

        @Test
        public void testFindAltObjects() throws Exception
        {
            String name = "Plane";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).isEnabled(true).withCamera(AltDriver.By.NAME, "Main Camera").build();
            AltObject[] altObjects = altDriver.findObjects(altFindObjectsParams);
            assertNotNull(altObjects);
            assertEquals(altObjects[0].name, name);
        }

    .. code-tab:: py

        def test_find_objects_by_layer(self):
                self.alt_driver.load_scene('Scene 1 AltDriverTestScene')
                altObjects = self.alt_driver.find_objects(By.LAYER,"Default")
                self.assertEquals(8, len(altObjects))

    .. code-tab:: robot

        Test Find Objects By Name
            ${alt_objects}=    Find Objects    NAME    Plane
            ${appears}=    Get Length    ${alt_objects}
            Should Be Equal As Integers    2    ${appears}

```

### FindObjectWhichContains

Finds the first object in the scene that respects the given criteria. Check [By](by-selector.html#by-selector) for more information about criteria.

```eval_rst

.. important::
     Indexer functionality was changed in 2.0.2 to match that of XPath and it no longer returns the n-th child of the object. Now it returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0 so the first object has the index 0 then the second object has the index 1 and so on. For example //Button/Text[1] will return the second object named `Text` that is the child of the `Button`
    
``````

**_Parameters_**

```eval_rst
.. list-table:: FindObjectWhichContains Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_strategy``
     - `By <by-selector.html#by-selector>`_
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to see if it respects the criteria or not.
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinates of the object will be calculated. If no camera is given, it will search through all cameras that are in the scene until some camera sees the object or return the screen coordinates of the object calculated to the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, will match only objects that are active in hierarchy. If `false`, will match all objects.
```

**_Returns_**

- [AltObject](altobject.html#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindObjectWhichContains()
        {
            var altObject = altDriver.FindObjectWhichContains(By.NAME, "Event");
            Assert.AreEqual("EventSystem", altObject.name);
        }

    .. code-tab:: java

        @Test
        public void TestFindObjectWhichContains()
        {
            String name = "Event";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                   name).isEnabled(true).withCamera(AltDriver.By.NAME, "Main Camera").build();
            AltObject altObject = altDriver.findObjectWhichContains(altFindObjectsParams);
            assertEquals("EventSystem", altObject.name);
        }

    .. code-tab:: py

        def test_find_object_which_contains(self):
            altObject = self.alt_driver.find_object_which_contains(By.NAME, "Event");
            self.assertEqual("EventSystem", altObject.name)

    .. code-tab:: robot

        Test Find Object Which Contains
            ${alt_object}=          Find Object Which Contains      NAME            Event
            ${alt_object_name}=     Get Object Name                 ${alt_object}
            Should Contain          ${alt_object_name}              EventSystem

```

### FindObjectsWhichContain

Finds all objects in the scene that respects the given criteria. Check [By](by-selector.html#by-selector) for more information about criteria.

```eval_rst

.. important::
     Indexer functionality was changed in 2.0.2 to match that of XPath and it no longer returns the n-th child of the object. Now it returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0 so the first object has the index 0 then the second object has the index 1 and so on. For example //Button/Text[1] will return the second object named `Text` that is the child of the `Button`
    
``````

**_Parameters_**

```eval_rst
.. list-table:: FindObjectsWhichContain Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_strategy``
     - `By <by-selector.html#by-selector>`_
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to see if it respects the criteria or not.
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinates of the object will be calculated. If no camera is given, it will search through all cameras that are in the scene until some camera sees the object or return the screen coordinates of the object calculated to the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, will match only objects that are active in hierarchy. If `false`, will match all objects.
```

**_Returns_**

- List of [AltObjects](altobject.html#altobject) or an empty list if no objects were found.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindObjects()
        {
            var planes = altDriver.FindObjectsWhichContain(By.NAME, "Plane");
            Assert.AreEqual(3, planes.Count);
        }

    .. code-tab:: java

        @Test
        public void testFindObjectsWhereNameContains() throws Exception
        {
            String name = "Pla";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                name).isEnabled(true).withCamera("Main Camera").build();
            AltObject[] altObjects = altDriver.findObjectsWhichContain(altFindObjectsParams);
            assertNotNull(altObjects);
            assertTrue(altObjects[0].name.contains(name));
        }

    .. code-tab:: py

        def test_creating_stars(self):
            self.alt_driver.load_scene("Scene 5 Keyboard Input")

            stars = self.alt_driver.find_objects_which_contain(By.NAME, "Star", "Player2")
            self.assertEqual(1, len(stars))
            player = self.alt_driver.find_objects_which_contain(By.NAME, "Player", "Player2")

            self.alt_driver.move_mouse(int(stars[0].x), int(player[0].y) + 500, 1)
            time.sleep(1.5)

            self.alt_driver.press_key(AltKeyCode.Mouse0, 1,0)
            self.alt_driver.move_mouse_and_wait(int(stars[0].x), int(player[0].y) - 500, 1)
            self.alt_driver.press_key(AltKeyCode.Mouse0, 1,0)

            stars = self.alt_driver.find_objects_which_contain(By.NAME, "Star")
            self.assertEqual(3, len(stars))

    .. code-tab:: robot
        
        Test Find Objects Which Contain By Name
            ${alt_objects}=    Find Objects Which Contain    NAME    Capsule
            ${appears}=    Get Length    ${alt_objects}
            Should Be Equal As Integers    2    ${appears}
            FOR    ${obj}    IN    @{alt_objects}
                ${name}=    Get Object Name    ${obj}
                Should Contain    ${name}    Capsule
            END

```

### FindObjectAtCoordinates

Retrieves the Unity object at given coordinates.

Uses `EventSystem.RaycastAll` to find object. If no object is found then it uses `UnityEngine.Physics.Raycast` and `UnityEngine.Physics2D.Raycast` and returns the one closer to the camera.

**_Parameters_**

```eval_rst
.. list-table:: FindObjectAtCoordinates Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``coordinates``
     - AltVector2
       *Python, Robot Framework:* list/tuple/dict
     - Yes
     - The screen coordinates.
```

**_Returns_**

- [AltObject](altobject.html#altobject) - The UI object hit by event system Raycast, nothing otherwise.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindElementAtCoordinates()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var element = altDriver.FindObjectAtCoordinates(new AltVector2(80 + counterButton.x, 15 + counterButton.y));
            Assert.AreEqual("Text", element.name);
        }

    .. code-tab:: java

        @Test
        public void testFindElementAtCoordinates() {
            AltObject counterButton = altDriver.findObject(new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "ButtonCounter").build());

            AltObject element = altDriver.findObjectAtCoordinates(
                    new AltFindObjectAtCoordinatesParams.Builder(new Vector2(80 + counterButton.x, 15 + counterButton.y))
                            .build());
            assertEquals("Text", element.name);
        }

    .. code-tab:: py

        def test_find_object_by_coordinates(self):
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene")
            counter_button = self.alt_driver.find_object(By.NAME, "ButtonCounter")

            element = self.alt_driver.find_object_at_coordinates([80 + counter_button.x, 15 + counter_button.y])
            assert "Text" == element.name

    .. code-tab:: robot

        Test Find Object By Coordinates
            ${counter_button}=    Find Object    NAME    ButtonCounter
            ${counter_button_x}=    Get Object X    ${counter_button}
            ${counter_button_y}=    Get Object Y    ${counter_button}
            ${coordinate_x}=    Evaluate    80+${counter_button_x}
            ${coordinate_y}=    Evaluate    15+${counter_button_y}
            ${coordinates}=    Create List    ${coordinate_x}    ${coordinate_y}
            ${element}=    Find Object At Coordinates    ${coordinates}
            ${element_name}=    Get Object Name    ${element}
            Should Be Equal As Strings    ${element_name}    Text

```

### GetAllElements

Returns information about every objects loaded in the currently loaded scenes. This also means objects that are set as DontDestroyOnLoad.

**_Parameters_**

```eval_rst
.. list-table:: GetAllElements Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinates of the object will be calculated. If no camera is given, it will search through all cameras that are in the scene until some camera sees the object or return the screen coordinates of the object calculated to the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, will match only objects that are active in hierarchy. If `false`, will match all objects.
```

**_Returns_**

- List of [AltObjects](altobject.html#altobject) or an empty list if no objects were found.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetAllEnabledObjects()
        {
            var altObjects = altDriver.GetAllElements(enabled: true);
            Assert.IsNotEmpty(altObjects);
        }

    .. code-tab:: java

        @Test
        public void testGetAllElements() throws Exception {
            AltGetAllElementsParams altGetAllElementsParams = new AltGetAllElementsParams.Builder().withCamera(AltDriver.By.NAME, "Main Camera").isEnabled(true).build();
            AltObject[] altObjects = altDriver.getAllElements(altGetAllElementsParams);
            assertFalse(altObjects.isEmpty());
        }

    .. code-tab:: py

        def test_get_all_elements(self):
            alt_elements = self.alt_driver.get_all_elements(enabled=False)
            assert alt_elements

    .. code-tab:: robot

        Test Get All Elements
            ${elements}=    Get All Elements    enabled=${False}
            Should Not Be Empty    ${elements}
            ${expected_names}=    Create List    EventSystem    Canvas    Panel Drag Area    Panel    Header    Text    Drag Zone    Resize Zone    Close Button    Debugging    SF Scene Elements    Main Camera    Background    Particle System
            ${input_marks}=    Create List
            ${names}=    Create List
            FOR    ${element}    IN    @{elements}
                ${name}=    Get Object Name    ${element}
                Run Keyword If    '${name}'== 'InputMark(Clone)'    Append TransformId To List    ${element}    ${input_marks}
                ${element_name}=    Get Object Name    ${element}
                Append To List    ${names}    ${element_name}
            END
            FOR    ${name}    IN    @{expected_names}
                Should Contain    ${names}    ${name}
            END

```

### WaitForObject

Waits until it finds an object that respects the given criteria or until the timeout limit is reached. Check [By](by-selector.html#by-selector) for more information about criteria.

```eval_rst

.. important::
     Indexer functionality was changed in 2.0.2 to match that of XPath and it no longer returns the n-th child of the object. Now it returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0 so the first object has the index 0 then the second object has the index 1 and so on. For example //Button/Text[1] will return the second object named `Text` that is the child of the `Button`
    
``````

**_Parameters_**

```eval_rst
.. list-table:: WaitForObject Parameters
   :widths: 15 15 10 60
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_type``
     - `By <by-selector.html#by-selector>`_
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to see if it meets the criteria.
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to determine if they respect the criteria or not. If no camera is given, it will search through all cameras in the scene until some camera sees the object or return the screen coordinates of the object based on the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, matches only objects that are active in the hierarchy. If `false`, matches all objects.
   * - ``timeout``
     - double
     - No
     - The number of seconds it will wait for the object. The default is **20 seconds**.
   * - ``interval``
     - double
     - No
     - The number of seconds after which it will retry finding the object. The interval should be **smaller than the timeout**.
```

**_Returns_**

- [AltObject](altobject.html#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForObject()
        {
            const string name = "Capsule";
            var altObject = altDriver.WaitForObject(By.NAME, name);
        }

    .. code-tab:: java

        @Test
        public void testWaitForObject() {
            String name = "Capsule";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                            name).build();
            AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                            altFindObjectsParams).build();
            AltObject altObject = altDriver.waitForObject(altWaitForObjectsParams);
        }

    .. code-tab:: py

        def test_wait_for_object(self):
            alt_object = self.alt_driver.wait_for_object(By.NAME, "Capsule")

    .. code-tab:: robot

        Test Wait For Object By Name
            ${capsule}=         Wait For Object    NAME         Capsule

```

### WaitForObjectWhichContains

Waits until it finds an object that respects the given criteria or time runs out and will throw an error. Check [By](by-selector.html#by-selector) for more information about criteria.

```eval_rst

.. important::
     Indexer functionality was changed in 2.0.2 to match that of XPath and it no longer returns the n-th child of the object. Now it returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0 so the first object has the index 0 then the second object has the index 1 and so on. For example //Button/Text[1] will return the second object named `Text` that is the child of the `Button`
    
``````

**_Parameters_**

```eval_rst
.. list-table:: WaitForObjectWhichContains Parameters
   :widths: 15 15 10 60
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_strategy``
     - `By <by-selector.html#by-selector>`_
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to determine if it meets the criteria.
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to determine if they respect the criteria or not. If no camera is given, it will search through all cameras in the scene until some camera sees the object or return the screen coordinates of the object based on the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, matches only objects that are active in the hierarchy. If `false`, matches all objects.
   * - ``timeout``
     - double
     - No
     - The number of seconds it will wait for the object. The default is **20 seconds**.
   * - ``interval``
     - double
     - No
     - The number of seconds after which it will retry finding the object. The interval should be **smaller than the timeout**.
```

**_Returns_**

- [AltObject](altobject.html#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForObjectWhichContains()
        {
            var altObject = altDriver.WaitForObjectWhichContains(By.NAME, "Canva");
        }

    .. code-tab:: java

        @Test
        public void TestWaitForObjectWhichContains() {
            AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME, "Canva").build();
            AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                    altFindObjectsParametersObject).build();
            AltObject altObject = altDriver.waitForObjectWhichContains(altWaitForObjectsParams);
        }

    .. code-tab:: py

        def test_wait_for_object_which_contains(self):
            alt_object = self.alt_driver.wait_for_object_which_contains(By.NAME, "Canva")

    .. code-tab:: robot

        Test Wait For Object Which Contains
            ${alt_object}=    Wait For Object Which Contains    NAME    Canva

```

### WaitForObjectNotBePresent

Waits until the object in the scene that respects the given criteria is no longer in the scene or until the timeout limit is reached. Check [By](by-selector.html#by-selector) for more information about criteria.

```eval_rst

.. important::
     Indexer functionality was changed in 2.0.2 to match that of XPath and it no longer returns the n-th child of the object. Now it returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0 so the first object has the index 0 then the second object has the index 1 and so on. For example //Button/Text[1] will return the second object named `Text` that is the child of the `Button`
    
``````

**_Parameters_**

```eval_rst
.. list-table:: WaitForObjectNotBePresent Parameters
   :widths: 15 15 10 60
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_strategy``
     - `By <by-selector.html#by-selector>`_
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to determine if it meets the criteria.
   * - ``cameraBy``
     - `By <by-selector.html#by-selector>`_
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to determine if they respect the criteria or not. If no camera is given, it will search through all cameras in the scene until some camera sees the object or return the screen coordinates of the object based on the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, matches only objects that are active in the hierarchy. If `false`, matches all objects.
   * - ``timeout``
     - double
     - No
     - The number of seconds it will wait for the object. The default is **20 seconds**.
   * - ``interval``
     - double
     - No
     - The number of seconds after which it will retry finding the object. The interval should be **smaller than the timeout**.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForObjectNotBePresent()
        {
            altDriver.WaitForObjectNotBePresent(By.NAME, "Capsulee");
        }

    .. code-tab:: java

        @Test
        public void TestWaitForObjectToNotBePresent(){
            AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Capsulee").build();
            AltWaitForObjectsParams altWaitForObjectsParameters = new AltWaitForObjectsParams.Builder(altFindObjectsParams).build();
            altDriver.waitForObjectToNotBePresent(altWaitForObjectsParams);
        }

    .. code-tab:: py

        def test_wait_for_object_to_not_be_present(self):
            self.alt_driver.wait_for_object_to_not_be_present(By.NAME, "Capsulee")

    .. code-tab:: robot

        Test Wait For Object To Not Be Present
            Wait For Object To Not Be Present    NAME    Capsulee

```
