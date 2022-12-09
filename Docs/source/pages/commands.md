# API

If you are looking for information on a specific function, class or method, this part of the documentation is for you.

## AltDriver

The **AltDriver** class represents the main game driver component. When you instantiate an AltDriver in your tests, you can use it to "drive" your game like one of your users would, by interacting with all the game objects, their properties and methods.

An AltDriver instance will connect to the running instrumented Unity application. In the constructor, we need to tell the driver where (on what IP and on what port) the instrumented Unity App is running. We can also set some more advanced parameters, as shown in the table below:

**_Parameters_**

| Name          | Type    | Required | Description                                                                           |
| ------------- | ------- | -------- | ------------------------------------------------------------------------------------- |
| host          | string  | No       | The IP or hostname AltTester Unity SDK is listening on. The default value is "127.0.0.1". |
| port          | int     | No       | The default value is 13000.                                                           |
| enableLogging | boolean | No       | The default value is false.                                                           |

Once you have an instance of the _AltDriver_, you can use all the available commands to interact with the game. The available methods are the following:

### Find Objects

#### FindObject

Finds the first object in the scene that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                      |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene.  |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- AltObject

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
            altObject = self.altDriver.find_object(By.NAME, "Capsule")
            self.assertEqual(altObject.name, "Capsule")
```

#### FindObjects

Finds all objects in the scene that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                      |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene.  |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- List of AltObjects or an empty list if no objects were found.

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
                self.altDriver.load_scene('Scene 1 AltDriverTestScene')
                altObjects = self.altDriver.find_objects(By.LAYER,"Default")
                self.assertEquals(8, len(altObjects))

```

#### FindObjectWhichContains

Finds the first object in the scene that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                      |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene.  |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- AltObject

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
            altObject = self.altDriver.find_object_which_contains(By.NAME, "Event");
            self.assertEqual("EventSystem", altObject.name)

```

#### FindObjectsWhichContain

Finds all objects in the scene that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                               |
| ----------- | ------------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                     |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                    |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                     |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene. |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                        |

**_Returns_**

- List of AltObjects or an empty list if no objects were found.

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
            self.altDriver.load_scene("Scene 5 Keyboard Input")

            stars = self.altDriver.find_objects_which_contain(By.NAME, "Star", "Player2")
            self.assertEqual(1, len(stars))
            player = self.altDriver.find_objects_which_contain(By.NAME, "Player", "Player2")

            self.altDriver.move_mouse(int(stars[0].x), int(player[0].y) + 500, 1)
            time.sleep(1.5)

            self.altDriver.press_key(AltKeyCode.Mouse0, 1,0)
            self.altDriver.move_mouse_and_wait(int(stars[0].x), int(player[0].y) - 500, 1)
            self.altDriver.press_key(AltKeyCode.Mouse0, 1,0)

            stars = self.altDriver.find_objects_which_contain(By.NAME, "Star")
            self.assertEqual(3, len(stars))

```

#### FindObjectAtCoordinates

Retrieves the Unity object at given coordinates.

Uses `EventSystem.RaycastAll` to find object. If no object is found then it uses `UnityEngine.Physics.Raycast` and `UnityEngine.Physics2D.Raycast` and returns the one closer to the camera.

**_Parameters_**

| Name        | Type    | Required | Description             |
| ----------- | ------- | -------- | ----------------------- |
| coordinates | Vector2 | Yes      | The screen coordinates. |

**_Returns_**

- AltObject - The UI object hit by event system Raycast, nothing otherwise.

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
            self.altdriver.load_scene("Scene 1 AltDriverTestScene")
            counter_button = self.altdriver.find_object(By.NAME, "ButtonCounter")

            element = self.altdriver.find_object_at_coordinates([80 + counter_button.x, 15 + counter_button.y])
            assert "Text" == element.name
```

#### GetAllElements

Returns information about every objects loaded in the currently loaded scenes. This also means objects that are set as DontDestroyOnLoad.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                      |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene.  |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- List of AltObjects or an empty list if no objects were found.

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
            alt_elements = self.altDriver.get_all_elements(enabled=False)
            assert alt_elements

```

#### WaitForObject

Waits until it finds an object that respects the given criteria or until timeout limit is reached. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                      |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene.  |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |
| timeout     | double             | No       | The number of seconds that it will wait for object.                                                                                                                                                                                                                                                                                                                                                        |
| interval    | double             | No       | The number of seconds after which it will try to find the object again. The interval should be smaller than timeout.                                                                                                                                                                                                                                                                                       |

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

       [Test]
        public void TestWaitForObjectToNotExistFail()
        {
            try
            {
                altDriver.WaitForObjectNotBePresent(By.NAME,"Capsule", timeout: 1, interval: 0.5f);
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
            AltFindObjectsParams altFindObjectsParametersButton = new AltFindObjectsParams.Builder(
                    AltDriver.By.PATH, "//Button").build();
            AltObject altButton = altDriver.findObject(altFindObjectsParametersButton);
            altButton.click();
            altButton.click();
            AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
                    "//Camera").build();
            AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);
            AltFindObjectsParams altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT,
                    "CapsuleCollider").withCamera(By.ID, String.valueOf(camera.id)).build();
            AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                    altFindObjectsParametersCapsule).build();
            AltObject altObject = altDriver.waitForObject(altWaitForObjectsParams);

            assertTrue("True", altObject.name.equals("Capsule"));

            altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH, "//Main Camera").build();
            AltObject camera2 = altDriver.findObject(altFindObjectsParametersCamera);
            altFindObjectsParametersCapsule = new AltFindObjectsParams.Builder(By.COMPONENT, "CapsuleCollider")
                    .withCamera(By.ID, String.valueOf(camera2.id)).build();
            altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(altFindObjectsParametersCapsule).build();
            AltObject altObject2 = altDriver.waitForObject(altWaitForObjectsParams);

            assertNotEquals(altObject.getScreenPosition(), altObject2.getScreenPosition());
        }

    .. code-tab:: py

        def test_wait_for_object(self):
            alt_object = self.altDriver.wait_for_object(By.NAME, "Capsule")
            assert alt_object.name == "Capsule"

```

#### WaitForObjectWhichContains

Waits until it finds an object that respects the given criteria or time runs out and will throw an error. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                               |
| ----------- | ------------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                     |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                    |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                     |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene. |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                        |
| timeout     | double             | No       | The number of seconds that it will wait for object                                                                                                                                                                                                                                                                                                                                                        |
| interval    | double             | No       | The number of seconds after which it will try to find the object again. interval should be smaller than timeout                                                                                                                                                                                                                                                                                           |

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForObjectWhichContains()
        {
            var altObject = altDriver.WaitForObjectWhichContains(By.NAME, "Canva");
            Assert.AreEqual("Canvas", altObject.name);
        }

    .. code-tab:: java

        @Test
        public void TestWaitForObjectWhichContainsWithCameraId() {
            AltFindObjectsParams altFindObjectsParametersCamera = new AltFindObjectsParams.Builder(By.PATH,
                    "//Main Camera").build();
            AltObject camera = altDriver.findObject(altFindObjectsParametersCamera);

            AltFindObjectsParams altFindObjectsParametersObject = new AltFindObjectsParams.Builder(By.NAME, "Canva")
                    .withCamera(By.ID, String.valueOf(camera.id)).build();
            AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                    altFindObjectsParametersObject).build();
            AltObject altObject = altDriver.waitForObjectWhichContains(altWaitForObjectsParams);
            assertEquals("Canvas", altObject.name);

        }

    .. code-tab:: py

        def test_wait_for_object_which_contains(self):
            alt_object = self.altDriver.wait_for_object_which_contains(By.NAME, "Main")
            assert alt_object.name == "Main Camera"

```

#### WaitForObjectNotBePresent

Waits until the object in the scene that respects the given criteria is no longer in the scene or until timeout limit is reached. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                               |
| ----------- | ------------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                     |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                    |
| cameraBy    | [By](#by-selector) | No       | Set what criteria to use in order to find the camera.                                                                                                                                                                                                                                                                                                                                                     |
| cameraValue | string             | No       | The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object calculated to the last camera in the scene. |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                        |
| timeout     | double             | No       | The number of seconds that it will wait for object.                                                                                                                                                                                                                                                                                                                                                       |
| interval    | double             | No       | The number of seconds after which it will try to find the object again. interval should be smaller than timeout.                                                                                                                                                                                                                                                                                          |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForObjectToNotExist()
        {
            altDriver.WaitForObjectNotBePresent(By.NAME, "Capsulee", timeout: 1, interval: 0.5f);
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
            self.altDriver.wait_for_object_to_not_be_present(By.NAME, "Capsuule")

```

### SetCommandResponseTimeout

Sets the value for the command response timeout.

**_Parameters_**

| Name           | Type | Required | Description                                          |
| -------------- | ---- | -------- | ---------------------------------------------------- |
| commandTimeout | int  | Yes      | The duration for a command response from the driver. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetCommandResponseTimeout(commandTimeout);

    .. code-tab:: java

        altDriver.setCommandResponseTimeout(commandTimeout);

    .. code-tab:: py

        altDriver.set_command_response_timeout(command_timeout)

```


### GetDelayAfterCommand

Gets the current delay after a command.

**_Parameters_**

None

**_Returns_**

- The current delay after a command.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.GetDelayAfterCommand();

    .. code-tab:: java

        altDriver.getDelayAfterCommand();

    .. code-tab:: py

        altDriver.get_delay_after_command()

```

### SetDelayAfterCommand

Set the delay after a command.

**_Parameters_**

| Name           | Type | Required | Description                      |
| -------------- | ---- | -------- | -------------------------------- |
| delay          | int  | Yes      | The new delay a after a command. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetDelayAfterCommand(5);

    .. code-tab:: java

        altDriver.setDelayAfterCommand(5);

    .. code-tab:: py

        altDriver.set_delay_after_command(5)

```

### Input Actions

#### KeyDown

Simulates a key down.

**_Parameters_**

| Name    | Type            | Required | Description                                                                            |
| ------- | --------------- | -------- | -------------------------------------------------------------------------------------- |
| keyCode | AltKeyCode      | Yes      | The keyCode of the key simulated to be pressed.                                        |
| power   | int             | Yes      | A value between [-1,1] used for joysticks to indicate how hard the button was pressed. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestKeyDownAndKeyUp()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");
            AltKeyCode kcode = AltKeyCode.A;

            altDriver.KeyDown(kcode, 1);
            var lastKeyDown = altDriver.FindObject(By.NAME, "LastKeyDownValue");
            var lastKeyPress = altDriver.FindObject(By.NAME, "LastKeyPressedValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyDown.GetText(), true));
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyPress.GetText(), true));

            altDriver.KeyUp(kcode);
            var lastKeyUp = altDriver.FindObject(By.NAME, "LastKeyUpValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyUp.GetText(), true));
        }

    .. code-tab:: java

        @Test
        public void TestKeyDownAndKeyUp() throws Exception {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyDownValue").build();
            AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyUpValue").build();
            AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyPressedValue").build();
            AltKeyCode kcode = AltKeyCode.A;
            AltKeyParams altKeyParams = new AltKeyParams.Builder(kcode).build();

            altDriver.KeyDown(altKeyParams);
            Thread.sleep(2000);
            AltObject lastKeyDown = altDriver.findObject(altFindObjectsParameters1);
            AltObject lastKeyPress = altDriver.findObject(altFindObjectsParameters3);
            assertEquals("A", AltKeyCode.valueOf(lastKeyDown.getText()).name());
            assertEquals("A", AltKeyCode.valueOf(lastKeyPress.getText()).name());

            altDriver.KeyUp(kcode);
            Thread.sleep(2000);
            AltObject lastKeyUp = altDriver.findObject(altFindObjectsParameters2);
            assertEquals("A", AltKeyCode.valueOf(lastKeyUp.getText()).name());
        }

    .. code-tab:: py

        def test_key_down_and_key_up(self):
            self.altDriver.load_scene('Scene 5 Keyboard Input')

            self.altDriver.key_down(AltKeyCode.A)
            time.sleep(5)
            lastKeyDown = self.altDriver.find_object(By.NAME, 'LastKeyDownValue')
            lastKeyPress = self.altDriver.find_object(By.NAME, 'LastKeyPressedValue')

            self.assertEqual("A", lastKeyDown.get_text())
            self.assertEqual("A", lastKeyPress.get_text())

            self.altDriver.key_up(AltKeyCode.A)
            time.sleep(5)
            lastKeyUp = self.altDriver.find_object(By.NAME, 'LastKeyUpValue')
            self.assertEqual("A", lastKeyUp.get_text())

```

#### KeyUp

Simulates a key up.

**_Parameters_**

| Name    | Type            | Required | Description                                      |
| ------- | --------------- | -------- | ------------------------------------------------ |
| keyCode | AltKeyCode      | Yes      | The keyCode of the key simulated to be released. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestKeyDownAndKeyUp()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");
            AltKeyCode kcode = AltKeyCode.A;

            altDriver.KeyDown(kcode, 1);
            var lastKeyDown = altDriver.FindObject(By.NAME, "LastKeyDownValue");
            var lastKeyPress = altDriver.FindObject(By.NAME, "LastKeyPressedValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyDown.GetText(), true));
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyPress.GetText(), true));

            altDriver.KeyUp(kcode);
            var lastKeyUp = altDriver.FindObject(By.NAME, "LastKeyUpValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyUp.GetText(), true));
        }

    .. code-tab:: java

        @Test
        public void TestKeyDownAndKeyUp() throws Exception {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyDownValue").build();
            AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyUpValue").build();
            AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyPressedValue").build();
            AltKeyCode kcode = AltKeyCode.A;
            AltKeyParams altKeyParams = new AltKeyParams.Builder(kcode).build();

            altDriver.KeyDown(altKeyParams);
            Thread.sleep(2000);
            AltObject lastKeyDown = altDriver.findObject(altFindObjectsParameters1);
            AltObject lastKeyPress = altDriver.findObject(altFindObjectsParameters3);
            assertEquals("A", AltKeyCode.valueOf(lastKeyDown.getText()).name());
            assertEquals("A", AltKeyCode.valueOf(lastKeyPress.getText()).name());

            altDriver.KeyUp(kcode);
            Thread.sleep(2000);
            AltObject lastKeyUp = altDriver.findObject(altFindObjectsParameters2);
            assertEquals("A", AltKeyCode.valueOf(lastKeyUp.getText()).name());
        }

    .. code-tab:: py

        def test_key_down_and_key_up(self):
            self.altDriver.load_scene('Scene 5 Keyboard Input')

            self.altDriver.key_down(AltKeyCode.A)
            time.sleep(5)
            lastKeyDown = self.altDriver.find_object(By.NAME, 'LastKeyDownValue')
            lastKeyPress = self.altDriver.find_object(By.NAME, 'LastKeyPressedValue')

            self.assertEqual("A", lastKeyDown.get_text())
            self.assertEqual("A", lastKeyPress.get_text())

            self.altDriver.key_up(AltKeyCode.A)
            time.sleep(5)
            lastKeyUp = self.altDriver.find_object(By.NAME, 'LastKeyUpValue')
            self.assertEqual("A", lastKeyUp.get_text())

```

#### HoldButton

Simulates holding left click button down for a specified amount of time at given coordinates.

**_Parameters_**

| Name        | Type    | Required | Default | Description                                           |
| ----------- | ------- | -------- | ------- | ----------------------------------------------------- |
| coordinates | Vector2 | Yes      |         | The coordinates where the button is held down.        |
| duration    | float   | No       | 0.1     | The time measured in seconds to keep the button down. |
| wait        | boolean | No       | true    | If set wait for command to finish.                    |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestHoldButton()
        {
            var button = altDriver.FindObject(By.NAME, "UIButton");
            altDriver.HoldButton(button.GetScreenPosition(), 1);
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            var text = capsuleInfo.GetText();
            Assert.AreEqual(text, "UIButton clicked to jump capsule!");
        }

    .. code-tab:: java

        @Test
        public void testHoldButton() throws Exception {
            AltObject button = altDriver
                    .findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME, "UIButton").build());
            altDriver.holdButton(new AltHoldParams.Builder(button.getScreenPosition()).withDuration(1).build());
            AltObject capsuleInfo = altDriver
                    .findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME, "CapsuleInfo").build());
            String text = capsuleInfo.getText();
            assertEquals(text, "UIButton clicked to jump capsule!");
        }

    .. code-tab:: py

        def test_hold_button(self):
            self.altdriver.load_scene("Scene 1 AltDriverTestScene")
            button = self.altdriver.find_object(By.NAME, "UIButton")
            self.altdriver.hold_button(button.get_screen_position(), 1)

            capsule_info = self.altdriver.find_object(By.NAME, "CapsuleInfo")
            text = capsule_info.get_text()
            assert text == "UIButton clicked to jump capsule!"

