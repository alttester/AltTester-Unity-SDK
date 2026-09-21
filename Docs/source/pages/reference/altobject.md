# AltObject

The **AltObject** class represents the objects present in the app and it allows you through the methods listed below to interact with them. It is the return type of the methods in the [FindObjects](#findobjects) category.

**_Fields_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: AltObject Fields
           :widths: 20 20 40
           :header-rows: 1

           * - Name
             - Type
             - Description
           * - name
             - string
             - The name of the object.
           * - id
             - int
             - The object's id.
           * - x
             - int
             - The value for x axis coordinate on screen.
           * - y
             - int
             - The value for y axis coordinate on screen.
           * - mobileY
             - int
             - The value for y axis for Appium.
           * - type
             - string
             - Object's type, for objects from the app is gameObject.
           * - enabled
             - bool
             - The local active state of the object. Note that an object may be inactive because a parent is not active, even if this returns true.
           * - worldX
             - float
             - The value for x axis coordinate in the app's world.
           * - worldY
             - float
             - The value for y axis coordinate in the app's world.
           * - worldZ
             - float
             - The value for z axis coordinate in the app's world.
           * - idCamera
             - int
             - The camera's id.
           * - transformId
             - int
             - The transform's component id.
           * - parentId
             - int
             - The transform parent's id. It's obsolete. Use transformParentId instead.
           * - transformParentId
             - int
             - The transform parent's id.

    .. tab:: Java

        .. list-table:: AltObject Fields
           :widths: 20 20 40
           :header-rows: 1

           * - Name
             - Type
             - Description
           * - name
             - String
             - The name of the object.
           * - id
             - int
             - The object's id.
           * - x
             - int
             - The value for x axis coordinate on screen.
           * - y
             - int
             - The value for y axis coordinate on screen.
           * - mobileY
             - int
             - The value for y axis for Appium.
           * - type
             - String
             - Object's type, for objects from the app is gameObject.
           * - enabled
             - boolean
             - The local active state of the object. Note that an object may be inactive because a parent is not active, even if this returns true.
           * - worldX
             - float
             - The value for x axis coordinate in the app's world.
           * - worldY
             - float
             - The value for y axis coordinate in the app's world.
           * - worldZ
             - float
             - The value for z axis coordinate in the app's world.
           * - idCamera
             - int
             - The camera's id.
           * - transformId
             - int
             - The transform's component id.
           * - parentId
             - int
             - The transform parent's id. It's obsolete. Use transformParentId instead.
           * - transformParentId
             - int
             - The transform parent's id.

    .. tab:: Python

        .. list-table:: AltObject Properties
           :widths: 20 20 40
           :header-rows: 1

           * - Name
             - Type
             - Description
           * - name
             - string
             - The name of the object.
           * - id
             - int
             - The object's id.
           * - x
             - int
             - The value for x axis coordinate on screen.
           * - y
             - int
             - The value for y axis coordinate on screen.
           * - mobileY
             - int
             - The value for y axis for Appium.
           * - type
             - string
             - Object's type, for objects from the app is gameObject.
           * - enabled
             - bool
             - The local active state of the object. Note that an object may be inactive because a parent is not active, even if this returns true.
           * - worldX
             - float
             - The value for x axis coordinate in the app's world.
           * - worldY
             - float
             - The value for y axis coordinate in the app's world.
           * - worldZ
             - float
             - The value for z axis coordinate in the app's world.
           * - idCamera
             - int
             - The camera's id.
           * - transformId
             - int
             - The transform's component id.
           * - parentId
             - int
             - The transform parent's id. It's obsolete. Use transform_parent_id instead.
           * - transformParentId
             - int
             - The transform parent's id.

    .. tab:: Robot

        .. list-table:: AltObject Properties
           :widths: 20 20 40
           :header-rows: 1

           * - Name
             - Type
             - Description
           * - name
             - string
             - The name of the object.
           * - id
             - int
             - The object's id.
           * - x
             - int
             - The value for x axis coordinate on screen.
           * - y
             - int
             - The value for y axis coordinate on screen.
           * - mobileY
             - int
             - The value for y axis for Appium.
           * - type
             - string
             - Object's type, for objects from the app is gameObject.
           * - enabled
             - boolean
             - The local active state of the object. Note that an object may be inactive because a parent is not active, even if this returns true.
           * - worldX
             - float
             - The value for x axis coordinate in the app's world.
           * - worldY
             - float
             - The value for y axis coordinate in the app's world.
           * - worldZ
             - float
             - The value for z axis coordinate in the app's world.
           * - idCamera
             - int
             - The camera's id.
           * - transformId
             - int
             - The transform's component id.
           * - parentId
             - int
             - The transform parent's id. It's obsolete. Use transform_parent_id instead.
           * - transformParentId
             - int
             - The transform parent's id.
```

The available methods are the following:

#### FindObjectFromObject

Finds the first child of the object that respects the given criteria. Check [By](#by-selector) for more information about criteria.

**_Parameters_**

```eval_rst
.. list-table:: FindObjectFromObject Parameters
   :widths: 20 20 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``by``
       *Python, Robot:* ``locator_strategy``
     - By
     - Yes
     - Set what criteria to use in order to find the object.
   * - ``value``
       *Python, Robot:* ``locator``
     - string
     - Yes
     - The value to which the object will be compared to see if it respects the criteria or not.
   * - ``cameraBy``
     - By
     - No
     - Set what criteria to use in order to find the camera.
   * - ``cameraValue``
     - string
     - No
     - The value to which all the cameras in the scene will be compared to see if they respect the criteria or not to get the camera for which the screen coordinates of the object will be calculated. If no camera is given, it will search through all cameras in the scene until some camera sees the object or return the screen coordinates of the object calculated to the last camera in the scene.
   * - ``enabled``
     - boolean
     - No
     - If `true`, will match only objects that are active in hierarchy. If `false`, will match all objects.
```

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestFindObjectFromObject()
        {
            var parent = altDriver.FindObject(By.NAME,"Canvas");
            var child = parent.FindObjectFromObject(By.TEXT,"Change Camera Mode");
            Assert.AreEqual(child.name, "Text);
        }

    .. code-tab:: java

        @Test
        public void testfindObjectFromObject() throws Exception
        {
            AltObject parent = altDriver.findObject(new AltFindObjectsParams.Builder(
                AltDriver.By.NAME, "Canvas").build());
            child = parent.findObjectFromObject(new AltFindObjectsParams.Builder(
                AltDriver.By.TEXT, "Change Camera Mode").build());
            assertEquals("Text", child.name);
        }

    .. code-tab:: py

        def test_find_object_from_object(self):
            parent = self.alt_driver.find_object(By.NAME, "Canvas")
            child = parent.find_object_from_object(By.TEXT, "Change Camera Mode")
            assert child.name == "Text

    .. code-tab:: robot

        Test Find Object From Object By Text
            ${parent}=    Find Object    NAME    Canvas
            ${child}=    Find Object From Object    ${parent}    TEXT    Change Camera Mode
            Should Be Equal    ${child.name}    Text

```

## CallComponentMethod

Invokes a method from an existing component of the object.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: CallComponentMethod Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - methodName
             - string
             - Yes
             - The name of the public method that will be called. If the method is inside a property/field to be able to call that method, methodName needs to be the following format "propertyName.MethodName".
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the component.
           * - parameters
             - array
             - Yes
             - An array containing the serialized parameters to be sent to the component method.
           * - typeOfParameters
             - array
             - No
             - An array containing the serialized type of parameters to be sent to the component method.

    .. tab:: Java

        .. list-table:: callComponentMethod Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - String
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - methodName
             - String
             - Yes
             - The name of the public method that will be called. If the method is inside a property/field to be able to call that method, methodName needs to be the following format "propertyName.MethodName".
           * - assemblyName
             - String
             - Yes
             - The name of the assembly containing the component.
           * - parameters
             - Array
             - Yes
             - An array containing the serialized parameters to be sent to the component method.
           * - typeOfParameters
             - Array
             - No
             - An array containing the serialized type of parameters to be sent to the component method.

    .. tab:: Python

        .. list-table:: call_component_method Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - method_name
             - string
             - Yes
             - The name of the public method that will be called. If the method is inside a property/field to be able to call that method, method_name needs to be the following format "property_name.method_name".
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - parameters
             - list
             - Yes
             - A list containing the serialized parameters to be sent to the component method.
           * - type_of_parameters
             - list
             - No
             - A list containing the serialized type of parameters to be sent to the component method.

    .. tab:: Robot

        .. list-table:: Call Component Method Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - method_name
             - string
             - Yes
             - The name of the public method that will be called. If the method is inside a property/field to be able to call that method, method_name needs to be the following format "property_name.method_name".
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - parameters
             - list
             - Yes
             - A list containing the serialized parameters to be sent to the component method.
           * - type_of_parameters
             - list
             - No
             - A list containing the serialized type of parameters to be sent to the component method.
```

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
            var initialRotation = capsule.GetComponentProperty<dynamic>("UnityEngine.Transform", "rotation", "UnityEngine.CoreModule");
            capsule.CallComponentMethod<string>("UnityEngine.Transform", "Rotate", "UnityEngine.CoreModule", new object[3] { 10, 10, 10 }, new[] { "System.Single", "System.Single", "System.Single" });
            AltObject capsuleAfterRotation = altDriver.FindObject(By.NAME, "Capsule");
            var finalRotation = capsuleAfterRotation.GetComponentProperty<dynamic>("UnityEngine.Transform", "rotation", "UnityEngine.CoreModule");
            Assert.IsTrue(initialRotation["x"] != finalRotation["x"] || initialRotation["y"] != finalRotation["y"] || initialRotation["z"] != finalRotation["z"] || initialRotation["w"] != finalRotation["w"]);
        }

        [Test]
        public void TestCallMethodWithNoParameters()
        {
            const string componentName = "UnityEngine.UI.Text";
            const string methodName = "get_text";
            const string assemblyName = "UnityEngine.UI";
            const string elementText = "Change Camera Mode";
            var altElement = altDriver.FindObject(By.PATH, "/Canvas/Button/Text");
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
            var altElement = altDriver.FindObject(By.PATH, "/Canvas/UnityUIInputField/Text");
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
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.PATH,
                "/Canvas/Button/Text").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParams);
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
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.PATH,
            "/Canvas/UnityUIInputField/Text").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParams);
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
            result = self.alt_driver.find_object(By.NAME, "Capsule").call_component_method(
            "AltExampleScriptCapsule", "Jump", "Assembly-CSharp", ["setFromMethod"])
            self.assertEqual(result, None)
            self.alt_driver.wait_for_object(By.PATH, '//CapsuleInfo[@text=setFromMethod]', timeout=1)
            self.assertEqual('setFromMethod', self.alt_driver.find_object(By.NAME, 'CapsuleInfo').get_text())

        def test_call_component_method_with_no_parameters(self):
            result = self.alt_driver.find_object(By.PATH, "/Canvas/Button/Text")
            text = result.call_component_method("UnityEngine.UI.Text", "get_text", "UnityEngine.UI")
            assert text == "Change Camera Mode"

        def test_call_component_method_with_parameters(self):
            fontSizeExpected =16
            altElement = self.alt_driver.find_object(By.PATH, "/Canvas/UnityUIInputField/Text")
            altElement.call_component_method("UnityEngine.UI.Text", "set_fontSize", "UnityEngine.UI", parameters=["16"])
            fontSize = altElement.call_component_method("UnityEngine.UI.Text", "get_fontSize", "UnityEngine.UI", parameters=[])
            assert fontSizeExpected == fontSize

    .. code-tab:: robot

        Test Call Component Method
            ${alt_object}=    Find Object    NAME    Capsule
            ${parameters}=    Create List    setFromMethod
            ${result}=    Call Component Method    ${alt_object}    AltExampleScriptCapsule    Jump    Assembly-CSharp    parameters=${parameters}
            Should Be Equal    ${result}    ${None}
            Wait For Object    PATH    //CapsuleInfo[@text=setFromMethod]    timeout=1
            ${capsule_info}=    Find Object    NAME    CapsuleInfo
            ${capsule_info_text}=    Get Text    ${capsule_info}
            Should Be Equal    ${capsule_info_text}    setFromMethod

        Test Call Component Method With No Parameters
            ${result}=    Find Object    PATH    /Canvas/Button/Text
            ${text}=    Call Component Method    ${result}    UnityEngine.UI.Text    get_text    UnityEngine.UI
            Should Be Equal    ${text}    Change Camera Mode

        Test Call Component Method With Parameters
            ${alt_object}=    Find Object    PATH    /Canvas/UnityUIInputField/Text
            ${params}=    Create List    ${16}
            Call Component Method    ${alt_object}    UnityEngine.UI.Text    set_fontSize    UnityEngine.UI    ${params}
            ${empty_list}=    Create List
            ${font_size}=    Call Component Method    ${alt_object}    UnityEngine.UI.Text    get_fontSize    UnityEngine.UI    ${empty_list}
            Should Be Equal As Integers    ${font_size}    16

```
## WaitForComponentProperty

Wait until a property has a specific value and returns the value of the given component property.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: WaitForComponentProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - propertyName
             - string
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - propertyValue
             - T
             - Yes
             - The value that the property should have.
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the component.
           * - timeout
             - double
             - No
             - The number of seconds that it will wait for the property. The default value is 20 seconds.
           * - interval
             - double
             - No
             - The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5 seconds.
           * - getPropertyAsString
             - bool
             - No
             - If `true`, it will treat the propertyValue as a string; if `false` it will consider the original type of the propertyValue. This is especially useful when you want to pass for example `[[], []]` as a propertyValue, which you can do by setting getPropertyAsString to `true` and propertyValue to `JToken.Parse("[[], []]")` (in C#).
           * - maxDepth
             - int
             - No
             - The value that defines the maximum level from which to retrieve properties. By default it is 2.

    .. tab:: Java

        .. list-table:: waitForComponentProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - String
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - propertyName
             - String
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - propertyValue
             - Object
             - Yes
             - The value that the property should have.
           * - assemblyName
             - String
             - Yes
             - The name of the assembly containing the component.
           * - timeout
             - double
             - No
             - The number of seconds that it will wait for the property. The default value is 20 seconds.
           * - interval
             - double
             - No
             - The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5 seconds.
           * - getPropertyAsString
             - boolean
             - No
             - If `true`, it will treat the propertyValue as a string; if `false` it will consider the original type of the propertyValue. This is especially useful when you want to pass for example `[[], []]` as a propertyValue, which you can do by setting getPropertyAsString to `true` and propertyValue to `JToken.Parse("[[], []]")` (in C#).
           * - maxDepth
             - int
             - No
             - The value that defines the maximum level from which to retrieve properties. By default it is 2.

    .. tab:: Python

        .. list-table:: wait_for_component_property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - property_name
             - string
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - property_value
             - string
             - Yes
             - The value that the property should have.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - timeout
             - float
             - No
             - The number of seconds that it will wait for the property. The default value is 20 seconds.
           * - interval
             - float
             - No
             - The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5 seconds.
           * - get_property_as_string
             - bool
             - No
             - If `true`, it will treat the property_value as a string; if `false` it will consider the original type of the property_value. This is especially useful when you want to pass for example `[[], []]` as a property_value, which you can do by setting get_property_as_string to `true` and property_value to `JToken.Parse("[[], []]")` (in C#).
           * - max_depth
             - int
             - No
             - The value that defines the maximum level from which to retrieve properties. By default it is 2.

    .. tab:: Robot

        .. list-table:: Wait Fo Component Property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - property_name
             - string
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - property_value
             - string
             - Yes
             - The value that the property should have.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - timeout
             - float
             - No
             - The number of seconds that it will wait for the property. The default value is 20 seconds.
           * - interval
             - float
             - No
             - The number of seconds after which it will try to find the object again. The interval should be smaller than the timeout. The default value is 0.5 seconds.
           * - get_property_as_string
             - bool
             - No
             - If `true`, it will treat the property_value as a string; if `false` it will consider the original type of the property_value. This is especially useful when you want to pass for example `[[], []]` as a property_value, which you can do by setting get_property_as_string to `true` and property_value to `JToken.Parse("[[], []]")` (in C#).
           * - max_depth
             - int
             - No
             - The value that defines the maximum level from which to retrieve properties. By default it is 2.
```

**_Returns_**

- Object

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForComponentProperty()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.AltServerPort";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);

            string portStr = System.Environment.GetEnvironmentVariable("ALTSERVER_PORT");
            int port = int.Parse(portStr);
            var propertyValue = altElement.WaitForComponentProperty<int>(componentName, propertyName, port, "AltTester.AltTesterUnitySDK", maxDepth: 1);
            Assert.AreEqual(port, propertyValue);
        }

        [Test]
        public void TestWaitForComponentPropertyAsString()
        {
            var Canvas = altDriver.WaitForObject(By.PATH, "/Canvas");
            Canvas.WaitForComponentProperty("UnityEngine.Canvas", "transform", JToken.Parse("[[], [[]], [[]], [[]], [[]], [[], [], []], [[[], [], []]], [], [], [[]], [[]], [[]]]"), "UnityEngine.UIModule", 1, getPropertyAsString: true);
        }

    .. code-tab:: java

        @Test
        public void testWaitForComponentProperty() throws InterruptedException {
            Thread.sleep(1000);
            String componentName = "UnityEngine.CapsuleCollider";
            String propertyName = "isTrigger";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                "Capsule").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParams);
                assertNotNull(altElement);
            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                componentName, propertyName, "").withMaxDepth(1).build();
            AltWaitForComponentPropertyParams<Boolean> altWaitForComponentPropertyParams = new AltWaitForComponentPropertyParams.Builder<Boolean>(altGetComponentPropertyParams).build();

            Boolean propertyValue = altElement.waitForComponentProperty(
                altWaitForComponentPropertyParams,
                false,
                Boolean.class);
            assertEquals(Boolean.FALSE, propertyValue);
        }

        @Test
        public void TestWaitForComponentPropertyGetPropertyAsString() throws InterruptedException {
            AltObject Canvas = altDriver.waitForObject(new AltWaitForObjectsParams.Builder(
            new AltFindObjectsParams.Builder(AltDriver.By.PATH, "/Canvas").build()).build());
            Canvas.waitForComponentProperty(
                new AltWaitForComponentPropertyParams.Builder<JsonElement>(new AltGetComponentPropertyParams.Builder(
                "UnityEngine.UI.CanvasScaler", "transform",
                "UnityEngine.UI").build()).build(),
                new Gson().toJsonTree("[[],[[]],[[]],[[]],[[]],[[],[],[]],[[[],[],[]]],[],[],[[]],[[]],[[]]]"),
                true,
                JsonElement.class);
        }

    .. code-tab:: py

        def test_wait_for_component_property(self):
            alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
            result = alt_object.wait_for_component_property(
                "AltExampleScriptCapsule", "TestBool", True,
                "Assembly-CSharp", max_depth=1)
            assert result is True

        def test_wait_for_component_property_get_property_as_string(self):
            Canvas = self.alt_driver.wait_for_object(By.PATH, "/Canvas")
            Canvas.wait_for_component_property("UnityEngine.RectTransform", "transform",
                                            "[[],[[]],[[]],[[]],[[]],[[],[],[]],[[[],[],[]]],[],[],[[]],[[]],[[]]]",
                                            "UnityEngine.CoreModule", 1, get_property_as_string=True)

    .. code-tab:: robot

        Test Wait For Component Property
            ${alt_object}=    Find Object    NAME    Capsule
            ${result}=    Wait For Component Property    ${alt_object}    AltExampleScriptCapsule    TestBool    ${True}    Assembly-CSharp
            Should Be Equal    ${result}    ${True}    max_depth=${1}

        Test Wait For Component Property Get Property As String
            ${Canvas} =    Wait For Object    PATH    /Canvas
            Wait For Component Property    ${Canvas}    UnityEngine.RectTransform    name    Canvas    UnityEngine.CoreModule    1    get_property_as_string=${True}

```

## GetComponentProperty

Returns the value of the given component property.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: GetComponentProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - propertyName
             - string
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the component.
           * - maxDepth
             - int
             - No
             - Set how deep the serialization of the property to do.

    .. tab:: Java

        .. list-table:: getComponentProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - String
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - propertyName
             - String
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - assemblyName
             - String
             - Yes
             - The name of the assembly containing the component.
           * - maxDepth
             - int
             - No
             - Set how deep the serialization of the property to do.

    .. tab:: Python

        .. list-table:: get_component_property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - property_name
             - string
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - max_depth
             - int
             - No
             - Set how deep the serialization of the property to do.

    .. tab:: Robot

        .. list-table:: Get Component Property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - property_name
             - string
             - Yes
             - Name of the property of which value you want. If the property is an array, you can specify which element of the array to return by doing property[index], or if you want a property inside of another property you can get by doing property.property2 for example position.x.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - max_depth
             - int
             - No
             - Set how deep the serialization of the property to do.
```

**_Returns_**

- Object

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetComponentProperty()
        {
            const string componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            const string propertyName = "InstrumentationSettings.AppName";
            var altElement = altDriver.FindObject(By.NAME, "AltTesterPrefab");
            Assert.NotNull(altElement);
            var propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName, "AltTester.AltTesterUnitySDK");

            Assert.AreEqual("__default__", propertyValue);
        }

    .. code-tab:: java

        @Test
        public void testGetComponentProperty() throws InterruptedException
        {
            Thread.sleep(1000);
            String componentName = "AltTester.AltTesterUnitySDK.Commands.AltRunner";
            String propertyName = "InstrumentationSettings.ResetConnectionData";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    "AltTesterPrefab").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParams);
            assertNotNull(altElement);

            Boolean propertyValue = altElement.getComponentProperty(
                    new AltGetComponentPropertyParams.Builder(componentName,
                            propertyName, "AltTester.AltTesterUnitySDK").build(),
                    Boolean.class);
            assertTrue(propertyValue);
        }

    .. code-tab:: py

        def test_get_component_property(self):
            self.alt_driver.load_scene('Scene 1 AltDriverTestScene')
            alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
            result = alt_object.get_component_property(
                "AltExampleScriptCapsule", "arrayOfInts", "Assembly-CSharp")

    .. code-tab:: robot

        Test Get Component Property
            ${alt_object}=    Find Object    NAME    Capsule
            ${result}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    arrayOfInts    Assembly-CSharp
            ${list}=    Create List    ${1}    ${2}    ${3}
            Should Be Equal    ${result}    ${list}

```

## SetComponentProperty

Sets value of the given component property.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: SetComponentProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - propertyName
             - string
             - Yes
             - The name of the property of which value you want to set.
           * - value
             - object
             - Yes
             - The value to be set for the chosen component's property.
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the component.

    .. tab:: Java

        .. list-table:: setComponentProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - String
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.componentName".
           * - propertyName
             - String
             - Yes
             - The name of the property of which value you want to set.
           * - value
             - Object
             - Yes
             - The value to be set for the chosen component's property.
           * - assemblyName
             - String
             - Yes
             - The name of the assembly containing the component.

    .. tab:: Python

        .. list-table:: set_component_property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - property_name
             - string
             - Yes
             - The name of the property of which value you want to set.
           * - value
             - object
             - Yes
             - The value to be set for the chosen component's property.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.

    .. tab:: Robot

        .. list-table:: Set Component Property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should look like this: "namespace.component_name".
           * - property_name
             - string
             - Yes
             - The name of the property of which value you want to set.
           * - value
             - object
             - Yes
             - The value to be set for the chosen component's property.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
```
                                                              
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
            var altElement = altDriver.FindObject(By.NAME, "Capsule");
            Assert.NotNull(altElement);
            altElement.SetComponentProperty(componentName, propertyName, "2", "Assembly-CSharp");

            var propertyValue = altElement.GetComponentProperty<string>(componentName, propertyName, "Assembly-CSharp");
            Assert.AreEqual("2", propertyValue);
        }

    .. code-tab:: java

        @Test
        public void testSetComponentProperty()
        {
            String componentName = "AltExampleScriptCapsule";
            String propertyName = "stringToSetFromTests";
            String assembly = "Assembly-CSharp";
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    "Capsule").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParams);
            assertNotNull(altElement);
            altElement.setComponentProperty(
                    new AltSetComponentPropertyParams.Builder(componentName, propertyName,
                            assembly, "2").build());
            int propertyValue = altElement.getComponentProperty(
                    new AltGetComponentPropertyParams.Builder(componentName,
                            propertyName,
                            assembly).build(),
                    int.class);
            assertEquals(2, propertyValue);
        }

    .. code-tab:: py

        def test_set_component_property(self):
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene")
            alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
            alt_object.set_component_property(
                "AltExampleScriptCapsule", "arrayOfInts", "Assembly-CSharp", [2, 3, 4])

            alt_object = self.alt_driver.find_object(By.NAME, "Capsule")
            result = alt_object.get_component_property(
                "AltExampleScriptCapsule", "arrayOfInts", "Assembly-CSharp")

    .. code-tab:: robot

        Test Set Component Property
            ${alt_object}=    Find Object    NAME    Capsule
            ${list}=    Create List    ${2}    ${3}    ${4}
            Set Component Property    ${alt_object}    AltExampleScriptCapsule    arrayOfInts    Assembly-CSharp    ${list}
            ${alt_object}=    Find Object    NAME    Capsule
            ${result}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    arrayOfInts    Assembly-CSharp
            Should Be Equal    ${result}    ${list}

```

## GetText

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

## SetText

Sets text value for a Button, Text, InputField. This also works with TextMeshPro elements.

**_Parameters_**

```eval_rst
.. list-table:: SetText Parameters
   :widths: 20 20 10 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``text``
     - string
     - Yes
     - N/A
     - The text to be set.
   * - ``submit``
     - bool
       *Java:* boolean
     - No
     - N/A
     - If set will trigger a submit event.
```

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [TestCase("UnityUIInputField")] // UI input field
        [TestCase("TextMeshInputField")] // text mesh input field
        public void TestSetTextForUnityUIInputField(string fieldName)
        {
            var inputField = altDriver.FindObject(By.NAME, fieldName).SetText("exampleUnityUIInputField", true);
            Assert.AreEqual("exampleUnityUIInputField", inputField.GetText());
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onValueChangedInvoked", "Assembly-CSharp"), "onValueChangedInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onSubmitInvoked", "Assembly-CSharp"), "onSubmitInvoked was false");
            Assert.IsTrue(inputField.GetComponentProperty<bool>("AltInputFieldRaisedEvents", "onEndEditInvoked", "Assembly-CSharp"), "onEndEditInvoked was false");
        }

    .. code-tab:: java

        @Test
        public void TestSetTextWithSubmit() {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "NonEnglishText").build();
            AltObject textObject = altDriver.findObject(altFindObjectsParameters1);
            String originalText = textObject.getText();
            String text = "ModifiedText";
            AltSetTextParams setTextParams = new AltSetTextParams.Builder(text).withSubmit(true).build();
            String afterText = textObject.setText(setTextParams).getText();
            assertNotEquals(originalText, afterText);
            assertEquals(text, afterText);
        }

    .. code-tab:: py

        def test_set_text(self):
            text_object = self.alt_driver.find_object(By.NAME, "NonEnglishText")
            original_text = text_object.get_text()
            after_text = text_object.set_text("ModifiedText").get_text()

            assert original_text != after_text
            assert after_text == "ModifiedText"

    .. code-tab:: robot

        Test Set Text
            ${text_object}=    Find Object    NAME    NonEnglishText
            ${original_text}=    Get Text    ${text_object}
            Set Text    ${text_object}    ModifiedText
            ${after_text}=    Get Text    ${text_object}
            Should Not Be Equal As Strings    ${original_text}    ${after_text}
            Should Be Equal As Strings    ${after_text}    ModifiedText

```

## Tap

Tap current object.

**_Parameters_**

```eval_rst
.. list-table:: Tap Parameters
   :widths: 20 20 10 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``count``
     - int
     - No
     - 1
     - Number of taps.
   * - ``interval``
     - float
     - No
     - 0.1
     - Interval between taps in seconds.
   * - ``wait``
     - boolean
     - No
     - true
     - Wait for command to finish.
```

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
            self.alt_driver.load_scene('Scene 1 AltDriverTestScene')
            capsule_element = self.alt_driver.find_object(By.NAME, 'Capsule')
            capsule_element.tap()

    .. code-tab:: robot

        Test Tap Object
            ${object}=    Find Object    NAME    Capsule
            Tap Object    ${object}
            ${capsule_info}=    Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1
            ${text}=    Get Text    ${capsule_info}
            Should Be Equal    ${text}    Capsule was clicked to jump!

```

## Click

Click current object.

**_Parameters_**

```eval_rst
.. list-table:: Click Parameters
   :widths: 20 20 10 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``count``
     - int
     - No
     - 1
     - Number of clicks.
   * - ``interval``
     - float
     - No
     - 0.1
     - Interval between clicks in seconds.
   * - ``wait``
     - boolean
     - No
     - true
     - Wait for command to finish.
```

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
            self.alt_driver.load_scene('Scene 1 AltDriverTestScene')
            capsule_element = self.alt_driver.find_object(By.NAME, 'Capsule')
            capsule_element.click()

    .. code-tab:: robot

        Test Click Element
            ${capsule_element}=    Find Object    NAME    Capsule
            Click Object    ${capsule_element}
            Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

```

## PointerDown

Simulates pointer down action on the object.

**_Parameters_**

None

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerDown()
        {
            altDriver.LoadScene("Scene 2 Draggable Panel");
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDown();
            Thread.Sleep(1000);
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
        }

    .. code-tab:: java

        @Test
        public void testPointerDownFromObject() throws InterruptedException {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").build());
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Panel").build();
            AltObject panel = altDriver.findObject(altFindObjectsParameters1);

            AltColor color1 = panel.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "normalColor",
                                            "Assembly-CSharp")
                                            .build(),
                            AltColor.class);
            panel.pointerDown();
            Thread.sleep(1000);
            AltColor color2 = panel.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "highlightColor",
                                            "Assembly-CSharp")
                                            .build(),
                            AltColor.class);
            assertTrue(color1.r != color2.r || color1.g != color2.g || color1.b != color2.b
                            || color1.a != color2.a);
        }

    .. code-tab:: py

        def test_pointer_down_from_object(self):
            self.alt_driver.load_scene('Scene 2 Draggable Panel')
            panel = self.alt_driver.find_object(By.NAME, "Panel")
            color1 = panel.get_component_property(
                "AltExampleScriptPanel",
                "normalColor",
                "Assembly-CSharp"
            )
            panel.pointer_down()

            color2 = panel.get_component_property(
                "AltExampleScriptPanel",
                "highlightColor",
                "Assembly-CSharp"
            )

            assert color1 != color2

    .. code-tab:: robot

        Test Pointer Down From Object
            ${panel}=    Find Object    NAME    Panel
            ${color1}=    Get Component Property    ${panel}    AltExampleScriptPanel    normalColor    Assembly-CSharp
            Pointer Down    ${panel}
            ${color2}=    Get Component Property    ${panel}    AltExampleScriptPanel    highlightColor    Assembly-CSharp
            Should Not Be Equal    ${color1}    ${color2}

```

## PointerUp

Simulates pointer up action on the object.

**_Parameters_**

None

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerUp()
        {
            altDriver.LoadScene("Scene 2 Draggable Panel");
            var panel = altDriver.FindObject(By.NAME, "Panel");
            var color1 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "normalColor", "Assembly-CSharp");
            panel.PointerDown();
            Thread.Sleep(1000);
            panel.PointerUp();
            var color2 = panel.GetComponentProperty<AltColor>("AltExampleScriptPanel", "highlightColor", "Assembly-CSharp");
            Assert.AreEqual(color1, color2);
        }

    .. code-tab:: java

        @Test
        public void testPointerUpFromObject() throws InterruptedException {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").build());
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                            AltDriver.By.NAME, "Panel").build();
            AltObject panel = altDriver.findObject(altFindObjectsParameters1);
            AltColor color1 = panel.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "normalColor",
                                            "Assembly-CSharp")
                                            .build(),
                            AltColor.class);
            panel.pointerDown();
            Thread.sleep(1000);
            panel.pointerUp();
            AltColor color2 = panel.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptPanel", "highlightColor",
                                            "Assembly-CSharp")
                                            .build(),
                            AltColor.class);
            assertTrue(color1.r == color2.r && color1.g == color2.g && color1.b == color2.b
                            && color1.a == color2.a);
        }

    .. code-tab:: py

        def test_pointer_up_from_object(self):
            self.alt_driver.load_scene('Scene 2 Draggable Panel')
            panel = self.alt_driver.find_object(By.NAME, "Panel")
            color1 = panel.get_component_property(
                "AltExampleScriptPanel",
                "normalColor",
                "Assembly-CSharp"
            )
            panel.pointer_down()

            panel.pointer_up()
            color2 = panel.get_component_property(
                "AltExampleScriptPanel",
                "highlightColor",
                "Assembly-CSharp"
            )

            assert color1 == color2            

    .. code-tab:: robot

        Test Pointer Up From Object
            ${panel}=    Find Object    NAME    Panel
            ${color1}=    Get Component Property    ${panel}    AltExampleScriptPanel    normalColor    Assembly-CSharp
            Pointer Down    ${panel}
            Pointer Up    ${panel}
            ${color2}=    Get Component Property    ${panel}    AltExampleScriptPanel    highlightColor    Assembly-CSharp
            Should Be Equal    ${color1}    ${color2}

