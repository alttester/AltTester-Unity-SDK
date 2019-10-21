# Command: GetAllElements

## Description:

Returns information about every objects loaded in the currently loaded scenes. This also means objects that are set as DontDestroyOnLoad.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| cameraName      |     string    |   true   | the name of the camera for which the screen coordinate of the object will be calculated. If no camera is given It will search through all camera that are in the scene until some camera sees the object or return the screen coordinate of the object  calculated to the last camera in the scene.|
| enabled         | boolean       |   true   | true => will return only if the object is active in hierarchy and false will return if the object is in hierarchy and doesn't matter if it is active or not|


###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **AltGetAllElementsParameters** which we use the parameters mentioned. The java example will also show how to build such an object.

## Return

- List of AltUnityObjects/ empty list if no objects were found

## Examples

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
    
        //TODO

```