```

#### MoveMouse

Simulate mouse movement in your game.

**_Parameters_**

| Name        | Type    | Required | Default | Description                                                                                            |
| ----------- | ------- | -------- | ------- | ------------------------------------------------------------------------------------------------------ |
| coordinates | Vector2 | Yes      |         | The screen coordinates.                                                                                |
| duration    | float   | No       | 0.1     | The time measured in seconds to move the mouse from the current mouse position to the set coordinates. |
| wait        | boolean | No       | true    | If set wait for command to finish.                                                                     |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestCreatingStars()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");

            var stars = altDriver.FindObjectsWhichContain(By.NAME, "Star", cameraValue: "Player2");
            var pressingpoint1 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint1", cameraValue: "Player2");
            Assert.AreEqual(1, stars.Count);

            altDriver.MoveMouse(new AltVector2(pressingpoint1.x, pressingpoint1.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            var pressingpoint2 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint2", cameraValue: "Player2");
            altDriver.MoveMouse(new AltVector2(pressingpoint2.x, pressingpoint2.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            stars = altDriver.FindObjectsWhichContain(By.NAME, "Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java

        @Test
        public void TestCreatingStars2() throws InterruptedException {
            AltObject[] stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(1, stars.length);

            AltObject pressingPoint1 = altDriver.findObject(new AltFindObjectsParams.Builder(By.NAME, "PressingPoint1").withCamera(By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint1.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            AltObject pressingPoint2 = altDriver.findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME, "PressingPoint2").withCamera(AltDriver.By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint2.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(3, stars.length);
        }

    .. code-tab:: py

        def test_creating_stars(self):
            self.altdriver.load_scene("Scene 5 Keyboard Input")
            stars = self.altdriver.find_objects_which_contain(By.NAME, "Star", By.NAME, "Player2")
            assert len(stars) == 1

            self.altdriver.find_objects_which_contain(By.NAME, "Player", By.NAME, "Player2")
            pressing_point_1 = self.altdriver.find_object(By.NAME, "PressingPoint1", By.NAME, "Player2")

            self.altdriver.move_mouse(pressing_point_1.get_screen_position(), duration=1)
            self.altdriver.press_key(AltKeyCode.Mouse0, 1, 1)
            pressing_point_2 = self.altdriver.find_object(By.NAME, "PressingPoint2", By.NAME, "Player2")
            self.altdriver.move_mouse(pressing_point_2.get_screen_position(), duration=1)
            self.altdriver.press_key(AltKeyCode.Mouse0, power=1, duration=1)

            stars = self.altdriver.find_objects_which_contain(By.NAME, "Star")
            assert len(stars) == 3

```

#### PressKey

Simulates key press action in your game.

**_Parameters_**

