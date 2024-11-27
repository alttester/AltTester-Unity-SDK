# API

If you are looking for information on a specific function, class or method, this part of the documentation is for you.

```eval_rst

.. important::
     Since we are using the same driver for both **AltTester® Unreal SDK** and **AltTester® Unity SDK**, certain features or methods might appear in code editors (e.g., Visual Studio Code) due to shared APIs but are not included in this documentation because they are not supported or relevant for the current platform. We recommend consulting the documentation for the latest platform-specific updates and verifying feature availability in your project.
``````

## AltDriver

The **AltDriver** class represents the main app driver component. When you instantiate an AltDriver in your tests, you can use it to "drive" your app like one of your users would, by interacting with all the app objects, their properties and methods.

An AltDriver instance will connect to the running instrumented Unreal application. In the constructor, we need to tell the driver where (on what IP and on what port) the instrumented Unreal App with a specific name is running and for how many seconds to let the communication opened.

**_Parameters_**

| Name           | Type    | Required | Description                                                                           |
| -------------- | ------- | -------- | ------------------------------------------------------------------------------------- |
| host           | string  | No       | The IP or hostname AltTester® Unreal SDK is listening on. The default value is `127.0.0.1`. |
| port           | int     | No       | The default value is `13000`.                                                              |
| appName        | string  | No       | The name of the Unreal application. The default value is `__default__`.                  |
| enableLogging  | boolean | No       | The default value is `false`.                                                           |
| connectTimeout | int     | No       | The connect timeout in seconds. The default value is `60`.                              |
| platform       | string  | No       | The platform of the Unreal application. The default value is `unknown`.                  |
| platformVersion| string  | No       | The platform version of the Unreal application. The default value is `unknown`.          |
| deviceInstanceId| string  | No      | The device instance id of the Unreal application. The default value is`unknown`.         |
| appId        | string  | No         | The unique id of the Unreal application. The default value is `unknown`.                 |

Once you have an instance of the _AltDriver_, you can use all the available commands to interact with the app. The available methods are the following:

### Find Objects

#### FindObject

Finds the first object in the level that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindAltObject()
        {
            const string name = "Capsule";
            var altObject = altDriver.FindObject(By.NAME, name);
            Assert.NotNull(altObject);
            Assert.AreEqual(name, altObject.name);
        }

    .. code-tab:: java

        @Test
        public void testfindObject() throws Exception
        {
            String name = "Capsule";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    name).isEnabled(true).build();
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

#### FindObjects

Finds all objects in the level that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- List of [AltObjects](#altobject) or an empty list if no objects were found.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindObjectsByTag()
        {
            var altObjects = altDriver.FindObjects(By.TAG, "plane");
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
                name).isEnabled(true).build();
            AltObject[] altObjects = altDriver.findObjects(altFindObjectsParams);
            assertNotNull(altObjects);
            assertEquals(altObjects[0].name, name);
        }

    .. code-tab:: py

        def test_find_objects_by_layer(self):
            altObjects = self.alt_driver.find_objects(By.LAYER, "Default")
            self.assertEquals(8, len(altObjects))

    .. code-tab:: robot

        Test Find Objects By Name
            ${alt_objects}=    Find Objects    NAME    Plane
            ${appears}=    Get Length    ${alt_objects}
            Should Be Equal As Integers    2    ${appears}

```

#### FindObjectWhichContains

Finds the first object in the level that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- [AltObject](#altobject)

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
                   name).isEnabled(true).build();
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

#### FindObjectsWhichContain

Finds all objects in the level that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                               |
| ----------- | ------------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                     |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                    |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                        |

**_Returns_**

- List of [AltObjects](#altobject) or an empty list if no objects were found.

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
                name).isEnabled(true).build();
            AltObject[] altObjects = altDriver.findObjectsWhichContain(altFindObjectsParams);
            assertNotNull(altObjects);
            assertTrue(altObjects[0].name.contains(name));
        }

    .. code-tab:: py

        def test_creating_stars(self):
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

#### FindObjectAtCoordinates

Retrieves the Unreal object at given coordinates.

Uses `WorldContext->LineTraceSingleByChannel` with `ECC_Visibility` to perform a line trace from the start to the end point. If no object is found at the given location, the trace will return an empty result.

**_Parameters_**

| Name        | Type    | Required | Description             |
| ----------- | ------- | -------- | ----------------------- |
| coordinates | Vector2 | Yes      | The screen coordinates. |

**_Returns_**

