# Command: WaitForObject

## Description:

Wait until finds an object in the scene that respect the given criteria or times run out and will throw an error. Check [By](../../other/by.html) for more information about criterias.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](../../other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|
| timeout         | double        |   true   | number of seconds that it will wait for object|
| interval        | double        |   true   | number of seconds after which it will try to find the object again. interval should be smaller than timeout |

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltWaitForObjectsParameters](../../other/java-builders.html#altwaitforobjectsparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

## Return

- An AltUnityObject

## Examples

```eval_rst
.. tabs::

    .. code-tab:: c#
    
        [Test]
            public void TestWaitForExistingElement()
            {
                const string name = "Capsule";
                var timeStart = DateTime.Now;
                var altElement = altUnityDriver.WaitForObject(By.NAME, name);
                var timeEnd = DateTime.Now;
                var time = timeEnd - timeStart;
                Assert.Less(time.TotalSeconds, 20);
                Assert.NotNull(altElement);
                Assert.AreEqual(altElement.name, name);
            }

    .. code-tab:: java
        
        @Test
            public void testWaitForExistingElement() throws Exception {
                String name = "Capsule";
                long timeStart = System.currentTimeMillis();
                AltUnityObject altElement = altUnityDriver.waitForObject(AltUnityDriver.By.NAME,name);
                long timeEnd = System.currentTimeMillis();
                long time = timeEnd - timeStart;
                assertTrue(time / 1000 < 20);
                assertNotNull(altElement);
                assertEquals(altElement.name, name);
            }


    .. code-tab:: py
    
        def test_wait_for_non_existing_object(self):
            try:
                alt_element = self.altdriver.wait_for_element("dlkasldkas",'',1,0.5)
                self.assertEqual(False,True)
            except WaitTimeOutException as e:
                self.assertEqual(e.args[0],"Element dlkasldkas not found after 1 seconds")


```