| Name     | Type            | Required | Default | Description                                                                              |
| -------- | --------------- | -------- | ------- | ---------------------------------------------------------------------------------------- |
| keycode  | AltKeyCode      | Yes      |         | The key code of the key simulated to be pressed.                                         |
| power    | float           | No       | 1       | A value between \[-1,1\] used for joysticks to indicate how hard the button was pressed. |
| duration | float           | No       | 0.1     | The time measured in seconds from the key press to the key release.                      |
| wait     | boolean         | No       | true    | If set wait for command to finish.                                                       |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::
    .. code-tab:: c#

        [Test]
        public void TestCreatingStars()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");

            var stars = altDriver.FindObjectsWhichContain(By.NAME, "Star", cameraValue: "Player2");
            var pressingpoint1 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint1", cameraValue: "Player2");
            Assert.AreEqual(1, stars.Count);

            altDriver.MoveMouse(new AltVector2(pressingpoint1.x, pressingpoint1.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            var pressingpoint2 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint2", cameraValue: "Player2");
            altDriver.MoveMouse(new AltVector2(pressingpoint2.x, pressingpoint2.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            stars = altDriver.FindObjectsWhichContain(By.NAME, "Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java

        @Test
        public void TestCreatingStars2() throws InterruptedException {
            AltObject[] stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(1, stars.length);

            AltObject pressingPoint1 = altDriver.findObject(new AltFindObjectsParams.Builder(By.NAME, "PressingPoint1").withCamera(By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint1.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            AltObject pressingPoint2 = altDriver.findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME, "PressingPoint2").withCamera(AltDriver.By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint2.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(3, stars.length);
        }

    .. code-tab:: py

        def test_creating_stars(self):
            self.altdriver.load_scene("Scene 5 Keyboard Input")
            stars = self.altdriver.find_objects_which_contain(By.NAME, "Star", By.NAME, "Player2")
            assert len(stars) == 1

            self.altdriver.find_objects_which_contain(By.NAME, "Player", By.NAME, "Player2")
            pressing_point_1 = self.altdriver.find_object(By.NAME, "PressingPoint1", By.NAME, "Player2")

            self.altdriver.move_mouse(pressing_point_1.get_screen_position(), duration=1)
            self.altdriver.press_key(AltKeyCode.Mouse0, 1, 1)
            pressing_point_2 = self.altdriver.find_object(By.NAME, "PressingPoint2", By.NAME, "Player2")
            self.altdriver.move_mouse(pressing_point_2.get_screen_position(), duration=1)
            self.altdriver.press_key(AltKeyCode.Mouse0, power=1, duration=1)

            stars = self.altdriver.find_objects_which_contain(By.NAME, "Star")
            assert len(stars) == 3

```

#### PressKeys

Simulates multiple key press action in your game.

**_Parameters_**

| Name     | Type               | Required | Default | Description                                                                                |
| -------- | ------------------ | -------- | ------- | ------------------------------------------------------------------------------------------ |
| keycodes | List\[AltKeyCode\] | Yes      |         | The list of keycodes simulated to be pressed simultaneously.                               |
| power    | float              | No       | 1       | A value between \[-1,1\] used for joysticks to indicate how hard the buttons were pressed. |
| duration | float              | No       | 0.1     | The time measured in seconds from the multiple key press to the multiple key release.      |
| wait     | boolean            | No       | true    | If set, wait for command to finish.                                                        |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::
    .. code-tab:: c#

        [Test]
        public void TestPressKeys()
        {
            AltKeyCode[] keys = { AltKeyCode.K, AltKeyCode.L };
            altDriver.PressKeys(keys);
            var altObject = altDriver.FindObject(By.NAME, "Capsule");
            var finalPropertyValue = altObject.GetComponentProperty<string>("AltExampleScriptCapsule", "stringToSetFromTests", "Assembly-CSharp");
            Assert.AreEqual("multiple keys pressed", finalPropertyValue);
        }

    .. code-tab:: java

        @Test
        public void testPressKeys()
        {
            AltKeyCode[] keys = {AltKeyCode.K, AltKeyCode.L};

            altDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());

            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Capsule").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);

            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule",
                "stringToSetFromTests", "Assembly-CSharp").build();
            String finalPropertyValue = altObject.getComponentProperty(altGetComponentPropertyParams, String.class);

            assertEquals(finalPropertyValue, "multiple keys pressed");
        }

    .. code-tab:: py

        def test_press_keys(self):
            keys = [AltKeyCode.K, AltKeyCode.L]
            self.altdriver.press_keys(keys)

            alt_unity_object = self.altdriver.find_object(By.NAME, "Capsule")
            property_value = alt_unity_object.get_component_property(
                "AltExampleScriptCapsule",
                "stringToSetFromTests",
                "Assembly-CSharp"
            )
            assert property_value == "multiple keys pressed"

```

#### Scroll

Simulate scroll action in your game.

**_Parameters_**

| Name            | Type    | Required | Default | Description                                                                                  |
| --------------- | ------- | -------- | ------- | -------------------------------------------------------------------------------------------- |
| speed           | float   | No       | 1       | Set how fast to scroll. Positive values will scroll up and negative values will scroll down. |
| duration        | float   | No       | 0.1     | The duration of the scroll in seconds.                                                       |
| wait            | boolean | No       | true    | If set wait for command to finish.                                                           |
| speedHorizontal | float   | No       | 1       |Set how fast to scroll right or left.                                                         |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestScroll()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");
            var player2 = altDriver.FindObject(By.NAME, "Player2");
            AltVector3 cubeInitialPosition = new AltVector3(player2.worldX, player2.worldY, player2.worldY);
            altDriver.Scroll(4, 2);

            player2 = altDriver.FindObject(By.NAME, "Player2");
            AltVector3 cubeFinalPosition = new AltVector3(player2.worldX, player2.worldY, player2.worldY);
            Assert.AreNotEqual(cubeInitialPosition, cubeFinalPosition);
        }

    .. code-tab:: java

        @Test
        public void TestScroll() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    "Player2").build();
            AltObject player2 = altDriver.findObject(altFindObjectsParams);
            Vector3 cubeInitialPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            altDriver.scroll(new AltScrollParams.Builder().withSpeed(4).withDuration(2).build());

            player2 = altDriver.findObject(altFindObjectsParams);
            Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            assertNotEquals(cubeInitialPosition, cubeFinalPosition);
        }

    .. code-tab:: py

        def test_scroll(self):
            self.altdriver.load_scene("Scene 5 Keyboard Input")
            player2 = self.altdriver.find_object(By.NAME, "Player2")
            cube_initial_position = [player2.worldX, player2.worldY, player2.worldY]
            self.altdriver.scroll(4, 2)

            player2 = self.altdriver.find_object(By.NAME, "Player2")
            cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]
            assert cube_initial_position != cubeFinalPosition

```

#### Swipe

Simulates a swipe action between two points.

**_Parameters_**

| Name     | Type    | Required | Default | Description                                                                |
| -------- | ------- | -------- | ------- | -------------------------------------------------------------------------- |
| start    | Vector2 | Yes      |         | Starting location of the swipe.                                            |
| end      | Vector2 | Yes      |         | Ending location of the swipe.                                              |
| duration | float   | No       | 0.1     | The time measured in seconds to move the mouse from start to end location. |
| wait     | boolean | No       | true    | If set wait for command to finish.                                         |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void MultipleDragAndDrop()
        {
            var altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image2");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box2");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image3");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);
            var imageSource = altDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            var imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource, imageSourceDropZone);

            imageSource = altDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource, imageSourceDropZone);
        }

    .. code-tab:: java

        @Test
        public void testMultipleDragAndDrop() throws Exception {

            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Image1").build();
            AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Box1").build();
            AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Image2").build();
            AltFindObjectsParams altFindObjectsParameters4 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Image3").build();
            AltFindObjectsParams altFindObjectsParameters5 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Box2").build();
            AltFindObjectsParams altFindObjectsParameters6 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Image").build();
            AltFindObjectsParams altFindObjectsParameters7 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Image").build();

            AltObject altElement1 = altDriver.findObject(altFindObjectsParameters1);
            AltObject altElement2 = altDriver.findObject(altFindObjectsParameters2);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(2).build());

            altElement1 = altDriver.findObject(altFindObjectsParameters3);
            altElement2 = altDriver.findObject(altFindObjectsParameters5);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(2).build());

            altElement1 = altDriver.findObject(altFindObjectsParameters4);
            altElement2 = altDriver.findObject(altFindObjectsParameters2);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(3).build());

            altElement1 = altDriver.findObject(altFindObjectsParameters1);
            altElement2 = altDriver.findObject(altFindObjectsParameters2);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(1).build());
            AltSprite imageSource = altDriver.findObject(altFindObjectsParameters1)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);

            AltSprite imageSourceDropZone = altDriver.findObject(altFindObjectsParameters6)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);

            assertNotSame(imageSource, imageSourceDropZone);

            imageSource = altDriver.findObject(altFindObjectsParameters3)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);

            imageSourceDropZone = altDriver.findObject(altFindObjectsParameters7)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);
            assertNotSame(imageSource, imageSourceDropZone);
        }

    .. code-tab:: py

        def test_multiple_swipes(self):
            self.altdriver.load_scene("Scene 3 Drag And Drop")

            image2 = self.altdriver.find_object(By.NAME, "Drag Image2")
            box2 = self.altdriver.find_object(By.NAME, "Drop Box2")

            self.altdriver.swipe(image2.get_screen_position(), box2.get_screen_position(), 2)

            image3 = self.altdriver.find_object(By.NAME, "Drag Image3")
            box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

            self.altdriver.swipe(image3.get_screen_position(), box1.get_screen_position(), 1)

            image1 = self.altdriver.find_object(By.NAME, "Drag Image1")
            box1 = self.altdriver.find_object(By.NAME, "Drop Box1")

            self.altdriver.swipe(image1.get_screen_position(), box1.get_screen_position(), 3)

            image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            image_source_drop_zone = self.altdriver.find_object(
                By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            assert image_source != image_source_drop_zone

            image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            image_source_drop_zone = self.altdriver.find_object(
                By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            assert image_source != image_source_drop_zone

```

#### MultipointSwipe

Simulates a multipoint swipe action.

**_Parameters_**

| Name      | Type                    | Required | Default | Description                                                                     |
| --------- | ----------------------- | -------- | ------- | ------------------------------------------------------------------------------- |
| positions | List\[AltVector2\]      | Yes      |         | A list of positions on the screen where the swipe be made.                      |
| duration  | float                   | No       | 0.1     | The time measured in seconds to swipe from first position to the last position. |
| wait      | boolean                 | No       | true    | If set wait for command to finish.                                              |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestResizePanelWithMultipointSwipe()
        {
            var altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            var pos = new[]
            {
                altElement.GetScreenPosition(),
                new AltVector2(altElement.x - 200, altElement.y - 200),
                new AltVector2(altElement.x - 300, altElement.y - 100),
                new AltVector2(altElement.x - 50, altElement.y - 100),
                new AltVector2(altElement.x - 100, altElement.y - 100)
            };
            altDriver.MultipointSwipe(pos, 4);

            altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);
            Assert.AreNotEqual(position, position2);
        }

    .. code-tab:: java

        @Test
        public void testResizePanelWithMultipointSwipe() throws Exception {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Resize Zone").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParameters1);

            List<Vector2> positions = Arrays.asList(altElement.getScreenPosition(),
                    new Vector2(altElement.x + 100, altElement.y + 100),
                    new Vector2(altElement.x + 100, altElement.y + 200));

            altDriver.multipointSwipe(new AltMultipointSwipeParams.Builder(positions).withDuration(3).build());

            AltObject altElementAfterResize = altDriver.findObject(altFindObjectsParameters1);
            assertNotSame(altElement.x, altElementAfterResize.x);
            assertNotSame(altElement.y, altElementAfterResize.y);
        }

    .. code-tab:: py

        def test_resize_panel_with_multipoint_swipe(self):
            self.altdriver.load_scene("Scene 2 Draggable Panel")

            alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
            position_init = (alt_unity_object.x, alt_unity_object.y)

            positions = [
                alt_unity_object.get_screen_position(),
                [alt_unity_object.x - 200, alt_unity_object.y - 200],
                [alt_unity_object.x - 300, alt_unity_object.y - 100],
                [alt_unity_object.x - 50, alt_unity_object.y - 100],
                [alt_unity_object.x - 100, alt_unity_object.y - 100]
            ]
            self.altdriver.multipoint_swipe(positions, duration=4)

            alt_unity_object = self.altdriver.find_object(By.NAME, "Resize Zone")
            position_final = (alt_unity_object.x, alt_unity_object.y)

            assert position_init != position_final

```

#### BeginTouch

Simulates starting of a touch on the screen. To further interact with the touch use [MoveTouch](#movetouch) and [EndTouch](#endtouch)

**_Parameters_**

| Name        | Type    | Required | Description         |
| ----------- | ------- | -------- | ------------------- |
| coordinates | Vector2 | Yes      | Screen coordinates. |

**_Returns_**

- int - the fingerId.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.GetScreenPosition());

        }

    .. code-tab:: java

        @Test
        public void testNewTouchCommands() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Zone").build();
            AltObject draggableArea = altDriver.findObject(altFindObjectsParameters1);
            Vector2 initialPosition = draggableArea.getScreenPosition();
            int fingerId = altDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
            Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
            altDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
            draggableArea = altDriver.findObject(altFindObjectsParameters1);
            assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
            assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
        }

    .. code-tab:: py

        def test_new_touch_commands(self):
            self.altDriver.load_scene('Scene 2 Draggable Panel')
            draggable_area = self.altDriver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.altDriver.begin_touch(draggable_area.get_screen_position())
            self.altDriver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.altDriver.end_touch(finger_id)
            draggable_area = self.altDriver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

```

#### MoveTouch

Simulates a touch movement on the screen. Move the touch created with [BeginTouch](#begintouch) from the previous position to the position given as parameters.

**_Parameters_**

| Name        | Type    | Required | Description                                               |
| ----------- | ------- | -------- | --------------------------------------------------------- |
| fingerId    | int     | Yes      | Identifier returned by [BeginTouch](#begintouch) command. |
| coordinates | Vector2 | Yes      | Screen coordinates where the touch will be moved.         |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.GetScreenPosition());

        }

    .. code-tab:: java

        @Test
        public void testNewTouchCommands() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Zone").build();
            AltObject draggableArea = altDriver.findObject(altFindObjectsParameters1);
            Vector2 initialPosition = draggableArea.getScreenPosition();
            int fingerId = altDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
            Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
            altDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
            draggableArea = altDriver.findObject(altFindObjectsParameters1);
            assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
            assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
        }

    .. code-tab:: py

        def test_new_touch_commands(self):
            self.altDriver.load_scene('Scene 2 Draggable Panel')
            draggable_area = self.altDriver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.altDriver.begin_touch(draggable_area.get_screen_position())
            self.altDriver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.altDriver.end_touch(finger_id)
            draggable_area = self.altDriver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

```

#### EndTouch

Simulates ending of a touch on the screen. This command will destroy the touch making it no longer usable to other movements.

**_Parameters_**

| Name     | Type | Required | Description                                               |
| -------- | ---- | -------- | --------------------------------------------------------- |
| fingerId | int  | Yes      | Identifier returned by [BeginTouch](#begintouch) command. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.GetScreenPosition());
        }

    .. code-tab:: java

        @Test
        public void testNewTouchCommands() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Zone").build();
            AltObject draggableArea = altDriver.findObject(altFindObjectsParameters1);
            Vector2 initialPosition = draggableArea.getScreenPosition();
            int fingerId = altDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
            Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
            altDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
            draggableArea = altDriver.findObject(altFindObjectsParameters1);
            assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
            assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
        }

    .. code-tab:: py

        def test_new_touch_commands(self):
            self.altDriver.load_scene('Scene 2 Draggable Panel')
            draggable_area = self.altDriver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.altDriver.begin_touch(draggable_area.get_screen_position())
            self.altDriver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.altDriver.end_touch(finger_id)
            draggable_area = self.altDriver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

```

#### Click

Click at screen coordinates.

**_Parameters_**

| Name        | Type    | Required | Default | Description                         |
| ----------- | ------- | -------- | ------- | ----------------------------------- |
| coordinates | Vector2 | Yes      |         | The screen coordinates.             |
| count       | int     | No       | 1       | Number of clicks.                   |
| interval    | float   | No       | 0.1     | Interval between clicks in seconds. |
| wait        | boolean | No       | true    | If set wait for command to finish.  |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestClickCoordinates()
        {
            const string name = "UIButton";
            var altObject = altDriver.FindObject(By.NAME,name);
            altDriver.Click(altObject.GetScreenPosition());
            Assert.AreEqual(name, altObject.name);
            altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text="UIButton clicked to jump capsule!"]");
        }

    .. code-tab:: java

        @Test()
        public void TestTapCoordinates() {
            AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                    .build();
            AltObject capsule = altDriver.findObject(findCapsuleParams);
            AltTapClickCoordinatesParams clickParams = new AltTapClickCoordinatesParams.Builder(
                    capsule.getScreenPosition()).build();
            altDriver.click(clickParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_tap_coordinates(self):
            capsule_element = self.altDriver.find_object(By.NAME, 'Capsule')
            self.altDriver.click(capsule_element.get_screen_position())

```

#### Tap

Tap at screen coordinates.

**_Parameters_**

| Name        | Type    | Required | Default | Description                         |
| ----------- | ------- | -------- | ------- | ----------------------------------- |
| coordinates | Vector2 | Yes      |         | The screen coordinates.             |
| count       | int     | No       | 1       | Number of taps.                     |
| interval    | float   | No       | 0.1     | Interval between taps in seconds.   |
| wait        | boolean | No       | true    | If set wait for command to finish.  |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestTapCoordinates()
        {
            const string name = "UIButton";
            var altObject = altDriver.FindObject(By.NAME,name);
            altDriver.Tap(altObject.GetScreenPosition());
            Assert.AreEqual(name, altObject.name);
            altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text="UIButton clicked to jump capsule!"]");
        }

    .. code-tab:: java

        @Test()
        public void TestTapCoordinates() {
            AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                    .build();
            AltObject capsule = altDriver.findObject(findCapsuleParams);
            AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
                    capsule.getScreenPosition()).build();
            altDriver.tap(tapParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_tap_coordinates(self):
            capsule_element = self.altDriver.find_object(By.NAME, 'Capsule')
            self.altDriver.tap(capsule_element.get_screen_position())

```

#### Tilt

Simulates device rotation action in your game.

**_Parameters_**

| Name         | Type    | Required | Default | Description                                 |
| ------------ | ------- | -------- | ------- | ------------------------------------------- |
| acceleration | Vector3 | Yes      |         | The linear acceleration of a device.        |
| duration     | float   | No       | 0.1     | How long the rotation will take in seconds. |
| wait         | boolean | No       | true    | If set wait for command to finish.          |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestAcceleration()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialWorldCoordinates = capsule.GetWorldPosition();
            altDriver.Tilt(new AltVector3(1, 1, 1), 1);
            Thread.Sleep(100);
            capsule = altDriver.FindObject(By.NAME, "Capsule");
            var afterTiltCoordinates = capsule.GetWorldPosition();
            Assert.AreNotEqual(initialWorldCoordinates, afterTiltCoordinates);
        }

    .. code-tab:: java

        @Test
        public void TestAcceleration() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Capsule").build();
            AltObject capsule = altDriver.findObject(altFindObjectsParameters1);
            Vector3 initialWorldCoordinates = capsule.getWorldPosition();
            altDriver.tilt(new AltTiltParams.Builder(new Vector3(1, 1, 1)).withDuration(1).build());
            capsule = altDriver.findObject(altFindObjectsParameters1);
            Vector3 afterTiltCoordinates = capsule.getWorldPosition();
            assertNotEquals(initialWorldCoordinates, afterTiltCoordinates);
        }


    .. code-tab:: py

        def test_acceleration(self):
            self.altdriver.load_scene("Scene 1 AltDriverTestScene")
            capsule = self.altdriver.find_object(By.NAME, "Capsule")
            initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            self.altdriver.tilt([1, 1, 1], 1)

            capsule = self.altdriver.find_object(By.NAME, "Capsule")
            final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            assert initial_position != final_position