- [AltObject](#altobject) - The object hit by `LineTraceSingleByChannel`, nothing otherwise.

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

#### GetAllElements

Returns information about every objects loaded in the currently loaded levels.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |

**_Returns_**

- List of [AltObjects](#altobject) or an empty list if no objects were found.

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
            AltGetAllElementsParams altGetAllElementsParams = new AltGetAllElementsParams.Builder().isEnabled(true).build();
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

```

#### WaitForObject

Waits until it finds an object that respects the given criteria or until timeout limit is reached. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                |
| ----------- | ------------------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                      |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                     |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                         |
| timeout     | double             | No       | The number of seconds that it will wait for object.                                                                                                                                                                                                                                                                                                                                                        |
| interval    | double             | No       | The number of seconds after which it will try to find the object again. The interval should be smaller than timeout.                                                                                                                                                                                                                                                                                       |

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForExistingElement()
        {
            const string name = "Capsule";
            var timeStart = DateTime.Now;
            var altElement = altDriver.WaitForObject(By.NAME, name);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            Assert.NotNull(altElement);
            Assert.AreEqual(altElement.name, name);
        }

    .. code-tab:: java

        @Test
        public void testWaitForExistingElement() {
            String name = "Capsule";
            long timeStart = System.currentTimeMillis();
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                            name).build();
            AltWaitForObjectsParams altWaitForObjectsParams = new AltWaitForObjectsParams.Builder(
                            altFindObjectsParams).build();
            AltObject altElement = altDriver.waitForObject(altWaitForObjectsParams);
            long timeEnd = System.currentTimeMillis();
            long time = timeEnd - timeStart;
            assertTrue(time / 1000 < 20);
            assertNotNull(altElement);
            assertEquals(altElement.name, name);
        }

    .. code-tab:: py

        def test_wait_for_object(self):
            alt_object = self.alt_driver.wait_for_object(By.NAME, "Capsule")
            assert alt_object.name == "Capsule"

    .. code-tab:: robot

        Test Wait For Object By Name
            ${capsule}=         Wait For Object    NAME         Capsule
            ${capsule_name}=    Get Object Name    ${capsule}
            Should Be Equal     ${capsule_name}    Capsule

```

#### WaitForObjectWhichContains

Waits until it finds an object that respects the given criteria or time runs out and will throw an error. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                               |
| ----------- | ------------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                     |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                    |
| enabled     | boolean            | No       | If `true` will match only objects that are active in hierarchy. If `false` will match all objects.                                                                                                                                                                                                                                                                                                        |
| timeout     | double             | No       | The number of seconds that it will wait for object                                                                                                                                                                                                                                                                                                                                                        |
| interval    | double             | No       | The number of seconds after which it will try to find the object again. interval should be smaller than timeout                                                                                                                                                                                                                                                                                           |

**_Returns_**

- [AltObject](#altobject)

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
        public void TestWaitForObjectWhichContains() {
            AltFindObjectsParams altFindObjectsParametersObject = 
                new AltFindObjectsParams.Builder(By.NAME, "Canva").build();
            AltWaitForObjectsParams altWaitForObjectsParams =
                new AltWaitForObjectsParams.Builder(altFindObjectsParametersObject).build();
            AltObject altObject = altDriver.waitForObjectWhichContains(altWaitForObjectsParams);
            assertEquals("Canvas", altObject.name);
        }

    .. code-tab:: py

        def test_wait_for_object_which_contains(self):
            alt_object = self.alt_driver.wait_for_object_which_contains(By.NAME, "Main")
            assert alt_object.name == "Main Camera"

    .. code-tab:: robot

        Test Wait For Object Which Contains
            ${alt_object}=    Wait For Object Which Contains    NAME    Main
            ${alt_object_name}=    Get Object Name    ${alt_object}
            Should Be Equal As Strings    ${alt_object_name}    Main Camera

```

#### WaitForObjectNotBePresent

Waits until the object in the level that respects the given criteria is no longer in the level or until timeout limit is reached. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

| Name        | Type               | Required | Description                                                                                                                                                                                                                                                                                                                                                                                               |
| ----------- | ------------------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| by          | [By](#by-selector) | Yes      | Set what criteria to use in order to find the object.                                                                                                                                                                                                                                                                                                                                                     |
| value       | string             | Yes      | The value to which object will be compared to see if they respect the criteria or not.                                                                                                                                                                                                                                                                                                                    |
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
            self.alt_driver.wait_for_object_to_not_be_present(By.NAME, "Capsuule")

    .. code-tab:: robot

        Test Wait For Object Not Be Present
            Wait For Object To Not Be Present    NAME    ObjectDestroyedIn5Secs
            ${elements}=    Get All Elements
            ${list}=    Convert To String    ${elements}
            Should Not Contain    ${list}    'name': 'ObjectDestroyedIn5Secs'

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

        alt_driver.set_command_response_timeout(command_timeout)

    .. code-tab:: robot

        Set Command Response Timeout    30

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

        alt_driver.get_delay_after_command()

    .. code-tab:: robot

        Get Delay After Command

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

        alt_driver.set_delay_after_command(5)

    .. code-tab:: robot

        Set Delay After Command     5

```

### Input Actions

#### KeyDown

Simulates a key down.

**_Parameters_**

| Name    | Type            | Required | Description                                                                            |
| ------- | --------------- | -------- | -------------------------------------------------------------------------------------- |
| keyCode | AltKeyCode      | Yes      | The keyCode of the key simulated to be pressed.                                        |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestKeyDownAndKeyUp()
        {
            AltKeyCode kcode = AltKeyCode.A;
            altDriver.KeyDown(kcode);

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
            self.alt_driver.key_down(AltKeyCode.A)

            time.sleep(5)
            lastKeyDown = self.alt_driver.find_object(By.NAME, 'LastKeyDownValue')
            lastKeyPress = self.alt_driver.find_object(By.NAME, 'LastKeyPressedValue')

            self.assertEqual("A", lastKeyDown.get_text())
            self.assertEqual("A", lastKeyPress.get_text())

            self.alt_driver.key_up(AltKeyCode.A)

            time.sleep(5)
            lastKeyUp = self.alt_driver.find_object(By.NAME, 'LastKeyUpValue')
            self.assertEqual("A", lastKeyUp.get_text())

    .. code-tab:: robot

        Test Key Down And Key Up
            Key Down    A
            ${last_key_down}=       Find Object     NAME            LastKeyDownValue
            ${last_key_press}=      Find Object     NAME            LastKeyPressedValue
            ${last_key_down_text}=      Get Text    ${last_key_down}
            ${last_key_press_text}=     Get Text    ${last_key_press}
            Should Be Equal As Numbers    ${last_key_down_text}     97
            Should Be Equal As Numbers    ${last_key_press_text}    97
            Key Up    A
            ${last_key_up}=         Find Object     NAME            LastKeyUpValue
            ${last_key_up_text}=    Get Text        ${last_key_up}
            Should Be Equal As Numbers    ${last_key_up_text}       97

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
            AltKeyCode kcode = AltKeyCode.A;
            altDriver.KeyDown(kcode);

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
            self.alt_driver.key_down(AltKeyCode.A)

            time.sleep(5)
            lastKeyDown = self.alt_driver.find_object(By.NAME, 'LastKeyDownValue')
            lastKeyPress = self.alt_driver.find_object(By.NAME, 'LastKeyPressedValue')

            self.assertEqual("A", lastKeyDown.get_text())
            self.assertEqual("A", lastKeyPress.get_text())

            self.alt_driver.key_up(AltKeyCode.A)

            time.sleep(5)
            lastKeyUp = self.alt_driver.find_object(By.NAME, 'LastKeyUpValue')
            self.assertEqual("A", lastKeyUp.get_text())

    .. code-tab:: robot

        Test Key Down And Key Up
            Key Down    A
            ${last_key_down}=       Find Object     NAME            LastKeyDownValue
            ${last_key_press}=      Find Object     NAME            LastKeyPressedValue
            ${last_key_down_text}=      Get Text    ${last_key_down}
            ${last_key_press_text}=     Get Text    ${last_key_press}
            Should Be Equal As Numbers    ${last_key_down_text}     97
            Should Be Equal As Numbers    ${last_key_press_text}    97
            Key Up    A
            ${last_key_up}=         Find Object     NAME            LastKeyUpValue
            ${last_key_up_text}=    Get Text        ${last_key_up}
            Should Be Equal As Numbers    ${last_key_up_text}       97

```

#### PressKey

Simulates key press action in your app.

**_Parameters_**

| Name     | Type            | Required | Default | Description                                                                              |
| -------- | --------------- | -------- | ------- | ---------------------------------------------------------------------------------------- |
| keycode  | AltKeyCode      | Yes      |         | The key code of the key simulated to be pressed.                                         |
| duration | float           | No       | 0.1     | The time measured in seconds from the key press to the key release.                      |
| wait     | boolean         | No       | true    | If set wait for command to finish.                                                       |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::
    .. code-tab:: c#

        [Test]
        public void TestPressLeftArrow()
        {
            AltKeyCode kcode = AltKeyCode.LeftArrow;
            altDriver.PressKey(kcode);
        }

    .. code-tab:: java

        @Test
        public void TestPressLeftArrow() throws InterruptedException {
            AltKeyCode kcode = AltKeyCode.LeftArrow;
            altDriver.pressKey(new AltPressKeyParams.Builder(kcode).build());
        }

    .. code-tab:: py

        def test_press_left_arrow(self):
            self.alt_driver.press_key(AltKeyCode.LeftArrow)

    .. code-tab:: robot

        Test Creating Stars
            Press Key    LeftArrow
```

#### PressKeys

Simulates multiple key press action in your app.

**_Parameters_**

| Name     | Type               | Required | Default | Description                                                                                |
| -------- | ------------------ | -------- | ------- | ------------------------------------------------------------------------------------------ |
| keycodes | List\[AltKeyCode\] | Yes      |         | The list of keycodes simulated to be pressed simultaneously.                               |
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
        }

    .. code-tab:: java

        @Test
        public void testPressKeys()
        {
            AltKeyCode[] keys = {AltKeyCode.K, AltKeyCode.L};
            altDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());
        }

    .. code-tab:: py

        def test_press_keys(self):
            keys = [AltKeyCode.K, AltKeyCode.L]
            self.alt_driver.press_keys(keys)

    .. code-tab:: robot

        Test Press Keys
            ${keys}=    Create List    K    L
            Press Keys    ${keys}

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
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.alt_driver.begin_touch(draggable_area.get_screen_position())
            self.alt_driver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.alt_driver.end_touch(finger_id)
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

    .. code-tab:: robot

        Test New Touch Commands
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${initial_position}=    Get Screen Position    ${draggable_area}
            ${finger_id}=    Begin Touch    ${initial_position}
            ${draggable_area_x}=    Get Object X    ${draggable_area}
            ${draggable_area_y}=    Get Object Y    ${draggable_area}
            ${new_x}=    Evaluate    ${draggable_area_x}+10
            ${new_y}=    Evaluate    ${draggable_area_y}+10
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Move Touch    ${finger_id}    ${new_screen_position}
            End Touch    ${finger_id}
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${final_position}=    Get Screen Position    ${draggable_area}
            Should Not Be Equal    ${initial_position}    ${final_position}

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
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.alt_driver.begin_touch(draggable_area.get_screen_position())
            self.alt_driver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.alt_driver.end_touch(finger_id)
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

    .. code-tab:: robot

        Test New Touch Commands
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${initial_position}=    Get Screen Position    ${draggable_area}
            ${finger_id}=    Begin Touch    ${initial_position}
            ${draggable_area_x}=    Get Object X    ${draggable_area}
            ${draggable_area_y}=    Get Object Y    ${draggable_area}
            ${new_x}=    Evaluate    ${draggable_area_x}+10
            ${new_y}=    Evaluate    ${draggable_area_y}+10
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Move Touch    ${finger_id}    ${new_screen_position}
            End Touch    ${finger_id}
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${final_position}=    Get Screen Position    ${draggable_area}
            Should Not Be Equal    ${initial_position}    ${final_position}        

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
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.alt_driver.begin_touch(draggable_area.get_screen_position())
            self.alt_driver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.alt_driver.end_touch(finger_id)
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

    .. code-tab:: robot

        Test New Touch Commands
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${initial_position}=    Get Screen Position    ${draggable_area}
            ${finger_id}=    Begin Touch    ${initial_position}
            ${draggable_area_x}=    Get Object X    ${draggable_area}
            ${draggable_area_y}=    Get Object Y    ${draggable_area}
            ${new_x}=    Evaluate    ${draggable_area_x}+10
            ${new_y}=    Evaluate    ${draggable_area_y}+10
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Move Touch    ${finger_id}    ${new_screen_position}
            End Touch    ${finger_id}
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${final_position}=    Get Screen Position    ${draggable_area}
            Should Not Be Equal    ${initial_position}    ${final_position}

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
        public void TestClickCoordinates() {
            AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                    .build();
            AltObject capsule = altDriver.findObject(findCapsuleParams);
            AltClickCoordinatesParams clickParams = new AltClickCoordinatesParams.Builder(
                    capsule.getScreenPosition()).build();
            altDriver.click(clickParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_click_coordinates(self):
            capsule_element = self.alt_driver.find_object(By.NAME, 'Capsule')
            self.alt_driver.click(capsule_element.get_screen_position())

    .. code-tab:: robot

        Test Click Coordinates
            ${capsule_element}=    Find Object    NAME    Capsule
            ${capsule_element_positions}=    Get Screen Position    ${capsule_element}
            Click    ${capsule_element_positions}
            Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

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
            self.alt_driver.get_png_screenshot(png_path)
            assert path.exists(png_path)

    .. code-tab:: robot

        Test Screenshot
            ${png_path}=    Set Variable    testPython.png
            Get Png Screenshot    ${png_path}
            File Should Exist    ${png_path}

```

### Other Commands

#### GetCurrentScene

Returns the name of the currently loaded level.

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
            altDriver.LoadScene("MainMenu");
            Assert.AreEqual("MainMenu", altDriver.GetCurrentScene());
        }

    .. code-tab:: java

        @Test
        public void testGetCurrentScene() throws Exception
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("MainMenu").build());
            assertEquals("MainMenu", altDriver.getCurrentScene());
        }

    .. code-tab:: py

        def test_get_current_scene(self):
            self.alt_driver.load_scene("MainMenu", True)
            self.assertEqual("MainMenu",self.alt_driver.get_current_scene())

    .. code-tab:: robot

        Test Load And Wait For Scene
            Load Scene                      ${scene1}           ${True}
            Wait For Current Scene To Be    ${scene1}           timeout=1
            ${current_scene}=               Get Current Scene
            Should Be Equal                 ${current_scene}    ${scene1}

```

#### LoadScene

Loads a level by its name.

**_Parameters_**

| Name       | Type   | Required | Description                                                                                                        |
| ---------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------ |
| scene      | string | Yes      | The name of the level to be loaded.                                                                                |
| loadSingle | bool   | No       | If set to false the level will be loaded additive, together with the current loaded levels. Default value is true. |

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetCurrentScene()
        {
            altDriver.LoadScene("MainMenu", true);
            Assert.AreEqual("MainMenu", altDriver.GetCurrentScene());
        }

    .. code-tab:: java

        @Test
        public void testGetCurrentScene()
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("MainMenu").build());
            assertEquals("MainMenu", altDriver.getCurrentScene());
        }

    .. code-tab:: py

        def test_get_current_scene(self):
            self.alt_driver.load_scene("MainMenu", True)
            self.assertEqual("MainMenu", self.alt_driver.get_current_scene())

    .. code-tab:: robot

        Test Load And Wait For Scene
            Load Scene                      ${scene1}           ${True}
            Wait For Current Scene To Be    ${scene1}           timeout=1
            ${current_scene}=               Get Current Scene
            Should Be Equal                 ${current_scene}    ${scene1}

```

#### WaitForCurrentSceneToBe

Waits for the specified level to be loaded within a given amount of time.

**_Parameters_**

| Name      | Type   | Required | Description                                                        |
| --------- | ------ | -------- | ------------------------------------------------------------------ |
| sceneName | string | Yes      | The name of the level to wait for.                                 |
| timeout   | double | No       | The time measured in seconds to wait for the specified level to load.      |
| interval  | double | No       | How often, in seconds, to check if the level was loaded. |

**_Returns_**

- None

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForCurrentSceneToBe()
        {
            const string name = "MainMenu";
            var timeStart = DateTime.Now;
            altDriver.WaitForCurrentSceneToBe(name);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            var currentScene = altDriver.GetCurrentScene();
            Assert.AreEqual("MainMenu", currentScene);
        }

    .. code-tab:: java

        @Test
        public void testWaitForCurrentSceneToBe() {
            String name = "MainMenu";
            long timeStart = System.currentTimeMillis();
            AltWaitForCurrentSceneToBeParams params = new AltWaitForCurrentSceneToBeParams.Builder(name).build();
            altDriver.waitForCurrentSceneToBe(params);
            long timeEnd = System.currentTimeMillis();
            long time = timeEnd - timeStart;
            assertTrue(time / 1000 < 20);

            String currentScene = altDriver.getCurrentScene();
            assertEquals(name, currentScene);
        }

    .. code-tab:: py

        def test_wait_for_current_scene_to_be_with_a_non_existing_scene(self):
            scene_name = "MainMenu"

            with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.alt_driver.wait_for_current_scene_to_be(scene_name, timeout=1, interval=0.5)

            assert str(execinfo.value) == "Scene {} not loaded after 1 seconds".format(scene_name)

    .. code-tab:: robot

        Test Load And Wait For Scene
            Load Scene                      ${scene1}           ${True}
            Wait For Current Scene To Be    ${scene1}           timeout=1
            ${current_scene}=               Get Current Scene
            Should Be Equal                 ${current_scene}    ${scene1}

```

#### GetApplicationScreenSize

Returns the value of the application screen size.

**_Parameters_**

None

**_Returns_**

- AltVector2

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetApplicationScreenSize()
        {
            var screensize = altDriver.GetApplicationScreenSize();
            Assert.AreEqual(1920, screensize.x);
            Assert.AreEqual(1080, screensize.y);
        }

    .. code-tab:: java

        @Test
        public void TestGetApplicationScreenSize() {
            int[] screensize = altDriver.getApplicationScreenSize();
            assertEquals(1920, screensize[0]);
            assertEquals(1080, screensize[1]);
        }

    .. code-tab:: py

        def test_get_application_screen_size(self):
            screensize = self.alt_driver.get_application_screensize()
            assert 1920 == screensize[0]
            assert 1080 == screensize[1]

    .. code-tab:: robot

        Test Get Application Screen Size
            ${screen_size}=    Get Application Screensize
            Should Not Be Equal As Numbers    ${screen_size[0]}    0
            Should Not Be Equal As Numbers    ${screen_size[1]}    0

```

#### GetTimeScale

Retrieves the current global time dilation value using `UGameplayStatics::GetGlobalTimeDilation`.

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
            var timeScaleFromApp = altDriver.GetTimeScale();
            Assert.AreEqual(0.1f, timeScaleFromApp);
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
            self.alt_driver.set_time_scale(0.1)
            time.sleep(1)
            time_scale = self.alt_driver.get_time_scale()
            self.assertEqual(0.1, time_scale)

    .. code-tab:: robot

        Test Set And Get Time Scale
            Set Time Scale    0.1
            ${time_scale}=    Get Time Scale
            Should Be Equal As Numbers    ${time_scale}    0.1
            Set Time Scale    1

```

#### SetTimeScale

Sets the global time dilation to the specified value using `UGameplayStatics::SetGlobalTimeDilation`.

**_Parameters_**

| Name      | Type  | Required | Description                                  |
| --------- | ----- | -------- | -------------------------------------------- |
| timeScale | float | Yes      | The value you want to set the global time dilation. |

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
            var timeScaleFromApp = altDriver.GetTimeScale();
            Assert.AreEqual(0.1f, timeScaleFromApp);
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
            self.alt_driver.set_time_scale(0.1)
            time.sleep(1)
            time_scale = self.alt_driver.get_time_scale()
            self.assertEqual(0.1, time_scale)

    .. code-tab:: robot

        Test Set And Get Time Scale
            Set Time Scale    0.1
            ${time_scale}=    Get Time Scale
            Should Be Equal As Numbers    ${time_scale}    0.1
            Set Time Scale    1

```

#### CallStaticMethod

Invokes static methods from your app.

**_Parameters_**

| Name             | Type   | Required | Description                                                                                                                                                                                               |
| ---------------- | ------ | -------- | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| typeName         | string | Yes      | The name of the class blueprint or C++ class.                                                                                             |
| methodName       | string | Yes      | The name of the method that we want to call. |
| assemblyName     | string | Yes       | -                                                                                                                                                          |
| parameters       | array  | Yes       | An array containing the serialized parameters, formatted as strings, to be sent to the component method.                                                                                                                         |
| typeOfParameters | array  | No       | An array containing the serialized type of parameters to be sent to the component method.                                                                                                                 |

```eval_rst

.. important::
     Since we are using the same driver for both **AltTester® Unreal SDK** and **AltTester® Unity SDK**, the `assemblyName` is required but can be set to an empty string in Unreal SDK.
    
``````

**_Returns_**

- This is a generic method. The return type is a string, which depends on the type parameter.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestCallStaticMethod()
        {
            var screenResolution = altDriver.CallStaticMethod<string>("GameUserSettings", "GetScreenResolution", "", []);

            var match = Regex.Match(screenResolution, @"X=(\d+),Y=(\d+)");
            int screenWidth = int.Parse(match.Groups[1].Value);
            int screenHeight = int.Parse(match.Groups[2].Value);

            Assert.Multiple(() =>
            {
                Assert.That(1920, Is.EqualTo(screenWidth));
                Assert.That(1080, Is.EqualTo(screenHeight));
            });
        }

    .. code-tab:: java

        @Test
        public void TestCallStaticMethod() throws Exception
        {
            String screenResolution = altDriver.callStaticMethod(
                new AltCallStaticMethodParams.Builder("GameUserSettings", "GetScreenResolution", "", new Object[] {}).build(), String.class
            );

            Pattern pattern = Pattern.compile(@"X=(\d+),Y=(\d+)");
            Matcher matcher = pattern.matcher(screenResolution);
            
            int screenWidth = 0;
            int screenHeight = 0;
            if (matcher.find()) {
                screenWidth = Integer.parseInt(matcher.group(1));
                screenHeight = Integer.parseInt(matcher.group(2));
            }

            assertEquals(1920, screenWidth);
            assertEquals(1080, screenHeight);
        }

    .. code-tab:: py

        def test_call_static_method(self):
            screen_resolution = self.alt_driver.call_static_method(
                "GameUserSettings", "GetScreenResolution", "", []
            )

            match = re.match(r"X=(\d+),Y=(\d+)", screen_resolution)
        
            if match:
                screen_width = int(match.group(1))
                screen_height = int(match.group(2))

            self.assertEqual(1920, screen_width)
            self.assertEqual(1080, screen_height)

    .. code-tab:: robot

        Test Call Static Method
            ${screen_resolution}=    Call Static Method    GameUserSettings    GetScreenResolution    ""    parameters=[]
            ${match}=    Get Regex Match Group    ${screen_resolution}    X=(\d+),Y=(\d+)
            ${screen_width}=    Get From List    ${match}    0
            ${screen_height}=    Get From List    ${match}    1
            Should Be Equal As Integers    ${screen_width}    1920
            Should Be Equal As Integers    ${screen_height}    1080

```

## AltObject

The **AltObject** class represents the objects present in the app and it allows you through the methods listed below to interact with them. It is the return type of the methods in the [FindObjects](#findobjects) category.

**_Fields_**

| Name              | Type   | Description                                                                                                                          |
| ----------------- | ------ | ------------------------------------------------------------------------------------------------------------------------------------ |
| name              | string | The name of the object.                                                                                                              |
| id                | int    | The objects's id.                                                                                                                    |
| x                 | int    | The value for x axis coordinate on screen.                                                                                           |
| y                 | int    | The value for y axis coordinate on screen.                                                                                           |
| type              | string | Object's type, this field corresponds to the object's class name.                                                                              |
| enabled           | bool   | The local active state of the object. Note that an object may be inactive because a parent is not active, even if this returns true. |
| worldX            | float  | The value for x axis coordinate in the app's world.                                                                                 |
| worldY            | float  | The value for y axis coordinate in the app's world.                                                                                 |
| worldZ            | float  | The value for z axis coordinate in the app's world.                                                                                 |
| transformId       | int    | The transform's component id.                                                                                                        |
| transformParentId | int    | The transform parent's id.                                                                                                           |

```eval_rst

.. important::
     In the **AltTester® Unreal SDK**, the `transformId` field is the same as the `id` field.
    
``````

The available methods are the following:

### CallComponentMethod

Invokes a method from an existing component of the object.

**_Parameters_**

| Name             | Type   | Required | Description                                                                                                                                                                                       |
| ---------------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  |
| componentName    | string | Yes      | The name of the component.                                                                          |
| methodName       | string | Yes      | The name of the method (public or private) that will be called. |
| assemblyName     | string | Yes      | -                                                                                                                                                |
| parameters       | array  | Yes       | An array containing the serialized parameters, formatted as strings, to be sent to the component method.                                                                                                                 |
| typeOfParameters | array  | No       | An array containing the serialized type of parameters to be sent to the component method.                                                                                                         |

```eval_rst

.. important::
     Since we are using the same driver for both **AltTester® Unreal SDK** and **AltTester® Unity SDK**, the `assemblyName` is required but can be set to an empty string in Unreal SDK.
    
``````

**_Returns_**

- This is a generic method. The return type is a string, which depends on the type parameter.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestCallMethodWithNoParameters()
        {
            const string componentName = "Actor";
            const string methodName = "K2_GetActorRotation";
            const string assemblyName = "";

            var player = altDriver.FindObject(By.NAME, "Hero");
            string playerRotation = player.CallComponentMethod<string>(componentName, methodName, assemblyName, []);
            Assert.That(playerRotation, Is.EqualTo("(Pitch=0.000000,Yaw=90.000000,Roll=0.000000)"));
        }

        [Test]
        public void TestCallMethodWithParameters()
        {
            const string componentName = "Controller";
            const string methodName = "SetControlRotation";
            const string assemblyName = "";
            const string playerRotation = "(Pitch=0.000000,Yaw=180.000000,Roll=0.000000)";

            var player = altDriver.FindObject(By.NAME, "Hero");
            player.CallComponentMethod<string>(componentName, methodName, assemblyName, [playerRotation]);
        }

    .. code-tab:: java

        @Test
        public void TestCallMethodWithNoParameters() throws Exception {
            String componentName = "Actor";
            String methodName = "K2_GetActorRotation";
            String assemblyName = "";
            
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Hero").build();
            AltObject player = altDriver.findObject(altFindObjectsParams);

            String playerRotation = player.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName, assemblyName, new Object[] {}).build(), String.class
            );
            
            assertEquals("(Pitch=0.000000,Yaw=90.000000,Roll=0.000000)", playerRotation);
        }

        @Test
        public void TestCallMethodWithParameters() throws Exception
        {
            String componentName = "Controller";
            String methodName = "SetControlRotation";
            String assemblyName = "";
            String playerRotation = "(Pitch=0.000000,Yaw=180.000000,Roll=0.000000)";

            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Hero").build();
            AltObject player = altDriver.findObject(altFindObjectsParams);

            player.callComponentMethod(
                new AltCallComponentMethodParams.Builder(componentName, methodName, assemblyName, new Object[] { playerRotation }).build(), Void.class
            );
        }

    .. code-tab:: py

        def test_call_method_with_no_parameters(self):
            component_name = "Actor"
            method_name = "K2_GetActorRotation"
            assembly_name = ""

            player = self.alt_driver.find_object(By.NAME, "Hero")
            player_rotation = player.call_component_method(component_name, method_name, assembly_name, [])
            self.assertEqual(player_rotation, "(Pitch=0.000000,Yaw=90.000000,Roll=0.000000)")

        def test_call_method_with_parameters(self):
            component_name = "Controller"
            method_name = "SetControlRotation"
            assembly_name = ""
            player_rotation = "(Pitch=0.000000,Yaw=180.000000,Roll=0.000000)"

            player = self.alt_driver.find_object(By.NAME, "Hero")
            player.call_component_method(component_name, method_name, assembly_name, [player_rotation])

    .. code-tab:: robot

        Test Call Method With No Parameters
            ${player}=    Find Object    NAME    Hero
            ${player_rotation}=    Call Component Method    Actor    K2_GetActorRotation    ""    parameters=[]
            Should Be Equal As Strings    ${player_rotation}    (Pitch=0.000000,Yaw=90.000000,Roll=0.000000)

        Test Call Method With Parameters
            ${player}=    Find Object    NAME    Hero
            ${player_rotation}=    Call Component Method    Controller    SetControlRotation    ""    parameters=(Pitch=0.000000,Yaw=180.000000,Roll=0.000000)

```
### WaitForComponentProperty

Wait until a property has a specific value and returns the value of the given component property.

**_Parameters_**

| Name             | Type   | Required | Description                                                                                                                                                                                       |
| ---------------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  |
| componentName | string | Yes      | The name of the component.                                    |
| propertyName  | string | Yes      | Name of the property of which value you want.                                                           |                                                                                                                                   
| propertyValue  | T | Yes       | The value that property should have.                             
| assemblyName  | string | Yes       | -                                                                                                                           
| timeout     | double             | No       | The number of seconds that it will wait for the property. The default value is 20 seconds.                                                                                                                            
| interval    | double             | No       | The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5 seconds.                                                                                                                                         | 
| getPropertyAsString    | bool             | No       | If `true`, it will treat the propertyValue as a string; if `false` it will consider the original type of the propertyValue. This is especially useful when you want to pass for example `[[], []]` as a propertyValue, which you can do by setting getPropertyAsString to `true` and propertyValue to `JToken.Parse("[[], []]")` (in C#).

```eval_rst

.. important::
     Since we are using the same driver for both **AltTester® Unreal SDK** and **AltTester® Unity SDK**, the `assemblyName` is required but can be set to an empty string in Unreal SDK.
    
``````

**_Returns_**

- Object

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForComponentProperty()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "TestBool";

            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            var result = altElement.WaitForComponentProperty<bool>(componentName, propertyName, true, "");
            Assert.IsTrue(result);
        }

    .. code-tab:: java

        @Test
        public void testWaitForComponentProperty() throws InterruptedException {
            AltObject altElement = altDriver.findObject(
                new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Capsule").build()
            );

            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "TestBool", "").build();

            AltWaitForComponentPropertyParams<Boolean> altWaitForComponentPropertyParams = 
                new AltWaitForComponentPropertyParams.Builder<Boolean>(altGetComponentPropertyParams).build();

            Boolean propertyValue = altElement.waitForComponentProperty(
                altWaitForComponentPropertyParams,
                true,
                Boolean.class);

            assertEquals(Boolean.TRUE, propertyValue);
        }

    .. code-tab:: py

        def test_wait_for_component_property(self):
            alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
            result = alt_object.wait_for_component_property("AltExampleScriptCapsule", "TestBool", True, "")
            assert result is True

    .. code-tab:: robot

        Test Wait For Component Property
            ${alt_object}=    Find Object    NAME    Capsule
            ${result}=    Wait For Component Property    ${alt_object}    AltExampleScriptCapsule    TestBool    ${True}    ""
            Should Be Equal    ${result}    ${True}

```

### GetComponentProperty

Returns the value of the given component property.

**_Parameters_**

| Name          | Type   | Required | Description                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| ------------- | ------ | -------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| componentName | string | Yes      | The name of the component.                                                                                                                                                                                                                                                                                                                            |
| propertyName  | string | Yes      | Name of the property of which value you want.                                                                                                                                                                  |
| assemblyName  | string | Yes       | -                                                                                                                                                                                                                                                                                                                                                                                                  |

```eval_rst

.. important::
     Since we are using the same driver for both **AltTester® Unreal SDK** and **AltTester® Unity SDK**, the `assemblyName` is required but can be set to an empty string in Unreal SDK.
    
``````

**_Returns_**

- Object

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetComponentProperty()
        {
            var player = altDriver.FindObject(By.NAME, "Hero");

            var capsuleHalfHeight = player.GetComponentProperty<float>("CapsuleComponent", "CapsuleHalfHeight", "");
            Assert.AreEqual(65f, capsuleHalfHeight);
        }

    .. code-tab:: java

        @Test
        public void testGetComponentProperty() throws InterruptedException
        {
            AltFindObjectsParams altFindObjectsParams = 
                new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Hero").build();
            AltObject player = altDriver.findObject(altFindObjectsParams);

            float propertyValue = player.getComponentProperty(
                    new AltGetComponentPropertyParams.Builder("CapsuleComponent",
                            "CapsuleHalfHeight", "").build(),
                    float.class);

            assertEquals(65.0f, capsuleHalfHeight, 0.01f);
        }

    .. code-tab:: py

        def test_get_component_property(self):
            player = self.alt_driver.find_object(By.NAME, "Hero")
            result = player.get_component_property(
                "CapsuleComponent", "CapsuleHalfHeight", "")

            self.assertEqual(property_value, 65.0)

    .. code-tab:: robot

        Test Get Component Property
            ${alt_object}=    Find Object    NAME    Hero
            ${result}=    Get Component Property    ${alt_object}    CapsuleComponent    CapsuleHalfHeight    ""
            Should Be Equal As Numbers    ${result}    65.0

```

### SetComponentProperty

Sets value of the given component property.

**_Parameters_**

| Name          | Type   | Required | Description                                                                                                              |
| ------------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------ |
| componentName | string | Yes      | The name of the component. |
| propertyName  | string | Yes      | The name of the property of which value you want to set                                                                  |
| value         | object | Yes      | The value to be set for the chosen component's property                                               |
| assemblyName  | string | Yes       | -                                               |

```eval_rst

.. important::
     Since we are using the same driver for both **AltTester® Unreal SDK** and **AltTester® Unity SDK**, the `assemblyName` is required but can be set to an empty string in Unreal SDK.
    
``````

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestSetComponentProperty()
        {
            const string componentName = "AltExampleScriptCapsule";
            const string propertyName = "stringToSetFromTests";

            var altElement = altDriver.FindObject(By.NAME, "BP_Capsule");
            Assert.That(altElement, Is.Not.Null);
            altElement.SetComponentProperty(componentName, propertyName, "2", "");

            var propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName, "");
            Assert.That(propertyValue, Is.EqualTo("2"));
        }

    .. code-tab:: java

        @Test
        public void testSetComponentProperty()
        {
            String componentName = "AltExampleScriptCapsule";
            String propertyName = "stringToSetFromTests";
            String assembly = "";

            AltFindObjectsParams altFindObjectsParams = 
                new AltFindObjectsParams.Builder(AltDriver.By.NAME, "BP_Capsule").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParams);
            assertNotNull(altElement);

            altElement.setComponentProperty(
                    new AltSetComponentPropertyParams.Builder(componentName, propertyName, assembly, "2")\
                    .build()
            );

            int propertyValue = altElement.getComponentProperty(
                    new AltGetComponentPropertyParams.Builder(componentName,
                            propertyName,
                            assembly)
                    .build(),
                    int.class);
            assertEquals(2, propertyValue);
        }

    .. code-tab:: py

        def test_set_component_property(self):
            alt_object = self.alt_driver.find_object(By.NAME, "BP_Capsule")
            alt_object.set_component_property(
                "AltExampleScriptCapsule", "stringToSetFromTests", "", "2")

            alt_object = self.alt_driver.find_object(By.NAME, "BP_Capsule")
            result = alt_object.get_component_property(
                "AltExampleScriptCapsule", "stringToSetFromTests", "")
            self.assertEqual(result, "2")

    .. code-tab:: robot

        Test Set Component Property
            ${alt_object}=    Find Object    NAME    Capsule
            Set Component Property    ${alt_object}    AltExampleScriptCapsule    stringToSetFromTests    ""    "2"
            ${alt_object}=    Find Object    NAME    Capsule
            ${result}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    stringToSetFromTests    ""
            Should Be Equal    ${result}    "2"

```

### GetText

Returns text value from a TextBlock, EditableText, EditableTextBox.

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
            string text = altDriver.FindObject(By.NAME, name).GetText();
            
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
        public void testFindElementWithText()
        {
            String name = "CapsuleInfo";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME, name).build();
            String text = altDriver.findObject(altFindObjectsParams).getText();
            altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.TEXT, text).build();
            AltObject altElement = altDriver.findObject(altFindObjectsParams);
            assertNotNull(altElement);
            assertEquals(altElement.getText(), text);
        }

    .. code-tab:: py

        def test_find_object_by_text(self):
            text = self.alt_driver.find_object(By.NAME, "CapsuleInfo").get_text()
            element = self.alt_driver.find_object(By.TEXT, text)
            assert element.get_text() == text

    .. code-tab:: robot

        Test Find Object By Text
            ${alt_object}=    Find Object    NAME    CapsuleInfo
            ${text}=    Get Text    ${alt_object}
            ${element}=    Find Object    TEXT    ${text}
            ${element_text}=    Get Text    ${element}
            Should Be Equal    ${element_text}    ${text}

```

### SetText

Sets text value for a TextBlock, EditableText, EditableTextBox.

**_Parameters_**

| Name   | Type   | Required | Description                         |
| ------ | ------ | -------- | ----------------------------------- |
| text   | string | Yes      | The text to be set.                 |

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        public void TestSetText(string fieldName)
        {
            string fieldName = "UnrealUIInputField";
            string text = "exampleUnrealUIInputField";
            
            AltObject inputField = altDriver.FindObject(By.NAME, fieldName).SetText(text);
            Assert.That(inputField.UpdateObject().GetText(), Is.EqualTo(text));
        }

    .. code-tab:: java

        @Test
        public void TestSetText() {
            String fieldName = "UnrealUIInputField";
            AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, fieldName).build();
            AltObject textObject = altDriver.findObject(altFindObjectsParameters);

            String originalText = textObject.getText();
            String text = "ModifiedText";
            AltSetTextParams setTextParams = new AltSetTextParams.Builder(text).build();

            String afterText = textObject.setText(setTextParams).getText();
            assertNotEquals(originalText, afterText);
            assertEquals(text, afterText);
        }

    .. code-tab:: py

        def test_set_text(self):
            text_object = self.alt_driver.find_object(By.NAME, "UnrealUIInputField")
            original_text = text_object.get_text()
            after_text = text_object.set_text("ModifiedText").get_text()

            assert original_text != after_text
            assert after_text == "ModifiedText"

    .. code-tab:: robot

        Test Set Text
            ${text_object}=    Find Object    NAME    UnrealUIInputField
            ${original_text}=    Get Text    ${text_object}
            Set Text    ${text_object}    ModifiedText
            ${after_text}=    Get Text    ${text_object}
            Should Not Be Equal As Strings    ${original_text}    ${after_text}
            Should Be Equal As Strings    ${after_text}    ModifiedText

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

            AltClickElementParams clickParams = new AltClickElementParams.Builder().build();
            capsule.Click(clickParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_click_element(self):
            capsule_element = self.alt_driver.find_object(By.NAME, 'Capsule')
            capsule_element.click()

    .. code-tab:: robot

        Test Click Element
            ${capsule_element}=    Find Object    NAME    Capsule
            Click Object    ${capsule_element}
            Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

```


### UpdateObject

Returns the object with new values.

**_Parameters_**

None

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestUpdateAltObject()
        {
            var cube = altDriver.FindObject(By.NAME, "Player1");
            AltVector3 cubeInitialPostion = cube.GetWorldPosition();

            altDriver.PressKey(AltKeyCode.W, 1);
            Assert.AreNotEqual(cubeInitialPostion, cube.UpdateObject().GetWorldPosition());
        }

    .. code-tab:: java

       @Test
        public void TestUpdateAltObject() throws InterruptedException {
                AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(
                                AltDriver.By.NAME, "Player1").build();
                AltObject cube = altDriver.findObject(altFindObjectsParameters);
                float cubeInitWorldZ = cube.worldZ;

                altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.W).withDuration(1)
                                .withWait(false).build());
                Thread.sleep(2000);
                assertNotEquals(cubeInitWorldZ, cube.UpdateObject().worldZ);
        }

    .. code-tab:: py

        def test_update_altObject(self):
            cube = self.alt_driver.find_object(By.NAME, "Player1")
            initial_position_z = cube.worldZ

            self.alt_driver.press_key(AltKeyCode.W, duration=0.1, wait=False)
            time.sleep(5)

            assert initial_position_z != cube.update_object().worldZ

    .. code-tab:: robot

        Test Update AltObject
            ${cube}=    Find Object    NAME    Player1
            ${initial_position_z}=    Get Object WorldZ    ${cube}
            Press Key    W    duration=0.1    wait=${False}
            Sleep    5
            ${cube_updated}=    Update Object    ${cube}
            ${final_position_z}=    Get Object WorldZ    ${cube_updated}
            Should Not Be Equal    ${initial_position_z}    ${final_position_z}

