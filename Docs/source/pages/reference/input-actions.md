# Input actions

## KeyDown

Simulates a key down.

**_Parameters_**

```eval_rst
.. list-table:: KeyDown Parameters
   :widths: 15 15 10 60
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``keyCode``
     - AltKeyCode
     - Yes
     - The keyCode of the key simulated to be pressed.
   * - ``power``
     - int
     - No
     - A value between [-1,1] used for joysticks to indicate how hard the button was pressed.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestKeyDownAndKeyUp()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");
            AltKeyCode kcode = AltKeyCode.A;

            altDriver.KeyDown(kcode, 1);
            var lastKeyDown = altDriver.FindObject(By.NAME, "LastKeyDownValue");
            var lastKeyPress = altDriver.FindObject(By.NAME, "LastKeyPressedValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyDown.GetText(), true));
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyPress.GetText(), true));

            altDriver.KeyUp(kcode);
            var lastKeyUp = altDriver.FindObject(By.NAME, "LastKeyUpValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyUp.GetText(), true));
        }

    .. code-tab:: java

        @Test
        public void TestKeyDownAndKeyUp() throws Exception {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyDownValue").build();
            AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyUpValue").build();
            AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyPressedValue").build();
            AltKeyCode kcode = AltKeyCode.A;
            AltKeyParams altKeyParams = new AltKeyParams.Builder(kcode).build();

            altDriver.keyDown(altKeyParams);
            Thread.sleep(2000);
            AltObject lastKeyDown = altDriver.findObject(altFindObjectsParameters1);
            AltObject lastKeyPress = altDriver.findObject(altFindObjectsParameters3);
            assertEquals("A", AltKeyCode.valueOf(lastKeyDown.getText()).name());
            assertEquals("A", AltKeyCode.valueOf(lastKeyPress.getText()).name());

            altDriver.keyUp(kcode);
            Thread.sleep(2000);
            AltObject lastKeyUp = altDriver.findObject(altFindObjectsParameters2);
            assertEquals("A", AltKeyCode.valueOf(lastKeyUp.getText()).name());
        }

    .. code-tab:: py

        def test_key_down_and_key_up(self):
            self.alt_driver.load_scene('Scene 5 Keyboard Input')

            self.alt_driver.key_down(AltKeyCode.A)
            time.sleep(5)
            lastKeyDown = self.alt_driver.find_object(By.NAME, 'LastKeyDownValue')
            lastKeyPress = self.alt_driver.find_object(By.NAME, 'LastKeyPressedValue')

            self.assertEqual("A", lastKeyDown.get_text())
            self.assertEqual("A", lastKeyPress.get_text())

            self.alt_driver.key_up(AltKeyCode.A)
            time.sleep(5)
            lastKeyUp = self.alt_driver.find_object(By.NAME, 'LastKeyUpValue')
            self.assertEqual("A", lastKeyUp.get_text())

    .. code-tab:: robot

        Test Key Down And Key Up
            Load Scene              ${scene5}
            Key Down    A
            ${last_key_down}=       Find Object     NAME            LastKeyDownValue
            ${last_key_press}=      Find Object     NAME            LastKeyPressedValue
            ${last_key_down_text}=      Get Text    ${last_key_down}
            ${last_key_press_text}=     Get Text    ${last_key_press}
            Should Be Equal As Numbers    ${last_key_down_text}     97
            Should Be Equal As Numbers    ${last_key_press_text}    97
            Key Up    A
            ${last_key_up}=         Find Object     NAME            LastKeyUpValue
            ${last_key_up_text}=    Get Text        ${last_key_up}
            Should Be Equal As Numbers    ${last_key_up_text}       97

```

## KeyUp

Simulates a key up.

**_Parameters_**

```eval_rst
.. list-table:: KeyUp Parameters
   :widths: 15 15 10 60
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``keyCode``
     - AltKeyCode
     - Yes
     - The keyCode of the key simulated to be released.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestKeyDownAndKeyUp()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");
            AltKeyCode kcode = AltKeyCode.A;

            altDriver.keyDown(kcode, 1);
            var lastKeyDown = altDriver.FindObject(By.NAME, "LastKeyDownValue");
            var lastKeyPress = altDriver.FindObject(By.NAME, "LastKeyPressedValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyDown.GetText(), true));
            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyPress.GetText(), true));

            altDriver.keyUp(kcode);
            var lastKeyUp = altDriver.FindObject(By.NAME, "LastKeyUpValue");

            Assert.AreEqual((int)kcode, (int)Enum.Parse(typeof(AltKeyCode), lastKeyUp.GetText(), true));
        }

    .. code-tab:: java

        @Test
        public void TestKeyDownAndKeyUp() throws Exception {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyDownValue").build();
            AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyUpValue").build();
            AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "LastKeyPressedValue").build();
            AltKeyCode kcode = AltKeyCode.A;
            AltKeyParams altKeyParams = new AltKeyParams.Builder(kcode).build();

            altDriver.keyDown(altKeyParams);
            Thread.sleep(2000);
            AltObject lastKeyDown = altDriver.findObject(altFindObjectsParameters1);
            AltObject lastKeyPress = altDriver.findObject(altFindObjectsParameters3);
            assertEquals("A", AltKeyCode.valueOf(lastKeyDown.getText()).name());
            assertEquals("A", AltKeyCode.valueOf(lastKeyPress.getText()).name());

            altDriver.keyUp(kcode);
            Thread.sleep(2000);
            AltObject lastKeyUp = altDriver.findObject(altFindObjectsParameters2);
            assertEquals("A", AltKeyCode.valueOf(lastKeyUp.getText()).name());
        }

    .. code-tab:: py

        def test_key_down_and_key_up(self):
            self.alt_driver.load_scene('Scene 5 Keyboard Input')

            self.alt_driver.key_down(AltKeyCode.A)
            time.sleep(5)
            lastKeyDown = self.alt_driver.find_object(By.NAME, 'LastKeyDownValue')
            lastKeyPress = self.alt_driver.find_object(By.NAME, 'LastKeyPressedValue')

            self.assertEqual("A", lastKeyDown.get_text())
            self.assertEqual("A", lastKeyPress.get_text())

            self.alt_driver.key_up(AltKeyCode.A)
            time.sleep(5)
            lastKeyUp = self.alt_driver.find_object(By.NAME, 'LastKeyUpValue')
            self.assertEqual("A", lastKeyUp.get_text())

    .. code-tab:: robot

        Test Key Down And Key Up
            Load Scene              ${scene5}
            Key Down    A
            ${last_key_down}=       Find Object     NAME            LastKeyDownValue
            ${last_key_press}=      Find Object     NAME            LastKeyPressedValue
            ${last_key_down_text}=      Get Text    ${last_key_down}
            ${last_key_press_text}=     Get Text    ${last_key_press}
            Should Be Equal As Numbers    ${last_key_down_text}     97
            Should Be Equal As Numbers    ${last_key_press_text}    97
            Key Up    A
            ${last_key_up}=         Find Object     NAME            LastKeyUpValue
            ${last_key_up_text}=    Get Text        ${last_key_up}
            Should Be Equal As Numbers    ${last_key_up_text}       97

```