```

#### ResetInput

Clears all active input actions simulated by AltTester.

**_Parameters_**

None

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestResetInput()
        {
            altDriver.KeyDown(AltKeyCode.Alpha1, 1);
            var oldId = altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<int>("Altom.AltTester.NewInputSystem", "Keyboard.deviceId", "Assembly-CSharp");
            altDriver.ResetInput();
            var newId = altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<int>("Altom.AltTester.NewInputSystem", "Keyboard.deviceId", "Assembly-CSharp");

            int countKeyDown = altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<int>("Input", "_keyCodesPressed.Count", "Assembly-CSharp");
            Assert.AreEqual(0, countKeyDown);
            Assert.AreNotEqual(newId, oldId);
        }

    .. code-tab:: java

           @Test
            public void testResetInput() throws InterruptedException {
                AltFindObjectsParams prefab = new AltFindObjectsParams.Builder(
                        AltDriver.By.NAME, "AltTesterPrefab").build();

                AltGetComponentPropertyParams deviceID = new AltGetComponentPropertyParams.Builder(
                        "Altom.AltTester.NewInputSystem",
                        "Keyboard.deviceId", "Assembly-CSharp").build();
                AltGetComponentPropertyParams count = new AltGetComponentPropertyParams.Builder(
                        "Input",
                        "_keyCodesPressed.Count", "Assembly-CSharp").build();
                altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.Alpha1).build());
                int oldId = altDriver.findObject(prefab).getComponentProperty(deviceID, Integer.class);
                altDriver.resetInput();
                int newId = altDriver.findObject(prefab).getComponentProperty(deviceID, Integer.class);

                int countKeyDown = altDriver.findObject(prefab).getComponentProperty(count, Integer.class);
                assertEquals(0, countKeyDown);
                assertNotEquals(newId, oldId);
            }


    .. code-tab:: py

        def test_reset_input(self):
            self.altdriver.key_down(AltKeyCode.Alpha1, 1)
            oldId = self.altdriver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
                "Altom.AltTester.NewInputSystem", "Keyboard.deviceId", "Assembly-CSharp")
            self.altdriver.reset_input()
            newId = self.altdriver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
                "Altom.AltTester.NewInputSystem", "Keyboard.deviceId", "Assembly-CSharp")

            countKeyDown = self.altdriver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
                "Input", "_keyCodesPressed.Count", "Assembly-CSharp")
            assert 0 == countKeyDown
            assert newId != oldId

```

### Screenshot

#### GetPNGScreenshot

Creates a screenshot of the current screen in png format.

**_Parameters_**

| Name | Type   | Required | Description                          |
| ---- | ------ | -------- | ------------------------------------ |
| path | string | Yes      | location where the image is created. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetScreenshot()
        {
            var path="testC.png";
            altDriver.GetPNGScreenshot(path);
            FileAssert.Exists(path);
        }


    .. code-tab:: java

        @Test
        public void testScreenshot()
        {
            String path="testJava2.png";
            altDriver.getPNGScreenshot(path);
            assertTrue(new File(path).isFile());
        }


    .. code-tab:: py

        def test_screenshot(self):
            png_path = "testPython.png"
            self.altDriver.get_png_screenshot(png_path)

            assert path.exists(png_path)

```

### Unity Commands

#### PlayerPrefKeyType

This is an enum type used for the **option** parameter in the [set_player_pref_key](#settingplayerprefs) command listed below and has the following values:

| Type   | Assigned Value |
| ------ | -------------- |
| Int    | 1              |
| String | 2              |
| Float  | 3              |

#### GettingPlayerPrefs

```eval_rst
.. tabs::

    .. tab:: C#

        **GetIntKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - int

            .. literalinclude:: ../_static/examples~/commands/csharp-player-pref-int.cs
                :language: C#
                :emphasize-lines: 6,11

        **GetFloatKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - float

            .. literalinclude:: ../_static/examples~/commands/csharp-player-pref-float.cs
                :language: C#
                :emphasize-lines: 6,11

        **GetStringKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - string

            .. literalinclude:: ../_static/examples~/commands/csharp-player-pref-string.cs
                :language: C#
                :emphasize-lines: 6,11

    .. tab:: Java

        **getFloatKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - float

            .. literalinclude:: ../_static/examples~/commands/java-player-pref-float.java
                :language: java
                :emphasize-lines: 6,11

        **getIntKeyPlayerPref**

        Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - int

            .. literalinclude:: ../_static/examples~/commands/java-player-pref-int.java
                :language: java
                :emphasize-lines: 6,11

        **getStringKeyPlayerPref**

        Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - string

            .. literalinclude:: ../_static/examples~/commands/java-player-pref-string.java
                :language: java
                :emphasize-lines: 6,11

    .. tab:: Python

        **get_player_pref_key**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - string/float/int

            .. literalinclude:: ../_static/examples~/commands/python-player-prefs.py
                :language: py
                :emphasize-lines: 6,10

```

##### SettingPlayerPrefs

```eval_rst
.. tabs::

    .. tab:: C#

        **SetKeyPlayerPref**

            Sets the value for a given key in PlayerPrefs.

            *Parameters*

            +------------+-----------------------+-----------+----------------------------------+
            |    Name    |          Type         |  Required |           Description            |
            +============+=======================+===========+==================================+
            |   keyname  |         string        |     Yes   |        Key to be set             |
            +------------+-----------------------+-----------+----------------------------------+
            |   value    |  integer/float/string |     Yes   |        Value to be set           |
            +------------+-----------------------+-----------+----------------------------------+

            *Returns*

            - Nothing

            *Examples*

            .. literalinclude:: ../_static/examples~/commands/csharp-player-pref-string.cs
                :language: C#
                :emphasize-lines: 5

    .. tab:: Java

        **setKeyPlayerPref**

            Sets the value for a given key in PlayerPrefs.

            *Parameters*

            +------------+-----------------------+-----------+----------------------------------+
            |    Name    |          Type         |  Required |           Description            |
            +============+=======================+===========+==================================+
            |   keyname  |         string        |     Yes   |        Key to be set             |
            +------------+-----------------------+-----------+----------------------------------+
            |   value    |  integer/float/string |     Yes   |        Value to be set           |
            +------------+-----------------------+-----------+----------------------------------+

            *Returns*

            - Nothing

            *Examples*

            .. literalinclude:: ../_static/examples~/commands/java-player-pref-string.java
                :language: java
                :emphasize-lines: 5

    .. tab:: Python

        **set_player_pref_key**

            Sets the value for a given key in PlayerPrefs.

            *Parameters*

            +------------+-----------------------+-----------+----------------------------------+
            |    Name    |          Type         |  Required |           Description            |
            +============+=======================+===========+==================================+
            |   keyname  |         string        |     Yes   |        Key to be set             |
            +------------+-----------------------+-----------+----------------------------------+
            |   value    |  integer/float/string |     Yes   |        Value to be set           |
            +------------+-----------------------+-----------+----------------------------------+
            |   option   |    PlayerPrefKeyType  |    Yes    |         Type of keyname          |
            +------------+-----------------------+-----------+----------------------------------+

            *Returns*

            - Nothing

            *Examples*

            .. literalinclude:: ../_static/examples~/commands/python-player-prefs.py
                :language: py
                :emphasize-lines: 4,5
```

#### DeleteKeyPlayerPref

Removes key and its corresponding value from PlayerPrefs.

**_Parameters_**

| Name    | Type  | Required | Description         |
| ------- | ----- | -------- | ------------------- |
| keyname | sting | Yes      | Key to be deleted.  |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestDeleteKey()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", 1);
            var val = altDriver.GetIntKeyPlayerPref("test");
            Assert.AreEqual(1, val);
            altDriver.DeleteKeyPlayerPref("test");
            try
            {
                altDriver.GetIntKeyPlayerPref("test");
                Assert.Fail();
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual("notFound", exception.Message);
            }

        }

    .. code-tab:: java

        @Test
        public void testDeleteKey() throws Exception
        {
            altDriver.deletePlayerPref();
            altDriver.setKeyPlayerPref("test", 1);
            int val = altDriver.getIntKeyPlayerPref("test");
            assertEquals(1, val);
            altDriver.deleteKeyPlayerPref("test");
            try
            {
                altDriver.getIntKeyPlayerPref("test");
                fail();
            }
            catch (NotFoundException e)
            {
                assertEquals(e.getMessage(), "notFound");
            }
        }

    .. tab:: Python

        .. literalinclude:: ../_static/examples~/commands/python-player-prefs.py
            :language: py
            :emphasize-lines: 8

```

#### DeletePlayerPref

Removes all keys and values from PlayerPref.

**_Parameters_**

None

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestSetKeyInt()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", 1);
            var val = altDriver.GetIntKeyPlayerPref("test");
            Assert.AreEqual(1, val);
        }

    .. code-tab:: java

        @Test
        public void testSetKeyFloat() throws Exception
        {
            altDriver.deletePlayerPref();
            altDriver.setKeyPlayerPref("test", 1f);
            float val = altDriver.getFloatKeyPlayerPref("test");
            assertEquals(1f, val, 0.01);
        }

    .. code-tab:: py

        def test_delete_key_player_pref(self):
            self.altDriver.load_scene("Scene 1 AltDriverTestScene")
            self.altDriver.delete_player_prefs()
            self.altDriver.set_player_pref_key("test", "1", PlayerPrefKeyType.String)
            val = self.altDriver.get_player_pref_key("test", player_pref_key_type)
            self.assertEqual("1", str(val))
```

#### GetCurrentScene

Returns the current active scene.

**_Parameters_**

None

**_Returns_**

- String

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetCurrentScene()
        {
            altDriver.LoadScene("Scene 1 AltDriverTestScene");
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetCurrentScene());
        }
    .. code-tab:: java

        @Test
        public void testGetCurrentScene() throws Exception
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
            assertEquals("Scene 1 AltDriverTestScene", altDriver.getCurrentScene());
        }

    .. code-tab:: py

        def test_get_current_scene(self):
            self.altDriver.load_scene("Scene 1 AltDriverTestScene")
            self.assertEqual("Scene 1 AltDriverTestScene",self.altDriver.get_current_scene())
```

#### LoadScene

Loads a scene.

**_Parameters_**

| Name       | Type   | Required | Description                                                                                                        |
| ---------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------ |
| scene      | string | Yes      | The name of the scene to be loaded.                                                                                |
| loadSingle | bool   | No       | If set to false the scene will be loaded additive, together with the current loaded scenes. Default value is true. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetCurrentScene()
        {
            altDriver.LoadScene("Scene 1 AltDriverTestScene",true);
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetCurrentScene());
        }

    .. code-tab:: java

        @Test
        public void testGetCurrentScene()
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
            assertEquals("Scene 1 AltDriverTestScene", altDriver.getCurrentScene());
        }

    .. code-tab:: py

        def test_get_current_scene(self):
            self.altDriver.load_scene("Scene 1 AltDriverTestScene",True)
            self.assertEqual("Scene 1 AltDriverTestScene",self.altDriver.get_current_scene())