```

### GetParent

Returns the parent of the object on which it is called.

**_Parameters_**

None

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetParent()
        {
            var altObject = altDriver.FindObject(By.NAME, "Panel");
            var altObjectParent = altObject.GetParent();
            Assert.AreEqual("Panel Drag Area", altObjectParent.name);
        }

    .. code-tab:: java

        @Test
        public void TestGetParent() {
            AltFindObjectsParams altFindObjectsParams = 
                new AltFindObjectsParams.Builder(By.NAME, "CapsuleInfo").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);
            AltObject altObjectParent = altObject.getParent();
            assertEquals("Canvas", altObjectParent.name);
        }

    .. code-tab:: py

        def test_get_parent(self):
            element = self.alt_driver.find_object(By.NAME, 'Canvas/CapsuleInfo')
            elementParent = element.get_parent()
            self.assertEqual('Canvas', elementParent.name)

    .. code-tab:: robot

        Test Get Parent
            ${element}=    Find Object    NAME    Canvas/CapsuleInfo
            ${element_parent}=    Get Parent    ${element}
            ${element_parent_name}=    Get Object Name    ${element_parent}
            Should Be Equal As Strings    ${element_parent_name}    Canvas

```
### GetScreenPosition

 Returns the screen position of the AltTester® object.

**_Parameters_**

