# Command: FindObjectsWhichContains

## Description:

Find all objects in the scene that respects the given criteria. Check [By](../../other/by.html) for more information about criterias.

###### Observation: Every criteria except of path works for this command

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
        {
           AltUnityDriver.LoadScene("Scene 5 Keyboard Input");
    
           var stars = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Star","Player2");
           var player = AltUnityDriver.FindObjectsWhichContain(By.NAME, "Player", "Player2");
            Assert.AreEqual(1, stars.Count);
    
           AltUnityDriver.MoveMouse(new UnityEngine.Vector2(player[0].x, player[0].y+500), 1);
           UnityEngine.Debug.Log(stars[0].x+"  "+stars[0].y);
           Thread.Sleep(1500);
    
           AltUnityDriver.PressKey(UnityEngine.KeyCode.Mouse0, 0);
           AltUnityDriver.MoveMouseAndWait(new UnityEngine.Vector2(player[0].x, player[0].y-500), 1);
           Thread.Sleep(1500);
           AltUnityDriver.PressKeyAndWait(UnityEngine.KeyCode.Mouse0, 1);
    
           stars = AltUnityDriver.FindObjectsWhichContain(By.NAME,"Star");
           Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java
      @Test
          public void testFindElementsWhereNameContains() throws Exception {
              String name = "Pla";
              AltUnityObject[] altElements = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,name);
              assertNotNull(altElements);
              assertTrue(altElements[0].name.contains(name));
          }


    .. code-tab:: py
        def test_creating_stars(self):
                self.altdriver.load_scene("Scene 5 Keyboard Input")
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star","Player2")
                self.assertEqual(1, len(stars))
                player = self.altdriver.find_objects_which_contains(By.NAME,"Player","Player2")
        
                self.altdriver.move_mouse(int(stars[0].x),int(player[0].y)+500, 1)
                time.sleep(1.5)
        
                self.altdriver.press_key('Mouse0', 1,0)
                self.altdriver.move_mouse_and_wait(int(stars[0].x),int(player[0].y)-500, 1)
                self.altdriver.press_key('Mouse0', 1,0)
        
                stars = self.altdriver.find_objects_which_contains(By.NAME,"Star")
                self.assertEqual(3, len(stars))
```