```

#### UnloadScene

Unloads a scene.

**_Parameters_**

| Name  | Type   | Required | Description                       |
| ----- | ------ | -------- | --------------------------------- |
| scene | string | Yes      | Name of the scene to be unloaded. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestUnloadScene()
        {
            altDriver.LoadScene("Scene 2 Draggable Panel", false);
            Assert.AreEqual(2, altDriver.GetAllLoadedScenes().Count);
            altDriver.UnloadScene("Scene 2 Draggable Panel");
            Assert.AreEqual(1, altDriver.GetAllLoadedScenes().Count);
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetAllLoadedScenes()[0]);
        }

    .. code-tab:: java

        @Test
        public void TestUnloadScene() {
            AltLoadSceneParams altLoadSceneParams = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel")
                    .loadSingle(false).build();
            altDriver.loadScene(altLoadSceneParams);
            assertEquals(2, altDriver.getAllLoadedScenes().length);
            altDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 2 Draggable Panel").build());
            assertEquals(1, altDriver.getAllLoadedScenes().length);
            assertEquals("Scene 1 AltDriverTestScene", altDriver.getAllLoadedScenes()[0]);
        }

    .. code-tab:: py

        def test_unload_scene(self):
            self.altDriver.load_scene('Scene 1 AltDriverTestScene', True)
            self.altDriver.load_scene('Scene 2 Draggable Panel', False)
            self.assertEqual(2, len(self.altDriver.get_all_loaded_scenes()))
            self.altDriver.unload_scene('Scene 2 Draggable Panel')
            self.assertEqual(1, len(self.altDriver.get_all_loaded_scenes()))
            self.assertEqual("Scene 1 AltDriverTestScene",
                            self.altDriver.get_all_loaded_scenes()[0])
```

#### GetAllLoadedScenes

Returns all the scenes that have been loaded.

**_Parameters_**

None

**_Returns_**

- List of strings

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetAllLoadedScenes()
        {
            altDriver.LoadScene("Scene 1 AltDriverTestScene");
            System.Collections.Generic.List<string> loadedSceneNames = altDriver.GetAllLoadedScenes();
            Assert.AreEqual(loadedSceneNames.Count, 1);
            altDriver.LoadScene("Scene 2 Draggable Panel", false);
            altDriver.LoadScene("Scene 3 Drag And Drop", false);
            altDriver.LoadScene("Scene 4 No Cameras", false);
            altDriver.LoadScene("Scene 5 Keyboard Input", false);
            loadedSceneNames = altDriver.GetAllLoadedScenes();
            Assert.AreEqual(loadedSceneNames.Count, 5);
        }

    .. code-tab:: java

        @Test
        public void TestGetAllLoadedScenes()
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
            List<String> loadedSceneNames = altDriver.getAllLoadedScenes();
            assertEquals(loadedSceneNames.size(), 1);

            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").loadSingle(false).build());
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 3 Drag And Drop").loadSingle(false).build());
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 4 No Cameras").loadSingle(false).build());
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 5 Keyboard Input").loadSingle(false).build());
            loadedSceneNames = altDriver.getAllLoadedScenes();
            assertEquals(loadedSceneNames.size(), 5);
        }

    .. code-tab:: py

        def test_get_all_loaded_scenes(self):
            self.altDriver.load_scene("Scene 1 AltDriverTestScene")
            scenes_loaded = self.altDriver.get_all_loaded_scenes()
            self.assertEqual(len(scenes_loaded), 1)
            self.altDriver.load_scene("Scene 2 Draggable Panel", False)
            self.altDriver.load_scene("Scene 3 Drag And Drop", False)
            self.altDriver.load_scene("Scene 4 No Cameras", False)
            self.altDriver.load_scene("Scene 5 Keyboard Input", False)
            scenes_loaded = self.altDriver.get_all_loaded_scenes()
            self.assertEqual(len(scenes_loaded), 5)

```

#### WaitForCurrentSceneToBe

Waits for the scene to be loaded for a specified amount of time. It returns the name of the current scene.

**_Parameters_**

| Name      | Type   | Required | Description                                                        |
| --------- | ------ | -------- | ------------------------------------------------------------------ |
| sceneName | string | Yes      | The name of the scene to wait for.                                 |
| timeout   | double | No       | The time measured in seconds to wait for the specified scene.      |
| interval  | double | No       | How often to check that the scene was loaded in the given timeout. |

**_Returns_**

- None

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForCurrentSceneToBe()
        {
            const string name = "Scene 1 AltDriverTestScene";
            var timeStart = DateTime.Now;
            var currentScene = altDriver.WaitForCurrentSceneToBe(name);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(currentScene);
            Assert.AreEqual("Scene 1 AltDriverTestScene", currentScene);
        }

    .. code-tab:: java

        @Test
        public void testWaitForCurrentSceneToBe() throws Exception {
            String name = "Scene 1 AltDriverTestScene";
            long timeStart = System.currentTimeMillis();
            AltWaitForCurrentSceneToBeParams params = new AltWaitForCurrentSceneToBeParams.Builder(name).build();
            String currentScene = altDriver.waitForCurrentSceneToBe(params);
            long timeEnd = System.currentTimeMillis();
            long time = timeEnd - timeStart;
            assertTrue(time / 1000 < 20);
            assertNotNull(currentScene);
            assertEquals("Scene 1 AltDriverTestScene", currentScene);
        }

    .. code-tab:: py

        def test_wait_for_current_scene_to_be(self):
            self.altDriver.load_scene('Scene 1 AltDriverTestScene')
            self.altDriver.wait_for_current_scene_to_be(
                'Scene 1 AltDriverTestScene', 1)
            self.altDriver.load_scene('Scene 2 Draggable Panel')
            self.altDriver.wait_for_current_scene_to_be(
                'Scene 2 Draggable Panel', 1)
            self.assertEqual('Scene 2 Draggable Panel',
                         self.altDriver.get_current_scene())

```

#### GetTimeScale

Returns the value of the time scale.

**_Parameters_**

None

**_Returns_**

- float

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestTimeScale()
        {
            altDriver.SetTimeScale(0.1f);
            Thread.Sleep(1000);
            var timeScaleFromGame = altDriver.GetTimeScale();
            Assert.AreEqual(0.1f, timeScaleFromGame);
        }

    .. code-tab:: java

        @Test
        public void TestTimeScale() {
            altDriver.setTimeScale(new AltSetTimeScaleParams.Builder(0.1f).build());
            float timeScale = altDriver.getTimeScale();
            assertEquals(0.1f, timeScale, 0);
        }

    .. code-tab:: py

        def test_time_scale(self):
            self.altDriver.set_time_scale(0.1)
            time.sleep(1)
            time_scale = self.altDriver.get_time_scale()
            self.assertEqual(0.1, time_scale)

```

#### SetTimeScale

Sets the value of the time scale.

**_Parameters_**

| Name      | Type  | Required | Description                                  |
| --------- | ----- | -------- | -------------------------------------------- |
| timeScale | float | Yes      | The value you want to set the time scale to. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestTimeScale()
        {
            altDriver.SetTimeScale(0.1f);
            Thread.Sleep(1000);
            var timeScaleFromGame = altDriver.GetTimeScale();
            Assert.AreEqual(0.1f, timeScaleFromGame);
        }

    .. code-tab:: java

        @Test
        public void TestTimeScale() {
            altDriver.setTimeScale(new AltSetTimeScaleParams.Builder(0.1f).build());
            float timeScale = altDriver.getTimeScale();
            assertEquals(0.1f, timeScale, 0);
        }

    .. code-tab:: py

        def test_time_scale(self):
            self.altDriver.set_time_scale(0.1)
            time.sleep(1)
            time_scale = self.altDriver.get_time_scale()
            self.assertEqual(0.1, time_scale)

```

#### CallStaticMethod

Invokes static methods from your game.

**_Parameters_**

| Name             | Type   | Required | Description                                                                                                                                                                                               |
| ---------------- | ------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| typeName         | string | Yes      | The name of the script. If the script has a namespace the format should look like this: "namespace.typeName".                                                                                             |
| methodName       | string | Yes      | The name of the public method that we want to call. If the method is inside a static property/field to be able to call that method, methodName need to be the following format "propertyName.MethodName". |
| assemblyName     | string | Yes       | The name of the assembly containing the script.                                                                                                                                                          |
| parameters       | array  | No       | An array containing the serialized parameters to be sent to the component method.                                                                                                                         |
| typeOfParameters | array  | No       | An array containing the serialized type of parameters to be sent to the component method.                                                                                                                 |

**_Returns_**

- This is a generic method. The return type depends on the type parameter.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestCallStaticMethod()
        {

            altDriver.CallStaticMethod<string>("UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", new[] { "Test", "1" });
            int a = altDriver.CallStaticMethod<int>("UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", new[] { "Test", "2" });
            Assert.AreEqual(1, a);

        }

    .. code-tab:: java

        @Test
        public void TestCallStaticMethod() throws Exception
        {

            AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", new Object[] {"Test", 1}).withTypeOfParameters("").build();
            altDriver.callStaticMethod(altCallStaticMethodParams, Void.class);
            altCallStaticMethodParams = new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", new Object[] {"Test", 2}).withTypeOfParameters("").build();
            int a = altDriver.callStaticMethod(altCallStaticMethodParams, Integer.class);
            assertEquals(1,a);
        }

    .. code-tab:: py

        def test_call_static_method(self):
            self.altdriver.call_static_method("UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", ["Test", "1"])
            a = int(self.altdriver.call_static_method("UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", ["Test", "2"]))
            self.assertEqual(1, a)

```

#### GetStaticProperty

Gets the value of the static field or property.

**_Parameters_**

| Name          | Type   | Required | Description                                                                                             |
| ------------- | ------ | -------- | ------------------------------------------------------------------------------------------------------- |
| componentName | string | Yes      | The name of the component which has the static field or property to be retrieved.                       |
| propertyName  | string | Yes      | The name of the static field or property to be retrieved.                                               |
| assembly      | string | Yes      | The name of the assembly the component belongs to.                                                      |
| maxDepth      | int    | No       | The maximum depth in the hierarchy to look for the static field or property. Its value is 2 by default. |

**_Returns_**

- This is a generic method. The return type depends on the type of the static field or property to be retrieved.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetStaticProperty()
        {
            altDriver.CallStaticMethod<string>("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule", new string[] {"1920", "1080", "true"}, new string[] {"System.Int32", "System.Int32", "System.Boolean"});
            var width = altDriver.GetStaticProperty<int>("UnityEngine.Screen", "currentResolution.width", "UnityEngine.CoreModule");
            Assert.AreEqual(1920, width);
        }

    .. code-tab:: java

        @Test
        public void testGetStaticProperty() {
            AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule", new Object[] {"1920", "1080", "True"}).withTypeOfParameters(new String[] {"System.Int32", "System.Int32", "System.Boolean"}).build();
            altDriver.callStaticMethod(altCallStaticMethodParams, Void.class);
            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder("UnityEngine.Screen", "currentResolution.width", "UnityEngine.CoreModule").build();
            int width = altDriver.GetStaticProperty(altGetComponentPropertyParams, Integer.class);
            assertEquals(width, 1920);
        }

    .. code-tab:: py

        def test_get_static_property(self):
            self.altdriver.load_scene('Scene 1 AltDriverTestScene')
            self.altdriver.call_static_method("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule", ["1920", "1080", "True"], ["System.Int32", "System.Int32", "System.Boolean"])
            width = self.altdriver.get_static_property(
                "UnityEngine.Screen", "currentResolution.width", "UnityEngine.CoreModule")
            self.assertEqual(int(width), 1920)

```

#### SetStaticProperty

Sets the value of the static field or property.

**_Parameters_**

| Name          | Type   | Required | Description                                                                                             |
| ------------- | ------ | -------- | ------------------------------------------------------------------------------------------------------- |
| componentName | string | Yes      | The name of the component which has the static field or property to be retrieved.                       |
| propertyName  | string | Yes      | The name of the static field or property to be retrieved.                                               |
| assembly      | string | Yes      | The name of the assembly the component belongs to.                                                      |
| updatedProperty | object | Yes      | The new value of the component which has the static field or property to be seted. 

**_Returns_**