```

## PointerEnter

Simulates pointer enter action on the object.

**_Parameters_**

None

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerEnterAndExit()
        {
            altDriver.LoadScene("Scene 3 Drag And Drop");
            var altElement = altDriver.FindObject(By.NAME, "Drop Image");
            var color1 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            altDriver.FindObject(By.NAME, "Drop Image").PointerEnter();
            var color2 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
            altDriver.FindObject(By.NAME, "Drop Image").PointerExit();
            var color3 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color3, color2);
            Assert.AreEqual(color1, color3);
        }

    .. code-tab:: java

        @Test
        public void testTestPointerEnterAndExit() throws Exception {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 3 Drag And Drop").build());
            AltObject altElement = FindObject(By.NAME, "Drop Image");
            AltColor color1 = altElement.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                            "Assembly-CSharp").build(),
                            AltColor.class);
            FindObject(By.NAME, "Drop Image").pointerEnter();
            AltColor color2 = altElement.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                            "Assembly-CSharp").build(),
                            AltColor.class);
            assertNotEquals(color1, color2);
            FindObject(By.NAME, "Drop Image").pointerEnter();
            AltColor color3 = altElement.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                            "Assembly-CSharp").build(),
                            AltColor.class);

            assertNotEquals(color3, color2);
            assertNotEquals(color1, color3);        
        }

    .. code-tab:: py

        def test_pointer_enter_and_exit(self):
            self.alt_driver.load_scene("Scene 3 Drag And Drop")
            alt_object = self.alt_driver.find_object(By.NAME, "Drop Image")
            color1 = alt_object.get_component_property(
                "AltExampleScriptDropMe",
                "highlightColor",
                "Assembly-CSharp"
            )
            alt_object.pointer_enter()
            color2 = alt_object.get_component_property(
                "AltExampleScriptDropMe",
                "highlightColor",
                "Assembly-CSharp"
            )

            assert color1["r"] != color2["r"] or \
                color1["g"] != color2["g"] or \
                color1["b"] != color2["b"] or \
                color1["a"] != color2["a"]

            alt_object.pointer_exit()
            color3 = alt_object.get_component_property(
                "AltExampleScriptDropMe",
                "highlightColor",
                "Assembly-CSharp"
            )

            assert color3["r"] != color2["r"] or \
                color3["g"] != color2["g"] or \
                color3["b"] != color2["b"] or \
                color3["a"] != color2["a"]

            assert color3["r"] == color1["r"] and \
                color3["g"] == color1["g"] and \
                color3["b"] == color1["b"] and \
                color3["a"] == color1["a"]

    .. code-tab:: robot

        Test Pointer Enter And Exit
            ${alt_object}=    Find Object    NAME    Drop Image
            ${color1}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
            Pointer Enter    ${alt_object}
            ${color2}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
            ${color1_r}=    Get Percent From Specific Color    ${color1}    r
            ${color1_g}=    Get Percent From Specific Color    ${color1}    g
            ${color1_b}=    Get Percent From Specific Color    ${color1}    b
            ${color1_a}=    Get Percent From Specific Color    ${color1}    a
            ${color2_r}=    Get Percent From Specific Color    ${color2}    r
            ${color2_g}=    Get Percent From Specific Color    ${color2}    g
            ${color2_b}=    Get Percent From Specific Color    ${color2}    b
            ${color2_a}=    Get Percent From Specific Color    ${color2}    a
            Evaluate    ${color1_r}!=${color2_r} or ${color1_g}!=${color2_g} or ${color1_b}!=${color2_b} or ${color1_a}!=${color2_a}
            Pointer Exit    ${alt_object}
            ${color3}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
            ${color3_r}=    Get Percent From Specific Color    ${color3}    r
            ${color3_g}=    Get Percent From Specific Color    ${color3}    g
            ${color3_b}=    Get Percent From Specific Color    ${color3}    b
            ${color3_a}=    Get Percent From Specific Color    ${color3}    a
            Evaluate    ${color3_r}!=${color2_r} or ${color3_g}!=${color2_g} or ${color3_b}!=${color2_b} or ${color3_a}!=${color2_a}
            Evaluate    ${color1_r}!=${color3_r} or ${color1_g}!=${color3_g} or ${color1_b}!=${color3_b} or ${color1_a}!=${color3_a}

```

