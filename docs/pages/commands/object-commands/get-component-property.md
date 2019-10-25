# Command: GetComponentPropery

## Description:

Get the value of a property from one of the component of the object.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| componentName      |     string    |   false   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| propertyName      |     string    |   false   |  name of the property of which value you want |
| assemblyName  | string | true | name of the assembly where the component is |

###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **AltGetAllElementsParameters** which we use the parameters mentioned. The java example will also show how to build such an object.

## Examples

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