-   Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestSetStaticProperty()
        {
            var expectedValue = 5;
            altDriver.SetStaticProperty("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue);
            var value = altDriver.GetStaticProperty<int>("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp");
            Assert.AreEqual(expectedValue, value);
        }

    .. code-tab:: java

        @Test
        public void testSetStaticProperty() {
            final Integer expectedValue = 5;
            AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue.toString()).build();
            altDriver.setStaticProperty(altSetComponentPropertyParams);
            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp").build();
            Integer value = altDriver.getStaticProperty(altGetComponentPropertyParams,Integer.class);
            assertEquals(expectedValue, value);
        }

    .. code-tab:: py

        def test_set_static_property(self):
            expectedValue = 5
            self.altdriver.set_static_property("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue)
            value = self.altdriver.get_static_property("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp")
            assert expectedValue == value

```

### Other

#### SetServerLogging

Sets the level of logging on AltTester Unity SDK.

**_Parameters_**

| Name     | Type             | Required | Description         |
| -------- | ---------------- | -------- | ------------------- |
| logger   | AltLogger   | Yes      | The type of logger. |
| logLevel | AltLogLevel | Yes      | The logging level.  |

**_Returns_**

-   Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        altDriver.SetServerLogging(AltLogger.File, AltLogLevel.Off);
        altDriver.SetServerLogging(AltLogger.Unity, AltLogLevel.Info);

    .. code-tab:: java

        altDriver.setServerLogging(AltLogger.File, AltLogLevel.Off);
        altDriver.setServerLogging(AltLogger.Unity, AltLogLevel.Info);

    .. code-tab:: py

        altDriver.set_server_logging(AltLogger.File, AltLogLevel.Off);
        altDriver.set_server_logging(AltLogger.Unity, AltLogLevel.Info);

```

<!--### Notifications

#### Scene loaded

If activated this notification will be called every time a scene is loaded in the unity app. To activate this notification use `AddNotificationListener` command and add `NotificationType.LoadScene` as a parameter.

**_Returns_**

-   sceneName - name of the loaded scene
-   loadSceneMode - the way how the scene was loaded (Additive or Single)

#### Scene unloaded

If activated this notification will be called every time a scene is unloaded in the unity app. To activate this notification use `AddNotificationListener` command and add `NotificationType.UnloadScene` as a parameter.

**_Returns_**

-   sceneName - name of the unloaded scene

#### Log notification

If activated this notification will be called every time a log is generated. To activate this notification use `AddNotificationListener` command and add `NotificationType.Log` as a parameter.

**_Returns_**

-   message - the message of the log
-   stackTrace - the stack trace of the log
-   level - the level of the log (ex. Error, Warning etc.)

#### Application paused

If activated this notification will be called every time the application has paused. To activate this notification use `AddNotificationListener` command and add `NotificationType.ApplicationPaused` as a parameter.

**_Returns_**

-   Nothing
-->

## AltObject

The **AltObject** class represents the objects present in the game and it allows you through the methods listed below to interact with them. It is the return type of the methods in the [FindObjects](#findobjects) category.

**_Fields_**

| Name              | Type   | Description                                                                                                                          |
| ----------------- | ------ | ------------------------------------------------------------------------------------------------------------------------------------ |
| name              | string | The name of the object.                                                                                                              |
| id                | int    | The objects's id.                                                                                                                    |
| x                 | int    | The value for x axis coordinate on screen.                                                                                           |
| y                 | int    | The value for y axis coordinate on screen.                                                                                           |
| mobileY           | int    | The value for y axis for appium.                                                                                                     |
| type              | string | Object's type, for objects from the game is gameObject.                                                                              |
| enabled           | bool   | The local active state of the object. Note that an object may be inactive because a parent is not active, even if this returns true. |
| worldX            | float  | The value for x axis coordinate in the game's world.                                                                                 |
| worldY            | float  | The value for y axis coordinate in the game's world.                                                                                 |
| worldZ            | float  | The value for z axis coordinate in the game's world.                                                                                 |
| idCamera          | int    | The camera's id.                                                                                                                     |
| transformId       | int    | The transform's component id.                                                                                                        |
| parentId          | int    | The transform parent's id. It's obsolete. Use transformParentId instead.                                                             |
| transformParentId | int    | The transform parent's id.                                                                                                           |

The available methods are the following:

### CallComponentMethod

Invokes a method from an existing component of the object.

**_Parameters_**

| Name             | Type   | Required | Description                                                                                                                                                                                       |
| ---------------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  |
| componentName    | string | Yes      | The name of the component. If the component has a namespace the format should look like this: "namespace.componentName".                                                                          |
| methodName       | string | Yes      | The name of the public method that will be called. If the method is inside a property/field to be able to call that method, methodName need to be the following format "propertyName.MethodName". |
| assemblyName     | string | Yes      | The name of the assembly containing the component.                                                                                                                                                |
| parameters       | array  | No       | An array containing the serialized parameters to be sent to the component method.                                                                                                                 |
| typeOfParameters | array  | No       | An array containing the serialized type of parameters to be sent to the component method.                                                                                                         |

**_Returns_**

- This is a generic method. The return type depends on the type parameter.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestCallMethodWithAssembly()
        {
            AltObject capsule = altDriver.FindObject(By.NAME, "Capsule");
            var initialRotation = capsule.GetComponentProperty("UnityEngine.Transform", "rotation");
            capsule.CallComponentMethod<string>("UnityEngine.Transform", "Rotate", "UnityEngine.CoreModule", new[] { "10", "10", "10" }, new[] { "System.Single", "System.Single", "System.Single" });
            AltObject capsuleAfterRotation = altDriver.FindObject(By.NAME, "Capsule");
            var finalRotation = capsuleAfterRotation.GetComponentProperty("UnityEngine.Transform", "rotation");
            Assert.AreNotEqual(initialRotation, finalRotation);
        }

        [Test]
        public void TestCallMethodWithNoParameters()
        {
            const string componentName = "UnityEngine.UI.Text";
            const string methodName = "get_text";
            const string assemblyName = "UnityEngine.UI";
            const string elementText = "Change Camera Mode";
            var altElement = altUnityDriver.FindObject(By.PATH, "/Canvas/Button/Text");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, new object[] { });
            Assert.AreEqual(elementText, data);
        }

        [Test]
        public void TestCallMethodWithParameters()
        {
            const string componentName = "UnityEngine.UI.Text";
            const string methodName = "set_fontSize";
            const string methodToVerifyName = "get_fontSize";
            const string assemblyName = "UnityEngine.UI";
            Int32 fontSizeExpected = 16;
            string[] parameters = new[] {"16"};
            var altElement = altUnityDriver.FindObject(By.PATH, "/Canvas/UnityUIInputField/Text");
            var data = altElement.CallComponentMethod<string>(componentName, methodName, assemblyName, parameters);
            var fontSize =  altElement.CallComponentMethod<Int32>(componentName, methodToVerifyName, assemblyName, new object[] { });
            Assert.AreEqual(fontSizeExpected, fontSize);
        }

    .. code-tab:: java


        @Test
        public void TestCallMethodWithMultipleDefinitions() throws Exception
        {
            String capsuleName = "Capsule";
            String capsuleInfo = "CapsuleInfo";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, capsuleName).isEnabled(true).withCamera("Main Camera").build();
            AltObject capsule=altDriver.findObject(altFindObjectsParams);

            AltCallComponentMethodParams altCallComponentMethodParameters=new AltCallComponentMethodParams.Builder("Capsule", "Test", "Assembly-CSharp", "2").withTypeOfParameters("System.Int32").build();
            capsule.callComponentMethod(altCallComponentMethodParams, Void.class);

            altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, capsuleInfo).isEnabled(true).withCamera("Main Camera").build();
            AltObject capsuleInfo=altDriver.findObject(altFindObjectsParams);

            assertEquals("6",capsuleInfo.getText());
        }

        @Test
        public void testCallMethodWithNoParameters()
        {
            String componentName = "UnityEngine.UI.Text";
            String methodName = "get_text";
            String assembly = "UnityEngine.UI";
            String expected_text = "Change Camera Mode";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.PATH,
                "/Canvas/Button/Text").build();
            AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
            assertEquals(expected_text, altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName, assembly, new Object[] {}).build(),
                String.class));
        }

        @Test
        public void testCallMethodWithParameters() throws Exception
        {
            String componentName = "UnityEngine.UI.Text";
            String methodName = "set_fontSize";
            String methodExpectedName = "get_fontSize";
            String assembly = "UnityEngine.UI";
            String[] parameters = new String[] { "16"};
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltUnityDriver.By.PATH,
            "/Canvas/UnityUIInputField/Text").build();
            AltUnityObject altElement = altUnityDriver.findObject(altFindObjectsParams);
            altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName, assembly, parameters)
                    .build(),
                Void.class);
            Integer fontSize = altElement.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodExpectedName, assembly, new Object[] {})
                    .build(),
                Integer.class);

            assert(16==fontSize);
        }

    .. code-tab:: py

        def test_call_component_method(self):
            result = self.altdriver.find_object(By.NAME, "Capsule").call_component_method(
            "AltExampleScriptCapsule", "Jump", "Assembly-CSharp", ["setFromMethod"])
            self.assertEqual(result, None)
            self.altdriver.wait_for_object(By.PATH, '//CapsuleInfo[@text=setFromMethod]', timeout=1)
            self.assertEqual('setFromMethod', self.altdriver.find_object(By.NAME, 'CapsuleInfo').get_text())

        def test_call_component_method_with_no_parameters(self):
            result = self.altdriver.find_object(By.PATH, "/Canvas/Button/Text")
            text = result.call_component_method("UnityEngine.UI.Text", "get_text", "UnityEngine.UI")
            assert text == "Change Camera Mode"

        def test_call_component_method_with_parameters(self):
            fontSizeExpected =16
            altElement = self.altdriver.find_object(By.PATH, "/Canvas/UnityUIInputField/Text")
            altElement.call_component_method("UnityEngine.UI.Text", "set_fontSize", "UnityEngine.UI", parameters=["16"])
            fontSize = altElement.call_component_method("UnityEngine.UI.Text", "get_fontSize", "UnityEngine.UI", parameters=[])
            assert fontSizeExpected == fontSize

```

### GetComponentProperty

Returns the value of the given component property.

**_Parameters_**

| Name          | Type   | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| ------------- | ------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| componentName | string | Yes      | The name of the component. If the component has a namespace the format should look like this: "namespace.componentName"                                                                                                                                                                                                                                                                                                                            |
| propertyName  | string | Yes      | Name of the property of which value you want. If the property is an array you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.                                                                                                                                                                  |
| assemblyName  | string | Yes       | The name of the assembly containing the component.                                                                                                                                                                                                                                                                                                                                                                                                  |
| maxDepth      | int    | No       | Set how deep the serialization of the property to do. For example for position property in transform the result are following: maxDepth=2 {"normalized":{"magnitude":1.0, "sqrMagnitude":1.0, "x":0.871575534, "y":0.490261227, "z":0.0}, "magnitude":1101.45361, "sqrMagnitude":1213200.0, "x":960.0,"y":540.0, "z":0.0} and for maxDepth=1 :{"normalized":{},"magnitude":1101.45361, "sqrMagnitude":1213200.0, "x":960.0,"y":540.0, "z":0.0} |

**_Returns_**

- Object

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetComponentProperty()
        {
            const string componentName = "Altom.AltTester.AltRunner";
            const string propertyName = "InstrumentationSettings.AltTesterPort";
            var altObject = altDriver.FindObject(By.NAME,"AltRunnerPrefab");
            Assert.NotNull(altObject);
            var propertyValue = altObject.GetComponentProperty<int>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual(propertyValue, 13000);
        }

    .. code-tab:: java

        @Test
        public void testGetComponentProperty() throws Exception
        {
            String componentName = "Altom.AltTester.AltRunner";
            String propertyName = "InstrumentationSettings.AltTesterPort";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "AltRunnerPrefab").isEnabled(true).withCamera("Main Camera").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            assertNotNull(altObject);
            AltGetComponentPropertyParams altGetComponentPropertyParameters=new AltGetComponentPropertyParams.Builder(componentName, propertyName, "Assembly-CSharp").build();
            int propertyValue = altObject.getComponentProperty(altGetComponentPropertyParams,Integer.class);
            assertEquals(propertyValue, 13000);
        }

    .. code-tab:: py

        def test_get_component_property(self):
            self.altDriver.load_scene('Scene 1 AltDriverTestScene')
            result = self.altDriver.find_element("Capsule").get_component_property("Capsule", "arrayOfInts")
            self.assertEqual(result, [1,2,3])

```

### SetComponentProperty

Sets value of the given component property.

**_Parameters_**