## PointerExit

Simulates pointer exit action on the object.

**_Parameters_**

None

**_Returns_**

- [AltObject](#altobject)

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestPointerEnterAndExit()
        {
            altDriver.LoadScene("Scene 3 Drag And Drop");
            var altElement = altDriver.FindObject(By.NAME, "Drop Image");
            var color1 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            altDriver.FindObject(By.NAME, "Drop Image").PointerEnter();
            var color2 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color1, color2);
            altDriver.FindObject(By.NAME, "Drop Image").PointerExit();
            var color3 = altElement.GetComponentProperty<dynamic>("AltExampleScriptDropMe", "highlightColor", "Assembly-CSharp");
            Assert.AreNotEqual(color3, color2);
            Assert.AreEqual(color1, color3);
        }

    .. code-tab:: java

        @Test
        public void testTestPointerEnterAndExit() throws Exception {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 3 Drag And Drop").build());
            AltObject altElement = FindObject(By.NAME, "Drop Image");
            AltColor color1 = altElement.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                            "Assembly-CSharp").build(),
                            AltColor.class);
            FindObject(By.NAME, "Drop Image").pointerEnter();
            AltColor color2 = altElement.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                            "Assembly-CSharp").build(),
                            AltColor.class);
            assertNotEquals(color1, color2);
            FindObject(By.NAME, "Drop Image").pointerEnter();
            AltColor color3 = altElement.getComponentProperty(
                            new AltGetComponentPropertyParams.Builder("AltExampleScriptDropMe", "highlightColor",
                                            "Assembly-CSharp").build(),
                            AltColor.class);

            assertNotEquals(color3, color2);
            assertNotEquals(color1, color3);        
        }

    .. code-tab:: py

        def test_pointer_enter_and_exit(self):
            self.alt_driver.load_scene("Scene 3 Drag And Drop")
            alt_object = self.alt_driver.find_object(By.NAME, "Drop Image")
            color1 = alt_object.get_component_property(
                "AltExampleScriptDropMe",
                "highlightColor",
                "Assembly-CSharp"
            )
            alt_object.pointer_enter()
            color2 = alt_object.get_component_property(
                "AltExampleScriptDropMe",
                "highlightColor",
                "Assembly-CSharp"
            )

            assert color1["r"] != color2["r"] or \
                color1["g"] != color2["g"] or \
                color1["b"] != color2["b"] or \
                color1["a"] != color2["a"]

            alt_object.pointer_exit()
            color3 = alt_object.get_component_property(
                "AltExampleScriptDropMe",
                "highlightColor",
                "Assembly-CSharp"
            )

            assert color3["r"] != color2["r"] or \
                color3["g"] != color2["g"] or \
                color3["b"] != color2["b"] or \
                color3["a"] != color2["a"]

            assert color3["r"] == color1["r"] and \
                color3["g"] == color1["g"] and \
                color3["b"] == color1["b"] and \
                color3["a"] == color1["a"]

    .. code-tab:: robot

        Test Pointer Enter And Exit
            ${alt_object}=    Find Object    NAME    Drop Image
            ${color1}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
            Pointer Enter    ${alt_object}
            ${color2}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
            ${color1_r}=    Get Percent From Specific Color    ${color1}    r
            ${color1_g}=    Get Percent From Specific Color    ${color1}    g
            ${color1_b}=    Get Percent From Specific Color    ${color1}    b
            ${color1_a}=    Get Percent From Specific Color    ${color1}    a
            ${color2_r}=    Get Percent From Specific Color    ${color2}    r
            ${color2_g}=    Get Percent From Specific Color    ${color2}    g
            ${color2_b}=    Get Percent From Specific Color    ${color2}    b
            ${color2_a}=    Get Percent From Specific Color    ${color2}    a
            Evaluate    ${color1_r}!=${color2_r} or ${color1_g}!=${color2_g} or ${color1_b}!=${color2_b} or ${color1_a}!=${color2_a}
            Pointer Exit    ${alt_object}
            ${color3}=    Get Component Property    ${alt_object}    AltExampleScriptDropMe    highlightColor    Assembly-CSharp
            ${color3_r}=    Get Percent From Specific Color    ${color3}    r
            ${color3_g}=    Get Percent From Specific Color    ${color3}    g
            ${color3_b}=    Get Percent From Specific Color    ${color3}    b
            ${color3_a}=    Get Percent From Specific Color    ${color3}    a
            Evaluate    ${color3_r}!=${color2_r} or ${color3_g}!=${color2_g} or ${color3_b}!=${color2_b} or ${color3_a}!=${color2_a}
            Evaluate    ${color1_r}!=${color3_r} or ${color1_g}!=${color3_g} or ${color1_b}!=${color3_b} or ${color1_a}!=${color3_a}

