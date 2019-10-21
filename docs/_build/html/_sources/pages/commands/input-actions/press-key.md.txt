# Command: PressKey

## Description:

Simulate key press action in your game. This command does not wait for the action to finish. To also wait for the action to finish use [PressKeyAndWait]({{ site.baseurl }}/pages/commands/input-actions/press-key-and-wait)

## Parameters:

|      Name       |     Type      | Optional | Description |
| --------------- | ------------- | -------- | ----------- |
| keycode      |     KeyCode(C#)/string(python/java)    |   false   | Name of the button. Please check [KeyCode for C#](https://docs.unity3d.com/ScriptReference/KeyCode.html) or [key section for python/java](https://docs.unity3d.com/Manual/ConventionalGameInput.html) for more information about key names|
| power      |     float    |   false   | A value from \[-1,1\] that defines how strong the key was pressed. This is mostly used for joystick button since the keyboard button will always be 1 or -1|
| duration      |     float    |   false   | The time measured in seconds to move the mouse from current position to the set location.|

###### Observation: Since Java doesn't have optional parameters we decided to go with an builder pattern approach but also didn't want to change the way how the commands are made. So instead of calling command with the parameters mentioned in the table, you will need to build an object name **[AltPressKeyParameters](../../other/java-builders.html#altpresskeyparameters)** which we use the parameters mentioned. The java example will also show how to build such an object.

## Examples
<!-- Language Specific -->
<div>
    <button class="language-btn active">C#</button>
    <button class="language-btn">Java</button>
    <button class="language-btn">Python</button>
</div>
<div id="language-c" class="languageContent" markdown=1 style="display:block;">

```eval_rst
.. tabs::

    .. code-tab:: c#
        [Test]
            public void TestCreatingStars()
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
            public void TestCreatingStars() throws InterruptedException {
        
                AltUnityObject[] stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
                assertEquals(1, stars.length);
                AltUnityObject player=altUnityDriver.findElement("Player1","Player2");
                altUnityDriver.moveMouse(player.x, player.y+500, 1);
                Thread.sleep(1500);
        
                altUnityDriver.pressKey("Mouse0", 1,1);
                altUnityDriver.moveMouseAndWait(player.x, player.y-500, 1);
                altUnityDriver.pressKeyAndWait("Mouse0", 1,1);
        
        
                stars = altUnityDriver.findObjectsWhichContains(AltUnityDriver.By.NAME,"Star","");
                assertEquals(3, stars.length);
        
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