| Name          | Type   | Required | Description                                                                                                              |
| ------------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------ |
| componentName | string | Yes      | The name of the component. If the component has a namespace the format should look like this: "namespace.componentName". |
| propertyName  | string | Yes      | The name of the property of which value you want to set                                                                  |
| assemblyName  | string | Yes       | The name of the assembly containing the component. It is NULL by default.                                               |
| value         | object | Yes      | The value to be set for the chosen component's property                                                                  |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestSetComponentProperty()
        {
            const string componentName = "Capsule";
            const string propertyName = "stringToSetFromTests";
            var altObject = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altObject);
            altObject.SetComponentProperty(componentName, propertyName, "Assembly-CSharp", "2");

            var propertyValue = altObject.GetComponentProperty<string>(componentName, propertyName);
            Assert.AreEqual("2", propertyValue);
        }

    .. code-tab:: java

        @Test
        public void testSetComponentProperty()
        {
            String componentName = "Capsule";
            String propertyName = "stringToSetFromTests";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Capsule").isEnabled(true).withCamera("Main Camera").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            assertNotNull(altObject);
            altElement.setComponentProperty(new AltSetComponentPropertyParams.Builder(componentName, propertyName, "Assembly-CSharp", "2").build());
            String propertyValue = altElement.getComponentProperty(new AltGetComponentPropertyParams.Builder(componentName,propertyName).build(), String.class);
            assertEquals("2", propertyValue);
        }

    .. code-tab:: py

        def test_set_component_property(self):
            self.altDriver.load_scene("Scene 1 AltDriverTestScene")
            componentName = "Capsule"
            propertyName = "stringToSetFromTests"
            altObject = self.altDriver.find_object(By.NAME, componentName)
            self.assertNotEqual(altObject, None)
            altObject.set_component_property(componentName, propertyName, "Assembly-CSharp", "2")
            propertyValue = altObject.get_component_property(componentName, propertyName)
            self.assertEqual("2", propertyValue)

```

### GetText

Returns text value from a Button, Text, InputField. This also works with TextMeshPro elements.

**_Parameters_**

None

**_Returns_**

- String

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForObjectWithText()
        {
            const string name = "CapsuleInfo";
            string text = altDriver.FindObject(By.NAME,name).GetText();
            var timeStart = DateTime.Now;
            var altObject = altDriver.WaitForObject(By.PATH, "//" + name + "[@text=" + text + "]");
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altObject);
            Assert.AreEqual(altObject.GetText(), text);

        }

    .. code-tab:: java

        @Test
        public void testWaitForObjectWithText() throws Exception
        {
            String name = "CapsuleInfo";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            String text = altDriver.findObject(altFindObjectsParams).getText();
            long timeStart = System.currentTimeMillis();
            AltWaitForObjectWithTextParams altWaitForElementWithTextParams = new AltWaitForObjectWithTextParams.Builder(altFindObjectsParams,text).withInterval(0).withTimeout(0).build();
            AltObject altObject = altDriver.waitForObjectWithText(altWaitForElementWithTextParams);
            long timeEnd = System.currentTimeMillis();
            long time = timeEnd - timeStart;
            assertTrue(time / 1000 < 20);
            assertNotNull(altObject);
            assertEquals(altObject.getText(), text);
        }

    .. code-tab:: py

        def test_call_component_method(self):
            self.altDriver.load_scene('Scene 1 AltDriverTestScene')
            result = self.altDriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
            self.assertEqual(result,"null")
            self.altDriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
            self.assertEqual('setFromMethod', self.altDriver.find_element('CapsuleInfo').get_text())

```

### SetText

Sets text value for a Button, Text, InputField. This also works with TextMeshPro elements.

**_Parameters_**

| Name   | Type   | Required | Description                         |
| ------ | ------ | -------- | ----------------------------------- |
| text   | string | Yes      | The text to be set.                 |
| submit | bool   | No       | If set will trigger a submit event. |

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestSetTextForElement()
        {
            const string name = "InputField";
            const string text = "InputFieldTest";
            var input = altDriver.FindObject(By.NAME, name).SetText(text, true);
            Assert.NotNull(input);
            Assert.AreEqual(input.GetText(), text);
        }

    .. code-tab:: java

        @Test
        public void testSetTextForElement()
        {
            String name = "InputField";
            String text = "InputFieldTest";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, name).isEnabled(true).withCamera("Main Camera").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            AltSetTextParams setTextParams = new AltSetTextParams.Builder(text).withSubmit(true).build();
            altObject.setText(setTextParams);
            assertNotNull(altObject);
            assertEquals(altObject.getText(), text);
        }

    .. code-tab:: py

        def test_set_text_for_element(self):
            self.altDriver.load_scene("Scene 1 AltDriverTestScene")
            name = "InputField"
            text = "InputFieldTest"
            input = self.altDriver.find_object(By.NAME, name).set_text(text, submit=True)
            self.assertNotEqual(input, None)
            self.assertEqual(input.get_text(), text)

```

### Tap

Tap current object.

**_Parameters_**

| Name     | Type    | Required | Default | Description                       |
| -------- | ------- | -------- | ------- | --------------------------------- |
| count    | int     | No       | 1       | Number of taps.                   |
| interval | float   | No       | 0.1     | Interval between taps in seconds. |
| wait     | boolean | No       | true    | Wait for command to finish.       |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestTap()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            counterButton.Tap();
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
        }

    .. code-tab:: java

        @Test()
        public void TestTapElement() {
            AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                    .build();
            AltObject capsule = altDriver.findObject(findCapsuleParams);

            AltTapClickElementParams tapParams = new AltTapClickElementParams.Builder().build();
            capsule.tap(tapParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_tap_element(self):
            self.altDriver.load_scene('Scene 1 AltDriverTestScene')
            capsule_element = self.altDriver.find_object(By.NAME, 'Capsule')
            capsule_element.tap()

```

### Click

Click current object.

**_Parameters_**

| Name     | Type    | Required | Default | Description                         |
| -------- | ------- | -------- | ------- | ----------------------------------- |
| count    | int     | No       | 1       | Number of clicks.                   |
| interval | float   | No       | 0.1     | Interval between clicks in seconds. |
| wait     | boolean | No       | true    | Wait for command to finish.         |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestClickElement()
        {
            var counterButton = altDriver.FindObject(By.NAME, "ButtonCounter");
            var counterButtonText = altDriver.FindObject(By.NAME, "ButtonCounter/Text");
            counterButton.Click();
            altDriver.WaitForObject(By.PATH, "//ButtonCounter/Text[@text=1]");
        }

    .. code-tab:: java

        @Test()
        public void TestClickElement() {
            AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                    .build();
            AltObject capsule = altDriver.findObject(findCapsuleParams);

            AltTapClickElementParams clickParams = new AltTapClickElementParams.Builder().build();
            capsule.Click(clickParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_click_element(self):
            self.altDriver.load_scene('Scene 1 AltDriverTestScene')
            capsule_element = self.altDriver.find_object(By.NAME, 'Capsule')
            capsule_element.click()

```

### PointerDown

Simulates pointer down action on the object.

**_Parameters_**

None

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerDownCommand()
        {
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty("PanelScript", "normalColor", "Assembly-CSharp");
            panel.PointerDownFromObject();
            Thread.Sleep(1000);
            var color2 = panel.GetComponentProperty("PanelScript", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
        }

    .. code-tab:: java

        @Test
        public void testPointerDownCommand() throws InterruptedException
        {
            AltObject panel = altDriver.findObject(AltDriver.By.NAME, "Panel");
            String color1 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("PanelScript", "normalColor", "Assembly-CSharp").build(), String.class);
            panel.pointerDownFromObject();
            Thread.sleep(1000);
            String color2 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder( "PanelScript", "highlightColor", "Assembly-CSharp").build(), String.class);
            assertTrue(color1 != color2);
        }

    .. code-tab:: py

        def test_pointer_down_command():
            self.altDriver.load_scene('Scene 2 Draggable Panel')
            time.sleep(1)
            p_panel = self.altDriver.find_object(By.NAME, 'Panel')
            color1 = p_panel.get_component_property('PanelScript', 'normalColor', 'Assembly-CSharp')
            p_panel.pointer_down_from_object()
            time.sleep(1)
            color2 = p_panel.get_component_property('PanelScript', 'highlightColor', 'Assembly-CSharp')
            self.assertNotEquals(color1, color2)

```

### PointerUp

Simulates pointer up action on the object.

**_Parameters_**

None

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerUpCommand()
        {
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty("PanelScript", "normalColor", "Assembly-CSharp");
            panel.PointerDownFromObject();
            Thread.Sleep(1000);
            panel.PointerUpFromObject();
            var color2 = panel.GetComponentProperty("PanelScript", "highlightColor", "Assembly-CSharp");
            Assert.AreEqual(color1, color2);
        }

    .. code-tab:: java

        @Test
        public void testPointerUpCommand() throws InterruptedException
        {
            AltObject panel = altDriver.findObject(AltDriver.By.NAME, "Panel");
            String color1 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("PanelScript", "normalColor", "Assembly-CSharp").build(), String.class);

            panel.pointerDownFromObject();
            Thread.sleep(1000);
            panel.pointerUpFromObject();
            String color2 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("PanelScript", "highlightColor", "Assembly-CSharp").build(), String.class);

            assertEquals(color1, color2);
        }

    .. code-tab:: py

        def test_pointer_up_command():
            self.altDriver.load_scene('Scene 2 Draggable Panel')
            time.sleep(1)
            p_panel = self.altDriver.find_object(By.NAME, 'Panel')
            color1 = p_panel.get_component_property('PanelScript', 'normalColor', 'Assembly-CSharp')
            p_panel.pointer_down_from_object()
            time.sleep(1)
            p_panel.pointer_up_from_object()
            color2 = p_panel.get_component_property('PanelScript', 'highlightColor', 'Assembly-CSharp')
            self.assertEquals(color1, color2)

```

### PointerEnter

Simulates pointer enter action on the object.

**_Parameters_**

None

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerEnterAndExit()
        {
            var altObject = altDriver.FindObject(By.NAME,"Drop Image");
            var color1 = altObject.GetComponentProperty("DropMe", "highlightColor", "Assembly-CSharp");
            altDriver.FindObject(By.NAME,"Drop Image").PointerEnterObject();
            var color2 = altObject.GetComponentProperty("DropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1,color2);
            altDriver.FindObject(By.NAME,"Drop Image").PointerExitObject();
            var color3 = altObject.GetComponentProperty("DropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color3, color2);
            Assert.AreEqual(color1,color3);
        }

    .. code-tab:: java

        @Test
        public void testPointerEnterAndExit()
        {
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Drop Image").isEnabled(true).withCamera("Main Camera").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            String color1 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("DropMe", "highlightColor", "Assembly-CSharp").build(), String.class);

            altDriver.findObject(altFindObjectsParams).pointerEnter();
            String color2 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("DropMe", "highlightColor", "Assembly-CSharp").build(), String.class);
            assertNotEquals(color1,color2);

            altDriver.findObject(altFindObjectsParams).pointerExit();
            String color3 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("DropMe", "highlightColor", "Assembly-CSharp").build(), String.class);
            assertNotEquals(color3, color2);
            assertEquals(color1,color3);
        }

    .. code-tab:: py

        def test_pointer_enter_and_exit(self):
            self.altDriver.load_scene("Scene 3 Drag And Drop")
            alt_unity_object = self.altDriver.find_object(By.NAME,"Drop Image")
            color1 = alt_unity_object.get_component_property("DropMe", "highlightColor", "Assembly-CSharp")
            self.altDriver.find_object(By.NAME,"Drop Image").pointer_enter()
            color2 = alt_unity_object.get_component_property("DropMe", "highlightColor", "Assembly-CSharp")
            self.assertNotEqual(color1, color2)
            self.altDriver.find_object(By.NAME,"Drop Image").pointer_exit()
            color3 = alt_unity_object.get_component_property("DropMe", "highlightColor", "Assembly-CSharp")
            self.assertNotEqual(color3, color2)
            self.assertEqual(color1, color3)

```

### PointerExit

Simulates pointer exit action on the object.

**_Parameters_**

None

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerEnterAndExit()
        {
            var altObject = altDriver.FindObject(By.NAME,"Drop Image");
            var color1 = altObject.GetComponentProperty("DropMe", "highlightColor", "Assembly-CSharp"));
            altDriver.FindObject(By.NAME,"Drop Image").PointerEnterObject();
            var color2 = altObject.GetComponentProperty("DropMe", "highlightColor", "Assembly-CSharp"));
            Assert.AreNotEqual(color1,color2);
            altDriver.FindObject(By.NAME,"Drop Image").PointerExitObject();
            var color3 = altObject.GetComponentProperty("DropMe", "highlightColor", "Assembly-CSharp"));
            Assert.AreNotEqual(color3, color2);
            Assert.AreEqual(color1,color3);
        }

    .. code-tab:: java

        @Test
        public void testPointerEnterAndExit()
        {
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Drop Image").isEnabled(true).withCamera("Main Camera").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            String color1 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("DropMe", "highlightColor", "Assembly-CSharp").build(), String.class);


            altDriver.findObject(altFindObjectsParams).pointerEnter();
            String color2 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("DropMe", "highlightColor", "Assembly-CSharp").build(), String.class);
            assertNotEquals(color1,color2);

            altDriver.findObject(altFindObjectsParams).pointerExit();
            String color3 = panel.getComponentProperty(new AltGetComponentPropertyParams.Builder("DropMe", "highlightColor", "Assembly-CSharp").build(), String.class);
            assertNotEquals(color3, color2);
            assertEquals(color1,color3);
        }

    .. code-tab:: py

        def test_pointer_enter_and_exit(self):
            self.altDriver.load_scene("Scene 3 Drag And Drop")
            alt_unity_object = self.altDriver.find_object(By.NAME,"Drop Image")
            color1 = alt_unity_object.get_component_property("DropMe", "highlightColor", "Assembly-CSharp")
            self.altDriver.find_object(By.NAME,"Drop Image").pointer_enter()
            color2 = alt_unity_object.get_component_property("DropMe", "highlightColor", "Assembly-CSharp")
            self.assertNotEqual(color1, color2)
            self.altDriver.find_object(By.NAME,"Drop Image").pointer_exit()
            color3 = alt_unity_object.get_component_property("DropMe", "highlightColor", "Assembly-CSharp")
            self.assertNotEqual(color3, color2)
            self.assertEqual(color1, color3)
