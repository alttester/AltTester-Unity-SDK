# Command: CallComponentMethod

## Description:

Invoke a method from an existing component of the object.

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| componentName      |     string    |   false   | name of the Unity component that has the public property we want to call a method for. This should be the assembly-qualified name of the type to get. If the type is in the currently executing assembly or in Mscorlib.dll, it is sufficient to supply the type name qualified by its namespace. [For more info](https://msdn.microsoft.com/en-us/library/w3f99sx1(v=vs.110).aspx )|
| methodName      |     string    |   false   |   The name of the public method that we want to call |
| parameters      |     string    |   false   |   a string containing the serialized parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'some string ? [1,2,3]' - this represents two parameters "some string" and "[1,2,3]" Each parameter will be deserialized to match the correct type, so '[1,2,3] will deserialized to an array of ints, '1' will be an integer etc.|
| typeOfParamaters      |     string    |   false   |  a string containing the serialized type of parameters to be sent to the component method. This uses **'?'** to separate between parameters, like this: 'System.Int32 ? System.Int32' - this represents that the signature of the method has two ints |
| assemblyName  | string | true | name of the assembly where the component is |

###### Observation: Since Java doesn't have optional paramaters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **AltGetAllElementsParameters** which we use the parameters mentioned. The java example will also show how to build such an object.

## Examples
<!-- Language Specific -->
<div>
    <button class="language-btn active">C#</button>
    <button class="language-btn">Java</button>
    <button class="language-btn">Python</button>
</div>
<div id="language-c" class="languageContent" markdown=1 style="display:block;">

``` c#

   [Test]
    public void TestCallMethodWithAssembly(){
        AltUnityObject capsule = altUnityDriver.FindObject(By.NAME,"Capsule");
        var initialRotation = capsule.GetComponentProperty("UnityEngine.Transform", "rotation");
        capsule.CallComponentMethod("UnityEngine.Transform", "Rotate", "10?10?10", "System.Single?System.Single?System.Single", "UnityEngine.CoreModule");
        AltUnityObject capsuleAfterRotation = altUnityDriver.FindObject(By.NAME,"Capsule");
        var finalRotation = capsuleAfterRotation.GetComponentProperty("UnityEngine.Transform", "rotation");
        Assert.AreNotEqual(initialRotation, finalRotation);
    }

```

</div>
<div id="language-python" class="languageContent" markdown=1>

``` python

 def test_call_component_method(self):
        self.altdriver.load_scene('Scene 1 AltUnityDriverTestScene')
        result = self.altdriver.find_element("Capsule").call_component_method("Capsule", "Jump", "setFromMethod")
        self.assertEqual(result,"null")
        self.altdriver.wait_for_element_with_text('CapsuleInfo', 'setFromMethod')
        self.assertEqual('setFromMethod', self.altdriver.find_element('CapsuleInfo').get_text())

```

</div>
<div id="language-java" class="languageContent" markdown=1>

``` java
 @Test
    public void TestCallMethodWithMultipleDefinitions() throws Exception {

        AltUnityObject capsule=altUnityDriver.findObject(AltUnityDriver.By.NAME,"Capsule");
        capsule.callComponentMethod("","Capsule", "Test","2","System.Int32");
        AltUnityObject capsuleInfo=altUnityDriver.findObject(AltUnityDriver.By.NAME,"CapsuleInfo");
        assertEquals("6",capsuleInfo.getText());
    }
```
</div>