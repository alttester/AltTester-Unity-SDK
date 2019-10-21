# Command: FindObjects

## Description:

Find all objects in the scene that respects the given criteria. Check [By](../../other/by.html) for more information about criterias.

###### Observation: No longer possible to search for object by name giving a path in hierarchy, if you want to search that way please use by path.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     [By](../../other/by.html)    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltFindObjectParameters](../../other/java-builders.html#altfindobjectparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

## Returns

- List of AltUnityObjects/ empty list if no objects were found


## Examples

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