## HoldButton

Simulates holding left click button down for a specified amount of time at given coordinates.

**_Parameters_**

```eval_rst
.. list-table:: HoldButton Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``coordinates``
     - AltVector2
       *Python, Robot:* list/tuple/dict
     - Yes
     - 
     - The coordinates where the button is held down.
   * - ``duration``
     - float
     - No
     - 0.1
     - The time measured in seconds to keep the button down.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestHoldButton()
        {
            var button = altDriver.FindObject(By.NAME, "UIButton");
            altDriver.HoldButton(button.GetScreenPosition(), 1);
            var capsuleInfo = altDriver.FindObject(By.NAME, "CapsuleInfo");
            var text = capsuleInfo.GetText();
            Assert.AreEqual(text, "UIButton clicked to jump capsule!");
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
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene")
            button = self.alt_driver.find_object(By.NAME, "UIButton")
            self.alt_driver.hold_button(button.get_screen_position(), 1)

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

## MoveMouse

Simulate mouse movement in your app.

**_Parameters_**

```eval_rst
.. list-table:: MoveMouse Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``coordinates``
     - AltVector2
       *Python, Robot:* list/tuple/dict
     - Yes
     - 
     - The screen coordinates.
   * - ``duration``
     - float
     - No
     - 0.1
     - The time measured in seconds to move the mouse from the current mouse position to the set coordinates.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestCreatingStars()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");

            var stars = altDriver.FindObjectsWhichContain(By.NAME, "Star", cameraValue: "Player2");
            var pressingpoint1 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint1", cameraValue: "Player2");
            Assert.AreEqual(1, stars.Count);

            altDriver.MoveMouse(new AltVector2(pressingpoint1.x, pressingpoint1.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            var pressingpoint2 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint2", cameraValue: "Player2");
            altDriver.MoveMouse(new AltVector2(pressingpoint2.x, pressingpoint2.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            stars = altDriver.FindObjectsWhichContain(By.NAME, "Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java

        @Test
        public void TestCreatingStars2() throws InterruptedException {
            AltObject[] stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(1, stars.length);

            AltObject pressingPoint1 = altDriver.findObject(new AltFindObjectsParams.Builder(By.NAME, "PressingPoint1").withCamera(By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint1.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            AltObject pressingPoint2 = altDriver.findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME, "PressingPoint2").withCamera(AltDriver.By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint2.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(3, stars.length);
        }

    .. code-tab:: py

        def test_creating_stars(self):
            self.alt_driver.load_scene("Scene 5 Keyboard Input")
            stars = self.alt_driver.find_objects_which_contain(By.NAME, "Star", By.NAME, "Player2")
            assert len(stars) == 1

            self.alt_driver.find_objects_which_contain(By.NAME, "Player", By.NAME, "Player2")
            pressing_point_1 = self.alt_driver.find_object(By.NAME, "PressingPoint1", By.NAME, "Player2")

            self.alt_driver.move_mouse(pressing_point_1.get_screen_position(), duration=1)
            self.alt_driver.press_key(AltKeyCode.Mouse0, 1, 1)
            pressing_point_2 = self.alt_driver.find_object(By.NAME, "PressingPoint2", By.NAME, "Player2")
            self.alt_driver.move_mouse(pressing_point_2.get_screen_position(), duration=1)
            self.alt_driver.press_key(AltKeyCode.Mouse0, power=1, duration=1)

            stars = self.alt_driver.find_objects_which_contain(By.NAME, "Star")
            assert len(stars) == 3

    .. code-tab:: robot

        Test Creating Stars
            ${stars}=    Find Objects Which Contain    NAME    Star    camera_by=NAME    camera_value=Player2
            ${appears}=    Get Length    ${stars}
            Should Be Equal As Integers    1    ${appears}
            Find Objects Which Contain    NAME    Player    camera_by=NAME    camera_value=Player2
            ${pressing_point_1}=    Find Object    NAME    PressingPoint1    camera_by=NAME    camera_value=Player2
            ${pressing_point_1_coordinates}=    Get Screen Position    ${pressing_point_1}
            Move Mouse    ${pressing_point_1_coordinates}    duration=0.1    wait=${False}
            Sleep    0.1
            Press Key    Mouse0    power=1    duration=0.1    wait=${False}
            ${pressing_point_2}=    Find Object    NAME    PressingPoint2    camera_by=NAME    camera_value=Player2
            ${pressing_point_2_coordinates}=    Get Screen Position    ${pressing_point_1}
            Move Mouse    ${pressing_point_2_coordinates}    duration=0.1    wait=${False}
            Press Key    Mouse0    power=1    duration=0.1    wait=${False}
            Sleep    0.1
            ${stars}=    Find Objects Which Contain    NAME    Star
            ${appears}=    Get Length    ${stars}
            Should Be Equal As Integers    3    ${appears}

```

## PressKey

Simulates key press action in your app.

**_Parameters_**

```eval_rst
.. list-table:: PressKey Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``keycode``
     - AltKeyCode
     - Yes
     - 
     - The key code of the key simulated to be pressed.
   * - ``power``
     - float
     - No
     - 1
     - A value between [-1,1] used for joysticks to indicate how hard the button was pressed.
   * - ``duration``
     - float
     - No
     - 0.1
     - The time measured in seconds from the key press to the key release.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::
    .. code-tab:: c#

        [Test]
        public void TestCreatingStars()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");

            var stars = altDriver.FindObjectsWhichContain(By.NAME, "Star", cameraValue: "Player2");
            var pressingpoint1 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint1", cameraValue: "Player2");
            Assert.AreEqual(1, stars.Count);

            altDriver.MoveMouse(new AltVector2(pressingpoint1.x, pressingpoint1.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            var pressingpoint2 = altDriver.FindObjectWhichContains(By.NAME, "PressingPoint2", cameraValue: "Player2");
            altDriver.MoveMouse(new AltVector2(pressingpoint2.x, pressingpoint2.y), 1);
            altDriver.PressKey(AltKeyCode.Mouse0, 0.1f);

            stars = altDriver.FindObjectsWhichContain(By.NAME, "Star");
            Assert.AreEqual(3, stars.Count);
        }

    .. code-tab:: java

        @Test
        public void TestCreatingStars2() throws InterruptedException {
            AltObject[] stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(1, stars.length);

            AltObject pressingPoint1 = altDriver.findObject(new AltFindObjectsParams.Builder(By.NAME, "PressingPoint1").withCamera(By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint1.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            AltObject pressingPoint2 = altDriver.findObject(new AltFindObjectsParams.Builder(AltDriver.By.NAME, "PressingPoint2").withCamera(AltDriver.By.NAME, "Player2").build());
            altDriver.moveMouse(new AltMoveMouseParams.Builder(pressingPoint2.getScreenPosition()).build());
            altDriver.pressKey(new AltPressKeyParams.Builder(AltKeyCode.Mouse0).build());

            stars = altDriver.findObjectsWhichContain(new AltFindObjectsParams.Builder(By.NAME, "Star").build());
            assertEquals(3, stars.length);
        }

    .. code-tab:: py

        def test_creating_stars(self):
            self.alt_driver.load_scene("Scene 5 Keyboard Input")
            stars = self.alt_driver.find_objects_which_contain(By.NAME, "Star", By.NAME, "Player2")
            assert len(stars) == 1

            self.alt_driver.find_objects_which_contain(By.NAME, "Player", By.NAME, "Player2")
            pressing_point_1 = self.alt_driver.find_object(By.NAME, "PressingPoint1", By.NAME, "Player2")

            self.alt_driver.move_mouse(pressing_point_1.get_screen_position(), duration=1)
            self.alt_driver.press_key(AltKeyCode.Mouse0, 1, 1)
            pressing_point_2 = self.alt_driver.find_object(By.NAME, "PressingPoint2", By.NAME, "Player2")
            self.alt_driver.move_mouse(pressing_point_2.get_screen_position(), duration=1)
            self.alt_driver.press_key(AltKeyCode.Mouse0, power=1, duration=1)

            stars = self.alt_driver.find_objects_which_contain(By.NAME, "Star")
            assert len(stars) == 3

    .. code-tab:: robot

        Test Creating Stars
            ${stars}=    Find Objects Which Contain    NAME    Star    camera_by=NAME    camera_value=Player2
            ${appears}=    Get Length    ${stars}
            Should Be Equal As Integers    1    ${appears}
            Find Objects Which Contain    NAME    Player    camera_by=NAME    camera_value=Player2
            ${pressing_point_1}=    Find Object    NAME    PressingPoint1    camera_by=NAME    camera_value=Player2
            ${pressing_point_1_coordinates}=    Get Screen Position    ${pressing_point_1}
            Move Mouse    ${pressing_point_1_coordinates}    duration=0.1    wait=${False}
            Sleep    0.1
            Press Key    Mouse0    power=1    duration=0.1    wait=${False}
            ${pressing_point_2}=    Find Object    NAME    PressingPoint2    camera_by=NAME    camera_value=Player2
            ${pressing_point_2_coordinates}=    Get Screen Position    ${pressing_point_1}
            Move Mouse    ${pressing_point_2_coordinates}    duration=0.1    wait=${False}
            Press Key    Mouse0    power=1    duration=0.1    wait=${False}
            Sleep    0.1
            ${stars}=    Find Objects Which Contain    NAME    Star
            ${appears}=    Get Length    ${stars}
            Should Be Equal As Integers    3    ${appears}

```

## PressKeys

Simulates multiple key press action in your app.

**_Parameters_**

```eval_rst
.. list-table:: PressKeys Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``keycodes``
     - List[AltKeyCode]
       *Java:* List<AltKeyCode>
       *Python, Robot:* list[AltKeyCode]
     - Yes
     - 
     - The list of keycodes simulated to be pressed simultaneously.
   * - ``power``
     - float
     - No
     - 1
     - A value between [-1,1] used for joysticks to indicate how hard the buttons were pressed.
   * - ``duration``
     - float
     - No
     - 0.1
     - The time measured in seconds from the multiple key press to the multiple key release.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::
    .. code-tab:: c#

        [Test]
        public void TestPressKeys()
        {
            AltKeyCode[] keys = { AltKeyCode.K, AltKeyCode.L };
            altDriver.PressKeys(keys);
            var altObject = altDriver.FindObject(By.NAME, "Capsule");
            var finalPropertyValue = altObject.GetComponentProperty<string>("AltExampleScriptCapsule", "stringToSetFromTests", "Assembly-CSharp");
            Assert.AreEqual("multiple keys pressed", finalPropertyValue);
        }

    .. code-tab:: java

        @Test
        public void testPressKeys()
        {
            AltKeyCode[] keys = {AltKeyCode.K, AltKeyCode.L};

            altDriver.pressKeys(new AltPressKeysParams.Builder(keys).build());

            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Capsule").build();
            AltObject altObject = altDriver.findObject(altFindObjectsParams);

            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule",
                "stringToSetFromTests", "Assembly-CSharp").build();
            String finalPropertyValue = altObject.getComponentProperty(altGetComponentPropertyParams, String.class);

            assertEquals(finalPropertyValue, "multiple keys pressed");
        }

    .. code-tab:: py

        def test_press_keys(self):
            keys = [AltKeyCode.K, AltKeyCode.L]
            self.alt_driver.press_keys(keys)

            alt_unity_object = self.alt_driver.find_object(By.NAME, "Capsule")
            property_value = alt_unity_object.get_component_property(
                "AltExampleScriptCapsule",
                "stringToSetFromTests",
                "Assembly-CSharp"
            )
            assert property_value == "multiple keys pressed"

    .. code-tab:: robot

        Test Press Keys
            ${keys}=    Create List    K    L
            Press Keys    ${keys}
            ${alt_object}=    Find Object    NAME    Capsule
            ${property_value}=    Get Component Property    ${alt_object}    AltExampleScriptCapsule    stringToSetFromTests    Assembly-CSharp
            Should Be Equal As Strings    ${property_value}    multiple keys pressed

```

## Scroll

Simulate scroll action in your app.

**_Parameters_**

```eval_rst
.. list-table:: Scroll Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``speed``
     - float
     - No
     - 1
     - Set how fast to scroll. Positive values will scroll up and negative values will scroll down.
   * - ``duration``
     - float
     - No
     - 0.1
     - The duration of the scroll in seconds.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
   * - ``speedHorizontal``
     - float
     - No
     - 1
     - Set how fast to scroll right or left.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestScroll()
        {
            altDriver.LoadScene("Scene 5 Keyboard Input");
            var player2 = altDriver.FindObject(By.NAME, "Player2");
            AltVector3 cubeInitialPosition = new AltVector3(player2.worldX, player2.worldY, player2.worldY);
            altDriver.Scroll(4, 2);

            player2 = altDriver.FindObject(By.NAME, "Player2");
            AltVector3 cubeFinalPosition = new AltVector3(player2.worldX, player2.worldY, player2.worldY);
            Assert.AreNotEqual(cubeInitialPosition, cubeFinalPosition);
        }

    .. code-tab:: java

        @Test
        public void TestScroll() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParams = new AltFindObjectsParams.Builder(AltDriver.By.NAME,
                    "Player2").build();
            AltObject player2 = altDriver.findObject(altFindObjectsParams);
            Vector3 cubeInitialPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            altDriver.scroll(new AltScrollParams.Builder().withSpeed(4).withDuration(2).build());

            player2 = altDriver.findObject(altFindObjectsParams);
            Vector3 cubeFinalPosition = new Vector3(player2.worldX, player2.worldY, player2.worldY);
            assertNotEquals(cubeInitialPosition, cubeFinalPosition);
        }

    .. code-tab:: py

        def test_scroll(self):
            self.alt_driver.load_scene("Scene 5 Keyboard Input")
            player2 = self.alt_driver.find_object(By.NAME, "Player2")
            cube_initial_position = [player2.worldX, player2.worldY, player2.worldY]
            self.alt_driver.scroll(4, 2)

            player2 = self.alt_driver.find_object(By.NAME, "Player2")
            cubeFinalPosition = [player2.worldX, player2.worldY, player2.worldY]
            assert cube_initial_position != cubeFinalPosition

    .. code-tab:: robot

        Test Scroll
            ${player2}=    Find Object    NAME    Player2
            ${cube_initial_position_x}=    Get Object WorldX    ${player2}
            ${cube_initial_position_y}=    Get Object WorldY    ${player2}
            ${cube_initial_position}=    Create List    ${cube_initial_position_x}    ${cube_initial_position_y}    ${cube_initial_position_y}
            Scroll    4    duration=1    wait=${False}
            Sleep    1
            ${player2}=    Find Object    NAME    Player2
            ${cube_final_position_x}=    Get Object WorldX    ${player2}
            ${cube_final_position_y}=    Get Object WorldY    ${player2}
            ${cube_final_position}=    Create List    ${cube_final_position_x}    ${cube_final_position_y}    ${cube_final_position_y}
            Should Not Be Equal    ${cube_initial_position}    ${cube_final_position}

```

## Swipe

Simulates a swipe action between two points.

**_Parameters_**

```eval_rst
.. list-table:: Swipe Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``start``
     - AltVector2
       *Python, Robot:* list/tuple/dict
     - Yes
     - 
     - Starting location of the swipe.
   * - ``end``
     - AltVector2
       *Python, Robot:* list/tuple/dict
     - Yes
     - 
     - Ending location of the swipe.
   * - ``duration``
     - float
     - No
     - 0.1
     - The time measured in seconds to move the mouse from start to end location.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void MultipleDragAndDrop()
        {
            var altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            var altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image2");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box2");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image3");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);

            altElement1 = altDriver.FindObject(By.NAME, "Drag Image1");
            altElement2 = altDriver.FindObject(By.NAME, "Drop Box1");
            altDriver.Swipe(new AltVector2(altElement1.x, altElement1.y), new AltVector2(altElement2.x, altElement2.y), 1);
            var imageSource = altDriver.FindObject(By.NAME, "Drag Image1").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            var imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop Image").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource, imageSourceDropZone);

            imageSource = altDriver.FindObject(By.NAME, "Drag Image2").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            imageSourceDropZone = altDriver.FindObject(By.NAME, "Drop").GetComponentProperty("UnityEngine.UI.Image", "sprite", "UnityEngine.UI");
            Assert.AreNotEqual(imageSource, imageSourceDropZone);
        }

    .. code-tab:: java

        @Test
        public void testMultipleDragAndDrop() throws Exception {

            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Image1").build();
            AltFindObjectsParams altFindObjectsParameters2 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Box1").build();
            AltFindObjectsParams altFindObjectsParameters3 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Image2").build();
            AltFindObjectsParams altFindObjectsParameters4 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Image3").build();
            AltFindObjectsParams altFindObjectsParameters5 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Box2").build();
            AltFindObjectsParams altFindObjectsParameters6 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Image").build();
            AltFindObjectsParams altFindObjectsParameters7 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drop Image").build();

            AltObject altElement1 = altDriver.findObject(altFindObjectsParameters1);
            AltObject altElement2 = altDriver.findObject(altFindObjectsParameters2);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(2).build());

            altElement1 = altDriver.findObject(altFindObjectsParameters3);
            altElement2 = altDriver.findObject(altFindObjectsParameters5);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(2).build());

            altElement1 = altDriver.findObject(altFindObjectsParameters4);
            altElement2 = altDriver.findObject(altFindObjectsParameters2);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(3).build());

            altElement1 = altDriver.findObject(altFindObjectsParameters1);
            altElement2 = altDriver.findObject(altFindObjectsParameters2);
            altDriver
                    .swipe(new AltSwipeParams.Builder(altElement1.getScreenPosition(), altElement2.getScreenPosition())
                            .withDuration(1).build());
            AltSprite imageSource = altDriver.findObject(altFindObjectsParameters1)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);

            AltSprite imageSourceDropZone = altDriver.findObject(altFindObjectsParameters6)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);

            assertNotSame(imageSource, imageSourceDropZone);

            imageSource = altDriver.findObject(altFindObjectsParameters3)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);

            imageSourceDropZone = altDriver.findObject(altFindObjectsParameters7)
                    .getComponentProperty(new AltGetComponentPropertyParams.Builder("UnityEngine.UI.Image", "sprite", "UnityEngine.UI").build(), AltSprite.class);
            assertNotSame(imageSource, imageSourceDropZone);
        }

    .. code-tab:: py

        def test_multiple_swipes(self):
            self.alt_driver.load_scene("Scene 3 Drag And Drop")

            image2 = self.alt_driver.find_object(By.NAME, "Drag Image2")
            box2 = self.alt_driver.find_object(By.NAME, "Drop Box2")

            self.alt_driver.swipe(image2.get_screen_position(), box2.get_screen_position(), 2)

            image3 = self.alt_driver.find_object(By.NAME, "Drag Image3")
            box1 = self.alt_driver.find_object(By.NAME, "Drop Box1")

            self.alt_driver.swipe(image3.get_screen_position(), box1.get_screen_position(), 1)

            image1 = self.alt_driver.find_object(By.NAME, "Drag Image1")
            box1 = self.alt_driver.find_object(By.NAME, "Drop Box1")

            self.alt_driver.swipe(image1.get_screen_position(), box1.get_screen_position(), 3)

            image_source = image1.get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            image_source_drop_zone = self.alt_driver.find_object(
                By.NAME, "Drop Image").get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            assert image_source != image_source_drop_zone

            image_source = image2.get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            image_source_drop_zone = self.alt_driver.find_object(
                By.NAME, "Drop").get_component_property("UnityEngine.UI.Image", "sprite", "UnityEngine.UI")
            assert image_source != image_source_drop_zone

    .. code-tab:: robot

        Test Multiple Swipes
            ${drag_location}=    Find Object    NAME    Drag Image2
            ${drop_location}=    Find Object    NAME    Drop Box2
            ${drag_location_position}=    Get Screen Position    ${drag_location}
            ${drop_location_position}=    Get Screen Position    ${drop_location}
            Swipe    ${drag_location_position}    ${drop_location_position}    duration=1   wait=${False}

            ${drag_location}=    Find Object    NAME    Drag Image2
            ${drop_location}=    Find Object    NAME    Drop Box1
            ${drag_location_position}=    Get Screen Position    ${drag_location}
            ${drop_location_position}=    Get Screen Position    ${drop_location}
            Swipe    ${drag_location_position}    ${drop_location_position}    duration=1    wait=${False}

            ${drag_location}=    Find Object    NAME    Drag Image1
            ${drop_location}=    Find Object    NAME    Drop Box1
            ${drag_location_position}=    Get Screen Position    ${drag_location}
            ${drop_location_position}=    Get Screen Position    ${drop_location}

            Swipe    ${drag_location_position}    ${drop_location_position}    duration=2    wait=${False}

            Wait For Object To Not Be Present    NAME    icon
            ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image1    Drop Image
            Should Be Equal    ${image_source}    ${image_source_drop_zone}
            ${image_source}    ${image_source_drop_zone}=    Get Sprite Name    Drag Image2    Drop
            Should Be Equal    ${image_source}    ${image_source_drop_zone}

```

## MultipointSwipe

Simulates a multipoint swipe action.

**_Parameters_**

```eval_rst
.. list-table:: MultiPointSwipe Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``positions``
     - List[AltVector2]
       *Java:* List<AltVector2>
       *Python, Robot:* list of lists
     - Yes
     - 
     - A list of positions on the screen where the swipe will be made.
   * - ``duration``
     - float
     - No
     - 0.1
     - The time measured in seconds to swipe from the first position to the last position.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestResizePanelWithMultipointSwipe()
        {
            var altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position = new AltVector2(altElement.x, altElement.y);
            var pos = new[]
            {
                altElement.GetScreenPosition(),
                new AltVector2(altElement.x - 200, altElement.y - 200),
                new AltVector2(altElement.x - 300, altElement.y - 100),
                new AltVector2(altElement.x - 50, altElement.y - 100),
                new AltVector2(altElement.x - 100, altElement.y - 100)
            };
            altDriver.MultipointSwipe(pos, 4);

            altElement = altDriver.FindObject(By.NAME, "Resize Zone");
            var position2 = new AltVector2(altElement.x, altElement.y);
            Assert.AreNotEqual(position, position2);
        }

    .. code-tab:: java

        @Test
        public void testResizePanelWithMultipointSwipe() throws Exception {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Resize Zone").build();
            AltObject altElement = altDriver.findObject(altFindObjectsParameters1);

            List<Vector2> positions = Arrays.asList(altElement.getScreenPosition(),
                    new Vector2(altElement.x + 100, altElement.y + 100),
                    new Vector2(altElement.x + 100, altElement.y + 200));

            altDriver.multipointSwipe(new AltMultipointSwipeParams.Builder(positions).withDuration(3).build());

            AltObject altElementAfterResize = altDriver.findObject(altFindObjectsParameters1);
            assertNotSame(altElement.x, altElementAfterResize.x);
            assertNotSame(altElement.y, altElementAfterResize.y);
        }

    .. code-tab:: py

        def test_resize_panel_with_multipoint_swipe(self):
            self.alt_driver.load_scene("Scene 2 Draggable Panel")

            alt_unity_object = self.alt_driver.find_object(By.NAME, "Resize Zone")
            position_init = (alt_unity_object.x, alt_unity_object.y)

            positions = [
                alt_unity_object.get_screen_position(),
                [alt_unity_object.x - 200, alt_unity_object.y - 200],
                [alt_unity_object.x - 300, alt_unity_object.y - 100],
                [alt_unity_object.x - 50, alt_unity_object.y - 100],
                [alt_unity_object.x - 100, alt_unity_object.y - 100]
            ]
            self.alt_driver.multipoint_swipe(positions, duration=4)

            alt_unity_object = self.alt_driver.find_object(By.NAME, "Resize Zone")
            position_final = (alt_unity_object.x, alt_unity_object.y)

            assert position_init != position_final

    .. code-tab:: robot

        Test Resize Panel With Multipoint Swipe
            ${alt_object}=    Find Object    NAME    Resize Zone
            ${alt_object_x}=    Get Object X    ${alt_object}
            ${alt_object_y}=    Get Object Y    ${alt_object}
            ${position_init}=    Create List    ${alt_object_x}    ${alt_object_y}
            ${screen_position}=    Get Screen Position    ${alt_object}
            ${positions}=    Create List    ${screen_position}
            ${new_x}=    Evaluate    ${alt_object_x}-200
            ${new_y}=    Evaluate    ${alt_object_y}-200
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Append To List    ${positions}    ${new_screen_position}
            ${new_x}=    Evaluate    ${alt_object_x}-300
            ${new_y}=    Evaluate    ${alt_object_y}-100
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Append To List    ${positions}    ${new_screen_position}
            ${new_x}=    Evaluate    ${alt_object_x}-50
            ${new_y}=    Evaluate    ${alt_object_y}-100
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Append To List    ${positions}    ${new_screen_position}
            ${new_x}=    Evaluate    ${alt_object_x}-100
            ${new_y}=    Evaluate    ${alt_object_y}-100
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Append To List    ${positions}    ${new_screen_position}
            Multipoint Swipe    ${positions}    duration=4
            ${alt_object}=    Find Object    NAME    Resize Zone
            ${alt_object_x}=    Get Object X    ${alt_object}
            ${alt_object_y}=    Get Object Y    ${alt_object}
            ${position_final}=    Create List    ${alt_object_x}    ${alt_object_y}
            Should Not Be Equal    ${position_init}    ${position_final}

```

## BeginTouch

Simulates starting of a touch on the screen. To further interact with the touch use [MoveTouch](#movetouch) and [EndTouch](#endtouch)

**_Parameters_**

```eval_rst
.. list-table:: BeginTouch Parameters
   :widths: 15 15 10 65
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``coordinates``
     - AltVector2
       *Python, Robot:* list/tuple/dict
     - Yes
     - Screen coordinates.
```

**_Returns_**

- int - the fingerId.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.GetScreenPosition());

        }

    .. code-tab:: java

        @Test
        public void testNewTouchCommands() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Zone").build();
            AltObject draggableArea = altDriver.findObject(altFindObjectsParameters1);
            Vector2 initialPosition = draggableArea.getScreenPosition();
            int fingerId = altDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
            Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
            altDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
            draggableArea = altDriver.findObject(altFindObjectsParameters1);
            assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
            assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
        }

    .. code-tab:: py

        def test_new_touch_commands(self):
            self.alt_driver.load_scene('Scene 2 Draggable Panel')
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.alt_driver.begin_touch(draggable_area.get_screen_position())
            self.alt_driver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.alt_driver.end_touch(finger_id)
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

    .. code-tab:: robot

        Test New Touch Commands
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${initial_position}=    Get Screen Position    ${draggable_area}
            ${finger_id}=    Begin Touch    ${initial_position}
            ${draggable_area_x}=    Get Object X    ${draggable_area}
            ${draggable_area_y}=    Get Object Y    ${draggable_area}
            ${new_x}=    Evaluate    ${draggable_area_x}+10
            ${new_y}=    Evaluate    ${draggable_area_y}+10
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Move Touch    ${finger_id}    ${new_screen_position}
            End Touch    ${finger_id}
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${final_position}=    Get Screen Position    ${draggable_area}
            Should Not Be Equal    ${initial_position}    ${final_position}

```

## MoveTouch

Simulates a touch movement on the screen. Move the touch created with [BeginTouch](#begintouch) from the previous position to the position given as parameters.

**_Parameters_**

```eval_rst
.. list-table:: MoveTouch Parameters
   :widths: 15 15 10 65
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``fingerId``
     - int
     - Yes
     - Identifier returned by `BeginTouch <#begintouch>`_ command.
   * - ``coordinates``
     - AltVector2
       *Python, Robot:* list/tuple/dict
     - Yes
     - Screen coordinates where the touch will be moved.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.GetScreenPosition());

        }

    .. code-tab:: java

        @Test
        public void testNewTouchCommands() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Zone").build();
            AltObject draggableArea = altDriver.findObject(altFindObjectsParameters1);
            Vector2 initialPosition = draggableArea.getScreenPosition();
            int fingerId = altDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
            Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
            altDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
            draggableArea = altDriver.findObject(altFindObjectsParameters1);
            assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
            assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
        }

    .. code-tab:: py

        def test_new_touch_commands(self):
            self.alt_driver.load_scene('Scene 2 Draggable Panel')
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.alt_driver.begin_touch(draggable_area.get_screen_position())
            self.alt_driver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.alt_driver.end_touch(finger_id)
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

    .. code-tab:: robot

        Test New Touch Commands
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${initial_position}=    Get Screen Position    ${draggable_area}
            ${finger_id}=    Begin Touch    ${initial_position}
            ${draggable_area_x}=    Get Object X    ${draggable_area}
            ${draggable_area_y}=    Get Object Y    ${draggable_area}
            ${new_x}=    Evaluate    ${draggable_area_x}+10
            ${new_y}=    Evaluate    ${draggable_area_y}+10
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Move Touch    ${finger_id}    ${new_screen_position}
            End Touch    ${finger_id}
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${final_position}=    Get Screen Position    ${draggable_area}
            Should Not Be Equal    ${initial_position}    ${final_position}        