None

**_Returns_**

- AltVector2

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetScreenPosition()
        {
            AltObject altObject = altDriver.FindObject(By.PATH, "//Plane");
            var screenPosition = altObject.GetScreenPosition();

            AltObject element = altDriver.FindObjectAtCoordinates(screenPosition);
            Assert.That(element, Is.Not.Null);
            Assert.That(element.name, Does.Match(@"^StaticMeshActor_\d+$"));
        }

    .. code-tab:: java

        @Test
        public void testGetScreenPosition() throws Exception {
            AltObject altObject = 
                altDriver.findObject(new AltFindObjectParams.Builder(AltDriver.By.PATH, "//Plane").build());
            Vector2 screenPosition = altObject.getScreenPosition();

            AltObject element = altDriver.findObjectAtCoordinates(
                new AltFindObjectAtCoordinatesParams.Builder(screenPosition).build()
            );
            assertNotNull(element);
            assertTrue(element.getName().matches("^StaticMeshActor_\\d+$"));
        }

    .. code-tab:: py

        def test_get_screen_position(self):
            alt_object = self.alt_driver.find_object(By.PATH, "//Plane")
            screen_position = alt_object.get_screen_position().

            element = self.alt_driver.find_object_at_coordinates(screen_position);
            self.assertIsNotNone(element)
            self.assertTrue(re.match(r"^StaticMeshActor_\d+$", element.name))

    .. code-tab:: robot

        Test Get Screen Position
            ${alt_object}=    Find Object    PATH    //Plane
            ${screen_position}=    Get Screen Position    ${alt_object}
            ${element}=    Find Object At Coordinates    ${screen_position}
            Should Not Be Empty    ${element}
            Should Match Regexp    ${element.name}    ^StaticMeshActor_\d+$