```

### GetParent

Returns the parent of the object on which it is called.

**_Parameters_**

None

**_Returns_**

- AltObject

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetParent()
        {
            var altObject = altDriver.FindObject(By.NAME, "Panel", By.NAME, "Main Camera");
            var altObjectParent = altObject.GetParent();
            Assert.AreEqual("Panel Drag Area", altObjectParent.name);
        }

    .. code-tab:: java

        @Test
        public void TestGetParent() {
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(By.NAME, "CapsuleInfo")
                    .build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            AltObject altObjectParent = altObject.getParent();
            assertEquals("Canvas", altObjectParent.name);
        }

    .. code-tab:: py

        def test_get_parent(self):
            self.altDriver.load_scene('Scene 1 AltDriverTestScene', True)
            element = self.altDriver.find_object(By.NAME, 'Canvas/CapsuleInfo')
            elementParent = element.get_parent()
            self.assertEqual('Canvas', elementParent.name)

```

## BY-Selector

It is used in find objects methods to set the criteria of which the objects are searched.
Currently there are 7 types implemented:

-   _By.TAG_ - search for objects that have a specific tag
-   _By.LAYER_ - search for objects that are set on a specific layer
-   _By.NAME_ - search for objects that are named in a certain way
-   _By.COMPONENT_ - search for objects that have certain component
-   _By.ID_ - search for objects that have assigned a certain id (every object has an unique id so this criteria always will return 1 or 0 objects). Id checks for InstanceId and [AltId](#altid)
-   _By.TEXT_ - search for objects that have a certain text
-   _By.PATH_ - search for objects that respect a certain path

**Searching object by PATH**

The following selecting nodes and attributes are implemented:

-   _object_ - Selects all object with the name "object"
-   _/_ - Selects from the root node
-   _//_ - Selects nodes in the document from the current node that match the selection no matter where they are
-   _.._ - Selects the parent of the current node
-   \* - Matches any element node
-   _contains_ - Selects objects that contain a certain string in the name
-   _[n-th]_ - Selects n-th child of the current node. 0 - represents the first child, 1 - is the second child and so on. -1 -represents the last child
-   _@tag_
-   _@layer_
-   _@name_
-   _@component_
-   _@id_
-   _@text_

**Examples**

```eval_rst
.. tabs::

    .. tab:: \*

        ``//NameOfParent/NameOfChild/*``

        ``//NameOfParent/NameOfChild//*``

        .. code-block:: c#

            altDriver.FindObjects(By.PATH, "//Canvas/Panel/*")

        - Returns all direct children from Panel

        .. code-block:: c#

            altDriver.FindObjects(By.PATH, "//Canvas/Panel//*")

        - Returns all children from Panel

    .. tab:: \..

        ``//CapsuleInfo/..``

        .. code-block:: c#

            altDriver.FindObject(By.PATH, "//CapsuleInfo/..")

        - Returns the parent of the object CapsuleInfo

    .. tab:: selectors

        .. tabs::

            .. tab:: @tag

                ``//NameOfParent/NameOfChild/*[@tag=tagName]``

                .. code-block:: c#

                    altDriver.FindObjects(By.PATH, "//Canvas/Panel/*[@tag=UI]")

                - Returns every object that is tagged as UI and is a direct child of Panel

            .. tab:: @layer

                ``//NameOfParent/NameOfChild/*[@layer=layerName]``

                .. code-block:: c#

                    altDriver.FindObjects(By.PATH, "//Canvas/Panel/*[@layer=UI]")

                - Returns every object that is in the UI layer and is a direct child of Panel

            .. tab:: @id

                ``//NameOfParent/NameOfChild/*[@id=idMethod]``

                .. code-block:: c#

                    altDriver.FindObject(By.PATH, "//*[@id=8756]")

                - Returns the object which has the id equal to 8756

            .. tab:: @text

                ``//NameOfParent/NameOfChild/*[@text=textName]``

                .. code-block:: c#

                    altDriver.FindObject(By.PATH, "//Canvas/Panel//*[@text=Start]")

                - Returns the first object that has the text "Start" and is a child of Panel

            .. tab:: contains

                ``//NameOfParent/NameOfChild/*[contains(@name,name)]``

                ``//NameOfParent/NameOfChild/*[contains(@text,text)]``

                .. code-block:: c#

                    altDriver.FindObjects(By.PATH, "//*[contains(@name,Cub)]")

                - Returns every object that contains the string "Cub" in the name

            .. tab:: multiple selectors

                ``//NameOfParent/NameOfChild/*[@selector1=selectorName1][@selector2=selectorName2][@selector3=selectorName3]``

                .. code-block:: c#

                    altDriver.FindObject(By.PATH, "//Canvas/Panel/*[@component=Button][@tag=Untagged][@layer=UI]"

                - Returns the first direct child of the Panel that is untagged, is in the UI layer and has a component named Button

    .. tab:: find object

        ``//NameOfParent/NameObject``

        .. code-block:: c#

            altDriver.FindObjects(By.PATH, "/Canvas//Button[@component=ButtonLogic]"

        - Returns every button which is in Canvas that is a root object and has a component named ButtonLogic

    .. tab:: find a child of an object

        ``//NameOfParent/NameOfChild``

        ``//*[@id=idOfParent]/NameOfChild``

        .. code-block:: c#

            altDriver.FindObjects(By.PATH, "//Canvas/Panel")

        - Returns all direct children from Canvas that have the name "Panel

        .. code-block:: c#

            altDriver.FindObjects(By.PATH, "//Canvas/*/text")

        - Returns all children on the second level from Canvas that are named "text"

        .. code-block:: c#

            altDriver.FindObject(By.PATH, "//Canvas/Panel/StartButton[1]")

        - Returns the second child of the first object that has the name "StartButton" and is a direct child of Panel

    .. tab:: indexer

        ``//NameOfParent[n]``

        ``//NameOfParent/NameOfChild[n]``

        .. code-block:: c#

            altDriver.FindObject(By.PATH, "//Canvas[5]")

        - Returns the 6th direct child of the root Canvas

        .. code-block:: c#

            altDriver.FindObject(By.PATH, "//Canvas/Panel/*[@tag=Player][-1]")

        - Returns the last direct child of Panel that is tagged as Player

```

### Escaping characters

There are several characters that you need to escape when you try to find an object. Some examples characters are the symbols for Request separator and Request ending, by default this are `;` and `&` but can be changed in Server settings. If you don't escape this characters the whole request is invalid and might shut down the server. Other characters are `!`, `[`, `]`, `(`, `)`, `/`, `\`, `.` or `,`. This characters are used in searching algorithm and if not escaped might return the wrong object or not found at all. To escape all the characters mentioned before just add `\\` before each character you want to escape.

**_Examples_**

* `//Q&A` - not escaped
* `//Q\\&A` - escaped

### AltId

Is a solution offered by AltTester Unity SDK in order to find object easier. This is an unique identifier stored in an component and added to every object.
**A limitation of this is that only the object already in the scene before building the game will have an AltId. Object instantiated during run time will not have an AltId**

To add AltId to every object simply just click _Add AltId to every object_ from AltTester menu.

![Add AltId](../_static/img/commands/add-alt-id.png)

## AltPortForwarding

API to interact with `adb` and `iproxy` programmatically.

### ForwardAndroid

This method calls `adb forward [-s {deviceId}] tcp:{localPort} tcp:{remotePort}`.

**_Parameters_**

| Name       | Type   | Required | Description                                                                                                                                                                                      |
| ---------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| localPort  | int    | No       | The local port to forward from.                                                                                                                                                                  |
| remotePort | int    | No       | The device port to forward to.                                                                                                                                                                   |
| deviceId   | string | No       | The id of the device.                                                                                                                                                                            |
| adbPath    | string | No       | The adb path. If no adb path is provided, it tries to use adb from `${ANDROID_SDK_ROOT}/platform-tools/adb`. If `ANDROID_SDK_ROOT` env variable is not set, it tries to execute adb from `PATH`. |

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeSetUp]
        public void SetUp()
        {
            AltPortForwarding.ForwardAndroid();
            altDriver = new AltDriver();
        }

    .. code-tab:: java

        @BeforeClass
        public static void setUp() throws IOException {
            AltPortForwarding.forwardAndroid();
            altDriver = new AltDriver();
        }

    .. code-tab:: py

        @classmethod
        def setUpClass(cls):
            AltPortForwarding.forward_android()
            cls.altDriver = AltDriver()

```

### RemoveForwardAndroid

This method calls `adb forward --remove [-s {deviceId}] tcp:{localPort}` or `adb forward --remove-all` if no local port is provided.

**_Parameters_**

| Name      | Type   | Required | Description                                                                                                                                                                                      |
| --------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| localPort | int    | No       | The local port to be removed.                                                                                                                                                                    |
| deviceId  | string | No       | The id of the device to be removed.                                                                                                                                                              |
| adbPath   | string | No       | The adb path. If no adb path is provided, it tries to use adb from `${ANDROID_SDK_ROOT}/platform-tools/adb`. If `ANDROID_SDK_ROOT` env variable is not set, it tries to execute adb from `PATH`. |

**_Returns_**

Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
            AltPortForwarding.RemoveForwardAndroid();
        }

    .. code-tab:: java

        @AfterClass
        public static void tearDown() throws Exception {
            altDriver.stop();
            AltPortForwarding.removeForwardAndroid();
        }

    .. code-tab:: py

        @classmethod
        def tearDownClass(cls):
            cls.altDriver.stop()
            AltPortForwarding.remove_forward_android()

```

### RemoveAllForwardAndroid

This method calls `adb forward --remove-all`.

**_Parameters_**

| Name    | Type   | Required | Description                                                                                                                                                                                      |
| ------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| adbPath | string | No       | The adb path. If no adb path is provided, it tries to use adb from `${ANDROID_SDK_ROOT}/platform-tools/adb`. If `ANDROID_SDK_ROOT` env variable is not set, it tries to execute adb from `PATH`. |

**_Returns_**

Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
            AltPortForwarding.RemoveAllForwardAndroid();
        }

    .. code-tab:: java

        @AfterClass
        public static void tearDown() throws Exception {
            altDriver.stop();
            AltPortForwarding.removeAllForwardAndroid();
        }

    .. code-tab:: py

        @classmethod
        def tearDownClass(cls):
            cls.altDriver.stop()
            AltPortForwarding.remove_all_forward_android()

```

### ForwardIos

This method calls `iproxy {localPort} {remotePort} -u {deviceId}`. **_Requires iproxy 2.0.2_**.

**_Parameters_**

| Name       | Type   | Required | Description                                                                                |
| ---------- | ------ | -------- | ------------------------------------------------------------------------------------------ |
| localPort  | int    | No       | The local port to forward from.                                                            |
| remotePort | int    | No       | The device port to forward to.                                                             |
| deviceId   | string | No       | The id of the device.                                                                      |
| iproxyPath | string | No       | The path to iProxy. If `iproxyPath` is not provided, iproxy should be available in `PATH`. |

**_Returns_**

Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeSetUp]
        public void SetUp()
        {
            AltPortForwarding.ForwardIos();
            altDriver = new AltDriver();
        }

    .. code-tab:: java

        @BeforeClass
        public static void setUp() throws IOException {
            AltPortForwarding.forwardIos();
            altDriver = new AltDriver();
        }


    .. code-tab:: py

        @classmethod
        def setUpClass(cls):
            AltPortForwarding.forward_ios()
            cls.altDriver = AltDriver()

```

### KillAllIproxyProcess

This method kills all iproxy processes. Calls `killall iproxy`.

**_Parameters_**

None

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeTearDown]
        public void TearDown()
        {
            altDriver.Stop();
            AltPortForwarding.KillAllIproxyProcess();
        }


    .. code-tab:: java

        @AfterClass
        public static void tearDown() throws Exception {
            altDriver.stop();
            AltPortForwarding.killAllIproxyProcess();
        }


    .. code-tab:: py

        @classmethod
        def tearDownClass(cls):
            cls.altDriver.stop()
            AltPortForwarding.kill_all_iproxy_process()

```
