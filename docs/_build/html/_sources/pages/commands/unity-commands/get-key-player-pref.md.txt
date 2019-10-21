# Command: FindObject

## Description:

Find the first object in the scene that respects the given criteria. Check :doc:'by' for more information about criterias.

###### Observation: No longer possible to search for object by name giving a path in hierarchy, if you want to search that way please use by path.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| by      |     By    |   false   | Set what criteria to use in order to find the object|
| value         | string       |   false   | The value to which object will be compared to see if they respect the criteria or not|
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **AltFindObjectParameters** which we use the parameters mentioned. The java example will also show how to build such an object.

## Examples
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