```
### GetWorldPosition

Returns the world position of the AltTester® object.

**_Parameters_**

None

**_Returns_**

- AltVector3

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetWorldPosition()
        {
            var capsule = altDriver.FindObject(By.NAME, "Capsule");
            var worldCoordinates = capsule.GetWorldPosition();

            var expectedCoordinates = new AltVector3(0, 0, 0);
            Assert.AreEqual(expectedCoordinates, worldCoordinates);
        }

    .. code-tab:: java

        @Test
        public void TestGetWorldPosition() throws InterruptedException {
            AltObject capsule = altDriver.findObject(
                new AltFindObjectsParams.Builder(AltDriver.By.NAME, "Capsule").build();
            );

            Vector3 worldCoordinates = capsule.getWorldPosition();
            Vector3 expectedCoordinates = new Vector3(0, 0, 0); 
            assertEquals(expectedCoordinates, worldCoordinates);
        }


    .. code-tab:: py

        def test_get_world_position(self):
            capsule = self.alt_driver.find_object(By.NAME, "Capsule")
            initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            expected_position = [0, 0, 0]
            assert expected_position == initial_position

    .. code-tab:: robot

        Test Get World Position
            ${capsule}=    Find Object    NAME    Capsule
            ${world_coordinates}=    Get World Position    ${capsule}
            
            Should Be Equal As Numbers    ${world_coordinates.x}    0
            Should Be Equal As Numbers    ${world_coordinates.y}    0
            Should Be Equal As Numbers    ${world_coordinates.z}    0

```