```

## EndTouch

Simulates ending of a touch on the screen. This command will destroy the touch making it no longer usable to other movements.

**_Parameters_**

```eval_rst
.. list-table:: EndTouch Parameters
   :widths: 15 15 10 65
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``fingerId``
     - int
     - Yes
     - Identifier returned by `BeginTouch <#begintouch>`_ command.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestNewTouchCommands()
        {
            var draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            var initialPosition = draggableArea.GetScreenPosition();
            int fingerId = altDriver.BeginTouch(draggableArea.GetScreenPosition());
            AltVector2 newPosition = new AltVector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.MoveTouch(fingerId, newPosition);
            altDriver.EndTouch(fingerId);
            draggableArea = altDriver.FindObject(By.NAME, "Drag Zone");
            Assert.AreNotEqual(initialPosition, draggableArea.GetScreenPosition());
        }

    .. code-tab:: java

        @Test
        public void testNewTouchCommands() throws InterruptedException {
            AltFindObjectsParams altFindObjectsParameters1 = new AltFindObjectsParams.Builder(
                    AltDriver.By.NAME, "Drag Zone").build();
            AltObject draggableArea = altDriver.findObject(altFindObjectsParameters1);
            Vector2 initialPosition = draggableArea.getScreenPosition();
            int fingerId = altDriver.beginTouch(new AltBeginTouchParams.Builder(initialPosition).build());
            Vector2 newPosition = new Vector2(draggableArea.x + 20, draggableArea.y + 10);
            altDriver.moveTouch(new AltMoveTouchParams.Builder(fingerId, newPosition).build());
            altDriver.endTouch(new AltEndTouchParams.Builder(fingerId).build());
            draggableArea = altDriver.findObject(altFindObjectsParameters1);
            assertNotEquals(initialPosition.x, draggableArea.getScreenPosition().x);
            assertNotEquals(initialPosition.y, draggableArea.getScreenPosition().y);
        }

    .. code-tab:: py

        def test_new_touch_commands(self):
            self.alt_driver.load_scene('Scene 2 Draggable Panel')
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            initial_position = draggable_area.get_screen_position()
            finger_id = self.alt_driver.begin_touch(draggable_area.get_screen_position())
            self.alt_driver.move_touch(finger_id, [int(draggable_area.x) + 10, int(draggable_area.y) + 10])
            self.alt_driver.end_touch(finger_id)
            draggable_area = self.alt_driver.find_object(By.NAME, 'Drag Zone')
            self.assertNotEqual(initial_position, draggable_area)

    .. code-tab:: robot

        Test New Touch Commands
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${initial_position}=    Get Screen Position    ${draggable_area}
            ${finger_id}=    Begin Touch    ${initial_position}
            ${draggable_area_x}=    Get Object X    ${draggable_area}
            ${draggable_area_y}=    Get Object Y    ${draggable_area}
            ${new_x}=    Evaluate    ${draggable_area_x}+10
            ${new_y}=    Evaluate    ${draggable_area_y}+10
            ${new_screen_position}=    Create List    ${new_x}    ${new_y}
            Move Touch    ${finger_id}    ${new_screen_position}
            End Touch    ${finger_id}
            ${draggable_area}=    Find Object    NAME    Drag Zone
            ${final_position}=    Get Screen Position    ${draggable_area}
            Should Not Be Equal    ${initial_position}    ${final_position}