```
## UpdateObject

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

            altDriver.PressKey(AltKeyCode.W, 1, 2);

            Assert.AreNotEqual(cubeInitialPostion, cube.UpdateObject().GetWorldPosition());
        }

    .. code-tab:: java

       @Test
        public void TestUpdateAltObject() throws InterruptedException {
                AltFindObjectsParams altFindObjectsParameters = new AltFindObjectsParams.Builder(
                                AltDriver.By.NAME, "Player1").build();
                AltObject cube = altDriver.findObject(altFindObjectsParameters);
                float cubeInitWorldZ = cube.worldZ;

                altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.W).withDuration(1).withPower(2)
                                .withWait(false).build());
                Thread.sleep(2000);
                assertNotEquals(cubeInitWorldZ, cube.UpdateObject().worldZ);
        }

    .. code-tab:: py

        def test_update_altObject(self):
            cube = self.alt_driver.find_object(By.NAME, "Player1")
            initial_position_z = cube.worldZ

            self.alt_driver.press_key(AltKeyCode.W, power=1, duration=0.1, wait=False)
            time.sleep(5)

            assert initial_position_z != cube.update_object().worldZ

    .. code-tab:: robot

        Test Update AltObject
            ${cube}=    Find Object    NAME    Player1
            ${initial_position_z}=    Get Object WorldZ    ${cube}
            Press Key    W    power=1    duration=0.1    wait=${False}
            Sleep    5
            ${cube_updated}=    Update Object    ${cube}
            ${final_position_z}=    Get Object WorldZ    ${cube_updated}
            Should Not Be Equal    ${initial_position_z}    ${final_position_z}

```