## BY-Selector

It is used in find objects methods to set the criteria of which the objects are searched.
Currently there are 7 types implemented:

-   _By.TAG_ - search for objects that have a specific actor tag
-   _By.LAYER_ - search for objects that are set on a specific layer
-   _By.NAME_ - search for objects that are named in a certain way
-   _By.COMPONENT_ - search for objects that have certain component
-   _By.ID_ - search for objects that have assigned a certain id (every object has an unique id so this criteria always will return 1 or 0 objects)
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
-   _[n-th]_ - Selects n-th object that respects the selectors. 0 - represents the first object, 1 - is the second object and so on. -1 -represents the last object
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

                    altDriver.FindObject(By.PATH, "//*[@id=8756]");

                - Returns the object which has the id equal to 8756

            .. tab:: @text

                ``//NameOfParent/NameOfChild/*[@text=textName]``

                .. code-block:: c#

                    altDriver.FindObject(By.PATH, "//Canvas/Panel//*[@text=Start]");

                - Returns the first object that has the text "Start" and is a child of Panel

            .. tab:: contains

                ``//NameOfParent/NameOfChild/*[contains(@name,name)]``

                ``//NameOfParent/NameOfChild/*[contains(@text,text)]``

                .. code-block:: c#

                    altDriver.FindObjects(By.PATH, "//*[contains(@name,Cub)]");

                - Returns every object that contains the string "Cub" in the name

            .. tab:: multiple selectors

                ``//NameOfParent/NameOfChild/*[@selector1=selectorName1][@selector2=selectorName2][@selector3=selectorName3]``

                .. code-block:: c#

                    altDriver.FindObject(By.PATH, "//Canvas/Panel/*[@component=Button][@tag=Untagged][@layer=UI]");

                - Returns the first direct child of the Panel that is untagged, is in the UI layer and has a component named Button

    .. tab:: find object

        ``//NameOfParent/NameObject``

        .. code-block:: c#

            altDriver.FindObjects(By.PATH, "/Canvas//Button[@component=ButtonLogic]");

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
        

        .. important::
            The indexer functionality matches that of XPath. It returns the n-th object that respects the selectors (Name, Component, Tag, etc.) from the objects with the same parent. The numbering starts from 0, so the first object has the index 0, then the second object has the index 1, and so on. For example, //Button/Text[1] will return the second object named `Text` that is the child of the `Button`.
    

        ``//NameOfParent[n]``

        ``//NameOfParent/NameOfChild[n]``

        .. code-block:: c#

            altDriver.FindObject(By.PATH, "//Canvas[2]");

        - Returns the 3th `Canvas` object only if there are at least 3 Canvas object somewhere in the hierarchy with the same parent

        .. code-block:: c#

            altDriver.FindObject(By.PATH, "//Canvas/Panel/*[@tag=Player][-1]");

        - Returns the last direct child of Panel that is tagged as Player