```

## Click

Click at screen coordinates.

**_Parameters_**

```eval_rst
.. list-table:: Click Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``coordinates``
     - AltVector2
       *Python, Robot:* list/tuple/dict
     - Yes
     - 
     - The screen coordinates.
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
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestClickCoordinates()
        {
            const string name = "UIButton";
            var altObject = altDriver.FindObject(By.NAME,name);
            altDriver.Click(altObject.GetScreenPosition());
            Assert.AreEqual(name, altObject.name);
            altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text="UIButton clicked to jump capsule!"]");
        }

    .. code-tab:: java

        @Test()
        public void TestTapCoordinates() {
            AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                    .build();
            AltObject capsule = altDriver.findObject(findCapsuleParams);
            AltTapClickCoordinatesParams clickParams = new AltTapClickCoordinatesParams.Builder(
                    capsule.getScreenPosition()).build();
            altDriver.click(clickParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_tap_coordinates(self):
            capsule_element = self.alt_driver.find_object(By.NAME, 'Capsule')
            self.alt_driver.click(capsule_element.get_screen_position())

    .. code-tab:: robot

        Test Click Coordinates
            ${capsule_element}=    Find Object    NAME    Capsule
            ${capsule_element_positions}=    Get Screen Position    ${capsule_element}
            Click    ${capsule_element_positions}
            Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

```

## Tap

Tap at screen coordinates.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: Tap Parameters
           :widths: 15 15 10 10 50
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Default
             - Description
           * - coordinates
             - AltVector2
             - Yes
             - (empty)
             - The screen coordinates.
           * - count
             - int
             - No
             - 1
             - Number of taps.
           * - interval
             - float
             - No
             - 0.1
             - Interval between taps in seconds.
           * - wait
             - boolean
             - No
             - true
             - If set, waits for the command to finish.

    .. tab:: Java

        .. list-table:: tap Parameters
           :widths: 15 15 10 10 50
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Default
             - Description
           * - coordinates
             - AltVector2
             - Yes
             - (empty)
             - The screen coordinates.
           * - count
             - int
             - No
             - 1
             - Number of taps.
           * - interval
             - float
             - No
             - 0.1
             - Interval between taps in seconds.
           * - wait
             - boolean
             - No
             - true
             - If set, waits for the command to finish.

    .. tab:: Python

        .. list-table:: tap Parameters
           :widths: 15 15 10 10 50
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Default
             - Description
           * - coordinates
             - list/tuple/dict
             - Yes
             - 
             - The screen coordinates.
           * - count
             - int
             - No
             - 1
             - Number of taps.
           * - interval
             - float
             - No
             - 0.1
             - Interval between taps in seconds.
           * - wait
             - boolean
             - No
             - true
             - If set, waits for the command to finish.

    .. tab:: Robot

        .. list-table:: Tap Parameters
           :widths: 15 15 10 10 50
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Default
             - Description
           * - coordinates
             - list/tuple/dict
             - Yes
             - 
             - The screen coordinates.
           * - count
             - int
             - No
             - 1
             - Number of taps.
           * - interval
             - float
             - No
             - 0.1
             - Interval between taps in seconds.
           * - wait
             - boolean
             - No
             - true
             - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestTapCoordinates()
        {
            const string name = "UIButton";
            var altObject = altDriver.FindObject(By.NAME,name);
            altDriver.Tap(altObject.GetScreenPosition());
            Assert.AreEqual(name, altObject.name);
            altDriver.WaitForObject(By.PATH, "//CapsuleInfo[@text="UIButton clicked to jump capsule!"]");
        }

    .. code-tab:: java

        @Test()
        public void TestTapCoordinates() {
            AltFindObjectsParams findCapsuleParams = new AltFindObjectsParams.Builder(By.NAME, "Capsule")
                    .build();
            AltObject capsule = altDriver.findObject(findCapsuleParams);
            AltTapClickCoordinatesParams tapParams = new AltTapClickCoordinatesParams.Builder(
                    capsule.getScreenPosition()).build();
            altDriver.tap(tapParams);

            AltFindObjectsParams findCapsuleInfoParams = new AltFindObjectsParams.Builder(By.PATH,
                    "//CapsuleInfo[@text=Capsule was clicked to jump!]").build();
            AltWaitForObjectsParams waitParams = new AltWaitForObjectsParams.Builder(findCapsuleInfoParams)
                    .build();
            altDriver.waitForObject(waitParams);
        }

    .. code-tab:: py

        def test_tap_coordinates(self):
            capsule_element = self.alt_driver.find_object(By.NAME, 'Capsule')
            self.alt_driver.tap(capsule_element.get_screen_position())

    .. code-tab:: robot

        Test Tap Coordinates
            ${capsule_element}=    Find Object    NAME    Capsule
            ${capsule_element_positions}=    Get Screen Position    ${capsule_element}
            Tap    ${capsule_element_positions}
            Wait For Object    PATH    //CapsuleInfo[@text=Capsule was clicked to jump!]    timeout=1

```

## Tilt

Simulates device rotation action in your app.

**_Parameters_**

```eval_rst
.. list-table:: Tilt Parameters
   :widths: 15 15 10 10 50
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Default
     - Description
   * - ``acceleration``
     - AltVector3
       *Python, Robot:* list/tuple/dict
     - Yes
     - 
     - The linear acceleration of a device.
   * - ``duration``
     - float
     - No
     - 0.1
     - How long the rotation will take in seconds.
   * - ``wait``
     - boolean
     - No
     - true
     - If set, waits for the command to finish.
```

**_Returns_**

- Nothing

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

        Test Tilt
            ${cube}=    Find Object    NAME    Cube (1)
            ${initial_position}=    Get World Position    ${cube}
            ${acceleration}=    Create List    1000    10    10
            Tilt    ${acceleration}    duration=1
            ${final_position}=    Get World Position    ${cube}
            ${is_moved}=    Get Component Property    ${cube}    AltCubeNIS    isMoved    Assembly-CSharp
            Should Be True    ${is_moved}

```

## ResetInput

Clears all active input actions simulated by AltTester®.

**_Parameters_**

None

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

         [Test]
        public void TestResetInput()
        {
            altDriver.KeyDown(AltKeyCode.P, 1);
            Assert.True(altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<bool>("Altom.AltTester.NewInputSystem", "Keyboard.pKey.isPressed", "Assembly-CSharp"));
            altDriver.ResetInput();
            Assert.False(altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<bool>("Altom.AltTester.NewInputSystem", "Keyboard.pKey.isPressed", "Assembly-CSharp"));

            int countKeyDown = altDriver.FindObject(By.NAME, "AltTesterPrefab").GetComponentProperty<int>("Input", "_keyCodesPressed.Count", "Assembly-CSharp");
            Assert.AreEqual(0, countKeyDown);
        }

    .. code-tab:: java

           @Test
            public void testResetInput() throws InterruptedException {
                    AltFindObjectsParams prefab = new AltFindObjectsParams.Builder(
                                    AltDriver.By.NAME, "AltTesterPrefab").build();

                    AltGetComponentPropertyParams pIsPressed = new AltGetComponentPropertyParams.Builder(
                                    "Altom.AltTester.NewInputSystem",
                                    "Keyboard.pKey.isPressed", "Assembly-CSharp").build();
                    AltGetComponentPropertyParams count = new AltGetComponentPropertyParams.Builder(
                                    "Input",
                                    "_keyCodesPressed.Count", "Assembly-CSharp").build();
                    altDriver.keyDown(new AltKeyDownParams.Builder(AltKeyCode.P).build());
                    assertTrue(altDriver.findObject(prefab).getComponentProperty(pIsPressed, Boolean.class));
                    altDriver.resetInput();
                    assertFalse(altDriver.findObject(prefab).getComponentProperty(pIsPressed, Boolean.class));

                    int countKeyDown = altDriver.findObject(prefab).getComponentProperty(count, Integer.class);
                    assertEquals(0, countKeyDown);
            }

    .. code-tab:: py

        def test_reset_input(self):
            self.alt_driver.key_down(AltKeyCode.P, 1)
            assert True == self.alt_driver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
                "Altom.AltTester.NewInputSystem", "Keyboard.pKey.isPressed", "Assembly-CSharp")
            self.alt_driver.reset_input()
            assert False == self.alt_driver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
                "Altom.AltTester.NewInputSystem", "Keyboard.pKey.isPressed", "Assembly-CSharp")

            countKeyDown = self.alt_driver.find_object(By.NAME, "AltTesterPrefab").get_component_property(
                "Input", "_keyCodesPressed.Count", "Assembly-CSharp")
            assert 0 == countKeyDown

    .. code-tab:: robot

        Test Reset Input
            Key Down    P    power=1
            ${object}=    Find Object    NAME    AltTesterPrefab
            ${nis}=    Get Component Property    ${object}    AltTester.AltTesterUnitySDK.NewInputSystem    Keyboard.pKey.isPressed    AltTester.AltTesterUnitySDK
            Should Be True    ${nis}
            Reset Input
            ${nis}=    Get Component Property    ${object}    AltTester.AltTesterUnitySDK.NewInputSystem    Keyboard.pKey.isPressed    AltTester.AltTesterUnitySDK
            Should Not Be True    ${nis}
            ${countKeyDown}=    Find Object    NAME    AltTesterPrefab
            ${count}=    Get Component Property    ${countKeyDown}    Input    _keyCodesPressed.Count    AltTester.AltTesterUnitySDK
            Should Be Equal As Integers    0    ${count}
        
```
