# Command: CallStaticMethod

## Description:

Invoke static methods from your game.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| typeName      |     string    |   false   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| methodName      |     string    |   false   |   The name of the public method that we want to call |
| parameters      |     string    |   false   |   a string containing the serialized parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]" Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.|
| typeOfParamaters      |     string    |   false   |  a string containing the serialized type of parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints |
| assemblyName  | string | true | name of the assembly where the component is |

###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **AltGetAllElementsParameters** which we use the parameters mentioned. The java example will also show how to build such an object.

## Examples

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