```

### Escaping characters

There are several characters that you need to escape when you try to find an object. Some examples characters are the symbols for Request separator and Request ending, by default this are `;` and `&` but can be changed in Server settings. If you don't escape this characters the whole request is invalid and might shut down the server. Other characters are `!`, `[`, `]`, `(`, `)`, `/`, `\`, `.` or `,`. This characters are used in searching algorithm and if not escaped might return the wrong object or not found at all. To escape all the characters mentioned before just add `\\` before each character you want to escape.

**_Examples_**

* `//Q&A` - not escaped
* `//Q\\&A` - escaped

## AltReversePortForwarding

API to interact with `adb` programmatically.

### ReversePortForwardingAndroid

This method calls `adb reverse [-s {deviceId}] tcp:{remotePort} tcp:{localPort}`.

**_Parameters_**

| Name       | Type   | Required | Description                                                                                                                                                                                      |
| ---------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| remotePort  | int    | No       | The device port to do reverse port forwarding from.                                                                                                                                                                  |
| localPort | int    | No       | The local port to do reverse port forwarding to.                                                                                                                                                                   |
| deviceId   | string | No       | The id of the device.                                                                                                                                                                            |
| adbPath    | string | No       | The adb path. If no adb path is provided, it tries to use adb from `${ANDROID_SDK_ROOT}/platform-tools/adb`. If `ANDROID_SDK_ROOT` env variable is not set, it tries to execute adb from `PATH`. |

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [OneTimeSetUp]
        public void SetUp()
        {
            AltReversePortForwarding.ReversePortForwardingAndroid();
            altDriver = new AltDriver();
        }

    .. code-tab:: java

        @BeforeClass
        public static void setUp() throws IOException {
            AltReversePortForwarding.reversePortForwardingAndroid();
            altDriver = new AltDriver();
        }

    .. code-tab:: py

        @classmethod
        def setUpClass(cls):
            AltReversePortForwarding.reverse_port_forwarding_android()
            cls.alt_driver = AltDriver()

    .. code-tab:: robot

        SetUp Tests
            Reverse Port Forwarding Android
            Initialize Altdriver