## GetParent

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
            self.alt_driver.load_scene('Scene 1 AltDriverTestScene', True)
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
## GetScreenPosition

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
        public void TestHoldButton()
        {
            const int duration = 1;
            var button = altDriver.FindObject(By.NAME, "UIButton");
            altDriver.HoldButton(button.GetScreenPosition(), duration);
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            var text = capsuleInfo.GetText();
            Assert.AreEqual(text, "UIButton clicked to jump capsule!");
            var time = float.Parse(altDriver.FindObject(By.NAME, "ChineseLetters").GetText());
            Assert.Greater(time, duration);
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
            button = self.alt_driver.find_object(By.NAME, "UIButton")
            self.alt_driver.hold_button(button.get_screen_position(), duration=1)
            capsule_info = self.alt_driver.find_object(By.NAME, "CapsuleInfo")
            text = capsule_info.get_text()
            assert text == "UIButton clicked to jump capsule!"

    .. code-tab:: robot

        Test Hold Button
            ${button}=    Find Object    NAME    UIButton
            ${button_position}=    Get Screen Position    ${button}
            Hold Button    ${button_position}    duration=1
            ${capsule_info}=    Find Object    NAME    CapsuleInfo
            ${text}=    Get Text    ${capsule_info}
            Should Be Equal As Strings    ${text}    UIButton clicked to jump capsule!

```
## GetWorldPosition

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
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene")
            capsule = self.alt_driver.find_object(By.NAME, "Capsule")
            initial_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            self.alt_driver.tilt([1, 1, 1], 1)

            capsule = self.alt_driver.find_object(By.NAME, "Capsule")
            final_position = [capsule.worldX, capsule.worldY, capsule.worldZ]
            assert initial_position != final_position

    .. code-tab:: robot

        Test Camera Movement
            ${cube}=    Find Object    NAME    Player1
            ${initial_position}=    Get World Position    ${cube}
            Press Key    W    power=1    duration=0.1    wait=${False}
            ${cube}=    Find Object    NAME    Player1
            ${final_position}=    Get World Position    ${cube}
            Should Not Be Equal    ${initial_position}    ${final_position}

```

## GetVisualElementProperty [Non-GPL]

Returns the value of the given property for a visual element.

**_Parameters_**

```eval_rst
.. list-table:: GetVisualElementProperty Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``propertyName``
     - string
     - Yes
     - The name of the property for which you want to retrieve the value. The list of supported properties can be found in the `Unity Documentation <https://docs.unity3d.com/ScriptReference/UIElements.IResolvedStyle.html>`_.
```

**_Returns_**

- Object

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetVisualElementProperty()
        {
            var playButton = altDriver.FindObject(By.NAME, "Play");
            var width = playButton.GetVisualElementProperty<float>("width");
            Assert.That(width, Is.EqualTo(300));
        }

    .. code-tab:: java

        @Test
        public void testGetVisualElementProperty() {
            AltObject playButton=altDriver.findObject(new AltFindObjectsParams.Builder(By.NAME, "Play").build());
            float width = playButton.GetVisualElementProperty("width", Float.class);
            assertTrue(width == 300);
        }

    .. code-tab:: py

        def test_get_visual_element_property(self):
            play_button = self.alt_driver.find_object(By.NAME, "Play") 
            width = play_button.get_visual_element_property("width")
            assert width == 300

    .. code-tab:: robot

        Test Get Visual Element Property
            ${play_button}=    Find Object    Name    Play
            ${width}=    Get Visual Element Property    ${play_button}    width
            Should Be Equal    ${width}    300

```

## WaitForVisualElementProperty [Non-GPL]

Waits for a visual element property to match a specified value.

**_Parameters_**

```eval_rst
.. list-table:: WaitForVisualElementProperty Parameters
   :widths: 20 20 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``propertyName``
     - string
     - Yes
     - The name of the property to wait for. The list of supported properties can be found in the `Unity Documentation <https://docs.unity3d.com/ScriptReference/UIElements.IResolvedStyle.html>`_.
   * - ``propertyValue``
     - T
       *Python, Robot:* string
     - Yes
     - The value to wait for the property to match.
   * - ``timeout``
     - number
     - No
     - The maximum time to wait for the property to match the value. Default is 20 seconds.
   * - ``interval``
     - number
     - No
     - The interval at which to check the property value. Default is 0.5 seconds.
   * - ``getPropertyAsString``
     - boolean
     - No
     - Whether to retrieve the property value as a string. Default is false.
```

**_Returns_**

- Object: the value of the property once it matches the specified value.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForVisualElementProperty()
        {
            var playButton = altDriver.FindObject(By.NAME, "Play");
            // Wait for the "width" property of a visual element to match 300f
            playButton.WaitForVisualElementProperty("width", 300f, 2, 0.5);
        }

    .. code-tab:: java

         @Test
        public void testWaitForVisualElementProperty() {
            AltObject playButton = altDriver.findObject(new AltFindObjectsParams.Builder(By.NAME, "Play").build());
            /// Wait for the "width" property of a visual element to match 300
            AltWaitForVisualElementPropertyParams<Float> altWaitForVisualElementPropertyParams = new AltWaitForVisualElementPropertyParams.Builder<Float>(
                    "width", 300f).withTimeout(10).withInterval(0.5).build();
            playButton.waitForVisualElementProperty(altWaitForVisualElementPropertyParams, 300f, Float.class);
        }

    .. code-tab:: py

        def test_wait_for_visual_element_property(self):
            play_button = self.alt_driver.find_object(By.NAME, "Play")
            # Wait for the "width" property of a visual element to match 300
            play_button.wait_for_visual_element_property("width", 300, 10, 0.5)

    .. code-tab:: robot
    
        Test Wait For Visual Element Property
            ${play_button}=    Find Object    Name    Play
            # Wait for the "width" property of a visual element to match 300
            Wait For Visual Element Property    ${play_button}    width    300    10    0.5

```
