# Command: WaitForObject

## Description:

Wait until there is no longer any objects that respect the given criteria or times run out and will throw an error. Check [By](../../other/by.html) for more information about criterias.

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
- Nothing
## Examples


```eval_rst
.. tabs::

    .. code-tab:: c#
    
       [Test]
           public void TestWaitForObjectToNotExistFail()
           {
               try
               {
                   altUnityDriver.WaitForObjectNotBePresent(By.NAME,"Capsule", timeout: 1, interval: 0.5f);
                   Assert.Fail();
               }
               catch (WaitTimeOutException exception)
               {
                   Assert.AreEqual("Element //Capsule still found after 1 seconds", exception.Message);
               }
           }

    .. code-tab:: java

        //TODO


    .. code-tab:: py
        //TODO

```