```
**Note:** Sometimes, the execution of reverse port forwarding method is too slow so the tests fail because the port is not actually forwarded when trying to establish the connection. In order to fix this problem, a `sleep()` method should be called after calling the ReversePortForwardingAndroid() method.
### RemoveReversePortForwardingAndroid

This method calls `adb reverse --remove [-s {deviceId}] tcp:{devicePort}` or `adb reverse --remove-all` if no port is provided.

**_Parameters_**

| Name      | Type   | Required | Description                                                                                                                                                                                      |
| --------- | ------ | -------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| devicePort | int    | No       | The device port to be removed.                                                                                                                                                                    |
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
            AltReversePortForwarding.RemoveReversePortForwardingAndroid();
        }

    .. code-tab:: java

        @AfterClass
        public static void tearDown() throws Exception {
            altDriver.stop();
            AltReversePortForwarding.removeReversePortForwardingAndroid();
        }

    .. code-tab:: py

        @classmethod
        def tearDownClass(cls):
            cls.alt_driver.stop()
            AltReversePortForwarding.remove_reverse_port_forwarding_android()

    .. code-tab:: robot

        TearDown Tests
            Stop Altdriver
            Remove Reverse Port Forwarding Android

```

### RemoveAllReversePortForwardingsAndroid

This method calls `adb reverse --remove-all`.

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
            AltReversePortForwarding.RemoveAllReversePortForwardingsAndroid();
        }

    .. code-tab:: java

        @AfterClass
        public static void tearDown() throws Exception {
            altDriver.stop();
            AltReversePortForwarding.removeAllReversePortForwardingsAndroid();
        }

    .. code-tab:: py

        @classmethod
        def tearDownClass(cls):
            cls.alt_driver.stop()
            AltReversePortForwarding.remove_all__reverse_port_forwardings_android()

    .. code-tab:: robot

        TearDown Tests
            Stop Altdriver
            Remove All Reverse Port Forwarding Android
```
