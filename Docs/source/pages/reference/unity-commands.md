# Unity commands

## PlayerPrefKeyType

This is an enum type used for the **option** parameter in the [set_player_pref_key](#settingplayerprefs) command listed below and has the following values:

```eval_rst
.. list-table:: PlayerPrefKeyType Assigned Value
   :widths: 20 20
   :header-rows: 1

   * - Type
     - Assigned Value
   * - ``Int``
     - 1
   * - ``String``
     - 2
   * - ``Float``
     - 3
```

## GettingPlayerPrefs

```eval_rst
.. tabs::

    .. tab:: C#

        **GetIntKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - int

            .. literalinclude:: ../../_static/examples~/commands/csharp-player-pref-int.cs
                :language: C#
                :emphasize-lines: 6,11

        **GetFloatKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - float

            .. literalinclude:: ../../_static/examples~/commands/csharp-player-pref-float.cs
                :language: C#
                :emphasize-lines: 6,11

        **GetStringKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - string

            .. literalinclude:: ../../_static/examples~/commands/csharp-player-pref-string.cs
                :language: C#
                :emphasize-lines: 6,11

    .. tab:: Java

        **getFloatKeyPlayerPref**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - float

            .. literalinclude:: ../../_static/examples~/commands/java-player-pref-float.java
                :language: java
                :emphasize-lines: 6,11

        **getIntKeyPlayerPref**

        Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - int

            .. literalinclude:: ../../_static/examples~/commands/java-player-pref-int.java
                :language: java
                :emphasize-lines: 6,11

        **getStringKeyPlayerPref**

        Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - string

            .. literalinclude:: ../../_static/examples~/commands/java-player-pref-string.java
                :language: java
                :emphasize-lines: 6,11

    .. tab:: Python

        **get_player_pref_key**

            Returns the value for a given key from PlayerPrefs.

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - string/float/int

            .. literalinclude:: ../../_static/examples~/commands/python-player-prefs.py
                :language: py
                :emphasize-lines: 6,10

    .. tab:: Robot

        **Get Player Pref Key**

            Returns the value for a given key from PlayerPrefs

            *Parameters*

            +---------+---------+----------+---------------------+
            |  Name   |  Type   | Required |     Description     |
            +=========+=========+==========+=====================+
            | keyname |  string |    Yes   | Key to be retrieved |
            +---------+---------+----------+---------------------+

            *Returns*

            - string/float/int

            .. code-block:: 

                Test Set Player Pref Keys Int
                    Delete Player Pref
                    Set Player Pref Key    test    ${1}    Int
                    ${actual_value}=    Get Player Pref Key    test    Int
                    Should Be Equal As Integers    ${actual_value}    ${1}

                Test Set Player Pref Keys Float
                    Delete Player Pref
                    Set Player Pref Key    test    ${1.3}    Float
                    ${actual_value}=    Get Player Pref Key    test    Float
                    Should Be Equal As Numbers    ${actual_value}    ${1.3}

                Test Set Player Pref Keys String
                    Delete Player Pref
                    Set Player Pref Key    test    string value    String
                    ${actual_value}=    Get Player Pref Key    test    String
                    Should Be Equal As Strings    ${actual_value}    string value

```

## SettingPlayerPrefs

```eval_rst
.. tabs::

    .. tab:: C#

        **SetKeyPlayerPref**

            Sets the value for a given key in PlayerPrefs.

            *Parameters*

            +------------+-----------------------+-----------+----------------------------------+
            |    Name    |          Type         |  Required |           Description            |
            +============+=======================+===========+==================================+
            |   keyname  |         string        |    Yes    |        Key to be set             |
            +------------+-----------------------+-----------+----------------------------------+
            |   value    |  integer/float/string |    Yes    |        Value to be set           |
            +------------+-----------------------+-----------+----------------------------------+

            *Returns*

            - Nothing

            *Examples*

            .. literalinclude:: ../../_static/examples~/commands/csharp-player-pref-string.cs
                :language: C#
                :emphasize-lines: 5

    .. tab:: Java

        **setKeyPlayerPref**

            Sets the value for a given key in PlayerPrefs.

            *Parameters*

            +------------+-----------------------+-----------+----------------------------------+
            |    Name    |          Type         |  Required |           Description            |
            +============+=======================+===========+==================================+
            |   keyname  |         string        |    Yes    |        Key to be set             |
            +------------+-----------------------+-----------+----------------------------------+
            |   value    |  integer/float/string |    Yes    |        Value to be set           |
            +------------+-----------------------+-----------+----------------------------------+

            *Returns*

            - Nothing

            *Examples*

            .. literalinclude:: ../../_static/examples~/commands/java-player-pref-string.java
                :language: java
                :emphasize-lines: 5

    .. tab:: Python

        **set_player_pref_key**

            Sets the value for a given key in PlayerPrefs.

            *Parameters*

            +------------+-----------------------+-----------+----------------------------------+
            |    Name    |          Type         |  Required |           Description            |
            +============+=======================+===========+==================================+
            |   keyname  |         string        |    Yes    |        Key to be set             |
            +------------+-----------------------+-----------+----------------------------------+
            |   value    |  integer/float/string |    Yes    |        Value to be set           |
            +------------+-----------------------+-----------+----------------------------------+
            |   option   |    PlayerPrefKeyType  |    Yes    |         Type of keyname          |
            +------------+-----------------------+-----------+----------------------------------+

            *Returns*

            - Nothing

            *Examples*

            .. literalinclude:: ../../_static/examples~/commands/python-player-prefs.py
                :language: py
                :emphasize-lines: 4,5

    .. tab:: Robot

        **Set Player Pref Key**

            Sets the value for a given key in PlayerPrefs

            *Parameters*

            +------------+-----------------------+-----------+----------------------------------+
            |    Name    |          Type         |  Required |           Description            |
            +============+=======================+===========+==================================+
            |   keyname  |         string        |    Yes    |        Key to be set             |
            +------------+-----------------------+-----------+----------------------------------+
            |   value    |  integer/float/string |    Yes    |        Value to be set           |
            +------------+-----------------------+-----------+----------------------------------+
            |   option   |    PlayerPrefKeyType  |    Yes    |         Type of keyname          |
            +------------+-----------------------+-----------+----------------------------------+

            *Returns*

            - Nothing

            .. code-block:: 

                Test Set Player Pref Keys Int
                    Delete Player Pref
                    Set Player Pref Key    test    ${1}    Int
                    ${actual_value}=    Get Player Pref Key    test    Int
                    Should Be Equal As Integers    ${actual_value}    ${1}

                Test Set Player Pref Keys Float
                    Delete Player Pref
                    Set Player Pref Key    test    ${1.3}    Float
                    ${actual_value}=    Get Player Pref Key    test    Float
                    Should Be Equal As Numbers    ${actual_value}    ${1.3}

                Test Set Player Pref Keys String
                    Delete Player Pref
                    Set Player Pref Key    test    string value    String
                    ${actual_value}=    Get Player Pref Key    test    String
                    Should Be Equal As Strings    ${actual_value}    string value
                    
```

## DeleteKeyPlayerPref

Removes key and its corresponding value from PlayerPrefs.

**_Parameters_**

```eval_rst
.. list-table:: DeleteKeyPlayerPref Parameters
   :widths: 20 20 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``keyName``
     - string
     - Yes
     - Key to be deleted.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestDeleteKey()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", 1);
            var val = altDriver.GetIntKeyPlayerPref("test");
            Assert.AreEqual(1, val);
            altDriver.DeleteKeyPlayerPref("test");
            try
            {
                altDriver.GetIntKeyPlayerPref("test");
                Assert.Fail();
            }
            catch (NotFoundException exception)
            {
                Assert.AreEqual("notFound", exception.Message);
            }

        }

    .. code-tab:: java

        @Test
        public void testDeleteKey() throws Exception
        {
            altDriver.deletePlayerPref();
            altDriver.setKeyPlayerPref("test", 1);
            int val = altDriver.getIntKeyPlayerPref("test");
            assertEquals(1, val);
            altDriver.deleteKeyPlayerPref("test");
            try
            {
                altDriver.getIntKeyPlayerPref("test");
                fail();
            }
            catch (NotFoundException e)
            {
                assertEquals(e.getMessage(), "notFound");
            }
        }

    .. tab:: Python

        .. literalinclude:: ../../_static/examples~/commands/python-player-prefs.py
            :language: py
            :emphasize-lines: 8

    .. code-tab:: robot

        Test Delete Player Pref Key
            Delete Player Pref
            Set Player Pref Key           test                  1       String
            ${actual_value}=              Get Player Pref Key   test    String
            Should Be Equal As Strings    ${actual_value}       1
            Delete Player Pref Key        test

            Run Keyword And Expect Error    NotFoundException: PlayerPrefs key test not found    
            ...    Get Player Pref Key      test    String

```

## DeletePlayerPref

Removes all keys and values from PlayerPref.

**_Parameters_**

None

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestSetKeyInt()
        {
            altDriver.DeletePlayerPref();
            altDriver.SetKeyPlayerPref("test", 1);
            var val = altDriver.GetIntKeyPlayerPref("test");
            Assert.AreEqual(1, val);
        }

    .. code-tab:: java

        @Test
        public void testSetKeyFloat() throws Exception
        {
            altDriver.deletePlayerPref();
            altDriver.setKeyPlayerPref("test", 1f);
            float val = altDriver.getFloatKeyPlayerPref("test");
            assertEquals(1f, val, 0.01);
        }

    .. code-tab:: py

        def test_delete_key_player_pref(self):
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene")
            self.alt_driver.delete_player_prefs()
            self.alt_driver.set_player_pref_key("test", "1", PlayerPrefKeyType.String)
            val = self.alt_driver.get_player_pref_key("test", player_pref_key_type)
            self.assertEqual("1", str(val))

    .. code-tab:: robot

        Test Set Player Pref Keys Int
            Delete Player Pref
            Set Player Pref Key    test    ${1}    Int
            ${actual_value}=    Get Player Pref Key    test    Int
            Should Be Equal As Integers    ${actual_value}    ${1}

```

## GetCurrentScene

Returns the current active scene.

**_Parameters_**

None

**_Returns_**

- String

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetCurrentScene()
        {
            altDriver.LoadScene("Scene 1 AltDriverTestScene");
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetCurrentScene());
        }
    .. code-tab:: java

        @Test
        public void testGetCurrentScene() throws Exception
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
            assertEquals("Scene 1 AltDriverTestScene", altDriver.getCurrentScene());
        }

    .. code-tab:: py

        def test_get_current_scene(self):
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene", True)
            self.assertEqual("Scene 1 AltDriverTestScene",self.alt_driver.get_current_scene())

    .. code-tab:: robot

        Test Load And Wait For Scene
            Load Scene                      ${scene1}           ${True}
            Wait For Current Scene To Be    ${scene1}           timeout=1
            ${current_scene}=               Get Current Scene
            Should Be Equal                 ${current_scene}    ${scene1}

```

## LoadScene

Loads a scene.

**_Parameters_**

```eval_rst
.. list-table:: LoadScene Parameters
   :widths: 20 20 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``scene``
     - string
     - Yes
     - The name of the scene to be loaded.
   * - ``loadSingle``
     - bool
     - No
     - If set to false, the scene will be loaded additive, together with the current loaded scenes. Default value is true.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetCurrentScene()
        {
            altDriver.LoadScene("Scene 1 AltDriverTestScene",true);
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetCurrentScene());
        }

    .. code-tab:: java

        @Test
        public void testGetCurrentScene()
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
            assertEquals("Scene 1 AltDriverTestScene", altDriver.getCurrentScene());
        }

    .. code-tab:: py

        def test_get_current_scene(self):
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene", True)
            self.assertEqual("Scene 1 AltDriverTestScene",self.alt_driver.get_current_scene())

    .. code-tab:: robot

        Test Load And Wait For Scene
            Load Scene                      ${scene1}           ${True}
            Wait For Current Scene To Be    ${scene1}           timeout=1
            ${current_scene}=               Get Current Scene
            Should Be Equal                 ${current_scene}    ${scene1}

```

## UnloadScene

Unloads a scene.

**_Parameters_**

```eval_rst
.. list-table:: UnloadScene Parameters
   :widths: 20 20 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``scene``
     - string
     - Yes
     - Name of the scene to be unloaded.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestUnloadScene()
        {
            altDriver.LoadScene("Scene 2 Draggable Panel", false);
            Assert.AreEqual(2, altDriver.GetAllLoadedScenes().Count);
            altDriver.UnloadScene("Scene 2 Draggable Panel");
            Assert.AreEqual(1, altDriver.GetAllLoadedScenes().Count);
            Assert.AreEqual("Scene 1 AltDriverTestScene", altDriver.GetAllLoadedScenes()[0]);
        }

    .. code-tab:: java

        @Test
        public void TestUnloadScene() {
            AltLoadSceneParams altLoadSceneParams = new AltLoadSceneParams.Builder("Scene 2 Draggable Panel")
                    .loadSingle(false).build();
            altDriver.loadScene(altLoadSceneParams);
            assertEquals(2, altDriver.getAllLoadedScenes().length);
            altDriver.unloadScene(new AltUnloadSceneParams.Builder("Scene 2 Draggable Panel").build());
            assertEquals(1, altDriver.getAllLoadedScenes().length);
            assertEquals("Scene 1 AltDriverTestScene", altDriver.getAllLoadedScenes()[0]);
        }

    .. code-tab:: py

        def test_unload_scene(self):
            self.alt_driver.load_scene('Scene 1 AltDriverTestScene', True)
            self.alt_driver.load_scene('Scene 2 Draggable Panel', False)
            self.assertEqual(2, len(self.alt_driver.get_all_loaded_scenes()))
            self.alt_driver.unload_scene('Scene 2 Draggable Panel')
            self.assertEqual(1, len(self.alt_driver.get_all_loaded_scenes()))
            self.assertEqual("Scene 1 AltDriverTestScene",
                            self.alt_driver.get_all_loaded_scenes()[0])
    .. code-tab:: robot

        Test Unload Scene
            Load Scene    ${scene1}    load_single=${True}
            Load Scene    ${scene2}    load_single=${False}
            ${scenes}=    Get All Loaded Scenes
            ${scenes_number}=    Get Length    ${scenes}
            Should Be Equal As Integers    ${scenes_number}    2
            Unload Scene    ${scene2}
            ${scenes}=    Get All Loaded Scenes
            ${scenes_number}=    Get Length    ${scenes}
            Should Be Equal As Integers    ${scenes_number}    1
            ${scenes}=    Get All Loaded Scenes
            ${scene}=    Get From List    ${scenes}    0
            Should Be Equal As Strings    ${scene}    ${scene1}

```

## GetAllLoadedScenes

Returns all the scenes that have been loaded.

**_Parameters_**

None

**_Returns_**

- List of strings

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetAllLoadedScenes()
        {
            altDriver.LoadScene("Scene 1 AltDriverTestScene");
            System.Collections.Generic.List<string> loadedSceneNames = altDriver.GetAllLoadedScenes();
            Assert.AreEqual(loadedSceneNames.Count, 1);
            altDriver.LoadScene("Scene 2 Draggable Panel", false);
            altDriver.LoadScene("Scene 3 Drag And Drop", false);
            altDriver.LoadScene("Scene 4 No Cameras", false);
            altDriver.LoadScene("Scene 5 Keyboard Input", false);
            loadedSceneNames = altDriver.GetAllLoadedScenes();
            Assert.AreEqual(loadedSceneNames.Count, 5);
        }

    .. code-tab:: java

        @Test
        public void TestGetAllLoadedScenes()
        {
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 1 AltDriverTestScene").build());
            List<String> loadedSceneNames = altDriver.getAllLoadedScenes();
            assertEquals(loadedSceneNames.size(), 1);

            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 2 Draggable Panel").loadSingle(false).build());
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 3 Drag And Drop").loadSingle(false).build());
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 4 No Cameras").loadSingle(false).build());
            altDriver.loadScene(new AltLoadSceneParams.Builder("Scene 5 Keyboard Input").loadSingle(false).build());
            loadedSceneNames = altDriver.getAllLoadedScenes();
            assertEquals(loadedSceneNames.size(), 5);
        }

    .. code-tab:: py

        def test_get_all_loaded_scenes(self):
            self.alt_driver.load_scene("Scene 1 AltDriverTestScene")
            scenes_loaded = self.alt_driver.get_all_loaded_scenes()
            self.assertEqual(len(scenes_loaded), 1)
            self.alt_driver.load_scene("Scene 2 Draggable Panel", False)
            self.alt_driver.load_scene("Scene 3 Drag And Drop", False)
            self.alt_driver.load_scene("Scene 4 No Cameras", False)
            self.alt_driver.load_scene("Scene 5 Keyboard Input", False)
            scenes_loaded = self.alt_driver.get_all_loaded_scenes()
            self.assertEqual(len(scenes_loaded), 5)

    .. code-tab:: robot

        Test Load Additive Scenes
            Load Scene    ${scene1}    load_single=${True}
            ${initial_number_of_elements}=    Get All Elements
            Load Scene    ${scene2}    load_single=${False}
            ${final_number_of_elements}=    Get All Elements
            ${initial_number_of_elements_length}=    Get Length    ${initial_number_of_elements}
            ${final_number_of_elements_length}=    Get Length    ${final_number_of_elements}
            Should Be True    ${final_number_of_elements_length}>${initial_number_of_elements_length}
            ${all_loaded_scenes}=    Get All Loaded Scenes
            ${number_of_scenes}=    Get Length    ${all_loaded_scenes}
            Should Be Equal As Integers    ${number_of_scenes}    2

```

## WaitForCurrentSceneToBe

Waits for the scene to be loaded for a specified amount of time.

**_Parameters_**

```eval_rst
.. list-table:: WaitForCurrentSceneToBe Parameters
   :widths: 20 20 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``sceneName``
     - string
     - Yes
     - The name of the scene to wait for.
   * - ``timeout``
     - double
       *Python, Robot:* float
     - No
     - The time measured in seconds to wait for the specified scene.
   * - ``interval``
     - double
       *Python, Robot:* float
     - No
     - How often to check that the scene was loaded in the given timeout.
```

**_Returns_**

- None

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestWaitForCurrentSceneToBe()
        {
            const string name = "Scene 1 AltDriverTestScene";
            var timeStart = DateTime.Now;
            altDriver.WaitForCurrentSceneToBe(name);
            var timeEnd = DateTime.Now;
            var time = timeEnd - timeStart;
            Assert.Less(time.TotalSeconds, 20);
            var currentScene = altDriver.GetCurrentScene();
            Assert.AreEqual("Scene 1 AltDriverTestScene", currentScene);
        }

    .. code-tab:: java

        @Test
        public void testWaitForCurrentSceneToBe() {
            String name = "Scene 1 AltDriverTestScene";
            long timeStart = System.currentTimeMillis();
            AltWaitForCurrentSceneToBeParams params = new AltWaitForCurrentSceneToBeParams.Builder(name).build();
            altDriver.waitForCurrentSceneToBe(params);
            long timeEnd = System.currentTimeMillis();
            long time = timeEnd - timeStart;
            assertTrue(time / 1000 < 20);

            String currentScene = altDriver.getCurrentScene();
            assertEquals(name, currentScene);
        }

    .. code-tab:: py

        def test_wait_for_current_scene_to_be_with_a_non_existing_scene(self):
            scene_name = "Scene 0"

            with pytest.raises(exceptions.WaitTimeOutException) as execinfo:
            self.alt_driver.wait_for_current_scene_to_be(scene_name, timeout=1, interval=0.5)

            assert str(execinfo.value) == "Scene {} not loaded after 1 seconds".format(scene_name)

    .. code-tab:: robot

        Test Load And Wait For Scene
            Load Scene                      ${scene1}           ${True}
            Wait For Current Scene To Be    ${scene1}           timeout=1
            ${current_scene}=               Get Current Scene
            Should Be Equal                 ${current_scene}    ${scene1}

```

## GetApplicationScreenSize

Returns the value of the application screen size.

**_Parameters_**

None

**_Returns_**

- AltVector2

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetApplicationScreenSize()
        {
            altDriver.CallStaticMethod<string>("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule", new string[] { "1920", "1080", "true" }, new string[] { "System.Int32", "System.Int32", "System.Boolean" });
            var screensize = altDriver.GetApplicationScreenSize();
            Assert.AreEqual(1920, screensize.x);
            Assert.AreEqual(1080, screensize.y);
        }

    .. code-tab:: java

        @Test
        public void TestGetApplicationScreenSize() {
            AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder(
                "UnityEngine.Screen", "SetResolution",
                "UnityEngine.CoreModule", new Object[] { "1920", "1080", "True"})
                .withTypeOfParameters(new String[] { "System.Int32", "System.Int32","System.Boolean" })
                .build();
            altDriver.callStaticMethod(altCallStaticMethodParams,Void.class);
            int[] screensize = altDriver.getApplicationScreenSize();
            
            assertEquals(1920, screensize[0]);
            assertEquals(1080, screensize[1]);
        }

    .. code-tab:: py

        def test_get_application_screen_size(self):
            self.alt_driver.call_static_method("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule",
            parameters=["1920", "1080", "True"],
            type_of_parameters=["System.Int32", "System.Int32", "System.Boolean"],)
            screensize = self.alt_driver.get_application_screensize()

            assert 1920 == screensize[0]
            assert 1080 == screensize[1]

    .. code-tab:: robot

        Test Get Application Screen Size
            ${screen_size}=    Get Application Screensize
            Should Not Be Equal As Numbers    ${screen_size[0]}    0
            Should Not Be Equal As Numbers    ${screen_size[1]}    0

```

## GetTimeScale

Returns the value of the time scale.

**_Parameters_**

None

**_Returns_**

- float

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestTimeScale()
        {
            altDriver.SetTimeScale(0.1f);
            Thread.Sleep(1000);
            var timeScaleFromApp = altDriver.GetTimeScale();
            Assert.AreEqual(0.1f, timeScaleFromApp);
        }

    .. code-tab:: java

        @Test
        public void TestTimeScale() {
            altDriver.setTimeScale(new AltSetTimeScaleParams.Builder(0.1f).build());
            float timeScale = altDriver.getTimeScale();
            assertEquals(0.1f, timeScale, 0);
        }

    .. code-tab:: py

        def test_time_scale(self):
            self.alt_driver.set_time_scale(0.1)
            time.sleep(1)
            time_scale = self.alt_driver.get_time_scale()
            self.assertEqual(0.1, time_scale)

    .. code-tab:: robot

        Test Set And Get Time Scale
            Set Time Scale    0.1
            ${time_scale}=    Get Time Scale
            Should Be Equal As Numbers    ${time_scale}    0.1
            Set Time Scale    1

```

## SetTimeScale

Sets the value of the time scale.

**_Parameters_**

```eval_rst
.. list-table:: SetTimeScale Parameters
   :widths: 20 20 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``timeScale``
     - float
     - Yes
     - The value you want to set the time scale to.
```

**_Returns_**

- Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestTimeScale()
        {
            altDriver.SetTimeScale(0.1f);
            Thread.Sleep(1000);
            var timeScaleFromApp = altDriver.GetTimeScale();
            Assert.AreEqual(0.1f, timeScaleFromApp);
        }

    .. code-tab:: java

        @Test
        public void TestTimeScale() {
            altDriver.setTimeScale(new AltSetTimeScaleParams.Builder(0.1f).build());
            float timeScale = altDriver.getTimeScale();
            assertEquals(0.1f, timeScale, 0);
        }

    .. code-tab:: py

        def test_time_scale(self):
            self.alt_driver.set_time_scale(0.1)
            time.sleep(1)
            time_scale = self.alt_driver.get_time_scale()
            self.assertEqual(0.1, time_scale)

    .. code-tab:: robot

        Test Set And Get Time Scale
            Set Time Scale    0.1
            ${time_scale}=    Get Time Scale
            Should Be Equal As Numbers    ${time_scale}    0.1
            Set Time Scale    1

```

## CallStaticMethod

Invokes static methods from your app.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: CallStaticMethod Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - typeName
             - string
             - Yes
             - The name of the script. If the script has a namespace, the format should look like this: "namespace.typeName".
           * - methodName
             - string
             - Yes
             - The name of the public method to be called. If the method is inside a static property/field, use the format "propertyName.MethodName".
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the script.
           * - parameters
             - array
             - Yes
             - An array containing the serialized parameters to be sent to the component method.
           * - typeOfParameters
             - array
             - No
             - An array containing the serialized type of parameters to be sent to the component method.

    .. tab:: Java

        .. list-table:: callStaticMethod Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - typeName
             - string
             - Yes
             - The name of the script. If the script has a namespace, the format should look like this: "namespace.typeName".
           * - methodName
             - string
             - Yes
             - The name of the public method to be called. If the method is inside a static property/field, use the format "propertyName.MethodName".
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the script.
           * - parameters
             - array
             - Yes
             - An array containing the serialized parameters to be sent to the component method.
           * - typeOfParameters
             - array
             - No
             - An array containing the serialized type of parameters to be sent to the component method.

    .. tab:: Python

        .. list-table:: call_static_method Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - type_name
             - string
             - Yes
             - The name of the script. If the script has a namespace, the format should look like this: "namespace.type_name".
           * - method_name
             - string
             - Yes
             - The name of the public method to be called. If the method is inside a static property/field, use the format "property_name.method_name".
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the script.
           * - parameters
             - list
             - Yes
             - A list containing the serialized parameters to be sent to the component method.
           * - type_of_parameters
             - list
             - No
             - A list containing the serialized type of parameters to be sent to the component method.

    .. tab:: Robot

        .. list-table:: Call Static Method Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - type_name
             - string
             - Yes
             - The name of the script. If the script has a namespace, the format should look like this: "namespace.type_name".
           * - method_name
             - string
             - Yes
             - The name of the public method to be called. If the method is inside a static property/field, use the format "property_name.method_name".
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the script.
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
        public void TestCallStaticMethod()
        {

            altDriver.CallStaticMethod<string>("UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", new[] { "Test", "1" });
            int a = altDriver.CallStaticMethod<int>("UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", new[] { "Test", "2" });
            Assert.AreEqual(1, a);

        }

    .. code-tab:: java

        @Test
        public void TestCallStaticMethod() throws Exception
        {
            altDriver.callStaticMethod(new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs", "SetInt", "", new Object[] { "Test", "1" }).build(), String.class);
            int a = altDriver.callStaticMethod(new AltCallStaticMethodParams.Builder("UnityEngine.PlayerPrefs", "GetInt", "", new Object[] { "Test", "2" }).build(), Integer.class);
            assertEquals(1, a);
        }

    .. code-tab:: py

        def test_call_static_method(self):
            self.alt_driver.call_static_method("UnityEngine.PlayerPrefs", "SetInt", "UnityEngine.CoreModule", ["Test", "1"])
            a = int(self.alt_driver.call_static_method("UnityEngine.PlayerPrefs", "GetInt", "UnityEngine.CoreModule", ["Test", "2"]))
            self.assertEqual(1, a)

    .. code-tab:: robot

        Test Call Static Method
            ${list_to_set}=    Create List    Test    ${1}
            ${list_to_get}=    Create List    Test    ${2}
            Call Static Method    UnityEngine.PlayerPrefs    SetInt    UnityEngine.CoreModule    parameters=${list_to_set}
            ${value}=    Call Static Method    UnityEngine.PlayerPrefs    GetInt    UnityEngine.CoreModule    parameters=${list_to_get}
            Should Be Equal As Integers    ${value}    ${1}

```

## GetStaticProperty

Gets the value of the static field or property.

**_Parameters_**

```eval_rst
.. list-table:: GetStaticProperty Parameters
   :widths: 20 20 10 40
   :header-rows: 1

   * - Name
     - Type
     - Required
     - Description
   * - ``componentName``
     - string
     - Yes
     - The name of the component which has the static field or property to be retrieved.
   * - ``propertyName``
     - string
     - Yes
     - The name of the static field or property to be retrieved.
   * - ``assemblyName``
     - string
     - Yes
     - The name of the assembly containing the component.
   * - ``maxDepth``
     - int
     - No
     - The maximum depth in the hierarchy to look for the static field or property. Default is 2.
```

**_Returns_**

- This is a generic method. The return type depends on the type of the static field or property to be retrieved.

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestGetStaticProperty()
        {
            altDriver.CallStaticMethod<string>("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule", new string[] {"1920", "1080", "true"}, new string[] {"System.Int32", "System.Int32", "System.Boolean"});
            var width = altDriver.GetStaticProperty<int>("UnityEngine.Screen", "currentResolution.width", "UnityEngine.CoreModule");
            Assert.AreEqual(1920, width);
        }

    .. code-tab:: java

        @Test
        public void testGetStaticProperty() {
            AltCallStaticMethodParams altCallStaticMethodParams = new AltCallStaticMethodParams.Builder("UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule", new Object[] {"1920", "1080", "True"}).withTypeOfParameters(new String[] {"System.Int32", "System.Int32", "System.Boolean"}).build();
            altDriver.callStaticMethod(altCallStaticMethodParams, Void.class);
            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder("UnityEngine.Screen", "currentResolution.width", "UnityEngine.CoreModule").build();
            int width = altDriver.GetStaticProperty(altGetComponentPropertyParams, Integer.class);
            assertEquals(width, 1920);
        }

    .. code-tab:: py

        def test_get_static_property(self):

            self.alt_driver.call_static_method(
                "UnityEngine.Screen", "SetResolution", "UnityEngine.CoreModule",
                parameters=["1920", "1080", "True"],
                type_of_parameters=["System.Int32",
                                    "System.Int32", "System.Boolean"]
            )
            width = self.alt_driver.get_static_property(
                "UnityEngine.Screen", "currentResolution.width",
                "UnityEngine.CoreModule"
            )

            assert int(width) == 1920

    .. code-tab:: robot

        Test Get Static Property
            ${parameters}=    Create List    1920    1080    True
            ${type_of_parameters}=    Create List    System.Int32    System.Int32    System.Boolean
            Call Static Method    UnityEngine.Screen    SetResolution    UnityEngine.CoreModule    parameters=${parameters}    type_of_parameters=${type_of_parameters}
            ${width}=    Get Static Property    UnityEngine.Screen    currentResolution.width    UnityEngine.CoreModule
            Should Be Equal As Integers    ${width}    1920

```

## SetStaticProperty

Sets the value of the static field or property.

**_Parameters_**

```eval_rst
.. tabs::

    .. tab:: C#

        .. list-table:: SetStaticProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should be "namespace.componentName".
           * - propertyName
             - string
             - Yes
             - The name of the property whose value you want to set.
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the component.
           * - updatedProperty
             - object
             - Yes
             - The value to be set for the chosen component's static property.

    .. tab:: Java

        .. list-table:: setStaticProperty Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - componentName
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should be "namespace.componentName".
           * - propertyName
             - string
             - Yes
             - The name of the property whose value you want to set.
           * - assemblyName
             - string
             - Yes
             - The name of the assembly containing the component.
           * - updatedProperty
             - object
             - Yes
             - The value to be set for the chosen component's static property.

    .. tab:: Python

        .. list-table:: set_static_property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should be "namespace.component_name".
           * - property_name
             - string
             - Yes
             - The name of the property whose value you want to set.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - updated_property
             - object
             - Yes
             - The value to be set for the chosen component's static property.

    .. tab:: Robot

        .. list-table:: Set Static Property Parameters
           :widths: 20 20 10 40
           :header-rows: 1

           * - Name
             - Type
             - Required
             - Description
           * - component_name
             - string
             - Yes
             - The name of the component. If the component has a namespace, the format should be "namespace.component_name".
           * - property_name
             - string
             - Yes
             - The name of the property whose value you want to set.
           * - assembly_name
             - string
             - Yes
             - The name of the assembly containing the component.
           * - updated_property
             - object
             - Yes
             - The value to be set for the chosen component's static property.
```

**_Returns_**

-   Nothing

**_Examples_**

```eval_rst
.. tabs::

    .. code-tab:: c#

        [Test]
        public void TestSetStaticProperty()
        {
            var expectedValue = 5;
            altDriver.SetStaticProperty("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue);
            var value = altDriver.GetStaticProperty<int>("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp");
            Assert.AreEqual(expectedValue, value);
        }

    .. code-tab:: java

        @Test
        public void testSetStaticProperty() {
            final Integer expectedValue = 5;
            AltSetComponentPropertyParams altSetComponentPropertyParams = new AltSetComponentPropertyParams.Builder(
                "AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue.toString()).build();
            altDriver.setStaticProperty(altSetComponentPropertyParams);
            AltGetComponentPropertyParams altGetComponentPropertyParams = new AltGetComponentPropertyParams.Builder("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp").build();
            Integer value = altDriver.getStaticProperty(altGetComponentPropertyParams,Integer.class);
            assertEquals(expectedValue, value);
        }

    .. code-tab:: py

        def test_set_static_property(self):
            expectedValue = 5
            self.alt_driver.set_static_property("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp", expectedValue)
            value = self.alt_driver.get_static_property("AltExampleScriptCapsule", "privateStaticVariable", "Assembly-CSharp")
            assert expectedValue == value

    .. code-tab:: robot

        Test Set Static Property
            Set Static Property    AltExampleScriptCapsule    privateStaticVariable    Assembly-CSharp    5
            ${value}=    Get Static Property    AltExampleScriptCapsule    privateStaticVariable    Assembly-CSharp
            Should Be Equal As Integers    5    ${value}

```
